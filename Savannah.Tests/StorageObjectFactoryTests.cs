using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Xml;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectFactoryTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectHasPartitionKeySetAccordingly()
        {
            var row = GetRow<StringValuesRow>();

            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(new { PartitionKey = row.Value });

            Assert.AreSame(row.Value, storageObject.PartitionKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectHasRowKeySetAccordingly()
        {
            var row = GetRow<StringValuesRow>();

            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(new { RowKey = row.Value });

            Assert.AreSame(row.Value, storageObject.RowKey);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectHasTimestampSetAccordingly()
        {
            var row = GetRow<DateTimeValuesRow>();

            var timestamp = row.Value;
            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    timestamp = timestamp.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    timestamp = timestamp.ToLocalTime();
                    break;

                case DateTimeKind.Unspecified:
                    timestamp = timestamp.ToUniversalTime();
                    timestamp = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second, DateTimeKind.Unspecified);
                    break;
            }
            var expectedTimestamp = timestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture);

            var storageObjectFactory = new StorageObjectFactory(timestamp);

            var storageObject = storageObjectFactory.CreateFrom(new object());

            Assert.AreEqual(expectedTimestamp, storageObject.Timestamp, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectIgnoresTimestampProperty()
        {
            var row = GetRow<DateTimeValuesRow>();
            var timestamp = row.Value;
            var timestampPropertyValue = timestamp.AddDays(1);
            var storageObjectFactory = new StorageObjectFactory(timestamp);

            var storageObject = storageObjectFactory.CreateFrom(new { Timestamp = timestampPropertyValue });

            Assert.AreNotEqual(
                storageObject.Timestamp,
                timestampPropertyValue.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemStringPropertyByName()
        {
            var @object = new { StringProperty = default(string) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.StringProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemStringPropertyType()
        {
            var @object = new { StringProperty = default(string) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.String, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemStringPropertyValue()
        {
            var row = GetRow<StringValuesRow>();

            var @object = new { StringProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.Value, storageObjectProperty.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBinaryPropertyByName()
        {
            var @object = new { BinaryProperty = default(byte[]) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.BinaryProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBinaryPropertyType()
        {
            var @object = new { BinaryProperty = default(byte[]) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Binary, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBinaryPropertyValue()
        {
            var row = GetRow<BinaryValuesRow>();

            var @object = new { BinaryProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBooleanPropertyByName()
        {
            var @object = new { BooleanProperty = default(bool) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.BooleanProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBooleanPropertyType()
        {
            var @object = new { BooleanProperty = default(bool) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Boolean, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemBooleanPropertyValue()
        {
            var row = GetRow<BooleanValuesRow>();

            var @object = new { BooleanProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDateTimePropertyByName()
        {
            var @object = new { DateTimeProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.DateTimeProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDateTimePropertyType()
        {
            var @object = new { DateTimeProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.DateTime, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDateTimePropertyValue()
        {
            var row = GetRow<DateTimeValuesRow>();

            var dateTime = row.Value;
            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    dateTime = dateTime.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    dateTime = dateTime.ToLocalTime();
                    break;

                case DateTimeKind.Unspecified:
                    dateTime = dateTime.ToUniversalTime();
                    dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Unspecified);
                    break;
            }
            var expectedValue = dateTime.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture);

            var @object = new { DateTimeProperty = dateTime };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDoublePropertyByName()
        {
            var @object = new { DoubleProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.DoubleProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDoublePropertyType()
        {
            var @object = new { DoubleProperty = default(double) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Double, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemDoublePropertyValue()
        {
            var row = GetRow<DoubleValuesRow>();

            var @object = new { DoubleProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemGuidPropertyByName()
        {
            var @object = new { GuidProperty = default(Guid) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.GuidProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemGuidPropertyType()
        {
            var @object = new { GuidProperty = default(Guid) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Guid, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemGuidPropertyValue()
        {
            var row = GetRow<GuidValuesRow>();

            var @object = new { GuidProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemIntPropertyByName()
        {
            var @object = new { IntProperty = default(int) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.IntProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemIntPropertyType()
        {
            var @object = new { IntProperty = default(int) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Int, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemIntPropertyValue()
        {
            var row = GetRow<IntValuesRow>();

            var @object = new { IntProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemLongPropertyByName()
        {
            var @object = new { LongProperty = default(long) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.LongProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemLongPropertyType()
        {
            var @object = new { LongProperty = default(long) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Long, storageObjectProperty.Type);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectMapsNonSystemLongPropertyValue()
        {
            var row = GetRow<LongValuesRow>();

            var @object = new { LongProperty = row.Value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(row.StringValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public void TestTryingToCreateFromNullObjectThrowsException()
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            storageObjectFactory.CreateFrom(null);
        }
    }
}