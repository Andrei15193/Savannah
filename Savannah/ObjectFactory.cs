using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Savannah
{
    internal class ObjectFactory<T>
        where T : new()
    {
        private static readonly PropertyValueFactory _PropertyValueFactory = new PropertyValueFactory();

        private readonly ObjectMetadata _metadata;

        internal ObjectFactory()
        {
            _metadata = ObjectMetadata.GetFor(typeof(T));
        }

        internal T CreateFrom(StorageObject storageObject, IEnumerable<string> propertiesToRetrieve = null)
        {
            if (storageObject == null)
                throw new ArgumentNullException(nameof(storageObject));

            var @object = new T();
            var storageProperties = _GetStoragePropertiesFrom(storageObject, propertiesToRetrieve);

            using (var objectProperty = _metadata.WritableProperties.GetEnumerator())
            using (var storageProperty = storageProperties.GetEnumerator())
            {
                var hasObjectProperty = objectProperty.MoveNext();
                var hasStorageProperty = storageProperty.MoveNext();

                while (hasObjectProperty && hasStorageProperty)
                {
                    var propertyNameComparison = ObjectStoreLimitations.StringComparer.Compare(objectProperty.Current.Name, storageProperty.Current.Name);

                    if (propertyNameComparison < 0)
                        hasObjectProperty = objectProperty.MoveNext();
                    else if (propertyNameComparison > 0)
                        hasStorageProperty = storageProperty.MoveNext();
                    else
                    {
                        var value = _PropertyValueFactory.GetPropertyValueFrom(storageProperty.Current);

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

        private static IEnumerable<StorageObjectProperty> _GetStoragePropertiesFrom(StorageObject storageObject, IEnumerable<string> propertiesToRetrieve)
        {
            var storageProperties = storageObject
                .Properties
                .Concat(new[] {
                    new StorageObjectProperty(nameof(StorageObject.PartitionKey), storageObject.PartitionKey, StorageObjectPropertyType.String),
                    new StorageObjectProperty(nameof(StorageObject.RowKey), storageObject.RowKey, StorageObjectPropertyType.String),
                    new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, StorageObjectPropertyType.DateTime)
                }.Where(property => property.Value != null));

            if (propertiesToRetrieve != null)
            {
                var propertiesToRetrieveSet = new HashSet<string>(propertiesToRetrieve, ObjectStoreLimitations.StringComparer);
                if (propertiesToRetrieveSet.Count > 0)
                    storageProperties = storageProperties.Where(property => propertiesToRetrieveSet.Contains(property.Name));
            }

            storageProperties = storageProperties.OrderBy(storageProperty => storageProperty.Name, ObjectStoreLimitations.StringComparer);
            return storageProperties;
        }
    }
}