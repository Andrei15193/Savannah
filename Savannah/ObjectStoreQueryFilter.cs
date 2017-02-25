using System;
using Savannah.Query;

namespace Savannah
{
    public abstract class ObjectStoreQueryFilter
    {
        public static ObjectStoreQueryFilter Equal(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Binary, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Boolean, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.DateTime, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Double, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Guid, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Int, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.Long, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter Equal(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.Equal, ValueType.String, value, ObjectStoreQueryOperator.NotEqual);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Binary, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Boolean, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.DateTime, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Double, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Guid, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Int, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.Long, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter NotEqual(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.NotEqual, ValueType.String, value, ObjectStoreQueryOperator.Equal);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Binary, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Boolean, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.DateTime, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Double, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Guid, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Int, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.Long, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThan(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThan, ValueType.String, value, ObjectStoreQueryOperator.GreaterThanOrEqual);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Binary, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Boolean, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.DateTime, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Double, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Guid, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Int, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.Long, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter LessThanOrEqual(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.LessThanOrEqual, ValueType.String, value, ObjectStoreQueryOperator.GreaterThan);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Binary, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Boolean, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.DateTime, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Double, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Guid, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Int, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.Long, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThan(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThan, ValueType.String, value, ObjectStoreQueryOperator.LessThanOrEqual);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, byte[] value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Binary, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, bool value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Boolean, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, DateTime value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            ObjectStoreLimitations.CheckValue(value);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.DateTime, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, double value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Double, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, Guid value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Guid, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, int value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Int, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, long value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.Long, value, ObjectStoreQueryOperator.LessThan);
        }

        public static ObjectStoreQueryFilter GreaterThanOrEqual(string propertyName, string value)
        {
            ObjectStoreLimitations.CheckPropertyName(propertyName);
            return new ObjectStoreQueryValueFilter(propertyName, ObjectStoreQueryOperator.GreaterThanOrEqual, ValueType.String, value, ObjectStoreQueryOperator.LessThan);
        }

        public abstract ObjectStoreQueryOperator Operator { get; }

        public abstract ObjectStoreQueryFilter Not();

        public ObjectStoreQueryFilter And(ObjectStoreQueryFilter other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return new ObjectStoreQueryLogicalFilter(this, ObjectStoreQueryOperator.And, other, ObjectStoreQueryOperator.Or);
        }

        public ObjectStoreQueryFilter Or(ObjectStoreQueryFilter other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return new ObjectStoreQueryLogicalFilter(this, ObjectStoreQueryOperator.Or, other, ObjectStoreQueryOperator.And);
        }
    }
}