using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Savannah.ObjectStoreOperations
{
    internal class RetrieveDynamicObjectStoreOperation
        : RetrieveDelegateObjectStoreOperation<dynamic>
    {
        internal RetrieveDynamicObjectStoreOperation(object @object, IEnumerable<string> propertiesToRetrieve = null)
            : base(@object, _ExpandoObjectResolver, propertiesToRetrieve)
        {
        }

        private static dynamic _ExpandoObjectResolver(string partitionKey, string rowKey, DateTime timestamp, IReadOnlyDictionary<string, object> properyValues)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();

            if (partitionKey != null)
                expandoObject[nameof(StorageObject.PartitionKey)] = partitionKey;
            if (rowKey != null)
                expandoObject[nameof(StorageObject.RowKey)] = rowKey;
            if (timestamp != default(DateTime))
                expandoObject[nameof(StorageObject.Timestamp)] = timestamp;

            foreach (var property in properyValues)
                expandoObject.Add(property);

            return (dynamic)expandoObject;
        }
    }
}