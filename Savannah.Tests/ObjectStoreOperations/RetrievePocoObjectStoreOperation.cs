using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class RetrievePocoObjectStoreOperation
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        public void TestRetrieveReturnsSameStorageObject()
        {
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(new object());
            var storageObject = new StorageObject(null, null, null);
            var executionContext = new ObjectStoreOperationExectionContext(
                storageObject,
                StorageObjectFactory,
                new List<object>());

            var actualStorageObject = retrieveOperation.GetStorageObjectFrom(executionContext);

            Assert.AreSame(storageObject, actualStorageObject);
        }

        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestRetrieveExistingObject(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(@object);
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(partitionKey, rowKey, null),
                StorageObjectFactory,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreEqual(
                @object,
                new { actualObject.PartitionKey, actualObject.RowKey });
        }

        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestRetrieveExistingObjectWithSpecifiedProperties(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(@object, new[] { nameof(MockObject.PartitionKey) });
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(partitionKey, rowKey, null),
                StorageObjectFactory,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreEqual(
                new { @object.PartitionKey, RowKey = default(string) },
                new { actualObject.PartitionKey, actualObject.RowKey });
        }

        [TestMethod]
        public void TestTryingToRetrieveNonExistingObjectThrowsException()
        {
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => retrieveOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be retrieved.");
        }
    }
}