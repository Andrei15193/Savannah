using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectSetsSamePartitionKey()
        {
            var row = GetRow<KeyValuesRow>();
            var partitionKey = row.Value;

            var storageObject = new StorageObject(partitionKey, null, null);

            Assert.AreSame(partitionKey, storageObject.PartitionKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectSetsSameRowKey()
        {
            var row = GetRow<KeyValuesRow>();
            var rowKey = row.Value;

            var storageObject = new StorageObject(null, rowKey, null);

            Assert.AreSame(rowKey, storageObject.RowKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectSetsSameTimestamp()
        {
            var row = GetRow<KeyValuesRow>();
            var timestamp = row.Value;

            var storageObject = new StorageObject(null, null, timestamp);

            Assert.AreSame(timestamp, storageObject.Timestamp);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PropertyCountsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectSetsSameProperties()
        {
            var row = GetRow<PropertyCountsRow>();
            var properties = Enumerable
                .Range(0, row.Value)
                .Select(rowIndex => new StorageObjectProperty(rowIndex.ToString(), rowIndex.ToString(), ValueType.Int))
                .ToList();

            var storageObject = new StorageObject(null, null, null, properties);

            Assert.AreSame(properties, storageObject.Properties);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectSetsEmptyPropertiesCollectionForNull()
        {
            IEnumerable<StorageObjectProperty> properties = null;

            var storageObject = new StorageObject(null, null, null, properties);

            Assert.IsFalse(storageObject.Properties.Any());
        }
    }
}