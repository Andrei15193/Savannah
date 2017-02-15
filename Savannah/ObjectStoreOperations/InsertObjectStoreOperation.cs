using System;

namespace Savannah.ObjectStoreOperations
{
    internal class InsertObjectStoreOperation
        : ObjectStoreOperation
    {
        internal InsertObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Insert;

        internal override StorageObject GetStorageObjectFrom(StorageObject existingObject, StorageObjectFactory storageObjectFactory)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
#endif
            if (existingObject != null)
                throw new InvalidOperationException(
                    "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");

            return storageObjectFactory.CreateFrom(Object);
        }
    }
}