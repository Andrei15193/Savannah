using System;
using System.Diagnostics;

namespace Savannah
{
    public class ObjectStoreOperation
    {
        public static ObjectStoreOperation Insert(object @object)
            => new ObjectStoreOperation(@object, ObjectStoreOperationType.Insert);

        public static ObjectStoreOperation Delete(object @object)
            => new ObjectStoreOperation(@object, ObjectStoreOperationType.Delete);

        private ObjectStoreOperation(object @object, ObjectStoreOperationType operationType)
        {
            Debug.Assert(
                Enum.IsDefined(typeof(ObjectStoreOperationType), operationType),
                $"The given {nameof(ObjectStoreOperationType)} does not exists.");

            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            Object = @object;
            OperationType = operationType;
        }

        public object Object { get; }

        public ObjectStoreOperationType OperationType { get; }
    }
}