using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Savannah.ObjectStoreOperations
{
    internal class RetrieveDynamicObjectStoreOperation
        : RetrieveDelegateObjectStoreOperation<dynamic>
    {
        public RetrieveDynamicObjectStoreOperation(object @object, IEnumerable<string> propertiesToRetrieve = null)
            : base(@object, _ExpandoObjectResolver, propertiesToRetrieve)
        {
        }

        private static dynamic _ExpandoObjectResolver(string partitionKey, string rowKey, DateTime timestamp, IReadOnlyDictionary<string, object> properyValues)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();

            expandoObject[nameof(StorageObject.PartitionKey)] = partitionKey;
            expandoObject[nameof(StorageObject.RowKey)] = rowKey;
            expandoObject[nameof(StorageObject.Timestamp)] = timestamp;
            foreach (var property in properyValues)
                expandoObject.Add(property);

            return (dynamic)expandoObject;
        }
    }
}