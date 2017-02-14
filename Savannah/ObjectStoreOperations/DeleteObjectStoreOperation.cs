using System;
using System.Threading;
using System.Threading.Tasks;

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

        internal override async Task ExecuteAsync(StorageObject existingObject, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
#if DEBUG
            if (context == null)
                throw new ArgumentNullException(nameof(context));
#endif
            if (existingObject == null)
                throw new InvalidOperationException("The object does not exist, it cannot be removed.");

            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}