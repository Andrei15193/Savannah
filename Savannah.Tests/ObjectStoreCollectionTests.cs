using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
                () => collection.QueryAsync<MockObject>(new ObjectStoreQuery()),
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.PartitionKey, storedObject.PartitionKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsRetrievedWithSameRowKey()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            var storedObject = storedObjects.Single();

            Assert.AreEqual(objectToStore.RowKey, storedObject.RowKey);
        }

        [TestMethod]
        public async Task TestStoredObjectIsNotCached()
        {
            var objectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var operation = ObjectStoreOperation.Insert(objectToStore);
            await _ObjectStoreCollection.ExecuteAsync(operation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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
        public async Task TestObjectStoreInsertTwoObjectsInDifferentPartitionInSameFile()
        {
            _HashProvider = new HashProviderMock(value => "testPartitonFile");

            var firstObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var firstOperation = ObjectStoreOperation.Insert(firstObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(firstOperation);

            var secondObjectToStore = new MockObject { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var secondOperation = ObjectStoreOperation.Insert(secondObjectToStore);
            await _ObjectStoreCollection.ExecuteAsync(secondOperation);

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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

            var storedObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
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
        public async Task TestTryingToDeleteAnObjectThatDoesNotExposeAPartitionKeyThrowsException()
        {
            var operation = ObjectStoreOperation.Delete(new { RowKey = Guid.NewGuid().ToString() });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStoreCollection.ExecuteAsync(operation),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public async Task TestTryingToDeleteAnObjectThatDoesNotExposeARowKeyThrowsException()
        {
            var operation = ObjectStoreOperation.Delete(new { PartitionKey = Guid.NewGuid().ToString() });

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStoreCollection.ExecuteAsync(operation),
                "The given object must expose a readable RowKey property of type string.");
        }

        [TestMethod]
        public async Task TestInsertingTwoObjectsThroughBatchOperation()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var firstObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(firstObject),
                ObjectStoreOperation.Insert(secondObject)
            };

            await _ObjectStoreCollection.ExecuteAsync(batchOperation);

            var existingObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            Assert.IsTrue(
                new[] { firstObject, secondObject }
                .OrderBy(
                    @object => @object.RowKey,
                    ObjectStoreLimitations.StringComparer)
                .SequenceEqual(
                    existingObjects.Select(
                        existingObject => new { existingObject.PartitionKey, existingObject.RowKey })));
        }

        [TestMethod]
        public async Task TestInsertingAndDeletingSameObjectInABatchLeavesTheStoreEmpty()
        {
            var @object = new { PartitionKey = Guid.NewGuid().ToString(), RowKey = Guid.NewGuid().ToString() };
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(@object),
                ObjectStoreOperation.Delete(@object)
            };

            await _ObjectStoreCollection.ExecuteAsync(batchOperation);

            var existingObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            Assert.IsFalse(existingObjects.Any());
        }

        [TestMethod]
        public async Task TestInsertingTwoObjectsAndDeletingOneOfThemInABatchLeavesTheStoreWithOneObject()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var firstObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(firstObject),
                ObjectStoreOperation.Insert(secondObject),
                ObjectStoreOperation.Delete(firstObject)
            };

            await _ObjectStoreCollection.ExecuteAsync(batchOperation);

            var existingObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            var existingObject = existingObjects.Single();
            Assert.AreEqual(secondObject, new { existingObject.PartitionKey, existingObject.RowKey });
        }

        [TestMethod]
        public async Task TestDeletingAnObjectThatDoesNotExistAfterTwoHaveBeenInsertedInABatchCrashesTheOperationInAnException()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var firstObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var thirdObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(firstObject),
                ObjectStoreOperation.Insert(secondObject),
                ObjectStoreOperation.Delete(thirdObject)
            };

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => _ObjectStoreCollection.ExecuteAsync(batchOperation),
                "The object does not exist, it cannot be removed.");
        }

        [TestMethod]
        public async Task TestDeletingAnObjectThatDoesNotExistAfterTwoHaveBeenInsertedInABatchLeavesTheCollectionInInitialState()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var firstObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var secondObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var thirdObject = new { PartitionKey = partitionKey, RowKey = Guid.NewGuid().ToString() };
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(firstObject),
                ObjectStoreOperation.Insert(secondObject),
                ObjectStoreOperation.Delete(thirdObject)
            };

            try
            {
                await _ObjectStoreCollection.ExecuteAsync(batchOperation);
                Assert.Fail("Expected exception.");
            }
            catch
            {
            }

            var existingObjects = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery());
            Assert.IsFalse(existingObjects.Any());
        }

        [TestMethod]
        public async Task TestQueryingForDynamicObjectsReturnsObjectsOfAllTypes()
        {
            var object1 = new { PartitionKey = "partitionKey", RowKey = "rowKey1", Property1 = "value1" };
            var object2 = new { PartitionKey = "partitionKey", RowKey = "rowKey2", Property2 = "value2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));

            var result = await _ObjectStoreCollection.QueryAsync(new ObjectStoreQuery());

            var actualObject1 = result.First();
            var actualObject2 = result.Last();

            Assert.IsTrue(
                new object[]
                {
                    object1,
                    object2
                }.SequenceEqual(
                    new object[]
                    {
                        new { PartitionKey = (string)actualObject1.PartitionKey, RowKey = (string)actualObject1.RowKey, Property1 = (string)actualObject1.Property1 },
                        new { PartitionKey = (string)actualObject2.PartitionKey, RowKey = (string)actualObject2.RowKey, Property2 = (string)actualObject2.Property2 }
                    }));
        }

        [TestMethod]
        public async Task TestQueryingForDynamicObjectsReturnsOnlySelectedProperties()
        {
            var object1 = new { PartitionKey = "partitionKey", RowKey = "rowKey1", Property1 = "value1" };
            var object2 = new { PartitionKey = "partitionKey", RowKey = "rowKey2", Property2 = "value2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));

            var result = await _ObjectStoreCollection.QueryAsync(new ObjectStoreQuery { Properties = new[] { nameof(object1.Property1) } });
            var actualObjects = result.Cast<IDictionary<string, object>>();
            var actualObject = actualObjects.Single();

            Assert.AreEqual(nameof(object1.Property1), actualObject.Keys.Single(), ignoreCase: false);
        }

        [TestMethod]
        public async Task TestQueryingForDynamicObjectsWhereNoneHaveSelectedPropertyReturnsEmptyResult()
        {
            var object1 = new { PartitionKey = "partitionKey", RowKey = "rowKey1", Property1 = "value1" };
            var object2 = new { PartitionKey = "partitionKey", RowKey = "rowKey2", Property2 = "value2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));

            var result = await _ObjectStoreCollection.QueryAsync(new ObjectStoreQuery { Properties = new[] { "Property3" } });

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task TestQueryingForPartitionKeyOnlyGetsObjectsFromThatPartition()
        {
            var object1 = new { PartitionKey = "partitionKey1", RowKey = "rowKey1" };
            var object2 = new { PartitionKey = "partitionKey1", RowKey = "rowKey2" };
            var object3 = new { PartitionKey = "partitionKey2", RowKey = "rowKey2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object3));

            var result = await _ObjectStoreCollection.QueryAsync<MockObject>(
                new ObjectStoreQuery
                {
                    Filter = ObjectStoreQueryFilter.Equal(nameof(StorageObject.PartitionKey), object1.PartitionKey)
                });

            Assert.IsTrue(new[] { object1, object2 }.SequenceEqual(result.Select(@object => new { @object.PartitionKey, @object.RowKey })));
        }

        [TestMethod]
        public async Task TestQueryingForPartitionKeyOnlyGetsAsManyObjectsAsSpecified()
        {
            var object1 = new { PartitionKey = "partitionKey1", RowKey = "rowKey1" };
            var object2 = new { PartitionKey = "partitionKey1", RowKey = "rowKey2" };
            var object3 = new { PartitionKey = "partitionKey2", RowKey = "rowKey2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object3));

            var result = await _ObjectStoreCollection.QueryAsync<MockObject>(new ObjectStoreQuery { Take = 2 });

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task TestQueryingWithEntityResolverCallsIntoCallback()
        {
            var callbackCallCount = 0;
            var object1 = new { PartitionKey = "partitionKey1", RowKey = "rowKey1" };
            var object2 = new { PartitionKey = "partitionKey1", RowKey = "rowKey2" };
            var object3 = new { PartitionKey = "partitionKey2", RowKey = "rowKey2" };

            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object1));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object2));
            await _ObjectStoreCollection.ExecuteAsync(ObjectStoreOperation.Insert(object3));

            var result = await _ObjectStoreCollection.QueryAsync(new ObjectStoreQuery(), delegate { callbackCallCount++; return new object(); });

            Assert.AreEqual(3, callbackCallCount);
        }
    }
}