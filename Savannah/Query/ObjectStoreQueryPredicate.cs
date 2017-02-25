using System;
using System.Collections.Generic;
using System.Linq;

namespace Savannah.Query
{
    internal class ObjectStoreQueryPredicate
    {
        private const string _partitionKeyPropertyName = "PartitionKey";
        private const string _rowKeyPropertyName = "RowKey";

        private static readonly PropertyValueFactory _valueFactory = new PropertyValueFactory();

        internal static ObjectStoreQueryPredicate CreateFrom(ObjectStoreQueryFilter filter)
        {
            if (filter == null)
                return new ObjectStoreQueryPredicate(null, null, null);

            var partitionKeys = new List<string>();
            var rowKeys = new List<string>();
            var predicate = _GetPredicateFrom(filter, ref partitionKeys, ref rowKeys);

            if (partitionKeys != null && partitionKeys.Count == 0)
                partitionKeys = null;
            if (rowKeys != null && rowKeys.Count == 0)
                rowKeys = null;

            return new ObjectStoreQueryPredicate(partitionKeys, rowKeys, predicate);
        }

        private static Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> _GetPredicateFrom(ObjectStoreQueryFilter filter, ref List<string> partitionKeys, ref List<string> rowKeys)
        {
#if DEBUG
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
#endif
            var logicalFilter = (filter as ObjectStoreQueryLogicalFilter);
            if (logicalFilter != null)
                return _GetLogicalPredicateFrom(logicalFilter, ref partitionKeys, ref rowKeys);

            var valueFiler = (ObjectStoreQueryValueFilter)filter;
            return _GetValueFilterPredicateFrom(valueFiler, ref partitionKeys, ref rowKeys);
        }

        private static Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> _GetLogicalPredicateFrom(ObjectStoreQueryLogicalFilter logicalFilter, ref List<string> partitionKeys, ref List<string> rowKeys)
        {
            var leftPredicate = _GetPredicateFrom(logicalFilter.LeftOperand, ref partitionKeys, ref rowKeys);
            var rightPredicate = _GetPredicateFrom(logicalFilter.RightOperand, ref partitionKeys, ref rowKeys);
            if (logicalFilter.Operator == ObjectStoreQueryOperator.And)
                return (properties => (leftPredicate(properties) && rightPredicate(properties)));
            else if (logicalFilter.Operator == ObjectStoreQueryOperator.Or)
                return (properties => (leftPredicate(properties) || rightPredicate(properties)));

            throw new InvalidOperationException("Unknown logical operator.");
        }

        private static Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> _GetValueFilterPredicateFrom(ObjectStoreQueryValueFilter valueFiler, ref List<string> partitionKeys, ref List<string> rowKeys)
        {
            if (valueFiler.Operator == ObjectStoreQueryOperator.Equal)
            {
                if (ObjectStoreLimitations.StringComparer.Equals(nameof(StorageObject.PartitionKey), valueFiler.PropertyName))
                    _AddKeyValue(valueFiler, ref partitionKeys);
                if (ObjectStoreLimitations.StringComparer.Equals(nameof(StorageObject.RowKey), valueFiler.PropertyName))
                    _AddKeyValue(valueFiler, ref rowKeys);

                return _GetPredicateFrom(valueFiler, _Equal);
            }

            if (ObjectStoreLimitations.StringComparer.Equals(nameof(StorageObject.PartitionKey), valueFiler.PropertyName))
                partitionKeys = null;
            if (ObjectStoreLimitations.StringComparer.Equals(nameof(StorageObject.RowKey), valueFiler.PropertyName))
                rowKeys = null;

            switch (valueFiler.Operator)
            {
                case ObjectStoreQueryOperator.NotEqual:
                    return _GetPredicateFrom(valueFiler, _NotEqual);

                case ObjectStoreQueryOperator.LessThan:
                    return _GetPredicateFrom(valueFiler, _LessThan);

                case ObjectStoreQueryOperator.LessThanOrEqual:
                    return _GetPredicateFrom(valueFiler, _LessThanOrEqual);

                case ObjectStoreQueryOperator.GreaterThan:
                    return _GetPredicateFrom(valueFiler, _GreaterThan);

                case ObjectStoreQueryOperator.GreaterThanOrEqual:
                    return _GetPredicateFrom(valueFiler, _GreaterThanOrEqual);
            }

            throw new InvalidOperationException("Unknown filter operator.");
        }

        private static Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> _GetPredicateFrom(ObjectStoreQueryValueFilter valueFilter, Func<StorageObjectProperty, ObjectStoreQueryValueFilter, bool> valueComparer)
        {
#if DEBUG
            if (valueFilter == null)
                throw new ArgumentNullException(nameof(valueFilter));
            if (valueComparer == null)
                throw new ArgumentNullException(nameof(valueComparer));
#endif
            return (
                properties =>
                {
                    StorageObjectProperty property;
                    if (!properties.TryGetValue(valueFilter.PropertyName, out property))
                        return false;

                    _EnsureTypeCompatibility(property.Type, valueFilter.ValueType);

                    return valueComparer(property, valueFilter);
                }
            );
        }

        private static void _EnsureTypeCompatibility(ValueType type1, ValueType type2)
        {
            var areTypesCompatible = true;
            if (type1 != type2)
                switch (type1)
                {
                    case ValueType.Double:
                    case ValueType.Int:
                    case ValueType.Long:
                        areTypesCompatible = (type2 == ValueType.Double || type2 == ValueType.Int || type2 == ValueType.Long);
                        break;

                    default:
                        areTypesCompatible = (type1 == type2);
                        break;
                }
            if (!areTypesCompatible)
                throw new InvalidOperationException($"Cannot compare {type1} value with {type2} property.");
        }

        private static bool _Equal(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
        {
            var propertyValue = _valueFactory.GetPropertyValueFrom(property);

            switch (valueFilter.ValueType)
            {
                case ValueType.Boolean:
                case ValueType.Guid:
                    return propertyValue.Equals(valueFilter.Value);

                case ValueType.Double:
                case ValueType.Int:
                case ValueType.Long:
                    if (property.Type == valueFilter.ValueType)
                        return propertyValue.Equals(valueFilter.Value);
                    else
                        return Convert.ToDouble(propertyValue).Equals(Convert.ToDouble(valueFilter.Value));

                case ValueType.DateTime:
                    return ((DateTime)propertyValue).ToUniversalTime().Equals(((DateTime)valueFilter.Value).ToUniversalTime());

                case ValueType.String:
                    return ObjectStoreLimitations.StringComparer.Equals((string)propertyValue, (string)valueFilter.Value);

                case ValueType.Binary:
                    return _Equal((byte[])valueFilter.Value, (byte[])propertyValue);
            }

            throw new NotImplementedException($"Invalid {nameof(ValueType)} for {nameof(valueFilter)}.");
        }

        private static bool _NotEqual(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
        {
            var propertyValue = _valueFactory.GetPropertyValueFrom(property);

            switch (valueFilter.ValueType)
            {
                case ValueType.Boolean:
                case ValueType.Guid:
                    return !propertyValue.Equals(valueFilter.Value);

                case ValueType.Double:
                case ValueType.Int:
                case ValueType.Long:
                    if (property.Type == valueFilter.ValueType)
                        return !propertyValue.Equals(valueFilter.Value);
                    else
                        return !Convert.ToDouble(propertyValue).Equals(Convert.ToDouble(valueFilter.Value));

                case ValueType.DateTime:
                    return !((DateTime)propertyValue).ToUniversalTime().Equals(((DateTime)valueFilter.Value).ToUniversalTime());

                case ValueType.String:
                    return !ObjectStoreLimitations.StringComparer.Equals((string)propertyValue, (string)valueFilter.Value);

                case ValueType.Binary:
                    return !_Equal((byte[])propertyValue, (byte[])valueFilter.Value);
            }

            throw new NotImplementedException($"Invalid {nameof(ValueType)} for {nameof(valueFilter)}.");
        }

        private static bool _Equal(byte[] array1, byte[] array2)
        {
            if (array1 != null)
                return (array2 != null && array1.SequenceEqual(array2));
            else
                return (array2 == null);
        }

        private static bool _LessThan(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
            => (_Compare(property, valueFilter) < 0);

        private static bool _LessThanOrEqual(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
            => (_Compare(property, valueFilter) <= 0);

        private static bool _GreaterThan(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
            => (_Compare(property, valueFilter) > 0);

        private static bool _GreaterThanOrEqual(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
            => (_Compare(property, valueFilter) >= 0);

        private static int _Compare(StorageObjectProperty property, ObjectStoreQueryValueFilter valueFilter)
        {
            var propertyValue = _valueFactory.GetPropertyValueFrom(property);

            switch (valueFilter.ValueType)
            {
                case ValueType.Boolean:
                case ValueType.Guid:
                    return ((IComparable)propertyValue).CompareTo(valueFilter.Value);

                case ValueType.Double:
                case ValueType.Int:
                case ValueType.Long:
                    if (property.Type == valueFilter.ValueType)
                        return ((IComparable)propertyValue).CompareTo(valueFilter.Value);
                    else
                        return Convert.ToDouble(propertyValue).CompareTo(Convert.ToDouble(valueFilter.Value));

                case ValueType.DateTime:
                    return ((DateTime)propertyValue).ToUniversalTime().CompareTo(((DateTime)valueFilter.Value).ToUniversalTime());

                case ValueType.String:
                    return ObjectStoreLimitations.StringComparer.Compare((string)propertyValue, (string)valueFilter.Value);

                case ValueType.Binary:
                    return _Compare((byte[])propertyValue, (byte[])valueFilter.Value);
            }

            throw new NotImplementedException($"Invalid {nameof(ValueType)} for {nameof(valueFilter)}.");
        }

        private static int _Compare(byte[] array1, byte[] array2)
        {
            if (array1 == null)
                if (array2 == null)
                    return 0;
                else
                    return 1;
            else if (array2 == null)
                return -1;
            else
            {
                var byteArrayComparison = (array1.Length.CompareTo(array2.Length));

                if (byteArrayComparison == 0)
                    byteArrayComparison = array1
                        .Zip(array2, (byte1, byte2) => byte1.CompareTo(byte2))
                        .Where(byteComparison => byteComparison != 0)
                        .FirstOrDefault();

                return byteArrayComparison;
            }
        }

        private static void _AddKeyValue(ObjectStoreQueryValueFilter valueFilterNode, ref List<string> keys)
        {
            _EnsureTypeCompatibility(valueFilterNode.ValueType, ValueType.String);
            if (valueFilterNode.Operator == ObjectStoreQueryOperator.Equal)
                keys?.Add((string)valueFilterNode.Value);
            else
                keys = null;
        }

        private readonly Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> _predicate;

        private ObjectStoreQueryPredicate(IEnumerable<string> partitionKeys, IEnumerable<string> rowKeys, Func<IReadOnlyDictionary<string, StorageObjectProperty>, bool> predicate)
        {
            PartitionKeys = partitionKeys;
            RowKeys = rowKeys;
            _predicate = predicate;
        }

        internal IEnumerable<string> PartitionKeys { get; }

        internal IEnumerable<string> RowKeys { get; }

        internal bool IsMatch(StorageObject storageObject)
        {
#if DEBUG
            if (storageObject == null)
                throw new ArgumentNullException(nameof(storageObject));
#endif
            if (_predicate == null)
                return true;

            IReadOnlyDictionary<string, StorageObjectProperty> storageObjectProperties = storageObject
                .Properties
                .Concat(new[]
                    {
                        new StorageObjectProperty(nameof(StorageObject.PartitionKey), storageObject.PartitionKey, ValueType.String),
                        new StorageObjectProperty(nameof(StorageObject.RowKey), storageObject.RowKey, ValueType.String),
                        new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, ValueType.DateTime)
                    })
                .ToDictionary(property => property.Name, ObjectStoreLimitations.StringComparer);

            return _predicate(storageObjectProperties);
        }
    }
}