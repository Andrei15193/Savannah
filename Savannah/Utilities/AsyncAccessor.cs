using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Savannah.Utilities
{
    internal class AsyncAccessor
    {
        private static Task _CompletedTask { get; } = Task.FromResult<object>(null);

        private readonly object _locker = new object();
        private volatile Task _lastReaderTask = _CompletedTask;
        private volatile Task _lastWriterTask = _CompletedTask;

        internal Task<TResult> WriteAsync<TResult>(Func<Task<TResult>> action)
            => WriteAsync(action, CancellationToken.None);

        internal Task<TResult> WriteAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken)
        {
            Task<TResult> writerTask;

            lock (_locker)
            {
                writerTask = new Func<Task<TResult>>(
                    async delegate
                    {
                        if (action == null)
                            throw new ArgumentNullException(nameof(action));

                        var currentReaderWriterTask = Task
                            .WhenAll(_lastReaderTask, _lastWriterTask)
                            .ContinueWith(delegate { Debug.WriteLine("Reader and writer tasks have completed."); });
                        await Task.Yield();

                        await _AsCancellableAsync(currentReaderWriterTask, cancellationToken).ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();

                        return await action().ConfigureAwait(false);
                    }).Invoke();
                _lastWriterTask = writerTask;
            }

            return writerTask;
        }

        internal Task WriteAsync(Func<Task> action)
            => WriteAsync(action, CancellationToken.None);

        internal Task WriteAsync(Func<Task> action, CancellationToken cancellationToken)
            => WriteAsync<object>(
                async () =>
                {
                    if (action == null)
                        throw new ArgumentNullException(nameof(action));

                    await action();
                    return null;
                },
                cancellationToken);

        internal Task<TResult> ReadAsync<TResult>(Func<Task<TResult>> action)
            => ReadAsync(action, CancellationToken.None);

        internal Task<TResult> ReadAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken)
        {
            Task<TResult> readerTask;

            lock (_locker)
            {
                readerTask = new Func<Task<TResult>>(
                    async delegate
                    {
                        if (action == null)
                            throw new ArgumentNullException(nameof(action));

                        var currentReaderWriterTask = _lastWriterTask
                            .ContinueWith(delegate { Debug.WriteLine("Writer task has completed."); });
                        await Task.Yield();

                        await _AsCancellableAsync(currentReaderWriterTask, cancellationToken).ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();

                        return await action().ConfigureAwait(false);
                    }).Invoke();
                _lastReaderTask = (_lastReaderTask.IsCompleted ? readerTask : Task.WhenAll(_lastReaderTask, readerTask));
            }

            return readerTask;
        }

        internal Task ReadAsync(Func<Task> action)
            => ReadAsync(action, CancellationToken.None);

        internal Task ReadAsync(Func<Task> action, CancellationToken cancellationToken)
            => ReadAsync<object>(
                async () =>
                {
                    if (action == null)
                        throw new ArgumentNullException(nameof(action));

                    await action();
                    return null;
                },
                cancellationToken);

        private static Task _AsCancellableAsync(Task task, CancellationToken cancellationToken)
        {
            Task result;

            if (cancellationToken.CanBeCanceled)
            {
                var taskCompletionSource = new TaskCompletionSource<object>();
                if (cancellationToken.IsCancellationRequested)
                {
                    taskCompletionSource.SetCanceled();
                    result = taskCompletionSource.Task;
                }
                else
                {
                    cancellationToken.Register(taskCompletionSource.SetCanceled, useSynchronizationContext: false);
                    result = Task.WhenAny(task, taskCompletionSource.Task);
                }
            }
            else
                result = task;

            return result;
        }
    }
}