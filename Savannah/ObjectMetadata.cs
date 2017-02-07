using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Savannah
{
    internal sealed class ObjectMetadata
    {
        private static readonly ConcurrentDictionary<Type, ObjectMetadata> _metadataCache = new ConcurrentDictionary<Type, ObjectMetadata>();

        internal static ObjectMetadata GetFor<T>()
            => GetFor(typeof(T));

        internal static ObjectMetadata GetFor(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _metadataCache.GetOrAdd(
                type,
                objectType =>
                {
                    var metadata = new ObjectMetadata(objectType);
                    ObjectStoreLimitations.Check(metadata);
                    return metadata;
                });
        }

        private const string _partitionKeyPropertyName = "PartitionKey";
        private const string _rowKeyPropertyName = "RowKey";
        private const string _timestampPropertyName = "Timestamp";

        private ObjectMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var readableProperties = new List<PropertyInfo>();
            var writableProperties = new List<PropertyInfo>();

            foreach (var property in type.GetRuntimeProperties())
                if (property.GetIndexParameters().Length == 0)
                {
                    var propertyAdded = false;
                    if (_IsNonStaticPublic(property.GetMethod))
                    {
                        readableProperties.Add(property);
                        propertyAdded = true;
                    }
                    if (_IsNonStaticPublic(property.SetMethod))
                    {
                        writableProperties.Add(property);
                        propertyAdded = true;
                    }

                    if (propertyAdded)
                        if (_partitionKeyPropertyName.Equals(property.Name, StringComparison.Ordinal))
                            PartitionKeyProperty = property;
                        else if (_rowKeyPropertyName.Equals(property.Name, StringComparison.Ordinal))
                            RowKeyProperty = property;
                        else if (_timestampPropertyName.Equals(property.Name, StringComparison.Ordinal))
                            TimestampProperty = property;
                }

            readableProperties.Sort((left, right) => StringComparer.Ordinal.Compare(left.Name, right.Name));
            writableProperties.Sort((left, right) => StringComparer.Ordinal.Compare(left.Name, right.Name));

            ReadableProperties = readableProperties;
            WritableProperties = writableProperties;
        }

        private static bool _IsNonStaticPublic(MethodInfo methodInfo)
            => (methodInfo != null && methodInfo.IsPublic && !methodInfo.IsStatic);

        internal PropertyInfo PartitionKeyProperty { get; }

        internal PropertyInfo RowKeyProperty { get; }

        internal PropertyInfo TimestampProperty { get; }

        internal IEnumerable<PropertyInfo> ReadableProperties { get; }

        internal IEnumerable<PropertyInfo> WritableProperties { get; }
    }
}