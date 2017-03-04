using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectPropertyTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectPropertySetsSamePropertyName()
        {
            var row = GetRow<KeyValuesRow>();
            var propertyName = row.Value;

            var storageObjectProperty = new StorageObjectProperty(propertyName, null, default(ValueType));

            Assert.AreSame(propertyName, storageObjectProperty.Name);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectPropertySetsSamePropertyValue()
        {
            var row = GetRow<KeyValuesRow>();
            var propertyValue = row.Value;

            var storageObjectProperty = new StorageObjectProperty(null, propertyValue, default(ValueType));

            Assert.AreSame(propertyValue, storageObjectProperty.Value);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValueTypesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectPropertySetsSamePropertyType()
        {
            var row = GetRow<ValueTypesRow>();
            var propertyType = (ValueType)Enum.Parse(typeof(ValueType), row.Value);

            var storageObjectProperty = new StorageObjectProperty(null, null, propertyType);

            Assert.AreEqual(propertyType, storageObjectProperty.Type);
        }
    }
}