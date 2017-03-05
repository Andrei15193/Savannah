using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Utilities;

namespace Savannah.Tests.Utilities
{
    [TestClass]
    public class AsyncAccessorTests
        : UnitTest
    {
        private static Task _CompletedTask { get; } = Task.FromResult<object>(null);

        private AsyncAccessor _ReaderWriterAwaiter { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _ReaderWriterAwaiter = new AsyncAccessor();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _ReaderWriterAwaiter = null;
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestReadOperationWaitsForWriteOperationToComplete()
        {
            using (var completeWriteOperation = new ManualResetEventSlim())
            {
                var writeTask = _ReaderWriterAwaiter.WriteAsync(() => Task.Factory.StartNew(completeWriteOperation.Wait));
                var readTask = _ReaderWriterAwaiter.ReadAsync(
                    () =>
                    {
                        Assert.IsTrue(writeTask.IsCompleted);
                        return _CompletedTask;
                    });

                await Task.Yield();

                completeWriteOperation.Set();
                await Task.WhenAll(writeTask, readTask);
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationWaitsForReadOperationToComplete()
        {
            using (var completeReadOperation = new ManualResetEventSlim())
            {
                var readTask = _ReaderWriterAwaiter.ReadAsync(() => Task.Factory.StartNew(completeReadOperation.Wait));
                var writeTask = _ReaderWriterAwaiter.WriteAsync(
                    () =>
                    {
                        Assert.IsTrue(readTask.IsCompleted);
                        return _CompletedTask;
                    });

                await Task.Yield();

                completeReadOperation.Set();
                await Task.WhenAll(readTask, writeTask);
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PositiveIntegerValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestMultipleReadOperationsExecuteInParallel()
        {
            var row = GetRow<PositiveIntegerValuesRow>();

            await _UsingManualResetEventsAsync(
                row.Value,
                async incrementedEvents =>
                {
                    var numberOfReadersInParallel = 0;

                    using (var completeReadOperations = new ManualResetEventSlim())
                    {
                        var readerTasks = incrementedEvents
                            .Select(incrementedEvent => _ReaderWriterAwaiter.ReadAsync(
                                async () =>
                                {
                                    await Task.Yield();
                                    Interlocked.Increment(ref numberOfReadersInParallel);
                                    incrementedEvent.Set();
                                    await Task.Factory.StartNew(completeReadOperations.Wait);
                                }))
                            .ToList();

                        await Task.WhenAll(incrementedEvents.Select(incrementedEvent => Task.Factory.StartNew(incrementedEvent.Wait)).ToList());

                        Assert.AreEqual(row.Value, numberOfReadersInParallel);

                        completeReadOperations.Set();
                        await Task.WhenAll(readerTasks);
                    }
                });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PositiveIntegerValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestMultipleWriteOperationsExecuteSequentially()
        {
            var row = GetRow<PositiveIntegerValuesRow>();

            var writerTasks = new List<Task>();
            writerTasks
                .AddRange(Enumerable.Range(0, row.Value)
                .Select(writerIndex => _ReaderWriterAwaiter.WriteAsync(
                    async () =>
                    {
                        await Task.Yield();
                        if (writerIndex > 0)
                            Assert.IsTrue(writerTasks[writerIndex - 1].IsCompleted);
                        if (writerIndex < (writerTasks.Count - 1))
                            Assert.IsFalse(writerTasks[writerIndex + 1].IsCompleted);
                    })));

            await Task.WhenAll(writerTasks);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        [Owner("Andrei Fangli")]
        public async Task TestFaultingWriterTaskThrowsExceptionWhenAwaited()
        {
            await _ReaderWriterAwaiter.WriteAsync(
                async delegate
                {
                    await Task.Yield();
                    throw new Exception();
                });
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestFaultedWriterTaskDoesNotFaultNextWriterTask()
        {
            var writerTask = _ReaderWriterAwaiter.WriteAsync(
                async delegate
                {
                    await Task.Yield();
                    throw new Exception();
                });
            await writerTask.ContinueWith(task => Assert.IsTrue(task.IsFaulted));

            await _ReaderWriterAwaiter.WriteAsync(() => _CompletedTask);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestFaultedWriterTaskDoesNotFaultNextReaderTask()
        {
            var writerTask = _ReaderWriterAwaiter.WriteAsync(
                async delegate
                {
                    await Task.Yield();
                    throw new Exception();
                });
            await writerTask.ContinueWith(task => Assert.IsTrue(task.IsFaulted));

            await _ReaderWriterAwaiter.ReadAsync(() => _CompletedTask);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestFaultedReaderTaskDoesNotFaultNextWriterTask()
        {
            var readerTask = _ReaderWriterAwaiter.ReadAsync(
                async delegate
                {
                    await Task.Yield();
                    throw new Exception();
                });
            await readerTask.ContinueWith(task => Assert.IsTrue(task.IsFaulted));

            await _ReaderWriterAwaiter.WriteAsync(() => _CompletedTask);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestFaultedReaderTaskDoesNotFaultNextReaderTask()
        {
            var readerTask = _ReaderWriterAwaiter.ReadAsync(
                async delegate
                {
                    await Task.Yield();
                    throw new Exception();
                });
            await readerTask.ContinueWith(task => Assert.IsTrue(task.IsFaulted));

            await _ReaderWriterAwaiter.ReadAsync(() => _CompletedTask);
        }


        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestCancelledWriterTaskDoesNotFaultNextWriterTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                var writerTask = _ReaderWriterAwaiter.WriteAsync(
                    delegate
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        return _CompletedTask;
                    });
                await writerTask.ContinueWith(task => Assert.IsTrue(task.IsCanceled));

                await _ReaderWriterAwaiter.WriteAsync(() => _CompletedTask);
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestCancelledWriterTaskDoesNotFaultNextReaderTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                var writerTask = _ReaderWriterAwaiter.WriteAsync(
                    delegate
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        return _CompletedTask;
                    });
                await writerTask.ContinueWith(task => Assert.IsTrue(task.IsCanceled));

                await _ReaderWriterAwaiter.ReadAsync(() => _CompletedTask);
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestCancelledReaderTaskDoesNotFaultNextWriterTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                var readerTask = _ReaderWriterAwaiter.ReadAsync(
                    delegate
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        return _CompletedTask;
                    });
                await readerTask.ContinueWith(task => Assert.IsTrue(task.IsCanceled));

                await _ReaderWriterAwaiter.WriteAsync(() => _CompletedTask);
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestCancelledReaderTaskDoesNotFaultNextReaderTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                var readerTask = _ReaderWriterAwaiter.ReadAsync(
                    delegate
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        return _CompletedTask;
                    });
                await readerTask.ContinueWith(task => Assert.IsTrue(task.IsCanceled));

                await _ReaderWriterAwaiter.ReadAsync(() => _CompletedTask);
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationCanBeCancelledBeforePreviousWriteOperationCompletes()
        {
            using (var completeOperation = new ManualResetEventSlim())
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var writeTask1 = _ReaderWriterAwaiter.WriteAsync(() => Task.Factory.StartNew(completeOperation.Wait));
                var writeTask2 = _ReaderWriterAwaiter
                    .WriteAsync(() => _CompletedTask, cancellationTokenSource.Token)
                    .ContinueWith(task => Assert.IsTrue(task.IsCanceled));
                await Task.Yield();

                cancellationTokenSource.Cancel();
                await writeTask2;

                completeOperation.Set();
                await writeTask1;
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationCanBeCancelledBeforePreviousReadOperationCompletes()
        {
            using (var completeOperation = new ManualResetEventSlim())
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var readTask = _ReaderWriterAwaiter.ReadAsync(() => Task.Factory.StartNew(completeOperation.Wait));
                var writeTask = _ReaderWriterAwaiter
                    .WriteAsync(() => _CompletedTask, cancellationTokenSource.Token)
                    .ContinueWith(task => Assert.IsTrue(task.IsCanceled));
                await Task.Yield();

                cancellationTokenSource.Cancel();
                await writeTask;

                completeOperation.Set();
                await readTask;
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestReadOperationCanBeCancelledBeforePreviousWriteOperationCompletes()
        {
            using (var completeOperation = new ManualResetEventSlim())
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var writeTask = _ReaderWriterAwaiter.WriteAsync(() => Task.Factory.StartNew(completeOperation.Wait));
                var readTask = _ReaderWriterAwaiter
                    .ReadAsync(() => _CompletedTask, cancellationTokenSource.Token)
                    .ContinueWith(task => Assert.IsTrue(task.IsCanceled));
                await Task.Yield();

                cancellationTokenSource.Cancel();
                await readTask;

                completeOperation.Set();
                await writeTask;
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationReturnsSameResult()
        {
            var expectedRsult = new object();

            var actualResult = await _ReaderWriterAwaiter.WriteAsync(() => Task.FromResult(expectedRsult));

            Assert.AreSame(expectedRsult, actualResult);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public async Task TestReadOperationReturnsSameResult()
        {
            var expectedRsult = new object();

            var actualResult = await _ReaderWriterAwaiter.ReadAsync(() => Task.FromResult(expectedRsult));

            Assert.AreSame(expectedRsult, actualResult);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationThrowsExceptionForNullAction()
            => await _ReaderWriterAwaiter.WriteAsync(null);

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public async Task TestWriteOperationThrowsExceptionForNullFunction()
            => await _ReaderWriterAwaiter.WriteAsync<object>(null);

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public async Task TestReadOperationThrowsExceptionForNullAction()
            => await _ReaderWriterAwaiter.ReadAsync(null);

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public async Task TestReadOperationThrowsExceptionForNullFunction()
            => await _ReaderWriterAwaiter.ReadAsync<object>(null);

        private static async Task _UsingManualResetEventsAsync(int count, Func<IEnumerable<ManualResetEventSlim>, Task> action)
        {
            var manualResetEvents = Enumerable.Range(0, count).Select(manualResetEvent => new ManualResetEventSlim()).ToList();

            try
            {
                await action(manualResetEvents);
            }
            finally
            {
                var exceptions = new List<Exception>();
                foreach (var manualResetEvent in manualResetEvents)
                    try
                    {
                        manualResetEvent.Dispose();
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                if (exceptions.Any())
                    throw new AggregateException(exceptions);
            }
        }
    }
}