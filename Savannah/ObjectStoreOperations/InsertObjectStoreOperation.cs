using System;

namespace Savannah.ObjectStoreOperations
{
    internal class InsertObjectStoreOperation
        : ObjectStoreOperation
    {
        private readonly bool _echoContent;

        internal InsertObjectStoreOperation(object @object, bool echoContent = false)
            : base(@object)
        {
            _echoContent = echoContent;
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

            if (_echoContent)
            {
                if (Metadata.TimestampProperty?.SetMethod.IsPublic ?? false)
                    Metadata.TimestampProperty.SetValue(Object, context.Timestamp);

                context.Result.Add(Object);
            }

            return context.StorageObjectFactory.CreateFrom(Object);
        }
    }
}