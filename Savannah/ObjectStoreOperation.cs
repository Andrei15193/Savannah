using System;
using System.Threading;
using System.Threading.Tasks;
using Savannah.ObjectStoreOperations;

namespace Savannah
{
    public abstract class ObjectStoreOperation
    {
        public static ObjectStoreOperation Insert(object @object)
            => new InsertObjectStoreOperation(@object);

        public static ObjectStoreOperation Delete(object @object)
            => new DeleteObjectStoreOperation(@object);

        internal ObjectStoreOperation(object @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            Object = @object;

            var metadata = ObjectMetadata.GetFor(@object.GetType());
            PartitionKey = (string)metadata?.PartitionKeyProperty?.GetValue(@object);
            RowKey = (string)metadata?.RowKeyProperty?.GetValue(@object);
        }

        public object Object { get; }

        public abstract ObjectStoreOperationType OperationType { get; }

        internal string PartitionKey { get; }

        internal string RowKey { get; }

        internal Task ExecuteAsync(StorageObject existingObject, ObjectStoreOperationContext context)
            => ExecuteAsync(existingObject, context, CancellationToken.None);

        internal abstract Task ExecuteAsync(StorageObject existingObject, ObjectStoreOperationContext context, CancellationToken cancellationToken);
    }
}