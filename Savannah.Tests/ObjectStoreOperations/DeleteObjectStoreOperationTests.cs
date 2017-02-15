using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class DeleteObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        public void TestDeleteExistingObject()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var existingStorageObject = new StorageObject(null, null, null);

            var storageObject = deleteOperation.GetStorageObjectFrom(existingStorageObject, StorageObjectFactory);

            Assert.IsNull(storageObject);
        }

        [TestMethod]
        public void TestTryingToDeleteNonExistingObjectThrowsException()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => deleteOperation.GetStorageObjectFrom(null, StorageObjectFactory),
                "The object does not exist, it cannot be removed.");
        }
    }
}