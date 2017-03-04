using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Tests.Mocks;
using Savannah.Xml;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectFactoryTests
        : UnitTest
    {
        private static ObjectFactory<TestObject> _Factory { get; } = new ObjectFactory<TestObject>();

        private class TestObject
        {
            public string PartitionKey { get; set; }

            public string RowKey { get; set; }

            public DateTime Timestamp { get; set; }

            public byte[] BinaryProperty { get; set; }

            public bool BooleanProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public double DoubleProperty { get; set; }

            public Guid GuidProperty { get; set; }

            public int IntProperty { get; set; }

            public long LongProperty { get; set; }

            public string StringProperty { get; set; }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestPartitionKeyIsSetAccordingly()
        {
            var row = GetRow<KeyValuesRow>();

            var storageObject = new StorageObject(row.Value, null, null);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreSame(row.Value, @object.PartitionKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRowKeyIsSetAccordingly()
        {
            var row = GetRow<KeyValuesRow>();

            var storageObject = new StorageObject(null, row.Value, null);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreSame(row.Value, @object.RowKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestTimestampIsSetAccordingly()
        {
            var row = GetRow<DateTimeValuesRow>();
            var timestamp = row.Value;
            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            switch (dateTimeKind)
            {
                case DateTimeKind.Unspecified:
                    timestamp = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second, timestamp.Millisecond, DateTimeKind.Unspecified);
                    break;

                case DateTimeKind.Utc:
                    timestamp = timestamp.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    timestamp = timestamp.ToLocalTime();
                    break;
            }

            var storageObject = new StorageObject(null, null, timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture));

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(timestamp, @object.Timestamp);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestBinaryPropertyIsSetAccordingly()
        {
            var row = GetRow<BinaryValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.BinaryProperty),
                row.StringValue,
                ValueType.Binary);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            if (row.Value == null)
                Assert.IsNull(@object.BinaryProperty);
            else
                Assert.IsTrue(row.Value.SequenceEqual(@object.BinaryProperty));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestBooleanPropertyIsSetAccordingly()
        {
            var row = GetRow<BooleanValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.BooleanProperty),
                row.StringValue,
                ValueType.Boolean);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.BooleanProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestDateTimePropertyIsSetAccordingly()
        {
            var row = GetRow<DateTimeValuesRow>();

            var dateTime = row.Value;
            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            switch (dateTimeKind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Unspecified);
                    break;

                case DateTimeKind.Utc:
                    dateTime = dateTime.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    dateTime = dateTime.ToLocalTime();
                    break;
            }

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.DateTimeProperty),
                dateTime.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                ValueType.DateTime);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(dateTime, @object.DateTimeProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestDateTimeThatWasProvidedThroughStorageObjectFactoryIsRetrievedExactlyTheSameThroughObjectFactory()
        {
            var row = GetRow<DateTimeValuesRow>();

            var dateTime = row.Value;
            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            switch (dateTimeKind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Unspecified);
                    break;

                case DateTimeKind.Utc:
                    dateTime = dateTime.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    dateTime = dateTime.ToLocalTime();
                    break;
            }

            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);
            var storageObject = storageObjectFactory.CreateFrom(new { DateTimeProperty = dateTime });

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(dateTime, @object.DateTimeProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestDoublePropertyIsSetAccordingly()
        {
            var row = GetRow<DoubleValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.DoubleProperty),
                row.StringValue,
                ValueType.Double);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.DoubleProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGuidPropertyIsSetAccordingly()
        {
            var row = GetRow<GuidValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.GuidProperty),
                row.StringValue,
                ValueType.Guid);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.GuidProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestIntPropertyIsSetAccordingly()
        {
            var row = GetRow<IntValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.IntProperty),
                row.StringValue,
                ValueType.Int);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.IntProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLongPropertyIsSetAccordingly()
        {
            var row = GetRow<LongValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.LongProperty),
                row.StringValue,
                ValueType.Long);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.LongProperty);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStringPropertyIsSetAccordingly()
        {
            var row = GetRow<StringValuesRow>();

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.StringProperty),
                row.Value,
                ValueType.String);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            var @object = _Factory.CreateFrom(storageObject);

            Assert.AreEqual(row.Value, @object.StringProperty, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestProjectingStorageObjectToDifferentObject()
        {
            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.StringProperty),
                "stringValue",
                ValueType.String);
            var storageObject = new StorageObject("partitionKey", "rowKey", null, storageProperty);

            var factory = new ObjectFactory<MockObject>();
            var @object = factory.CreateFrom(storageObject);

            Assert.AreSame("partitionKey", @object.PartitionKey);
            Assert.AreSame("rowKey", @object.RowKey);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToSetPropertyOfDifferentTypeThrowsException()
        {
            var stringValue = "stringValue";

            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.IntProperty),
                stringValue,
                ValueType.String);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => _Factory.CreateFrom(storageObject),
                "Property type mismatch. Cannot set System.String to property of type System.Int32.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToSetNullToPropertyOfValueTypeThrowsException()
        {
            var storageProperty = new StorageObjectProperty(
                nameof(TestObject.IntProperty),
                null,
                ValueType.String);
            var storageObject = new StorageObject(null, null, null, storageProperty);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => _Factory.CreateFrom(storageObject),
                "Cannot set null to property of value type.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PropertyNamesToProjectTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestFactoryOnlyRetrievesSpecifiedProperties()
        {
            var row = GetRow<PropertyNamesToProjectRow>();

            var propertiesToRetrieve = row.PropertyNames.Split(',');

            var partitionKey = string.Empty;
            var rowKey = string.Empty;
            var timestamp = DateTime.UtcNow;
            var testObject = new TestObject
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Timestamp = timestamp,
                BinaryProperty = new byte[0],
                BooleanProperty = true,
                DateTimeProperty = timestamp,
                DoubleProperty = 1,
                GuidProperty = Guid.NewGuid(),
                IntProperty = 1,
                LongProperty = 1,
                StringProperty = string.Empty
            };
            var storageObjectFactory = new StorageObjectFactory(timestamp);
            var storageObject = storageObjectFactory.CreateFrom(testObject);

            var actualTestObject = _Factory.CreateFrom(storageObject, propertiesToRetrieve);

            foreach (var property in typeof(TestObject).GetRuntimeProperties())
            {
                var propertyValue = property.GetValue(actualTestObject);
                var propertyDefaultValue = (property.PropertyType.GetTypeInfo().IsValueType ? Activator.CreateInstance(property.PropertyType) : null);
                if (propertiesToRetrieve.Contains(property.Name, StringComparer.Ordinal))
                    Assert.AreNotEqual(propertyDefaultValue, propertyValue);
                else
                    Assert.AreEqual(propertyDefaultValue, propertyValue);
            }
        }
    }
}