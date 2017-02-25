using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Savannah
{
    public static class ObjectStoreLimitations
    {
        internal static int DateTimeSize { get; } = 64;

        internal static int GuidSize { get; } = 128;

        public static DateTime MinimumDateTimeValue { get; } = new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime MaximumDateTimeValue { get; } = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);

        public static int MaximumPropertyNameLength { get; } = 255;

        public static int MaximumPartitionKeyLength { get; } = ((1024 * sizeof(byte)) / sizeof(char));

        public static int MaximumRowKeyLength { get; } = ((1024 * sizeof(byte)) / sizeof(char));

        public static int MaximumStringLength { get; } = ((64 * 1024 * sizeof(byte)) / sizeof(char));

        public static int MaximumByteArrayLength { get; } = (64 * 1024 * sizeof(byte));

        public static int MaximumObjectSizeInBytes { get; } = (1024 * 1024 * sizeof(byte));

        public static int MaximumBatchSizeInBytes { get; } = (4 * 1024 * 1024 * sizeof(byte));

        public static int MaximumOperationsInBatch { get; } = 100;

        public static StringComparer StringComparer { get; } = StringComparer.Ordinal;

        public static StringComparer CollectionNameComparer { get; } = StringComparer.OrdinalIgnoreCase;

        public static Regex AcceptedPropertyNamePattern { get; } = new Regex(
            "^(?!xml)[_a-z][_a-z0-9]{0,254}$",
            (RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));

        public static Regex AcceptedTableNamePattern { get; } = new Regex(
            "^[a-z][a-z0-9]{2,62}$",
            (RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));

        public static int MaximumNumberOfProperties { get; } = 255;

        private static readonly IEnumerable<char> _forbiddenNonControlCharacters = new List<char> { '/', '\\', '#', '?' };

        public static IEnumerable<Type> SupportedPropertyTypes { get; } = new HashSet<Type>
        {
            typeof(byte[]),
            typeof(bool),
            typeof(DateTime),
            typeof(double),
            typeof(Guid),
            typeof(int),
            typeof(long),
            typeof(string)
        };

        internal static void CheckCollectionName(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !AcceptedTableNamePattern.IsMatch(value))
                throw new InvalidOperationException(
                    "Table names can contain only alphanumeric characters. They may not start with a number, are at least 3 characters short and at most 63 charachers long.");
        }

        internal static void Check(ObjectMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            if (!metadata.ReadableProperties.Concat(metadata.WritableProperties).All(property => SupportedPropertyTypes.Contains(property.PropertyType)))
                throw new InvalidOperationException(
                    "Only properties of type byte[], bool, DateTime, double, Guid, int, long and string are supported.");

            if (metadata.PartitionKeyProperty != null && metadata.PartitionKeyProperty.PropertyType != typeof(string))
                throw new InvalidOperationException(
                    "The PartitionKey property must be of type string.");

            if (metadata.RowKeyProperty != null && metadata.RowKeyProperty.PropertyType != typeof(string))
                throw new InvalidOperationException(
                    "The RowKey property must be of type string.");

            if (metadata.TimestampProperty != null && metadata.TimestampProperty.PropertyType != typeof(DateTime))
                throw new InvalidOperationException(
                    "The Timestamp property must be of type DateTime.");

            if (metadata.ReadableProperties.Count() > MaximumNumberOfProperties || metadata.WritableProperties.Count() > MaximumNumberOfProperties)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The maximum number of allowed properties on a single object is {0:n0}. This includes PartitionKey, RowKey and Timestamp properties.",
                        MaximumNumberOfProperties));

            foreach (var property in metadata.ReadableProperties.Concat(metadata.WritableProperties))
                CheckPropertyName(property.Name);
        }

        internal static void CheckPropertyName(string propertyName)
        {
            if (propertyName == null || !AcceptedPropertyNamePattern.IsMatch(propertyName))
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Property names can contain only alphanumeric and underscore characters. They can be only {0:n0} characters long and may not begin with XML (in any casing). Property names themselves are case sensitive.",
                        MaximumPropertyNameLength));
        }

        internal static void CheckPartitionKey(string value)
        {
            if (value == null)
                throw new InvalidOperationException(
                    "Null values are not supported for partition keys.");

            if (value.Length > MaximumPartitionKeyLength)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The maximum supported length of a partition key is {0:n0} characters.",
                        MaximumPartitionKeyLength));

            if (!value.All(_IsValidKeyCharacter))
                throw new InvalidOperationException(
                    @"A partition key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        internal static void CheckRowKey(string value)
        {
            if (value == null)
                throw new InvalidOperationException(
                    "Null values are not supported for row keys.");

            if (value.Length > MaximumRowKeyLength)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The maximum supported length of a row key is {0:n0} characters.",
                        MaximumRowKeyLength));

            if (!value.All(_IsValidKeyCharacter))
                throw new InvalidOperationException(
                    @"A row key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        internal static void CheckValue(DateTime dateTime)
        {
            if (dateTime < MinimumDateTimeValue || MaximumDateTimeValue < dateTime)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The supported date time range is between {0:g} and {1:g}.",
                        MinimumDateTimeValue,
                        MaximumDateTimeValue));
        }

        internal static void CheckValue(byte[] byteArray)
        {
            if (byteArray != null && byteArray.Length > MaximumByteArrayLength)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The maximum supported length of a byte array is {0:n0} bytes.",
                        MaximumByteArrayLength));
        }

        internal static void CheckValue(string @string)
        {
            if (@string != null && @string.Length > MaximumStringLength)
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The maximum supported length of a string is {0:n0} characters.",
                        MaximumStringLength));
        }

        internal static void Check(object @object)
        {
            int size;
            _Check(@object, out size);
        }

        internal static void Check(ObjectStoreBatchOperation batchOperation)
        {
            if (batchOperation == null)
                throw new ArgumentNullException(nameof(batchOperation));

            if (batchOperation.Count > MaximumOperationsInBatch)
                throw new InvalidOperationException(
                   string.Format(
                       CultureInfo.InvariantCulture,
                       "The maximum number of allowed operations in a batch is {0:n0}.",
                       MaximumOperationsInBatch));

            var totalSize = 0;
            string partitionKey = null;
            foreach (var operation in batchOperation)
            {
                int size;
                _Check(operation.Object, out size);
                totalSize += size;
                if (totalSize > MaximumBatchSizeInBytes)
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "The maximum supported total size of objects in a batch operation is {0:n0} bytes.",
                            MaximumBatchSizeInBytes));

                if (partitionKey == null)
                    partitionKey = operation.PartitionKey;
                else if (!StringComparer.Equals(partitionKey, operation.PartitionKey))
                    throw new InvalidOperationException(
                        "All operations in a batch must be carried out on objectes having the same PartitionKey.");

                if (operation.OperationType == ObjectStoreOperationType.Retrieve && batchOperation.Count > 1)
                    throw new InvalidOperationException(
                        "A batch may contain a retrieve operation when it is the only one.");
            }
        }

        private static void _CheckMetadataForObjectStoreOperation(ObjectMetadata metadata)
        {
            if (metadata.PartitionKeyProperty == null)
                throw new InvalidOperationException("The given object must expose a readable PartitionKey property of type string.");
            if (metadata.RowKeyProperty == null)
                throw new InvalidOperationException("The given object must expose a readable RowKey property of type string.");
        }

        private static void _Check(object @object, out int objectSize)
        {
            if (@object == null)
                throw new InvalidOperationException("Cannot operate with null value.");

            var metadata = ObjectMetadata.GetFor(@object.GetType());
            _CheckMetadataForObjectStoreOperation(metadata);

            var partitionKey = (string)metadata.PartitionKeyProperty.GetValue(@object);
            CheckPartitionKey(partitionKey);

            var rowKey = (string)metadata.RowKeyProperty.GetValue(@object);
            CheckRowKey(rowKey);

            objectSize = ((partitionKey.Length * sizeof(char)) + (rowKey.Length * sizeof(char)) + DateTimeSize);
            foreach (var property in metadata.ReadableProperties)
            {
                var value = property.GetValue(@object);

                if (property.PropertyType == typeof(byte[]))
                {
                    var binaryValue = (byte[])value;
                    CheckValue(binaryValue);
                    objectSize += ((binaryValue?.Length ?? 0) * sizeof(byte));
                }
                else if (property.PropertyType == typeof(bool))
                    objectSize += sizeof(bool);
                else if (property.PropertyType == typeof(DateTime))
                {
                    var dateTimeValue = (DateTime)value;
                    CheckValue(dateTimeValue);
                    objectSize += DateTimeSize;
                }
                else if (property.PropertyType == typeof(double))
                    objectSize += sizeof(double);
                else if (property.PropertyType == typeof(Guid))
                    objectSize += GuidSize;
                else if (property.PropertyType == typeof(int))
                    objectSize += sizeof(int);
                else if (property.PropertyType == typeof(long))
                    objectSize += sizeof(long);
                else
                {
                    var stringValue = (string)value;
                    CheckValue(stringValue);
                    objectSize += ((stringValue?.Length ?? 0) * sizeof(char));
                }

                if (objectSize > MaximumObjectSizeInBytes)
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "The maximum supported size of an object is {0:n0} bytes.",
                            MaximumObjectSizeInBytes));
            }
        }

        private static bool _IsValidKeyCharacter(char character)
            => !char.IsControl(character) && !_forbiddenNonControlCharacters.Contains(character);
    }
}