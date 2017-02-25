using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using Savannah.Xml;

namespace Savannah
{
    internal class StorageObjectFactory
    {
        private readonly string _timeStamp;

        internal StorageObjectFactory(DateTime timeStamp)
        {
            _timeStamp = timeStamp.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture);
        }

        internal StorageObject CreateFrom(object @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            var metadata = ObjectMetadata.GetFor(@object.GetType());

            var partitionKey = _TryGetStringValueFrom(@object, metadata.PartitionKeyProperty);
            var rowKey = _TryGetStringValueFrom(@object, metadata.RowKeyProperty);
            var properties = _GetPropertiesFrom(@object, metadata);

            return new StorageObject(partitionKey, rowKey, _timeStamp, properties);
        }

        private IEnumerable<StorageObjectProperty> _GetPropertiesFrom(object @object, ObjectMetadata metadata)
            => (
                from property in metadata.ReadableProperties
                where (property != metadata.PartitionKeyProperty
                    && property != metadata.RowKeyProperty
                    && property != metadata.TimestampProperty)
                select _GetStorageObjectPropertyTypeFrom(@object, property)
            ).ToList();

        private static string _TryGetStringValueFrom(object @object, PropertyInfo property)
            => (property?.GetMethod == null ? null : (string)property?.GetValue(@object));

        private StorageObjectProperty _GetStorageObjectPropertyTypeFrom(object @object, PropertyInfo property)
        {
            var value = property.GetValue(@object);

            if (property.PropertyType == typeof(string))
                return new StorageObjectProperty(
                    property.Name,
                    (string)value,
                    ValueType.String);

            if (property.PropertyType == typeof(byte[]))
                return new StorageObjectProperty(
                    property.Name,
                    _ConvertToByteString((byte[])value),
                    ValueType.Binary);

            if (property.PropertyType == typeof(bool))
                return new StorageObjectProperty(
                    property.Name,
                    XmlConvert.ToString((bool)value),
                    ValueType.Boolean);

            if (property.PropertyType == typeof(DateTime))
                return new StorageObjectProperty(
                    property.Name,
                    ((DateTime)value).ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                    ValueType.DateTime);

            if (property.PropertyType == typeof(double))
                return new StorageObjectProperty(
                    property.Name,
                    XmlConvert.ToString((double)value),
                    ValueType.Double);

            if (property.PropertyType == typeof(Guid))
                return new StorageObjectProperty(
                    property.Name,
                    XmlConvert.ToString((Guid)value),
                    ValueType.Guid);

            if (property.PropertyType == typeof(int))
                return new StorageObjectProperty(
                    property.Name,
                    XmlConvert.ToString((int)value),
                    ValueType.Int);

            if (property.PropertyType == typeof(long))
                return new StorageObjectProperty(
                    property.Name,
                    XmlConvert.ToString((long)value),
                    ValueType.Long);

            throw new InvalidOperationException("Not supported property type.");
        }

        private string _ConvertToByteString(byte[] byteArray)
        {
            if (byteArray == null)
                return null;
            if (byteArray.Length == 0)
                return string.Empty;

            var charArray = new char[byteArray.Length * 2];

            for (int byteIndex = 0, charIndex = 0; byteIndex < byteArray.Length; byteIndex++, charIndex += 2)
            {
                var @byte = byteArray[byteIndex];
                var highPart = (byte)((0xF0 & @byte) >> 4);
                var lowPart = (byte)(0x0F & @byte);

                charArray[charIndex] = _GetCharFor(highPart);
                charArray[charIndex + 1] = _GetCharFor(lowPart);
            }

            return new string(charArray);
        }

        private char _GetCharFor(byte @byte)
        {
            if (@byte <= 9)
                return (char)('0' + @byte);
            else
                return (char)('A' + @byte - 10);
        }
    }
}