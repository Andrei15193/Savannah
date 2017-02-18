using System;

namespace Savannah.ObjectStoreOperations
{
    internal abstract class RetrieveObjectStoreOperation<T>
        : ObjectStoreOperation
    {
        protected RetrieveObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.Retrieve;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            if (context.ExistingObject == null)
                throw new InvalidOperationException("The object does not exist, it cannot be retrieved.");

            var @object = GetObjectFrom(context.ExistingObject);
            context.Result.Add(@object);

            return context.ExistingObject;
        }

        protected abstract T GetObjectFrom(StorageObject storageObject);
    }
}