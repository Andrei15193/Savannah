using System;
using System.Data;
using System.Globalization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Savannah.Tests
{
    [TestClass]
    public abstract class UnitTest
    {
        internal const string DataProviderName = "Microsoft.VisualStudio.TestTools.DataSource.XML";
        internal const string DataFilePath = (nameof(Savannah) + "." + nameof(Tests) + "\\" + nameof(UnitTest) + "Data.xml");
        internal const string DataFileName = (nameof(UnitTest) + "Data.xml");

        internal const string KeyValuesTable = "KeyValues";
        internal const string PropertyCountsTable = "PropertyCounts";
        internal const string ValueTypesTable = "ValueTypes";
        internal const string BinaryValuesTable = "BinaryValues";
        internal const string BooleanValuesTable = "BooleanValues";
        internal const string DateTimeValuesTable = "DateTimeValues";
        internal const string DoubleValuesTable = "DoubleValues";
        internal const string GuidValuesTable = "GuidValues";
        internal const string IntValuesTable = "IntValues";
        internal const string LongValuesTable = "LongValues";
        internal const string StringValuesTable = "StringValues";
        internal const string Md5HashValuesTable = "Md5HashValues";
        internal const string ObjectKeysTable = "ObjectKeys";
        internal const string ObjectStoreCollectionNamesTable = "ObjectStoreCollectionNames";
        internal const string ObjectSetRowKeyValuesTable = "ObjectSetRowKeyValues";
        internal const string PartitionKeyDeleteSetTable = "PartitionKeyDeleteSet";
        internal const string RowKeyDeleteSetTable = "RowKeyDeleteSet";
        internal const string PropertyNamesToProjectTable = "PropertyNamesToProject";
        internal const string OperationTestSetTable = "OperationTestSet";
        internal const string PositiveIntegerValuesTable = "PositiveIntegerValues";
        internal const string ValidKeyValuesTable = "ValidKeyValues";
        internal const string KeyLengthsTable = "KeyLengths";
        internal const string InvalidDateValuesTable = "InvalidDateValues";
        internal const string InvalidStringLengthsTable = "InvalidStringLengths";
        internal const string ValidStringLengthsTable = "ValidStringLengths";
        internal const string InvalidByteArrayLengthsTable = "InvalidByteArrayLengths";
        internal const string ValidByteArrayLengthsTable = "ValidByteArrayLengths";
        internal const string InvalidCollectionNamesTable = "InvalidCollectionNames";
        internal const string ValidCollectionNamesTable = "ValidCollectionNames";
        internal const string InvalidPropertyNameTable = "InvalidPropertyName";
        internal const string ValidPropertyNameTable = "ValidPropertyName";
        internal const string ValidDateTimeValuesTable = "ValidDateTimeValues";
        internal const string UnsupportedTypeNamesTable = "UnsupportedTypeNames";
        internal const string ObjectStoreValueFileterQueryOperatorTable = "ObjectStoreValueFileterQueryOperator";
        internal const string XmlObjectPartitionKeyTable = "XmlObjectPartitionKey";
        internal const string XmlObjectRowKeyTable = "XmlObjectRowKey";
        internal const string XmlObjectTimestampTable = "XmlObjectTimestamp";
        internal const string XmlObjectPropertyNamesTable = "XmlObjectPropertyNames";
        internal const string XmlObjectPropertyValuesTable = "XmlObjectPropertyValues";
        internal const string XmlObjectPropertyTypesTable = "XmlObjectPropertyTypes";
        internal const string XmlObjectsInPartitionTable = "XmlObjectsInPartition";
        internal const string BinaryValueComparisonsTable = "BinaryValueComparisons";
        internal const string BooleanValueComparisonsTable = "BooleanValueComparisons";
        internal const string DateTimeValueComparisonsTable = "DateTimeValueComparisons";
        internal const string DoubleValueComparisonsTable = "DoubleValueComparisons";
        internal const string GuidValueComparisonsTable = "GuidValueComparisons";
        internal const string IntValueComparisonsTable = "IntValueComparisons";
        internal const string LongValueComparisonsTable = "LongValueComparisons";
        internal const string StringValueComparisonsTable = "StringValueComparisons";
        internal const string LogicalOperatorResultsTable = "LogicalOperatorResults";

        public TestContext TestContext { get; set; }

        internal TRow GetRow<TRow>()
            => (TRow)Activator.CreateInstance(typeof(TRow), TestContext.DataRow);

        internal class KeyValuesRow
        {
            private readonly DataRow _dataRow;

            public KeyValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class PropertyCountsRow
        {
            private readonly DataRow _dataRow;

            public PropertyCountsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class ValueTypesRow
        {
            private readonly DataRow _dataRow;

            public ValueTypesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class BinaryValuesRow
        {
            private readonly DataRow _dataRow;

            public BinaryValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal byte[] Value
                => _GetByteArrayFrom(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class BooleanValuesRow
        {
            private readonly DataRow _dataRow;

            public BooleanValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal bool Value
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class DateTimeValuesRow
        {
            private readonly DataRow _dataRow;

            public DateTimeValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal DateTime Value
                => _GetDateTimeFrom(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string DateTimeKind
                => _GetStringFrom(_dataRow[nameof(DateTimeKind)]);
        }

        internal class DoubleValuesRow
        {
            private readonly DataRow _dataRow;

            public DoubleValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal double Value
                => _GetDoubleFrom(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class GuidValuesRow
        {
            private readonly DataRow _dataRow;

            public GuidValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal Guid Value
                => _GetGuidFrom(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class IntValuesRow
        {
            private readonly DataRow _dataRow;

            public IntValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class LongValuesRow
        {
            private readonly DataRow _dataRow;

            public LongValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal long Value
                => _GetInt64From(_GetStringFrom(_dataRow[nameof(Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class StringValuesRow
        {
            private readonly DataRow _dataRow;

            public StringValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class Md5HashValuesRow
        {
            private readonly DataRow _dataRow;

            public Md5HashValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);

            internal string HashValue
                => _GetStringFrom(_dataRow[nameof(HashValue)]);
        }

        internal class ObjectKeysRow
        {
            private readonly DataRow _dataRow;

            public ObjectKeysRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string PartitionKey
                => _GetStringFrom(_dataRow[nameof(PartitionKey)]);

            internal string RowKey
                => _GetStringFrom(_dataRow[nameof(RowKey)]);
        }

        internal class ObjectStoreCollectionNamesRow
        {
            private readonly DataRow _dataRow;

            public ObjectStoreCollectionNamesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class ObjectSetRowKeyValuesRow
        {
            private readonly DataRow _dataRow;

            public ObjectSetRowKeyValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string RowKey1
                => _GetStringFrom(_dataRow[nameof(RowKey1)]);

            internal string RowKey2
                => _GetStringFrom(_dataRow[nameof(RowKey2)]);

            internal string RowKey3
                => _GetStringFrom(_dataRow[nameof(RowKey3)]);
        }

        internal class PartitionKeyDeleteSetRow
        {
            private readonly DataRow _dataRow;

            public PartitionKeyDeleteSetRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string PartitionKey1
                => _GetStringFrom(_dataRow[nameof(PartitionKey1)]);

            internal string PartitionKey2
                => _GetStringFrom(_dataRow[nameof(PartitionKey2)]);

            internal string PartitionKey3
                => _GetStringFrom(_dataRow[nameof(PartitionKey3)]);

            internal string PartitionKey4
                => _GetStringFrom(_dataRow[nameof(PartitionKey4)]);

            internal string PartitionKeyToRemove
                => _GetStringFrom(_dataRow[nameof(PartitionKeyToRemove)]);
        }

        internal class RowKeyDeleteSetRow
        {
            private readonly DataRow _dataRow;

            public RowKeyDeleteSetRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string RowKey1
                => _GetStringFrom(_dataRow[nameof(RowKey1)]);

            internal string RowKey2
                => _GetStringFrom(_dataRow[nameof(RowKey2)]);

            internal string RowKey3
                => _GetStringFrom(_dataRow[nameof(RowKey3)]);

            internal string RowKey4
                => _GetStringFrom(_dataRow[nameof(RowKey4)]);

            internal string RowKeyToRemove
                => _GetStringFrom(_dataRow[nameof(RowKeyToRemove)]);
        }

        internal class PropertyNamesToProjectRow
        {
            private readonly DataRow _dataRow;

            public PropertyNamesToProjectRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string PropertyNames
                => _GetStringFrom(_dataRow[nameof(PropertyNames)]);
        }

        internal class OperationTestSetRow
        {
            private readonly DataRow _dataRow;

            public OperationTestSetRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int NumberOfOperations
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(NumberOfOperations)]));

            internal int Index
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Index)]));
        }

        internal class PositiveIntegerValuesRow
        {
            private readonly DataRow _dataRow;

            public PositiveIntegerValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class ValidKeyValuesRow
        {
            private readonly DataRow _dataRow;

            public ValidKeyValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class KeyLengthsRow
        {
            private readonly DataRow _dataRow;

            public KeyLengthsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class InvalidDateValuesRow
        {
            private readonly DataRow _dataRow;

            public InvalidDateValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal DateTime Value
                => _GetDateTimeFrom(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class InvalidStringLengthsRow
        {
            private readonly DataRow _dataRow;

            public InvalidStringLengthsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class ValidStringLengthsRow
        {
            private readonly DataRow _dataRow;

            public ValidStringLengthsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class InvalidByteArrayLengthsRow
        {
            private readonly DataRow _dataRow;

            public InvalidByteArrayLengthsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class ValidByteArrayLengthsRow
        {
            private readonly DataRow _dataRow;

            public ValidByteArrayLengthsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal int Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class InvalidCollectionNamesRow
        {
            private readonly DataRow _dataRow;

            public InvalidCollectionNamesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class ValidCollectionNamesRow
        {
            private readonly DataRow _dataRow;

            public ValidCollectionNamesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class InvalidPropertyNameRow
        {
            private readonly DataRow _dataRow;

            public InvalidPropertyNameRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class ValidPropertyNameRow
        {
            private readonly DataRow _dataRow;

            public ValidPropertyNameRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class ValidDateTimeValuesRow
        {
            private readonly DataRow _dataRow;

            public ValidDateTimeValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal DateTime Value
                => _GetDateTimeFrom(_GetStringFrom(_dataRow[nameof(Value)]));
        }

        internal class UnsupportedTypeNamesRow
        {
            private readonly DataRow _dataRow;

            public UnsupportedTypeNamesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Value
                => _GetStringFrom(_dataRow[nameof(Value)]);
        }

        internal class ObjectStoreValueFileterQueryOperatorRow
        {
            private readonly DataRow _dataRow;

            public ObjectStoreValueFileterQueryOperatorRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Name
                => _GetStringFrom(_dataRow[nameof(Name)]);

            internal byte[] BinaryValue
                => _GetByteArrayFrom(_GetStringFrom(_dataRow[nameof(BinaryValue)]));

            internal bool BooleanValue
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(BooleanValue)]));

            internal DateTime DateTimeValue
                => _GetDateTimeFrom(_GetStringFrom(_dataRow[nameof(DateTimeValue)]));

            internal double DoubleValue
                => _GetDoubleFrom(_GetStringFrom(_dataRow[nameof(DoubleValue)]));

            internal Guid GuidValue
                => _GetGuidFrom(_GetStringFrom(_dataRow[nameof(GuidValue)]));

            internal int Int32Value
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Int32Value)]));

            internal long Int64Value
                => _GetInt64From(_GetStringFrom(_dataRow[nameof(Int64Value)]));

            internal string StringValue
                => _GetStringFrom(_dataRow[nameof(StringValue)]);
        }

        internal class XmlObjectPartitionKeyRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectPartitionKeyRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string PartitionKey
                => _GetStringFrom(_dataRow[nameof(PartitionKey)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectRowKeyRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectRowKeyRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string RowKey
                => _GetStringFrom(_dataRow[nameof(RowKey)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectTimestampRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectTimestampRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string Timestamp
                => _GetStringFrom(_dataRow[nameof(Timestamp)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectPropertyNamesRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectPropertyNamesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string PropertyNames
                => _GetStringFrom(_dataRow[nameof(PropertyNames)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectPropertyValuesRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectPropertyValuesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string PropertyValues
                => _GetStringFrom(_dataRow[nameof(PropertyValues)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectPropertyTypesRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectPropertyTypesRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal string PropertyTypes
                => _GetStringFrom(_dataRow[nameof(PropertyTypes)]);

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class XmlObjectsInPartitionRow
        {
            private readonly DataRow _dataRow;

            public XmlObjectsInPartitionRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Xml
                => _GetStringFrom(_dataRow[nameof(Xml)]);

            internal int NumberOfObjects
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(NumberOfObjects)]));

            internal string XmlOut
                => _GetStringFrom(_dataRow[nameof(XmlOut)]);
        }

        internal class BinaryValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public BinaryValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal byte[] Left
                => _GetByteArrayFrom(_GetStringFrom(_dataRow[nameof(Left)]));

            internal byte[] Right
                => _GetByteArrayFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class BooleanValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public BooleanValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal bool Right
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class DateTimeValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public DateTimeValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal DateTime Right
                => _GetDateTimeFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class DoubleValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public DoubleValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal string ValueType
                => _GetStringFrom(_dataRow[nameof(ValueType)]);

            internal double Right
                => _GetDoubleFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class GuidValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public GuidValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal Guid Right
                => _GetGuidFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class IntValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public IntValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal string ValueType
                => _GetStringFrom(_dataRow[nameof(ValueType)]);

            internal int Right
                => _GetInt32From(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class LongValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public LongValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal string ValueType
                => _GetStringFrom(_dataRow[nameof(ValueType)]);

            internal long Right
                => _GetInt64From(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class StringValueComparisonsRow
        {
            private readonly DataRow _dataRow;

            public StringValueComparisonsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal string Left
                => _GetStringFrom(_dataRow[nameof(Left)]);

            internal string Right
                => _GetStringFrom(_dataRow[nameof(Right)]);

            internal bool Equal
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Equal)]));

            internal bool NotEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(NotEqual)]));

            internal bool LessThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThan)]));

            internal bool LessThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(LessThanOrEqual)]));

            internal bool GreaterThan
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThan)]));

            internal bool GreaterThanOrEqual
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(GreaterThanOrEqual)]));
        }

        internal class LogicalOperatorResultsRow
        {
            private readonly DataRow _dataRow;

            public LogicalOperatorResultsRow(DataRow dataRow)
            {
                if (dataRow == null)
                    throw new ArgumentNullException(nameof(dataRow));
                _dataRow = dataRow;
            }

            internal bool Left
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Left)]));

            internal bool Right
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(Right)]));

            internal bool AndResult
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(AndResult)]));

            internal bool OrResult
                => _GetBooleanFrom(_GetStringFrom(_dataRow[nameof(OrResult)]));
        }

        private static string _GetStringFrom(object value)
            => (value == DBNull.Value ? null : (string)value);

        private static byte[] _GetByteArrayFrom(string value)
        {
            byte[] byteArray = null;

            if (value != null)
            {
                if (value.Length % 2 == 1)
                    value = ("0" + value);

                byteArray = new byte[(value.Length / 2)];
                for (var byteIndex = 0; byteIndex < byteArray.Length; byteIndex++)
                {
                    var @byte = byte.Parse(value.Substring((byteIndex * 2), 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    byteArray[byteIndex] = @byte;
                }
            }

            return byteArray;
        }

        private static bool _GetBooleanFrom(string value)
        {
            var @bool = XmlConvert.ToBoolean(value);
            return @bool;
        }

        private static DateTime _GetDateTimeFrom(string value)
        {
            var dateTime = DateTime.Parse(value, CultureInfo.InvariantCulture);
            return dateTime;
        }

        private static double _GetDoubleFrom(string value)
        {
            var @double = XmlConvert.ToDouble(value);
            return @double;
        }

        private static Guid _GetGuidFrom(string value)
        {
            var guid = XmlConvert.ToGuid(value);
            return guid;
        }

        private static int _GetInt32From(string value)
        {
            var @int = XmlConvert.ToInt32(value);
            return @int;
        }

        private static long _GetInt64From(string value)
        {
            var @long = XmlConvert.ToInt64(value);
            return @long;
        }
    }
}