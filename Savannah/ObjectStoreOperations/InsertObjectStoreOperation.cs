using System;
using System.Threading;
using System.Threading.Tasks;

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

        internal override async Task ExecuteAsync(StorageObject existingObject, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
#if DEBUG
            if (context == null)
                throw new ArgumentNullException(nameof(context));
#endif

            if (existingObject != null)
                throw new InvalidOperationException(
                    "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");

            var storageObject = context.StorageObjectFactory.CreateFrom(Object);
            await context.XmlWriter.WriteAsync(storageObject, cancellationToken).ConfigureAwait(false);
        }
    }
}