using System;
using System.Collections.Generic;

namespace Savannah.ObjectStoreOperations
{
    internal struct ObjectStoreOperationExectionContext
    {
        internal ObjectStoreOperationExectionContext(StorageObject existingObject, StorageObjectFactory storageObjectFactory, DateTime timestamp, ICollection<object> result)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
            if (result == null)
                throw new ArgumentNullException(nameof(result));
#endif
            ExistingObject = existingObject;
            StorageObjectFactory = storageObjectFactory;
            Timestamp = timestamp;
            Result = result;
        }

        internal StorageObject ExistingObject { get; }

        internal StorageObjectFactory StorageObjectFactory { get; }

        internal DateTime Timestamp { get; }

        internal ICollection<object> Result { get; }
    }
}