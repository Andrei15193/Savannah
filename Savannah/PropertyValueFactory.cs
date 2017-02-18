using System;
using System.Globalization;
using System.Xml;

namespace Savannah
{
    internal class PropertyValueFactory
    {
        internal object GetPropertyValueFrom(StorageObjectProperty storageObjectProperty)
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
                    if ('z'.Equals(char.ToLowerInvariant(value[value.Length - 1])))
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

        private static byte[] _ConvertTypeByteArray(string value)
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

        private static char _GetCharFor(byte @byte)
        {
            if (@byte <= 9)
                return (char)('0' + @byte);
            else
                return (char)('A' + @byte - 10);
        }

        private static byte _GetByteFor(char @char)
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