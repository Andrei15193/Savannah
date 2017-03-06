using System;
using System.Collections.Generic;
using System.Xml;

namespace Savannah.ObjectStoreOperations
{
    internal class ObjectStoreReadOperationContext
    {
        internal ObjectStoreReadOperationContext(StorageObjectFactory storageObjectFactory, DateTime timestamp, XmlReader xmlReader)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
#endif
            StorageObjectFactory = storageObjectFactory;
            Timestamp = timestamp;
            XmlReader = xmlReader;
            Result = new List<object>();
        }

        internal StorageObjectFactory StorageObjectFactory { get; }

        internal XmlReader XmlReader { get; }

        internal DateTime Timestamp { get; }

        internal ICollection<object> Result { get; }
    }
}