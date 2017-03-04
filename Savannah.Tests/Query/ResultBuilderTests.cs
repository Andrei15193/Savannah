using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Query;

namespace Savannah.Tests.Query
{
    [TestClass]
    public class ResultBuilderTests
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestAddingTwoStorageObjectsAddsThemSortedByPartitionKeyThenByRowKey()
        {
            var partitionKey = "value";
            var storageObject1 = new StorageObject(partitionKey, "rowKey1", null);
            var storageObject2 = new StorageObject(partitionKey, "rowKey2", null);
            var resultBuilder = new ResultBuilder();

            resultBuilder.TryAdd(storageObject1);
            resultBuilder.TryAdd(storageObject2);

            Assert.IsTrue(new[] { storageObject1, storageObject2 }.SequenceEqual(resultBuilder.Result));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToAddStorageObjectWithMaxCount1AfterExistingObjectReturnsFalse()
        {
            var partitionKey = "value";
            var storageObject1 = new StorageObject(partitionKey, "rowKey1", null);
            var storageObject2 = new StorageObject(partitionKey, "rowKey2", null);
            var resultBuilder = new ResultBuilder(1);

            resultBuilder.TryAdd(storageObject1);
            Assert.IsFalse(resultBuilder.TryAdd(storageObject2));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToAddStorageObjectWithMaxCount1AfterExistingObjectReturnsFirstObjectAsResult()
        {
            var partitionKey = "value";
            var storageObject1 = new StorageObject(partitionKey, "rowKey1", null);
            var storageObject2 = new StorageObject(partitionKey, "rowKey2", null);
            var resultBuilder = new ResultBuilder(1);

            resultBuilder.TryAdd(storageObject1);
            resultBuilder.TryAdd(storageObject2);

            Assert.AreSame(storageObject1, resultBuilder.Result.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToAddStorageObjectWithMaxCount1BeforeExistingObjectReturnsTrue()
        {
            var partitionKey = "value";
            var storageObject1 = new StorageObject(partitionKey, "rowKey1", null);
            var storageObject2 = new StorageObject(partitionKey, "rowKey2", null);
            var resultBuilder = new ResultBuilder(1);

            resultBuilder.TryAdd(storageObject2);
            Assert.IsTrue(resultBuilder.TryAdd(storageObject1));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToAddStorageObjectWithMaxCount1BeforeExistingObjectReturnsLastObjectAsResult()
        {
            var partitionKey = "value";
            var storageObject1 = new StorageObject(partitionKey, "rowKey1", null);
            var storageObject2 = new StorageObject(partitionKey, "rowKey2", null);
            var resultBuilder = new ResultBuilder(1);

            resultBuilder.TryAdd(storageObject2);
            resultBuilder.TryAdd(storageObject1);

            Assert.AreSame(storageObject1, resultBuilder.Result.Single());
        }
    }
}