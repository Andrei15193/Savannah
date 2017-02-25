using System;
using System.Collections.Generic;
using System.Linq;

namespace Savannah.ObjectStoreOperations
{
    internal class RetrieveDelegateObjectStoreOperation<T>
        : RetrieveObjectStoreOperation<T>
    {
        private static readonly PropertyValueFactory _PropertyValueFactory = new PropertyValueFactory();

        private readonly ObjectResolver<T> _objectResolver;
        private readonly IEnumerable<string> _propertiesToRetrieve;

        internal RetrieveDelegateObjectStoreOperation(object @object, ObjectResolver<T> objectResolver, IEnumerable<string> propertiesToRetrieve = null)
            : base(@object)
        {
            if (objectResolver == null)
                throw new ArgumentNullException(nameof(objectResolver));

            _objectResolver = objectResolver;
            _propertiesToRetrieve = propertiesToRetrieve;
        }

        protected override T GetObjectFrom(StorageObject storageObject)
        {
            var partitionKey = default(string);
            var rowKey = default(string);
            var timestamp = default(DateTime);

            var properties = storageObject.Properties;

            if (_propertiesToRetrieve != null)
            {
                var propertiesToRetrieveSet = new HashSet<string>(_propertiesToRetrieve, ObjectStoreLimitations.StringComparer);
                properties = properties.Where(property => propertiesToRetrieveSet.Contains(property.Name));

                if (propertiesToRetrieveSet.Contains(nameof(StorageObject.PartitionKey)))
                    partitionKey = storageObject.PartitionKey;
                if (propertiesToRetrieveSet.Contains(nameof(StorageObject.RowKey)))
                    rowKey = storageObject.RowKey;
                if (propertiesToRetrieveSet.Contains(nameof(StorageObject.Timestamp)))
                    timestamp = (DateTime)_PropertyValueFactory.GetPropertyValueFrom(
                        new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, ValueType.DateTime));
            }
            else
            {
                partitionKey = storageObject.PartitionKey;
                rowKey = storageObject.RowKey;
                timestamp = (DateTime)_PropertyValueFactory.GetPropertyValueFrom(
                    new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, ValueType.DateTime));
            }
            var propertyValues = properties.ToDictionary(
                property => property.Name,
                _PropertyValueFactory.GetPropertyValueFrom,
                ObjectStoreLimitations.StringComparer);

            return _objectResolver(partitionKey, rowKey, timestamp, propertyValues);
        }
    }
}