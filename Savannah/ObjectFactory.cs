using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Savannah
{
    internal class ObjectFactory<T> where T : new()
    {
        private readonly ObjectMetadata _metadata;

        internal ObjectFactory()
        {
            _metadata = ObjectMetadata.GetFor(typeof(T));
        }

        internal T CreateFrom(StorageObject storageObject)
        {
            if (storageObject == null)
                throw new ArgumentNullException(nameof(storageObject));

            var @object = new T();

            var storageProperties = storageObject
                .Properties
                .Concat(new[] {
                    new StorageObjectProperty(nameof(StorageObject.PartitionKey), storageObject.PartitionKey, StorageObjectPropertyType.String),
                    new StorageObjectProperty(nameof(StorageObject.RowKey), storageObject.RowKey, StorageObjectPropertyType.String),
                    new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, StorageObjectPropertyType.DateTime)
                }.Where(property => property.Value != null))
                .OrderBy(storageProperty => storageProperty.Name, StringComparer.Ordinal);

            using (var objectProperty = _metadata.WritableProperties.GetEnumerator())
            using (var storageProperty = storageProperties.GetEnumerator())
            {
                var hasObjectProperty = objectProperty.MoveNext();
                var hasStorageProperty = storageProperty.MoveNext();

                while (hasObjectProperty && hasStorageProperty)
                {
                    var propertyNameComparison = string.Compare(objectProperty.Current.Name, storageProperty.Current.Name, StringComparison.Ordinal);

                    if (propertyNameComparison < 0)
                        hasObjectProperty = objectProperty.MoveNext();
                    else if (propertyNameComparison > 0)
                        hasStorageProperty = storageProperty.MoveNext();
                    else
                    {
                        var value = _GetPropertyValueFrom(storageProperty.Current);

                        if (value == null)
                        {
                            if (objectProperty.Current.PropertyType.GetTypeInfo().IsValueType)
                                throw new InvalidOperationException(
                                    "Cannot set null to property of value type.");
                        }
                        else if (value.GetType() != objectProperty.Current.PropertyType)
                            throw new InvalidOperationException(
                                $"Property type mismatch. Cannot set {value.GetType().Namespace}.{value.GetType().Name} to property of type {objectProperty.Current.PropertyType.Namespace}.{objectProperty.Current.PropertyType.Name}.");

                        objectProperty.Current.SetValue(@object, value);

                        hasObjectProperty = objectProperty.MoveNext();
                        hasStorageProperty = storageProperty.MoveNext();
                    }
                }
            }

            return @object;
        }

        private object _GetPropertyValueFrom(StorageObjectProperty storageObjectProperty)
        {
            var value = storageObjectProperty.Value;

            switch (storageObjectProperty.Type)
            {
                case StorageObjectPropertyType.String:
                    return value;

                case StorageObjectPropertyType.Binary:
                    return _ConvertTypeByteArray(value);

                case StorageObjectPropertyType.Boolean:
                    return XmlConvert.ToBoolean(value);

                case StorageObjectPropertyType.DateTime:
                    var dateTime = DateTime.ParseExact(value, XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture);
                    if (char.ToLowerInvariant(value[value.Length - 1]) == 'z')
                        dateTime = dateTime.ToUniversalTime();
                    return dateTime;

                case StorageObjectPropertyType.Double:
                    return XmlConvert.ToDouble(value);

                case StorageObjectPropertyType.Guid:
                    return XmlConvert.ToGuid(value);

                case StorageObjectPropertyType.Int:
                    return XmlConvert.ToInt32(value);

                case StorageObjectPropertyType.Long:
                    return XmlConvert.ToInt64(value);
            }

            throw new InvalidOperationException("Not supported property type.");
        }

        private byte[] _ConvertTypeByteArray(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                if (value == null)
                    return null;
                else
                    return new byte[0];

            byte[] byteArray;
            var charIndex = 0;
            var byteIndex = 0;

            if (value.Length % 2 == 1)
            {
                byteArray = new byte[value.Length / 2 + 1];
                byteArray[0] = _GetByteFor(value[0]);
                charIndex = 1;
                byteIndex = 1;
            }
            else
                byteArray = new byte[value.Length / 2];

            for (; charIndex < value.Length; charIndex += 2, byteIndex++)
            {
                var @byte = _GetByteFor(value[charIndex]);
                @byte <<= 4;
                @byte |= _GetByteFor(value[charIndex + 1]);

                byteArray[byteIndex] = @byte;
            }

            return byteArray;
        }

        private char _GetCharFor(byte @byte)
        {
            if (@byte <= 9)
                return (char)('0' + @byte);
            else
                return (char)('A' + @byte - 10);
        }

        private byte _GetByteFor(char @char)
        {
            if ('0' <= @char && @char <= '9')
                return (byte)(@char - '0');
            if ('A' <= @char && @char <= 'F')
                return (byte)(@char - 'A' + 10);
            if ('a' <= @char && @char <= 'f')
                return (byte)(@char - 'a' + 10);

            throw new ArgumentException("The character does not represent a hexazecimal digit.", nameof(@char));
        }
    }
}