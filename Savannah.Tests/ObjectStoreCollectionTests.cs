using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;
using Savannah.Utilities;
using Windows.Storage;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreCollectionTests
    {
        private const string _objectStoreFolderName = nameof(ObjectStoreCollectionTests);
        private const string _objectStoreCollectionName = "TestCollection";

        private ObjectStore _ObjectStore { get; set; }

        private ObjectStoreCollection _ObjectStoreCollection { get; set; }

        private IHashProvider _HashProvider { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _HashProvider = new Md5HashProvider();
            _ObjectStore = new ObjectStore(_objectStoreFolderName, new HashProviderMock(value => _HashProvider.GetHashFor(value)));
            _ObjectStoreCollection = _ObjectStore.GetCollection(_objectStoreCollectionName);
            Task.Run(() => _ObjectStoreCollection.CreateIfNotExistsAsync()).Wait();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _ObjectStore = null;
            _ObjectStoreCollection = null;
            _HashProvider = null;
            var localTestFolder = ApplicationData.Current.LocalFolder;
            var objectStoreFolder = Task.Run(localTestFolder.CreateFolderAsync(_objectStoreFolderName, CreationCollisionOption.OpenIfExists).AsTask).Result;

            Task.Run(objectStoreFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask).Wait();
        }

        [TestMethod]
        public async Task TestTryingToCreteACollectionThatAlreadyExistsThrowsException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestTryingToCreteACollectionThatAlreadyExistsThrowsException));
            await collection.CreateAsync();

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => collection.CreateAsync(),
                "The Object Store Collection already exists. To ensure that a collection exists call one of the CreateIfNotExistsAsync overloads.");
        }

        [TestMethod]
        public async Task TestEnsuringThatACollectionIsCreatedDoesNotThrowException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestEnsuringThatACollectionIsCreatedDoesNotThrowException));
            await collection.CreateAsync();

            await collection.CreateIfNotExistsAsync();
        }

        [TestMethod]
        public async Task TestTryingToExecuteAnOperationOnACollectionThatIsNotCreatedThrowsException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestTryingToQueryACollectionThatIsNotCreatedThrowsException));
            var operation = ObjectStoreOperation.Insert(new { PartitionKey = "partitionKey", RowKey = "rowKey" });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => collection.ExecuteAsync(operation),
                "The Object Store Collection does not exist. Call one of the CreateAsync or CreateIfNotExistsAsync overloads first.");
        }

        [TestMethod]
        public async Task TestTryingToQueryACollectionThatIsNotCreatedThrowsException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestTryingToQueryACollectionThatIsNotCreatedThrowsException));

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => collection.QueryAsync<MockObject>(ObjectStoreQuery.All),
                "The Object Store Collection does not exist. Call one of the CreateAsync or CreateIfNotExistsAsync overloads first.");
        }

        [TestMethod]
        public async Task TestTryingToDeleteCollectionThatDoesNotExistThrowsException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestTryingToDeleteCollectionThatDoesNotExistThrowsException));

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => collection.DeleteAsync(),
                "The Object Store Collection does not exists. To ensure that a collection is removed call one of the DeleteIfExistsAsync overloads.");
        }

        [TestMethod]
        public async Task TestEnsuringThatACollectionIsDeletedDoesNotThrowException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestEnsuringThatACollectionIsDeletedDoesNotThrowException));

            await collection.DeleteIfExists();
        }

        [TestMethod]
        public async Task TestDeletingAnExistingCollectionDoesNotThrowException()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestDeletingAnExistingCollectionDoesNotThrowException));

            await collection.CreateAsync();
            await collection.DeleteAsync();
        }

        [TestMethod]
        public async Task TestEnsuringThatAnExistingCollectionIsDeleted()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestEnsuringThatAnExistingCollectionIsDeleted));

            await collection.CreateAsync();
            await collection.DeleteIfExists();
        }

        [TestMethod]
        public async Task TestRecreatingACollection()
        {
            var collection = _ObjectStore.GetCollection(nameof(TestRecreatingACollection));

            await collection.CreateAsync();
            await collection.DeleteAsync();
            await collection.CreateAsync();
        }

        [TestMethod]
        public async Task TestStoredObjectIsRetrievedWithSamePartitionKey()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.PartitionKey, storedObject.PartitionKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsRetrievedWithSameRowKey()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.RowKey, storedObject.RowKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsNotCached()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
            var storedObject = storedObjects.Single();

            Assert.AreNotSame(objectToStore, storedObject);
        }

        [TestMethod]
        public async Task TestObjectStoreCannotContainSameObjectTwice()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStoreCollection.ExecuteAsync(operation),
                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
        }

        [TestMethod]
        public async Task TestObjectStoreInsertTwoObjectsInSamePartition()
        {
            var firstObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var firstOperation = ObjectStoreOperation.Insert(firstObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(firstOperation);

            var secondObjectToStore = new MockObject { PartitionKey = firstObjectToStore.PartitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondOperation = ObjectStoreOperation.Insert(secondObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(secondOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
        public void TestTryingToAddObjectWithoutPartitionKeyThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreOperation.Insert(new { RowKey = Guid.NewGuid().ToString() }),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public void TestTryingToAddObjectWithoutRowKeyThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreOperation.Insert(new { PartitionKey = Guid.NewGuid().ToString() }),
                "The given object must expose a readable RowKey property of type string.");
        }

        [TestMethod]
        public void TestAddingObjectAtMaximumSizeDoesNotThrowException()
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
        }

        [TestMethod]
        public void TestTryingToAddObjectLargerThanSupportedThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreOperation.Insert(
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
                    }),
                "The maximum supported size of an object is 1,048,576 bytes.");
        }

        [TestMethod]
        public async Task TestObjectStoreInsertTwoObjectsInDifferentPartitionInSameFile()
        {
            _HashProvider = new HashProviderMock(value => "testPartitonFile");

            var firstObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var firstOperation = ObjectStoreOperation.Insert(firstObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(firstOperation);

            var secondObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var secondOperation = ObjectStoreOperation.Insert(secondObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(secondOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
                await _ObjectStoreCollection.ExecuteAsync(operation);
            }

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
            await _ObjectStoreCollection.ExecuteAsync(insertOperation);

            var deleteOperation = ObjectStoreOperation.Delete(@object);
            await _ObjectStoreCollection.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
                await _ObjectStoreCollection.ExecuteAsync(insertOperation);
            }

            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = partitionKeyToRemove, RowKey = rowKey });
            await _ObjectStoreCollection.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
                await _ObjectStoreCollection.ExecuteAsync(insertOperation);
            }

            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = partitionKey, RowKey = rowKeyToRemove });
            await _ObjectStoreCollection.ExecuteAsync(deleteOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(ObjectStoreQuery.All);
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
                () => _ObjectStoreCollection.ExecuteAsync(deleteOperation),
                "The object does not exist, it cannot be removed.");
        }

        [TestMethod]
        public void TestTryingToDeleteAnObjectThatDoesNotExposeAPartitionKeyThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreOperation.Delete(new { RowKey = Guid.NewGuid().ToString() }),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public void TestTryingToDeleteAnObjectThatDoesNotExposeARowKeyThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreOperation.Delete(new { PartitionKey = Guid.NewGuid().ToString() }),
                "The given object must expose a readable RowKey property of type string.");
        }
    }
}