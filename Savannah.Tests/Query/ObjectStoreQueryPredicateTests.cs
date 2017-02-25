using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Query;

namespace Savannah.Tests.Query
{
    [TestClass]
    public class ObjectStoreQueryPredicateTests
    {
        [TestMethod]
        public void TestFilterWithNoPartitionKeySetsNullForPartitionKeys()
        {
            var filter = ObjectStoreQueryFilter.Equal("RowKey", "value");

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsNull(predicate.PartitionKeys);
        }

        [TestMethod]
        public void TestFilterWithNoRowKeySetsNullForRowKeys()
        {
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", "value");

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsNull(predicate.RowKeys);
        }

        [TestMethod]
        public void TestGetPartitionKeyFromSimpleFilterOnPartitionKey()
        {
            var partitionKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue);

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(partitionKeyValue, predicate.PartitionKeys.Single());
        }

        [TestMethod]
        public void TestGetPartitionKeyFromComplexFilterOnPartitionKey()
        {
            var partitionKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue)
                .And(ObjectStoreQueryFilter.Equal("RowKey", "testValue"));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(partitionKeyValue, predicate.PartitionKeys.Single());
        }

        [TestMethod]
        public void TestGetPartitionKeysFromComplexFilterOnPartitionKey()
        {
            var partitionKeyValue1 = "value1";
            var partitionKeyValue2 = "value2";
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue1)
                .And(ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue2));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsTrue(
                new[]
                {
                    partitionKeyValue1,
                    partitionKeyValue2
                }.OrderBy(value => value)
                .SequenceEqual(predicate
                    .PartitionKeys
                    .Select(value => value)
                    .OrderBy(value => value)));
        }

        [TestMethod]
        public void TestGetRowKeyFromSimpleFilterOnRowKey()
        {
            var rowKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue);

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(rowKeyValue, predicate.RowKeys.Single());
        }

        [TestMethod]
        public void TestGetRowKeyFromComplexFilterOnRowKey()
        {
            var rowKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue)
                .And(ObjectStoreQueryFilter.Equal("PartitionKey", "testValue"));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(rowKeyValue, predicate.RowKeys.Single());
        }

        [TestMethod]
        public void TestGetRowKeysFromComplexFilterOnRowKey()
        {
            var rowKeyValue1 = "value1";
            var rowKeyValue2 = "value2";
            var filter = ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue1)
                .And(ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue2));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsTrue(
                new[]
                {
                    rowKeyValue1,
                    rowKeyValue2
                }.OrderBy(value => value)
                .SequenceEqual(predicate
                    .RowKeys
                    .Select(value => value)
                    .OrderBy(value => value)));
        }

        [DataTestMethod]
        [DataRow("00", new byte[] { 0x00 }, true)]
        [DataRow("0", new byte[] { 0x00 }, true)]
        [DataRow("01", new byte[] { 0x01 }, true)]
        [DataRow("FA", new byte[] { 0xFA }, true)]
        [DataRow("FA", new byte[] { 0xFB }, false)]
        [DataRow("0FA", new byte[] { 0xFA }, false)]
        [DataRow("0FA", default(byte[]), false)]
        [DataRow(default(string), new byte[] { 0xFA }, false)]
        public void TestEqualFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("true", true, true)]
        [DataRow("true", false, false)]
        [DataRow("false", true, false)]
        [DataRow("false", false, true)]
        public void TestEqualFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, true)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, true)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, true)]
        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        public void TestEqualFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, true)]
        [DataRow("2", ValueType.Int, 2D, true)]
        [DataRow("0", ValueType.Int, 0D, true)]
        [DataRow("-1", ValueType.Int, -1D, true)]
        [DataRow("1", ValueType.Int, -1D, false)]
        [DataRow("1", ValueType.Int, 2D, false)]

        [DataRow("1", ValueType.Long, 1D, true)]
        [DataRow("2", ValueType.Long, 2D, true)]
        [DataRow("0", ValueType.Long, 0D, true)]
        [DataRow("-1", ValueType.Long, -1D, true)]
        [DataRow("1", ValueType.Long, -1D, false)]
        [DataRow("1", ValueType.Long, 2D, false)]

        [DataRow("1", ValueType.Double, 1D, true)]
        [DataRow("2", ValueType.Double, 2D, true)]
        [DataRow("0", ValueType.Double, 0D, true)]
        [DataRow("-1", ValueType.Double, -1D, true)]
        [DataRow("1", ValueType.Double, -1D, false)]
        [DataRow("1", ValueType.Double, 2D, false)]
        public void TestEqualFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, true)]
        [DataRow("fd394eb7-e584-44be-b1c5-faf4980562dc", new byte[16] { 0x9C, 0x23, 0xCC, 0xED, 0x53, 0x12, 0x64, 0x49, 0x9A, 0xE3, 0xFB, 0x36, 0x74, 0x05, 0x26, 0x79 }, false)]
        [DataRow("86e85ca7-7ee7-4cb4-9772-a4e7ec98b491", new byte[16] { 0xD5, 0xAC, 0xA9, 0xE5, 0x25, 0x53, 0xEB, 0x48, 0xAC, 0xD9, 0x7D, 0xCB, 0xC2, 0xD2, 0x50, 0x82 }, false)]
        public void TestEqualFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, true)]
        [DataRow("2", ValueType.Int, 2, true)]
        [DataRow("0", ValueType.Int, 0, true)]
        [DataRow("-1", ValueType.Int, -1, true)]
        [DataRow("1", ValueType.Int, -1, false)]
        [DataRow("1", ValueType.Int, 2, false)]

        [DataRow("1", ValueType.Long, 1, true)]
        [DataRow("2", ValueType.Long, 2, true)]
        [DataRow("0", ValueType.Long, 0, true)]
        [DataRow("-1", ValueType.Long, -1, true)]
        [DataRow("1", ValueType.Long, -1, false)]
        [DataRow("1", ValueType.Long, 2, false)]

        [DataRow("1", ValueType.Double, 1, true)]
        [DataRow("2", ValueType.Double, 2, true)]
        [DataRow("0", ValueType.Double, 0, true)]
        [DataRow("-1", ValueType.Double, -1, true)]
        [DataRow("1", ValueType.Double, -1, false)]
        [DataRow("1", ValueType.Double, 2, false)]
        public void TestEqualFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, true)]
        [DataRow("2", ValueType.Int, 2L, true)]
        [DataRow("0", ValueType.Int, 0L, true)]
        [DataRow("-1", ValueType.Int, -1L, true)]
        [DataRow("1", ValueType.Int, -1L, false)]
        [DataRow("1", ValueType.Int, 2L, false)]

        [DataRow("1", ValueType.Long, 1L, true)]
        [DataRow("2", ValueType.Long, 2L, true)]
        [DataRow("0", ValueType.Long, 0L, true)]
        [DataRow("-1", ValueType.Long, -1L, true)]
        [DataRow("1", ValueType.Long, -1L, false)]
        [DataRow("1", ValueType.Long, 2L, false)]

        [DataRow("1", ValueType.Double, 1L, true)]
        [DataRow("2", ValueType.Double, 2L, true)]
        [DataRow("0", ValueType.Double, 0L, true)]
        [DataRow("-1", ValueType.Double, -1L, true)]
        [DataRow("1", ValueType.Double, -1L, false)]
        [DataRow("1", ValueType.Double, 2L, false)]
        public void TestEqualFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("test", "test", true)]
        [DataRow("test", "Test", false)]
        [DataRow("test", default(string), false)]
        [DataRow(default(string), "test", false)]
        public void TestEqualFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.Equal(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("00", new byte[] { 0x00 }, false)]
        [DataRow("0", new byte[] { 0x00 }, false)]
        [DataRow("01", new byte[] { 0x01 }, false)]
        [DataRow("FA", new byte[] { 0xFA }, false)]
        [DataRow("FA", new byte[] { 0xFB }, true)]
        [DataRow("0FA", new byte[] { 0xFA }, true)]
        [DataRow("0FA", default(byte[]), true)]
        [DataRow(default(string), new byte[] { 0xFA }, true)]
        public void TestNotEqualFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("true", true, false)]
        [DataRow("true", false, true)]
        [DataRow("false", true, true)]
        [DataRow("false", false, false)]
        public void TestNotEqualFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, false)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, false)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, false)]
        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        public void TestNotEqualFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, false)]
        [DataRow("2", ValueType.Int, 2D, false)]
        [DataRow("0", ValueType.Int, 0D, false)]
        [DataRow("-1", ValueType.Int, -1D, false)]
        [DataRow("1", ValueType.Int, -1D, true)]
        [DataRow("1", ValueType.Int, 2D, true)]

        [DataRow("1", ValueType.Long, 1D, false)]
        [DataRow("2", ValueType.Long, 2D, false)]
        [DataRow("0", ValueType.Long, 0D, false)]
        [DataRow("-1", ValueType.Long, -1D, false)]
        [DataRow("1", ValueType.Long, -1D, true)]
        [DataRow("1", ValueType.Long, 2D, true)]

        [DataRow("1", ValueType.Double, 1D, false)]
        [DataRow("2", ValueType.Double, 2D, false)]
        [DataRow("0", ValueType.Double, 0D, false)]
        [DataRow("-1", ValueType.Double, -1D, false)]
        [DataRow("1", ValueType.Double, -1D, true)]
        [DataRow("1", ValueType.Double, 2D, true)]
        public void TestNotEqualFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, false)]
        [DataRow("fd394eb7-e584-44be-b1c5-faf4980562dc", new byte[16] { 0x9C, 0x23, 0xCC, 0xED, 0x53, 0x12, 0x64, 0x49, 0x9A, 0xE3, 0xFB, 0x36, 0x74, 0x05, 0x26, 0x79 }, true)]
        [DataRow("86e85ca7-7ee7-4cb4-9772-a4e7ec98b491", new byte[16] { 0xD5, 0xAC, 0xA9, 0xE5, 0x25, 0x53, 0xEB, 0x48, 0xAC, 0xD9, 0x7D, 0xCB, 0xC2, 0xD2, 0x50, 0x82 }, true)]
        public void TestNotEqualFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, false)]
        [DataRow("2", ValueType.Int, 2, false)]
        [DataRow("0", ValueType.Int, 0, false)]
        [DataRow("-1", ValueType.Int, -1, false)]
        [DataRow("1", ValueType.Int, -1, true)]
        [DataRow("1", ValueType.Int, 2, true)]

        [DataRow("1", ValueType.Long, 1, false)]
        [DataRow("2", ValueType.Long, 2, false)]
        [DataRow("0", ValueType.Long, 0, false)]
        [DataRow("-1", ValueType.Long, -1, false)]
        [DataRow("1", ValueType.Long, -1, true)]
        [DataRow("1", ValueType.Long, 2, true)]

        [DataRow("1", ValueType.Double, 1, false)]
        [DataRow("2", ValueType.Double, 2, false)]
        [DataRow("0", ValueType.Double, 0, false)]
        [DataRow("-1", ValueType.Double, -1, false)]
        [DataRow("1", ValueType.Double, -1, true)]
        [DataRow("1", ValueType.Double, 2, true)]
        public void TestNotEqualFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, false)]
        [DataRow("2", ValueType.Int, 2L, false)]
        [DataRow("0", ValueType.Int, 0L, false)]
        [DataRow("-1", ValueType.Int, -1L, false)]
        [DataRow("1", ValueType.Int, -1L, true)]
        [DataRow("1", ValueType.Int, 2L, true)]

        [DataRow("1", ValueType.Long, 1L, false)]
        [DataRow("2", ValueType.Long, 2L, false)]
        [DataRow("0", ValueType.Long, 0L, false)]
        [DataRow("-1", ValueType.Long, -1L, false)]
        [DataRow("1", ValueType.Long, -1L, true)]
        [DataRow("1", ValueType.Long, 2L, true)]

        [DataRow("1", ValueType.Double, 1L, false)]
        [DataRow("2", ValueType.Double, 2L, false)]
        [DataRow("0", ValueType.Double, 0L, false)]
        [DataRow("-1", ValueType.Double, -1L, false)]
        [DataRow("1", ValueType.Double, -1L, true)]
        [DataRow("1", ValueType.Double, 2L, true)]
        public void TestNotEqualFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("test", "test", false)]
        [DataRow("test", "Test", true)]
        [DataRow("test", default(string), true)]
        [DataRow(default(string), "test", true)]
        public void TestNotEqualFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("01", new byte[] { 0x00 }, false)]
        [DataRow("01", new byte[] { 0x01 }, false)]
        [DataRow("01", new byte[] { 0x02 }, true)]
        [DataRow("0101", new byte[] { 0x01 }, false)]
        [DataRow("01", new byte[] { 0x01, 0x01 }, true)]
        public void TestLessThanFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("false", false, false)]
        [DataRow("true", false, false)]
        [DataRow("false", true, true)]
        [DataRow("false", false, false)]
        public void TestLessThanFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, false)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, false)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, false)]

        [DataRow("2015/01/22 02:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]
        [DataRow("2015/01/22 00:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]

        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 06:06:10:0140000+03:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        public void TestLessThanFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, false)]
        [DataRow("1", ValueType.Int, 0D, false)]
        [DataRow("1", ValueType.Int, 2D, true)]

        [DataRow("1", ValueType.Long, 1D, false)]
        [DataRow("1", ValueType.Long, 0D, false)]
        [DataRow("1", ValueType.Long, 2D, true)]

        [DataRow("1", ValueType.Double, 1D, false)]
        [DataRow("1", ValueType.Double, 0D, false)]
        [DataRow("1", ValueType.Double, 2D, true)]
        public void TestLessThanFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        [DataRow("00000000-0000-0000-0000-000000000000", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, false)]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, false)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        public void TestLessThanFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, false)]
        [DataRow("1", ValueType.Int, 0, false)]
        [DataRow("1", ValueType.Int, 2, true)]

        [DataRow("1", ValueType.Long, 1, false)]
        [DataRow("1", ValueType.Long, 0, false)]
        [DataRow("1", ValueType.Long, 2, true)]

        [DataRow("1", ValueType.Double, 1, false)]
        [DataRow("1", ValueType.Double, 0, false)]
        [DataRow("1", ValueType.Double, 2, true)]
        public void TestLessThanFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, false)]
        [DataRow("1", ValueType.Int, 0L, false)]
        [DataRow("1", ValueType.Int, 2L, true)]

        [DataRow("1", ValueType.Long, 1L, false)]
        [DataRow("1", ValueType.Long, 0L, false)]
        [DataRow("1", ValueType.Long, 2L, true)]

        [DataRow("1", ValueType.Double, 1L, false)]
        [DataRow("1", ValueType.Double, 0L, false)]
        [DataRow("1", ValueType.Double, 2L, true)]
        public void TestLessThanFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("str", "str", false)]
        [DataRow("str", "STR", false)]
        [DataRow("str1", "str11", true)]
        [DataRow("str11", "str1", false)]
        public void TestLessThanFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.LessThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("01", new byte[] { 0x00 }, false)]
        [DataRow("01", new byte[] { 0x01 }, true)]
        [DataRow("01", new byte[] { 0x02 }, true)]
        [DataRow("0101", new byte[] { 0x01 }, false)]
        [DataRow("01", new byte[] { 0x01, 0x01 }, true)]
        public void TestLessThanOrEqualFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("false", false, true)]
        [DataRow("true", false, false)]
        [DataRow("false", true, true)]
        [DataRow("false", false, true)]
        public void TestLessThanOrEqualFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, true)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, true)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, true)]

        [DataRow("2015/01/22 02:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]
        [DataRow("2015/01/22 00:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]

        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 06:06:10:0140000+03:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        public void TestLessThanOrEqualFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, true)]
        [DataRow("1", ValueType.Int, 0D, false)]
        [DataRow("1", ValueType.Int, 2D, true)]

        [DataRow("1", ValueType.Long, 1D, true)]
        [DataRow("1", ValueType.Long, 0D, false)]
        [DataRow("1", ValueType.Long, 2D, true)]

        [DataRow("1", ValueType.Double, 1D, true)]
        [DataRow("1", ValueType.Double, 0D, false)]
        [DataRow("1", ValueType.Double, 2D, true)]
        public void TestLessThanOrEqualFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        [DataRow("00000000-0000-0000-0000-000000000000", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, false)]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, false)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        public void TestLessThanOrEqualFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, true)]
        [DataRow("1", ValueType.Int, 0, false)]
        [DataRow("1", ValueType.Int, 2, true)]

        [DataRow("1", ValueType.Long, 1, true)]
        [DataRow("1", ValueType.Long, 0, false)]
        [DataRow("1", ValueType.Long, 2, true)]

        [DataRow("1", ValueType.Double, 1, true)]
        [DataRow("1", ValueType.Double, 0, false)]
        [DataRow("1", ValueType.Double, 2, true)]
        public void TestLessThanOrEqualFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, true)]
        [DataRow("1", ValueType.Int, 0L, false)]
        [DataRow("1", ValueType.Int, 2L, true)]

        [DataRow("1", ValueType.Long, 1L, true)]
        [DataRow("1", ValueType.Long, 0L, false)]
        [DataRow("1", ValueType.Long, 2L, true)]

        [DataRow("1", ValueType.Double, 1L, true)]
        [DataRow("1", ValueType.Double, 0L, false)]
        [DataRow("1", ValueType.Double, 2L, true)]
        public void TestLessThanOrEqualFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("str", "str", true)]
        [DataRow("str", "STR", false)]
        [DataRow("str1", "str11", true)]
        [DataRow("str11", "str1", false)]
        public void TestLessThanOrEqualFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("01", new byte[] { 0x00 }, true)]
        [DataRow("01", new byte[] { 0x01 }, false)]
        [DataRow("01", new byte[] { 0x02 }, false)]
        [DataRow("0101", new byte[] { 0x01 }, true)]
        [DataRow("01", new byte[] { 0x01, 0x01 }, false)]
        public void TestGreaterThanFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("false", false, false)]
        [DataRow("true", false, true)]
        [DataRow("false", true, false)]
        [DataRow("false", false, false)]
        public void TestGreaterThanFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, false)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, false)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, false)]

        [DataRow("2015/01/22 02:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]
        [DataRow("2015/01/22 00:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]

        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 06:06:10:0140000+03:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        public void TestGreaterThanFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, false)]
        [DataRow("1", ValueType.Int, 0D, true)]
        [DataRow("1", ValueType.Int, 2D, false)]

        [DataRow("1", ValueType.Long, 1D, false)]
        [DataRow("1", ValueType.Long, 0D, true)]
        [DataRow("1", ValueType.Long, 2D, false)]

        [DataRow("1", ValueType.Double, 1D, false)]
        [DataRow("1", ValueType.Double, 0D, true)]
        [DataRow("1", ValueType.Double, 2D, false)]
        public void TestGreaterThanFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        [DataRow("00000000-0000-0000-0000-000000000000", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, true)]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, true)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        public void TestGreaterThanFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, false)]
        [DataRow("1", ValueType.Int, 0, true)]
        [DataRow("1", ValueType.Int, 2, false)]

        [DataRow("1", ValueType.Long, 1, false)]
        [DataRow("1", ValueType.Long, 0, true)]
        [DataRow("1", ValueType.Long, 2, false)]

        [DataRow("1", ValueType.Double, 1, false)]
        [DataRow("1", ValueType.Double, 0, true)]
        [DataRow("1", ValueType.Double, 2, false)]
        public void TestGreaterThanFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, false)]
        [DataRow("1", ValueType.Int, 0L, true)]
        [DataRow("1", ValueType.Int, 2L, false)]

        [DataRow("1", ValueType.Long, 1L, false)]
        [DataRow("1", ValueType.Long, 0L, true)]
        [DataRow("1", ValueType.Long, 2L, false)]

        [DataRow("1", ValueType.Double, 1L, false)]
        [DataRow("1", ValueType.Double, 0L, true)]
        [DataRow("1", ValueType.Double, 2L, false)]
        public void TestGreaterThanFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("str", "str", false)]
        [DataRow("str", "STR", true)]
        [DataRow("str1", "str11", false)]
        [DataRow("str11", "str1", true)]
        public void TestGreaterThanFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("01", new byte[] { 0x00 }, true)]
        [DataRow("01", new byte[] { 0x01 }, true)]
        [DataRow("01", new byte[] { 0x02 }, false)]
        [DataRow("0101", new byte[] { 0x01 }, true)]
        [DataRow("01", new byte[] { 0x01, 0x01 }, false)]
        public void TestGreaterThanOrEqualFilterForBinaryValue(string propertyValue, byte[] value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("false", false, true)]
        [DataRow("true", false, true)]
        [DataRow("false", true, false)]
        [DataRow("false", false, true)]
        public void TestGreaterThanOrEqualFilterForBooleanValue(string propertyValue, bool value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Boolean));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local, true)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local, true)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified, true)]

        [DataRow("2015/01/22 02:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, true)]
        [DataRow("2015/01/22 00:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc, false)]

        [DataRow("2016/02/23 06:06:10:0140000+04:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        [DataRow("2016/02/23 06:06:10:0140000+05:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, false)]
        [DataRow("2016/02/23 06:06:10:0140000+03:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Utc, true)]
        public void TestGreaterThanOrEqualFilterForDateTimeValue(string propertyValue, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind, bool expected)
        {
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.DateTime));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1D, true)]
        [DataRow("1", ValueType.Int, 0D, true)]
        [DataRow("1", ValueType.Int, 2D, false)]

        [DataRow("1", ValueType.Long, 1D, true)]
        [DataRow("1", ValueType.Long, 0D, true)]
        [DataRow("1", ValueType.Long, 2D, false)]

        [DataRow("1", ValueType.Double, 1D, true)]
        [DataRow("1", ValueType.Double, 0D, true)]
        [DataRow("1", ValueType.Double, 2D, false)]
        public void TestGreaterThanOrEqualFilterForDoubleValue(string propertyValue, object valueType, double value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, true)]
        [DataRow("00000000-0000-0000-0000-000000000000", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, true)]
        [DataRow("fb1908dd-5521-4466-b3df-b437593f9e58", new byte[16] { 0x1D, 0x60, 0x9E, 0x8F, 0xFE, 0x2F, 0xC9, 0x40, 0x9F, 0xDF, 0xD6, 0x7E, 0xE7, 0xB5, 0x5F, 0x69 }, true)]
        [DataRow("8f9e601d-2ffe-40c9-9fdf-d67ee7b55f69", new byte[16] { 0xDD, 0x08, 0x19, 0xFB, 0x21, 0x55, 0x66, 0x44, 0xB3, 0xDF, 0xB4, 0x37, 0x59, 0x3F, 0x9E, 0x58 }, false)]
        public void TestGreaterThanOrEqualFilterForGuidValue(string propertyValue, byte[] guidBytes, bool expected)
        {
            var value = new Guid(guidBytes);
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Guid));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1, true)]
        [DataRow("1", ValueType.Int, 0, true)]
        [DataRow("1", ValueType.Int, 2, false)]

        [DataRow("1", ValueType.Long, 1, true)]
        [DataRow("1", ValueType.Long, 0, true)]
        [DataRow("1", ValueType.Long, 2, false)]

        [DataRow("1", ValueType.Double, 1, true)]
        [DataRow("1", ValueType.Double, 0, true)]
        [DataRow("1", ValueType.Double, 2, false)]
        public void TestGreaterThanOrEqualFilterForIntValue(string propertyValue, object valueType, int value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("1", ValueType.Int, 1L, true)]
        [DataRow("1", ValueType.Int, 0L, true)]
        [DataRow("1", ValueType.Int, 2L, false)]

        [DataRow("1", ValueType.Long, 1L, true)]
        [DataRow("1", ValueType.Long, 0L, true)]
        [DataRow("1", ValueType.Long, 2L, false)]

        [DataRow("1", ValueType.Double, 1L, true)]
        [DataRow("1", ValueType.Double, 0L, true)]
        [DataRow("1", ValueType.Double, 2L, false)]
        public void TestGreaterThanOrEqualFilterForLongValue(string propertyValue, object valueType, long value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, (ValueType)valueType));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("str", "str", true)]
        [DataRow("str", "STR", true)]
        [DataRow("str1", "str11", false)]
        [DataRow("str11", "str1", true)]
        public void TestGreaterThanOrEqualFilterForStringValue(string propertyValue, string value, bool expected)
        {
            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.String));
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, value);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        public void TestComparingWithANonExistingPropertyReturnsFalse()
        {
            var storageObject = new StorageObject(null, null, null);
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("property", "value");
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsFalse(predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("true", "true", true)]
        [DataRow("true", "false", false)]
        [DataRow("false", "true", false)]
        [DataRow("false", "false", false)]
        public void TestAndFilter(string value1, string value2, bool expected)
        {
            var storageObject = new StorageObject(
                null,
                null,
                null,
                new StorageObjectProperty(nameof(value1), value1, ValueType.Boolean),
                new StorageObjectProperty(nameof(value2), value2, ValueType.Boolean));
            var andFilter = ObjectStoreQueryFilter.Equal(nameof(value1), true)
                .And(ObjectStoreQueryFilter.Equal(nameof(value2), true));
            var predicate = ObjectStoreQueryPredicate.CreateFrom(andFilter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }

        [DataTestMethod]
        [DataRow("true", "true", true)]
        [DataRow("true", "false", true)]
        [DataRow("false", "true", true)]
        [DataRow("false", "false", false)]
        public void TestOrFilter(string value1, string value2, bool expected)
        {
            var storageObject = new StorageObject(
                null,
                null,
                null,
                new StorageObjectProperty(nameof(value1), value1, ValueType.Boolean),
                new StorageObjectProperty(nameof(value2), value2, ValueType.Boolean));
            var orFilter = ObjectStoreQueryFilter.Equal(nameof(value1), true)
                .Or(ObjectStoreQueryFilter.Equal(nameof(value2), true));
            var predicate = ObjectStoreQueryPredicate.CreateFrom(orFilter);

            Assert.AreEqual(expected, predicate.IsMatch(storageObject));
        }
    }
}