using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Tests.Mocks;
using Savannah.Utilities;
using Windows.Storage;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreTests
    {
        private const string _objectStoreFolderName = nameof(ObjectStoreTests);

        private ObjectStore _ObjectStore { get; set; }

        private IHashProvider _HashProvider { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _HashProvider = new Md5HashProvider();
            _ObjectStore = new ObjectStore(_objectStoreFolderName, new HashProviderMock(value => _HashProvider.GetHashFor(value)));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _ObjectStore = null;
            _HashProvider = null;
            var localTestFolder = ApplicationData.Current.LocalFolder;
            var objectStoreFolder = Task.Run(localTestFolder.CreateFolderAsync(_objectStoreFolderName, CreationCollisionOption.OpenIfExists).AsTask).Result;

            Task.Run(objectStoreFolder.DeleteAsync().AsTask).Wait();
        }

        [TestMethod]
        public async Task TestStoredObjectIsRetrievedWithSamePartitionKey()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStore.ExecuteAsync(operation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.PartitionKey, storedObject.PartitionKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsRetrievedWithSameRowKey()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStore.ExecuteAsync(operation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.RowKey, storedObject.RowKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsNotCached()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStore.ExecuteAsync(operation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreNotSame(objectToStore, storedObject);
        }

        [TestMethod]
        public async Task TestObjectStoreCannotContainSameObjectTwice()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStore.ExecuteAsync(operation);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(operation),
                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
        }

        [TestMethod]
        public async Task TestObjectStoreInsertTwoObjectsInSamePartition()
        {
            var firstObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var firstOperation = ObjectStoreOperation.Insert(firstObjectToStore);
            await _ObjectStore.ExecuteAsync(firstOperation);

            var secondObjectToStore = new MockObject { PartitionKey = firstObjectToStore.PartitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondOperation = ObjectStoreOperation.Insert(secondObjectToStore);
            await _ObjectStore.ExecuteAsync(secondOperation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsTrue(
                new[]
                {
                    firstObjectToStore,
                    secondObjectToStore
                }
                .OrderBy(@object => @object.RowKey)
                .Select(@object => @object.RowKey)
                .SequenceEqual(storedObjects
                    .OrderBy(@object => @object.RowKey)
                    .Select(@object => @object.RowKey)));
        }

        [TestMethod]
        public async Task TestTryingToAddObjectWithoutPartitionKeyThrowsException()
        {
            var operation = ObjectStoreOperation.Insert(new { RowKey = Guid.NewGuid().ToString() });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(operation),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public async Task TestTryingToAddObjectWithoutRowKeyThrowsException()
        {
            var operation = ObjectStoreOperation.Insert(new { PartitionKey = Guid.NewGuid().ToString() });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(operation),
                "The given object must expose a readable RowKey property of type string.");
        }

        [TestMethod]
        public async Task TestAddingObjectAtMaximumSizeDoesNotThrowException()
        {
            var operation = ObjectStoreOperation.Insert(
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize]
                });

            await _ObjectStore.ExecuteAsync(operation);
        }

        [TestMethod]
        public async Task TestTryingToAddObjectLargerThanSupportedThrowsException()
        {
            var operation = ObjectStoreOperation.Insert(
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize + 1]
                });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(operation),
                "The maximum supported size of an object is 1,048,576 bytes.");
        }

        [TestMethod]
        public async Task TestObjectStoreInsertTwoObjectsInDifferentPartitionInSameFile()
        {
            _HashProvider = new HashProviderMock(value => "testPartitonFile");

            var firstObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var firstOperation = ObjectStoreOperation.Insert(firstObjectToStore);
            await _ObjectStore.ExecuteAsync(firstOperation);

            var secondObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var secondOperation = ObjectStoreOperation.Insert(secondObjectToStore);
            await _ObjectStore.ExecuteAsync(secondOperation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsTrue(
                new[]
                {
                    firstObjectToStore,
                    secondObjectToStore
                }
                .OrderBy(@object => @object.PartitionKey)
                .ThenBy(@object => @object.RowKey)
                .Select(@object => new { @object.PartitionKey, @object.RowKey })
                .SequenceEqual(storedObjects
                    .OrderBy(@object => @object.PartitionKey)
                    .ThenBy(@object => @object.RowKey)
                    .Select(@object => new { @object.PartitionKey, @object.RowKey })));
        }

        [DataTestMethod]
        [DataRow(new[] { "1", "2", "3" })]
        [DataRow(new[] { "1", "3", "2" })]
        [DataRow(new[] { "2", "1", "3" })]
        [DataRow(new[] { "2", "3", "1" })]
        [DataRow(new[] { "3", "2", "1" })]
        [DataRow(new[] { "3", "1", "2" })]
        public async Task TestObjectStoresObjectsSortedByTheirRowKeyInTheSamePartition(string[] rowKeys)
        {
            var partitionKey = Guid.NewGuid().ToString();
            foreach (var rowKey in rowKeys)
            {
                var objectToStore = new MockObject { PartitionKey = partitionKey, RowKey = rowKey };
                var operation = ObjectStoreOperation.Insert(objectToStore);
                await _ObjectStore.ExecuteAsync(operation);
            }

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsTrue(
                rowKeys
                    .OrderBy(rowKey => rowKey, StringComparer.Ordinal)
                    .SequenceEqual(storedObjects
                        .Select(@object => @object.RowKey)));
        }

        [TestMethod]
        public async Task TestDeletingOnlyObjectFromObjectStoreLeavesItEmpty()
        {
            var @object = new
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString()
            };
            var insertOperation = ObjectStoreOperation.Insert(@object);
            await _ObjectStore.ExecuteAsync(insertOperation);

            var deleteOperation = ObjectStoreOperation.Delete(@object);
            await _ObjectStore.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsFalse(storedObjects.Any());
        }

        [DataTestMethod]
        [DataRow(new[] { "partitionKey1", "partitionKey2", "partitionKey3", "partitionKey4" }, "partitionKey1")]
        [DataRow(new[] { "partitionKey1", "partitionKey2", "partitionKey3", "partitionKey4" }, "partitionKey2")]
        [DataRow(new[] { "partitionKey1", "partitionKey2", "partitionKey3", "partitionKey4" }, "partitionKey3")]
        [DataRow(new[] { "partitionKey1", "partitionKey2", "partitionKey3", "partitionKey4" }, "partitionKey4")]
        public async Task TestDeletingObjectFromDifferentPartition(string[] paritionKeys, string partitionKeyToRemove)
        {
            var rowKey = Guid.NewGuid().ToString();
            foreach (var partitionKey in paritionKeys)
            {
                var @object = new
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey
                };
                var insertOperation = ObjectStoreOperation.Insert(@object);
                await _ObjectStore.ExecuteAsync(insertOperation);
            }

            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = partitionKeyToRemove, RowKey = rowKey });
            await _ObjectStore.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsTrue(storedObjects
                .Select(storedObject => storedObject.PartitionKey)
                .OrderBy(partitionKey => partitionKey)
                .SequenceEqual(paritionKeys
                    .Except(new[] { partitionKeyToRemove })
                    .OrderBy(partitionKey => partitionKey)));
        }

        [DataTestMethod]
        [DataRow(new[] { "rowKey1", "rowKey2", "rowKey3", "rowKey4" }, "rowKey1")]
        [DataRow(new[] { "rowKey1", "rowKey2", "rowKey3", "rowKey4" }, "rowKey2")]
        [DataRow(new[] { "rowKey1", "rowKey2", "rowKey3", "rowKey4" }, "rowKey3")]
        [DataRow(new[] { "rowKey1", "rowKey2", "rowKey3", "rowKey4" }, "rowKey4")]
        public async Task TestDeletingObjectFromSamePartition(string[] rowKeys, string rowKeyToRemove)
        {
            var partitionKey = Guid.NewGuid().ToString();
            foreach (var rowKey in rowKeys)
            {
                var @object = new
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey
                };
                var insertOperation = ObjectStoreOperation.Insert(@object);
                await _ObjectStore.ExecuteAsync(insertOperation);
            }

            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = partitionKey, RowKey = rowKeyToRemove });
            await _ObjectStore.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStore.QueryAsync<MockObject>(ObjectStoreQuery.All);
            Assert.IsTrue(storedObjects
                .Select(storedObject => storedObject.RowKey)
                .OrderBy(rowKey => rowKey)
                .SequenceEqual(rowKeys
                    .Except(new[] { rowKeyToRemove })
                    .OrderBy(rowKey => rowKey)));
        }

        [TestMethod]
        public async Task TestTryingToDeleteAnObjectThatDoesNotExistThrowsException()
        {
            var @object = new
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString()
            };
            var deleteOperation = ObjectStoreOperation.Delete(@object);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(deleteOperation),
                "The object does not exist, it cannot be removed.");
        }

        [TestMethod]
        public async Task TestTryingToDeleteAnObjectThatDoesNotExposeAPartitionKeyThrowsException()
        {
            var @object = new
            {
                RowKey = Guid.NewGuid().ToString()
            };
            var deleteOperation = ObjectStoreOperation.Delete(@object);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(deleteOperation),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public async Task TestTryingToDeleteAnObjectThatDoesNotExposeARowKeyThrowsException()
        {
            var @object = new
            {
                PartitionKey = Guid.NewGuid().ToString()
            };
            var deleteOperation = ObjectStoreOperation.Delete(@object);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStore.ExecuteAsync(deleteOperation),
                "The given object must expose a readable RowKey property of type string.");
        }
    }
}