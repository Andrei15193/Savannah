using System;
using System.Collections.Generic;

namespace Savannah
{
    internal struct ObjectStoreOperationExectionContext
    {
        internal ObjectStoreOperationExectionContext(StorageObject existingObject, StorageObjectFactory storageObjectFactory, ICollection<object> result)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
            if (result == null)
                throw new ArgumentNullException(nameof(result));
#endif
            ExistingObject = existingObject;
            StorageObjectFactory = storageObjectFactory;
            Result = result;
        }

        internal StorageObject ExistingObject { get; }

        internal StorageObjectFactory StorageObjectFactory { get; }

        internal ICollection<object> Result { get; }
    }
}