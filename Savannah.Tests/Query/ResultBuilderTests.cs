using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Query;

namespace Savannah.Tests.Query
{
    [TestClass]
    public class ResultBuilderTests
    {
        [TestMethod]
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