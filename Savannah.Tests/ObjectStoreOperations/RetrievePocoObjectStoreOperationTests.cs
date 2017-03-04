using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class RetrievePocoObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveReturnsSameStorageObject()
        {
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(new object());
            var storageObject = new StorageObject(null, null, null);
            var executionContext = new ObjectStoreOperationExectionContext(
                storageObject,
                StorageObjectFactory,
                DateTime.UtcNow,
                new List<object>());

            var actualStorageObject = retrieveOperation.GetStorageObjectFrom(executionContext);

            Assert.AreSame(storageObject, actualStorageObject);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRetrieveExistingObject()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(@object);
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, null),
                StorageObjectFactory,
                DateTime.UtcNow,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreEqual(
                @object,
                new { actualObject.PartitionKey, actualObject.RowKey });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRetrieveExistingObjectWithSpecifiedProperties()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(@object, new[] { nameof(MockObject.PartitionKey) });
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, null),
                StorageObjectFactory,
                DateTime.UtcNow,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreEqual(
                new { @object.PartitionKey, RowKey = default(string) },
                new { actualObject.PartitionKey, actualObject.RowKey });
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToRetrieveNonExistingObjectThrowsException()
        {
            var retrieveOperation = new RetrievePocoObjectStoreOperation<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => retrieveOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be retrieved.");
        }
    }
}