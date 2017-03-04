using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Xml;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class InsertOrReplaceObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestInsertNewObject()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());
            var inserOrReplaceOperation = new InsertOrReplaceObjectStoreOperation(@object);

            var storageObject = inserOrReplaceOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new { @object.PartitionKey, @object.RowKey, Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture) },
                new { storageObject.PartitionKey, storageObject.RowKey, storageObject.Timestamp });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestInsertingObjectWhileOneAlreadyExistsReplacesIt()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
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