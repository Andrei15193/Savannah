using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Query;

namespace Savannah.Tests.Query
{
    [TestClass]
    public class ObjectStoreQueryPredicateTests
        : UnitTest
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestFilterWithNoPartitionKeySetsNullForPartitionKeys()
        {
            var filter = ObjectStoreQueryFilter.Equal("RowKey", "value");

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsNull(predicate.PartitionKeys);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestFilterWithNoRowKeySetsNullForRowKeys()
        {
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", "value");

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsNull(predicate.RowKeys);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGetPartitionKeyFromSimpleFilterOnPartitionKey()
        {
            var partitionKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue);

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(partitionKeyValue, predicate.PartitionKeys.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGetPartitionKeyFromComplexFilterOnPartitionKey()
        {
            var partitionKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("PartitionKey", partitionKeyValue)
                .And(ObjectStoreQueryFilter.Equal("RowKey", "testValue"));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(partitionKeyValue, predicate.PartitionKeys.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
        public void TestGetRowKeyFromSimpleFilterOnRowKey()
        {
            var rowKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue);

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(rowKeyValue, predicate.RowKeys.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGetRowKeyFromComplexFilterOnRowKey()
        {
            var rowKeyValue = "value";
            var filter = ObjectStoreQueryFilter.Equal("RowKey", rowKeyValue)
                .And(ObjectStoreQueryFilter.Equal("PartitionKey", "testValue"));

            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreSame(rowKeyValue, predicate.RowKeys.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
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

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BinaryValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForBinaryValue()
        {
            var row = GetRow<BinaryValueComparisonsRow>();

            var propertyName = "property";
            var propertyValue = (row.Left == null ? null : string.Join(string.Empty, row.Left.Select(@byte => @byte.ToString("X2"))));
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, propertyValue, ValueType.Binary));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForBooleanValue()
        {
            var row = GetRow<BooleanValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Boolean));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DateTimeValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForDateTimeValue()
        {
            var row = GetRow<DateTimeValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.DateTime));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForDoubleValue()
        {
            var row = GetRow<DoubleValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForGuidValue()
        {
            var row = GetRow<GuidValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.Guid));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForIntValue()
        {
            var row = GetRow<IntValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForLongValue()
        {
            var row = GetRow<LongValueComparisonsRow>();

            var propertyName = "property";
            var valueType = (ValueType)Enum.Parse(typeof(ValueType), row.ValueType);
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, valueType));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestEqualFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.Equal(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.Equal, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestNotEqualFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.NotEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.NotEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.LessThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.LessThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.LessThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.GreaterThan(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThan, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, StringValueComparisonsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualFilterForStringValue()
        {
            var row = GetRow<StringValueComparisonsRow>();

            var propertyName = "property";
            var storageObject = new StorageObject(null, null, null, new StorageObjectProperty(propertyName, row.Left, ValueType.String));

            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, row.Right);
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.AreEqual(row.GreaterThanOrEqual, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestComparingWithANonExistingPropertyReturnsFalse()
        {
            var storageObject = new StorageObject(null, null, null);
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("property", "value");
            var predicate = ObjectStoreQueryPredicate.CreateFrom(filter);

            Assert.IsFalse(predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LogicalOperatorResultsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestAndFilter()
        {
            var row = GetRow<LogicalOperatorResultsRow>();

            var storageObject = new StorageObject(
                null,
                null,
                null,
                new StorageObjectProperty(nameof(row.Left), "true", ValueType.Boolean),
                new StorageObjectProperty(nameof(row.Right), "true", ValueType.Boolean));
            var andFilter = ObjectStoreQueryFilter.Equal(nameof(row.Left), row.Left)
                .And(ObjectStoreQueryFilter.Equal(nameof(row.Right), row.Right));
            var predicate = ObjectStoreQueryPredicate.CreateFrom(andFilter);

            Assert.AreEqual(row.AndResult, predicate.IsMatch(storageObject));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LogicalOperatorResultsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestOrFilter()
        {
            var row = GetRow<LogicalOperatorResultsRow>();

            var storageObject = new StorageObject(
                null,
                null,
                null,
                new StorageObjectProperty(nameof(row.Left), "true", ValueType.Boolean),
                new StorageObjectProperty(nameof(row.Right), "true", ValueType.Boolean));
            var orFilter = ObjectStoreQueryFilter.Equal(nameof(row.Left), row.Left)
                .Or(ObjectStoreQueryFilter.Equal(nameof(row.Right), row.Right));
            var predicate = ObjectStoreQueryPredicate.CreateFrom(orFilter);

            Assert.AreEqual(row.OrResult, predicate.IsMatch(storageObject));
        }
    }
}