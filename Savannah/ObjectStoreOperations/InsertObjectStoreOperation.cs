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

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Insert;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            if (context.ExistingObject != null)
                throw new InvalidOperationException(
                    "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");

            return context.StorageObjectFactory.CreateFrom(Object);
        }
    }
}