using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectTests
    {
        private string _PartitionKey { get; set; }

        private string _RowKey { get; set; }

        private string _Timestamp { get; set; }

        private IEnumerable<StorageObjectProperty> _Properties { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _PartitionKey = Guid.NewGuid().ToString();
            _RowKey = Guid.NewGuid().ToString();
            _Timestamp = Guid.NewGuid().ToString();
            _Properties = Enumerable.Empty<StorageObjectProperty>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _Properties = null;
            _Timestamp = null;
            _RowKey = null;
            _PartitionKey = null;
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("test")]
        [DataRow("test value")]
        public void TestStorageObjectSetsSamePartitionKey(string partitionKey)
        {
            var storageObject = new StorageObject(partitionKey, _RowKey, _Timestamp, _Properties);

            Assert.AreSame(partitionKey, storageObject.PartitionKey);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("test")]
        [DataRow("test value")]
        public void TestStorageObjectSetsSameRowKey(string rowKey)
        {
            var storageObject = new StorageObject(_PartitionKey, rowKey, _Timestamp, _Properties);

            Assert.AreSame(rowKey, storageObject.RowKey);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("test")]
        [DataRow("test value")]
        public void TestStorageObjectSetsSameTimestamp(string timestamp)
        {
            var storageObject = new StorageObject(_PartitionKey, _RowKey, timestamp, _Properties);

            Assert.AreSame(timestamp, storageObject.Timestamp);
        }

        [DataTestMethod]
        [DataRow(default(string[]))]
        [DataRow(new string[0])]
        [DataRow(new string[] { null })]
        [DataRow(new[] { "test" })]
        [DataRow(new[] { "test value" })]
        [DataRow(new[] { "test", "test value" })]
        public void TestStorageObjectSetsSameProperties(string[] propertyValues)
        {
            IEnumerable<StorageObjectProperty> properties;
            if (propertyValues == null)
                properties = Enumerable.Empty<StorageObjectProperty>();
            else
                properties = propertyValues
                    .Select(propertyValue => (propertyValue == null ? null : new StorageObjectProperty("test property name", propertyValue, StorageObjectPropertyType.String)))
                    .ToList();

            var storageObject = new StorageObject(_PartitionKey, _RowKey, _Timestamp, properties);

            Assert.AreSame(properties, storageObject.Properties);
        }

        [TestMethod]
        public void TestStorageObjectSetsEmptyCollectionForNullProperties()
        {
            var storageObject = new StorageObject(_PartitionKey, _RowKey, _Timestamp, null);

            Assert.IsFalse(storageObject.Properties.Any());
        }
    }
}