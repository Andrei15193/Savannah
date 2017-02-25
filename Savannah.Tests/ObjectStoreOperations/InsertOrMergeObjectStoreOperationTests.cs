using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;
using Savannah.Xml;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class InsertOrMergeObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestInsertNewObject(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var inserOrMergeOperation = new InsertOrMergeObjectStoreOperation(@object);
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            var storageObject = inserOrMergeOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new { @object.PartitionKey, @object.RowKey, Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture) },
                new { storageObject.PartitionKey, storageObject.RowKey, storageObject.Timestamp });
        }

        [TestMethod]
        public void TestInsertingWithExistingObjectReturnsAMergedObject()
        {
            var inserOrMergeOperation = new InsertOrMergeObjectStoreOperation(
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Property1 = string.Empty,
                    Property3 = "newValue"
                });
            var existingStorageObject = new StorageObject(
                string.Empty,
                string.Empty,
                string.Empty,
                new StorageObjectProperty(
                    "Property2",
                    string.Empty,
                    ValueType.String),
                new StorageObjectProperty(
                    "Property3",
                    "oldValue",
                    ValueType.String));
            var executionContext = new ObjectStoreOperationExectionContext(existingStorageObject, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            var result = inserOrMergeOperation.GetStorageObjectFrom(executionContext);

            Assert.AreEqual(
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Timestamp = Timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                    Property1 = string.Empty,
                    Property2 = string.Empty,
                    Property3 = "newValue"
                },
                new
                {
                    result.PartitionKey,
                    result.RowKey,
                    result.Timestamp,
                    Property1 = result.Properties.Single(property => property.Name == "Property1").Value,
                    Property2 = result.Properties.Single(property => property.Name == "Property2").Value,
                    Property3 = result.Properties.Single(property => property.Name == "Property3").Value
                });
        }
    }
}