using System;
using System.Globalization;
using System.Linq;
using Savannah.Xml;

namespace Savannah.Query
{
    internal class ObjectStoreQueryValueFilter
        : ObjectStoreQueryFilter
    {
        private readonly ObjectStoreQueryOperator _operator;
        private readonly ObjectStoreQueryOperator _notOperator;

        internal ObjectStoreQueryValueFilter(string propertyName, ObjectStoreQueryOperator @operator, ValueType valueType, object value, ObjectStoreQueryOperator notOperator)
        {
#if DEBUG
            ObjectStoreLimitations.CheckPropertyName(propertyName);
#endif
            _operator = @operator;
            _notOperator = notOperator;

            PropertyName = propertyName;
            Value = value;
            ValueType = valueType;
        }

        public string PropertyName { get; }

        public override ObjectStoreQueryOperator Operator
            => _operator;

        public object Value { get; }

        internal ValueType ValueType { get; }

        public override ObjectStoreQueryFilter Not()
            => new ObjectStoreQueryValueFilter(PropertyName, _notOperator, ValueType, Value, _operator);

        public sealed override string ToString()
        {
            var stringValue = _GetStringValue();
            return $"{PropertyName} {Operator} {stringValue}";
        }

        private string _GetStringValue()
        {
            if (Value == null)
                return "null";

            switch (ValueType)
            {
                case ValueType.Binary:
                    return $"[{ValueType.Binary}] {string.Join(string.Empty, ((byte[])Value).Select(@byte => @byte.ToString("X2", CultureInfo.InvariantCulture)))}";

                case ValueType.Boolean:
                    return $"[{ValueType.Boolean}] {(((bool)Value) ? bool.TrueString : bool.FalseString)}";

                case ValueType.DateTime:
                    return $"[{ValueType.DateTime}] {((DateTime)Value).ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)}";

                case ValueType.Double:
                    return $"[{ValueType.Double}] {((double)Value).ToString("e", CultureInfo.InvariantCulture)}";

                case ValueType.Guid:
                    return $"[{ValueType.Guid}] {(Guid)Value}";

                case ValueType.Int:
                    return $"[{ValueType.Int}] {((int)Value).ToString("n0", CultureInfo.InvariantCulture)}";

                case ValueType.Long:
                    return $"[{ValueType.Long}] {((long)Value).ToString("n0", CultureInfo.InvariantCulture)}";

                default:
                case ValueType.String:
                    return $"[{ValueType.String}] {(string)Value}";
            }
        }
    }
}