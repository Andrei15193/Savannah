using System;

namespace Savannah.ObjectStoreOperations
{
    internal class InsertOrMergeObjectStoreOperation
        : ObjectStoreOperation
    {
        private static readonly StorageObjectMerger _merger = new StorageObjectMerger();

        internal InsertOrMergeObjectStoreOperation(object @object) : base(@object)
        {
        }

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.InsertOrMerge;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            var storageObject = context.StorageObjectFactory.CreateFrom(Object);
            if (context.ExistingObject != null)
                storageObject = _merger.Merge(context.ExistingObject, storageObject);

            return storageObject;
        }
    }
}