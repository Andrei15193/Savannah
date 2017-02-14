using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class DeleteObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        public async Task TestDeleteExistingObject()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var existingStorageObject = new StorageObject(null, null, null);

            await deleteOperation.ExecuteAsync(existingStorageObject, Context);

            Assert.AreEqual(0, Result.Length);
        }

        [TestMethod]
        public async Task TestTryingToDeleteNonExistingObjectThrowsException()
        {
            var deleteOperation = new DeleteObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => deleteOperation.ExecuteAsync(null, Context),
                "The object does not exist, it cannot be removed.");
        }
    }
}