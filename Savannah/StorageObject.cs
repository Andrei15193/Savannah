using System.Collections.Generic;
using System.Linq;

namespace Savannah
{
    internal class StorageObject
    {
        internal StorageObject(string partitionKey, string rowKey, string timestamp)
            : this(partitionKey, rowKey, timestamp, null)
        {
        }

        internal StorageObject(string partitionKey, string rowKey, string timestamp, IEnumerable<StorageObjectProperty> properties)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
            Properties = (properties ?? Enumerable.Empty<StorageObjectProperty>());
        }
        internal StorageObject(string partitionKey, string rowKey, string timestamp, params StorageObjectProperty[] properties)
            : this(partitionKey, rowKey, timestamp, properties.AsEnumerable())
        {
        }

        internal string PartitionKey { get; }

        internal string RowKey { get; }

        internal string Timestamp { get; }

        internal IEnumerable<StorageObjectProperty> Properties { get; }
    }
}