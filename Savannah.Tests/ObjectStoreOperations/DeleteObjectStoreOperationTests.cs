using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class DeleteObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestDeleteExistingObject()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var existingStorageObject = new StorageObject(null, null, null);
            var executionContext = new ObjectStoreOperationExectionContext(existingStorageObject, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            var storageObject = deleteOperation.GetStorageObjectFrom(executionContext);

            Assert.IsNull(storageObject);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToDeleteNonExistingObjectThrowsException()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => deleteOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be removed.");
        }
    }
}