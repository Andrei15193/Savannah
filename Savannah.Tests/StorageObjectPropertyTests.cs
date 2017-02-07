using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectPropertyTests
    {
        private string _PropertyName { get; set; }

        private string _PropertyValue { get; set; }

        private StorageObjectPropertyType _PropertyType { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _PropertyName = Guid.NewGuid().ToString();
            _PropertyValue = Guid.NewGuid().ToString();
            _PropertyType = default(StorageObjectPropertyType);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _PropertyType = default(StorageObjectPropertyType);
            _PropertyValue = Guid.NewGuid().ToString();
            _PropertyName = Guid.NewGuid().ToString();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("test")]
        [DataRow("test value")]
        public void TestStorageObjectPropertySetsSamePropertyName(string propertyName)
        {
            var storageObjectProperty = new StorageObjectProperty(propertyName, _PropertyValue, _PropertyType);

            Assert.AreSame(propertyName, storageObjectProperty.Name);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("test")]
        [DataRow("test value")]
        public void TestStorageObjectPropertySetsSamePropertyValue(string propertyValue)
        {
            var storageObjectProperty = new StorageObjectProperty(_PropertyName, propertyValue, _PropertyType);

            Assert.AreSame(propertyValue, storageObjectProperty.Value);
        }

        [DataTestMethod]
        [DataRow((StorageObjectPropertyType)(-1))]
        [DataRow(StorageObjectPropertyType.String)]
        [DataRow(StorageObjectPropertyType.Binary)]
        [DataRow(StorageObjectPropertyType.Boolean)]
        [DataRow(StorageObjectPropertyType.DateTime)]
        [DataRow(StorageObjectPropertyType.Double)]
        [DataRow(StorageObjectPropertyType.Guid)]
        [DataRow(StorageObjectPropertyType.Int)]
        [DataRow(StorageObjectPropertyType.Long)]
        public void TestStorageObjectPropertySetsSamePropertyType(object propertyType)
        {
            var storageObjectProperty = new StorageObjectProperty(_PropertyName, _PropertyValue, (StorageObjectPropertyType)propertyType);

            Assert.AreEqual(propertyType, storageObjectProperty.Type);
        }
    }
}