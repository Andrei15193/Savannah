using System.Collections.Generic;

namespace Savannah.ObjectStoreOperations
{
    internal class RetrievePocoObjectStoreOperation<T>
        : RetrieveObjectStoreOperation<T>
        where T : new()
    {
        private static readonly ObjectFactory<T> _objectFactory = new ObjectFactory<T>();

        private readonly IEnumerable<string> _propertiesToRetrieve;

        internal RetrievePocoObjectStoreOperation(object @object, IEnumerable<string> propertiesToRetrieve = null)
            : base(@object)
        {
            _propertiesToRetrieve = propertiesToRetrieve;
        }

        protected override T GetObjectFrom(StorageObject storageObject)
            => _objectFactory.CreateFrom(storageObject, _propertiesToRetrieve);
    }
}