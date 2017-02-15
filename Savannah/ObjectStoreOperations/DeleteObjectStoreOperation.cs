using System;

namespace Savannah.ObjectStoreOperations
{
    internal class DeleteObjectStoreOperation
        : ObjectStoreOperation
    {
        internal DeleteObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Delete;

        internal override StorageObject GetStorageObjectFrom(StorageObject existingObject, StorageObjectFactory storageObjectFactory)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
#endif
            if (existingObject == null)
                throw new InvalidOperationException("The object does not exist, it cannot be removed.");

            return null;
        }
    }
}