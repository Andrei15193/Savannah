﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Xml;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class ReplaceObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestReplacingAnExistingObject()
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

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToReplaceNonExistingObjectThrowsException()
        {
            var deleteOperation = new ReplaceObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => deleteOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be replaced.");
        }
    }
}