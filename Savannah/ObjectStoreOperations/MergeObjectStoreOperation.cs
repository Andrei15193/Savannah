using System;

namespace Savannah.ObjectStoreOperations
{
    internal class MergeObjectStoreOperation
        : ObjectStoreOperation
    {
        private static readonly StorageObjectMerger _merger = new StorageObjectMerger();

        internal MergeObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Merge;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            if (context.ExistingObject == null)
                throw new InvalidOperationException("The object does not exist, it cannot be merged.");

            var newStorageObject = context.StorageObjectFactory.CreateFrom(Object);
            var mergedStorageObject = _merger.Merge(context.ExistingObject, newStorageObject);

            return mergedStorageObject;
        }
    }
}