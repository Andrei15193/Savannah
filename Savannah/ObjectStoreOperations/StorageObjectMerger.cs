using System;
using System.Collections.Generic;
using System.Linq;

namespace Savannah.ObjectStoreOperations
{
    internal class StorageObjectMerger
    {
        internal StorageObject Merge(StorageObject oldObject, StorageObject newObject)
        {
#if DEBUG
            if (oldObject == null)
                throw new ArgumentNullException(nameof(oldObject));
            if (newObject == null)
                throw new ArgumentNullException(nameof(newObject));
            if (!ObjectStoreLimitations.StringComparer.Equals(oldObject.PartitionKey, newObject.PartitionKey))
                throw new InvalidOperationException("The PartitionKey must be the same for both old and new objects.");
            if (!ObjectStoreLimitations.StringComparer.Equals(oldObject.RowKey, newObject.RowKey))
                throw new InvalidOperationException("The RowKey must be the same for both old and new objects.");
#endif
            var properties = _Merge(oldObject.Properties, newObject.Properties);

            return new StorageObject(oldObject.PartitionKey, oldObject.RowKey, newObject.Timestamp, properties);
        }

        private static IEnumerable<StorageObjectProperty> _Merge(IEnumerable<StorageObjectProperty> oldProperties, IEnumerable<StorageObjectProperty> newProperties)
        {
            var properties = new List<StorageObjectProperty>();

            using (var oldProperty = oldProperties.OrderBy(property => property.Name, ObjectStoreLimitations.StringComparer).GetEnumerator())
            using (var newProperty = newProperties.OrderBy(property => property.Name, ObjectStoreLimitations.StringComparer).GetEnumerator())
            {
                var hasOldProperty = oldProperty.MoveNext();
                var hasNewProperty = newProperty.MoveNext();

                while (hasOldProperty && hasNewProperty)
                {
                    var nameComparison = ObjectStoreLimitations.StringComparer.Compare(oldProperty.Current.Name, newProperty.Current.Name);

                    if (nameComparison == 0)
                    {
                        properties.Add(newProperty.Current);
                        hasOldProperty = oldProperty.MoveNext();
                        hasNewProperty = newProperty.MoveNext();
                    }
                    else if (nameComparison < 0)
                    {
                        properties.Add(oldProperty.Current);
                        hasOldProperty = oldProperty.MoveNext();
                    }
                    else
                    {
                        properties.Add(newProperty.Current);
                        hasNewProperty = newProperty.MoveNext();
                    }
                }

                while (hasOldProperty)
                {
                    properties.Add(oldProperty.Current);
                    hasOldProperty = oldProperty.MoveNext();
                }

                while (hasNewProperty)
                {
                    properties.Add(newProperty.Current);
                    hasNewProperty = newProperty.MoveNext();
                }
            }

            return properties;
        }
    }
}