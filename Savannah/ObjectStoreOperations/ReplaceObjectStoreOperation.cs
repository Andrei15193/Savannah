using System;

namespace Savannah.ObjectStoreOperations
{
    internal class ReplaceObjectStoreOperation
        : ObjectStoreOperation
    {
        internal ReplaceObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Replace;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            if (context.ExistingObject == null)
                throw new InvalidOperationException("The object does not exist, it cannot be replaced.");

            return context.StorageObjectFactory.CreateFrom(Object);
        }
    }
}