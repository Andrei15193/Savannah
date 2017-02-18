using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class InsertOrReplaceObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestInsertNewObject(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());
            var inserOrReplaceOperation = new InsertOrReplaceObjectStoreOperation(@object);

            var storageObject = inserOrReplaceOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new { @object.PartitionKey, @object.RowKey, Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture) },
                new { storageObject.PartitionKey, storageObject.RowKey, storageObject.Timestamp });
        }

        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestInsertingObjectWhileOneAlreadyExistsReplacesIt(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var existingStorageObject = new StorageObject(null, null, null);
            var executionContext = new ObjectStoreOperationExectionContext(existingStorageObject, StorageObjectFactory, DateTime.UtcNow, new List<object>());
            var inserOrReplaceOperation = new InsertOrReplaceObjectStoreOperation(@object);

            var storageObject = inserOrReplaceOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new { @object.PartitionKey, @object.RowKey, Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture) },
                new { storageObject.PartitionKey, storageObject.RowKey, storageObject.Timestamp });
        }
    }
}