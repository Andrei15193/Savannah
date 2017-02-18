using System;

namespace Savannah.ObjectStoreOperations
{
    internal class InsertOrReplaceObjectStoreOperation
         : ObjectStoreOperation
    {
        internal InsertOrReplaceObjectStoreOperation(object @object)
            : base(@object)
        {
        }

        public sealed override ObjectStoreOperationType OperationType
            => ObjectStoreOperationType.InsertOrReplace;

        internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context)
        {
#if DEBUG
            if (default(ObjectStoreOperationExectionContext).Equals(context))
                throw new InvalidOperationException("Expected " + nameof(context) + " to be initalized.");
#endif
            return context.StorageObjectFactory.CreateFrom(Object);
        }
    }
}