using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectPropertyTests
    {
        private string _PropertyName { get; set; }

        private string _PropertyValue { get; set; }

        private ValueType _PropertyType { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _PropertyName = Guid.NewGuid().ToString();
            _PropertyValue = Guid.NewGuid().ToString();
            _PropertyType = default(ValueType);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _PropertyType = default(ValueType);
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
        [DataRow((ValueType)(-1))]
        [DataRow(ValueType.String)]
        [DataRow(ValueType.Binary)]
        [DataRow(ValueType.Boolean)]
        [DataRow(ValueType.DateTime)]
        [DataRow(ValueType.Double)]
        [DataRow(ValueType.Guid)]
        [DataRow(ValueType.Int)]
        [DataRow(ValueType.Long)]
        public void TestStorageObjectPropertySetsSamePropertyType(object propertyType)
        {
            var storageObjectProperty = new StorageObjectProperty(_PropertyName, _PropertyValue, (ValueType)propertyType);

            Assert.AreEqual(propertyType, storageObjectProperty.Type);
        }
    }
}