using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class StorageObjectFactoryTests
    {
        [DataTestMethod]
        [DataRow("partitionKey")]
        [DataRow(default(string))]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]
        public void TestStorageObjectHasPartitionKeySetAccordingly(string partitionKey)
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(new { PartitionKey = partitionKey });

            Assert.AreSame(partitionKey, storageObject.PartitionKey);
        }

        [DataTestMethod]
        [DataRow("rowKey")]
        [DataRow(default(string))]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]
        public void TestStorageObjectHasRowKeySetAccordingly(string rowKey)
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(new { RowKey = rowKey });

            Assert.AreSame(rowKey, storageObject.RowKey);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc, "0001/01/01 00:00:00:0000000Z")]
        [DataRow(2015, 1, 22, 1, 4, 7, 10, DateTimeKind.Utc, "2015/01/22 01:04:07:0100000Z")]
        [DataRow(2016, 2, 23, 2, 5, 8, 11, DateTimeKind.Local, "2016/02/23 02:05:08:0110000")]
        [DataRow(2017, 3, 24, 3, 6, 9, 12, DateTimeKind.Unspecified, "2017/03/24 03:06:09:0120000")]
        public void TestStorageObjectHasTimestampSetAccordingly(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, string expectedTimestamp)
        {
            var timestamp = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var storageObjectFactory = new StorageObjectFactory(timestamp);
            if (dateTimeKind == DateTimeKind.Local)
                // The value for 'K' depends on the *local* machine's offset to UTC.
                // https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#KSpecifier
                // https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#zzzSpecifier
                expectedTimestamp += timestamp.ToString("%K");

            var storageObject = storageObjectFactory.CreateFrom(new object());

            Assert.AreEqual(expectedTimestamp, storageObject.Timestamp, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(2015, 1, 22, 1, 4, 7, 10, DateTimeKind.Utc)]
        [DataRow(2016, 2, 23, 2, 5, 8, 11, DateTimeKind.Local)]
        [DataRow(2017, 3, 24, 3, 6, 9, 12, DateTimeKind.Unspecified)]
        public void TestStorageObjectIgnoresTimestampProperty(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind)
        {
            var timestamp = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var timestampPropertyValue = timestamp.AddDays(1);
            var storageObjectFactory = new StorageObjectFactory(timestamp);

            var storageObject = storageObjectFactory.CreateFrom(new { Timestamp = timestampPropertyValue });

            Assert.AreNotEqual(storageObject.Timestamp, timestampPropertyValue);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemStringPropertyByName()
        {
            var @object = new { StringProperty = default(string) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.StringProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemStringPropertyType()
        {
            var @object = new { StringProperty = default(string) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.String, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow("value")]
        [DataRow("test")]
        [DataRow("")]
        [DataRow(default(string))]
        public void TestStorageObjectMapsNonSystemStringPropertyValue(string value)
        {
            var @object = new { StringProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(value, storageObjectProperty.Value);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemBinaryPropertyByName()
        {
            var @object = new { BinaryProperty = default(byte[]) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.BinaryProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemBinaryPropertyType()
        {
            var @object = new { BinaryProperty = default(byte[]) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Binary, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(new byte[] { 0xFF, 0xAB }, "FFAB")]
        [DataRow(new byte[] { 0x0F, 0xAB }, "0FAB")]
        [DataRow(new byte[0], "")]
        [DataRow(default(byte[]), default(string))]
        public void TestStorageObjectMapsNonSystemBinaryPropertyValue(byte[] value, string expectedPropertyValue)
        {
            var @object = new { BinaryProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemBooleanPropertyByName()
        {
            var @object = new { BooleanProperty = default(bool) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.BooleanProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemBooleanPropertyType()
        {
            var @object = new { BooleanProperty = default(bool) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Boolean, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(true, "true")]
        [DataRow(false, "false")]
        public void TestStorageObjectMapsNonSystemBooleanPropertyValue(bool value, string expectedPropertyValue)
        {
            var @object = new { BooleanProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemDateTimePropertyByName()
        {
            var @object = new { DateTimeProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.DateTimeProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemDateTimePropertyType()
        {
            var @object = new { DateTimeProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.DateTime, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc, "0001/01/01 00:00:00:0000000Z")]
        [DataRow(2015, 1, 22, 1, 4, 7, 10, DateTimeKind.Utc, "2015/01/22 01:04:07:0100000Z")]
        [DataRow(2016, 2, 23, 2, 5, 8, 11, DateTimeKind.Local, "2016/02/23 02:05:08:0110000")]
        [DataRow(2017, 3, 24, 3, 6, 9, 12, DateTimeKind.Unspecified, "2017/03/24 03:06:09:0120000")]
        public void TestStorageObjectMapsNonSystemDateTimePropertyValue(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, string expectedPropertyValue)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var @object = new { DateTimeProperty = value };
            if (dateTimeKind == DateTimeKind.Local)
                // The value for 'K' depends on the *local* machine's offset to UTC.
                // https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#KSpecifier
                // https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#zzzSpecifier
                expectedPropertyValue += value.ToString("%K");
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemDoublePropertyByName()
        {
            var @object = new { DoubleProperty = default(DateTime) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.DoubleProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemDoublePropertyType()
        {
            var @object = new { DoubleProperty = default(double) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Double, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(1D, "1")]
        [DataRow(1.1D, "1.1")]
        [DataRow(-1D, "-1")]
        [DataRow(-1.1D, "-1.1")]
        [DataRow(0D, "0")]
        public void TestStorageObjectMapsNonSystemDoublePropertyValue(double value, string expectedPropertyValue)
        {
            var @object = new { DoubleProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemGuidPropertyByName()
        {
            var @object = new { GuidProperty = default(Guid) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.GuidProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemGuidPropertyType()
        {
            var @object = new { GuidProperty = default(Guid) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Guid, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(new byte[16] { 0x60, 0x0C, 0xD3, 0x02, 0xE9, 0x18, 0xB2, 0x4F, 0xBB, 0x64, 0xBE, 0xD1, 0x7B, 0x8E, 0x08, 0x1B }, "02d30c60-18e9-4fb2-bb64-bed17b8e081b")]
        [DataRow(new byte[16] { 0x41, 0x95, 0x36, 0xC7, 0x85, 0xDD, 0xC0, 0x4B, 0xB1, 0x7B, 0x3A, 0xA5, 0x3A, 0xB9, 0xEF, 0x8D }, "c7369541-dd85-4bc0-b17b-3aa53ab9ef8d")]
        [DataRow(new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "00000000-0000-0000-0000-000000000000")]
        public void TestStorageObjectMapsNonSystemGuidPropertyValue(byte[] guidBytes, string expectedPropertyValue)
        {
            var value = new Guid(guidBytes);
            var @object = new { GuidProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemIntPropertyByName()
        {
            var @object = new { IntProperty = default(int) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.IntProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemIntPropertyType()
        {
            var @object = new { IntProperty = default(int) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Int, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(1, "1")]
        [DataRow(-1, "-1")]
        [DataRow(2147483647, "2147483647")]
        [DataRow(-2147483648, "-2147483648")]
        [DataRow(0, "0")]
        public void TestStorageObjectMapsNonSystemIntPropertyValue(int value, string expectedPropertyValue)
        {
            var @object = new { IntProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemLongPropertyByName()
        {
            var @object = new { LongProperty = default(long) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(nameof(@object.LongProperty), storageObjectProperty.Name);
        }

        [TestMethod]
        public void TestStorageObjectMapsNonSystemLongPropertyType()
        {
            var @object = new { LongProperty = default(long) };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(ValueType.Long, storageObjectProperty.Type);
        }

        [DataTestMethod]
        [DataRow(1L, "1")]
        [DataRow(-1L, "-1")]
        [DataRow(2147483647L, "2147483647")]
        [DataRow(-2147483648L, "-2147483648")]
        [DataRow(9223372036854775807, "9223372036854775807")]
        [DataRow(-9223372036854775808, "-9223372036854775808")]
        [DataRow(0L, "0")]
        public void TestStorageObjectMapsNonSystemLongPropertyValue(long value, string expectedPropertyValue)
        {
            var @object = new { LongProperty = value };
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            var storageObject = storageObjectFactory.CreateFrom(@object);

            var storageObjectProperty = storageObject.Properties.Single();
            Assert.AreEqual(expectedPropertyValue, storageObjectProperty.Value, ignoreCase: false);
        }

        [TestMethod]
        public void TestTryingToCreateFromNullObjectThrowsException()
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.Now);

            Assert.ThrowsException<ArgumentNullException>(() => storageObjectFactory.CreateFrom(null));
        }
    }
}