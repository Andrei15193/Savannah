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
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

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
            var executionContext = new ObjectStoreOperationExectionContext(existingStorageObject, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => inserOperation.GetStorageObjectFrom(executionContext),
                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
        }

        [TestMethod]
        public void TestEchoingContentPlacesSameObjectIntoResult()
        {
            var @object = new { PartitionKey = string.Empty, RowKey = string.Empty };
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, result);
            var inserOperation = new InsertObjectStoreOperation(@object, echoContent: true);

            inserOperation.GetStorageObjectFrom(executionContext);

            var actualObject = result.Single();
            Assert.AreSame(@object, actualObject);
        }

        private sealed class TestObject
        {
            public string PartitionKey { get; set; }

            public string RowKey { get; set; }

            public DateTime Timestamp { get; set; }
        }

        [TestMethod]
        public void TestEchoingContentUpdatesObjectTimestampProperty()
        {
            var timestamp = DateTime.UtcNow;
            var @object = new TestObject { PartitionKey = string.Empty, RowKey = string.Empty };
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, timestamp, result);
            var inserOperation = new InsertObjectStoreOperation(@object, echoContent: true);

            inserOperation.GetStorageObjectFrom(executionContext);

            var actualObject = (TestObject)result.Single();
            Assert.AreEqual(timestamp, actualObject.Timestamp);
        }
    }
}