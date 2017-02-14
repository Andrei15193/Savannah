using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class InsertObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public async Task TestInsertNewObject(string partitionKey, string rowKey)
        {
            var inserOperation = new InsertObjectStoreOperation(new { PartitionKey = partitionKey, RowKey = rowKey });

            await inserOperation.ExecuteAsync(null, Context);

            Assert.AreEqual(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
                + $"<{ObjectStoreXmlNameTable.Object} "
                    + $"{ObjectStoreXmlNameTable.PartitionKey}=\"{partitionKey}\" "
                    + $"{ObjectStoreXmlNameTable.RowKey}=\"{rowKey}\" "
                    + $"{ObjectStoreXmlNameTable.Timestamp}=\"{Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)}\" "
                + "/>",
                Result,
                ignoreCase: false);
        }

        [TestMethod]
        public async Task TestTryingToInsertWithExistingStorageObjectThrowsException()
        {
            var inserOperation = new InsertObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });

            var existingStorageObject = new StorageObject(null, null, null);

            await AssertExtra.ThrowsExceptionAsync<InvalidOperationException>(
                () => inserOperation.ExecuteAsync(existingStorageObject, Context),
                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
        }
    }
}