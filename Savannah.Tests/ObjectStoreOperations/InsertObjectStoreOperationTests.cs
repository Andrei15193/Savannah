﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void TestInsertNewObject(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var inserOperation = new InsertObjectStoreOperation(@object);
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, new List<object>());

            var storageObject = inserOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new { @object.PartitionKey, @object.RowKey, Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture) },
                new { storageObject.PartitionKey, storageObject.RowKey, storageObject.Timestamp });
        }

        [TestMethod]
        public void TestTryingToInsertWithExistingStorageObjectThrowsException()
        {
            var inserOperation = new InsertObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var existingStorageObject = new StorageObject(null, null, null);
            var executionContext = new ObjectStoreOperationExectionContext(existingStorageObject, StorageObjectFactory, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => inserOperation.GetStorageObjectFrom(executionContext),
                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
        }
    }
}