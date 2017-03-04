using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Xml;

namespace Savannah.Tests
{
    [TestClass]
    public class PropertyValueFactoryTests
        : UnitTest
    {
        private static PropertyValueFactory _Factory { get; } = new PropertyValueFactory();

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestBinaryPropertyIsConvertedAccordingly()
        {
            var row = GetRow<BinaryValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Binary);

            var result = (byte[])_Factory.GetPropertyValueFrom(storageObjectProperty);

            if (row.Value == null)
                Assert.IsNull(result);
            else
                Assert.IsTrue(row.Value.SequenceEqual(result));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestBooleanPropertyIsSetAccordingly()
        {
            var row = GetRow<BooleanValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Boolean);

            var result = (bool)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestDateTimePropertyIsSetAccordingly()
        {
            var row = GetRow<DateTimeValuesRow>();

            var dateTimeKind = (DateTimeKind)Enum.Parse(typeof(DateTimeKind), row.DateTimeKind);
            var expectedTimestamp = row.Value;
            switch (dateTimeKind)
            {
                case DateTimeKind.Unspecified:
                    expectedTimestamp = new DateTime(expectedTimestamp.Year, expectedTimestamp.Month, expectedTimestamp.Day, expectedTimestamp.Hour, expectedTimestamp.Minute, expectedTimestamp.Second, expectedTimestamp.Millisecond, dateTimeKind);
                    break;

                case DateTimeKind.Utc:
                    expectedTimestamp = expectedTimestamp.ToUniversalTime();
                    break;

                case DateTimeKind.Local:
                    expectedTimestamp = expectedTimestamp.ToLocalTime();
                    break;
            }

            var storageObjectProperty = new StorageObjectProperty(
                null,
                expectedTimestamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                ValueType.DateTime);

            var result = (DateTime)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedTimestamp, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestDoublePropertyIsSetAccordingly()
        {
            var row = GetRow<DoubleValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Double);

            var result = (double)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGuidPropertyIsSetAccordingly()
        {
            var row = GetRow<GuidValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Guid);

            var result = (Guid)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestIntPropertyIsSetAccordingly()
        {
            var row = GetRow<IntValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Int);

            var result = (int)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLongPropertyIsSetAccordingly()
        {
            var row = GetRow<LongValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.StringValue,
                ValueType.Long);

            var result = (long)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestStringPropertyIsSetAccordingly()
        {
            var row = GetRow<StringValuesRow>();

            var storageObjectProperty = new StorageObjectProperty(
                null,
                row.Value,
                ValueType.String);
            var storageObject = new StorageObject(null, null, null, storageObjectProperty);

            var result = (string)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(row.Value, result, ignoreCase: false);
        }
    }
}