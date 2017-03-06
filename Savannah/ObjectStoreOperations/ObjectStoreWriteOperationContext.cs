using System;
using System.Collections.Generic;
using System.Xml;

namespace Savannah.ObjectStoreOperations
{
    internal sealed class ObjectStoreWriteOperationContext
    {
        internal ObjectStoreWriteOperationContext(StorageObjectFactory storageObjectFactory, DateTime timestamp, XmlReader xmlReader, XmlWriterProvider xmlWriterProvider)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (xmlWriterProvider == null)
                throw new ArgumentNullException(nameof(xmlWriterProvider));
#endif
            StorageObjectFactory = storageObjectFactory;
            Timestamp = timestamp;
            XmlReader = xmlReader;
            XmlWriterProvider = xmlWriterProvider;
            Result = new List<object>();
        }

        internal StorageObjectFactory StorageObjectFactory { get; }

        internal XmlReader XmlReader { get; }

        internal XmlWriterProvider XmlWriterProvider { get; }

        internal DateTime Timestamp { get; }

        internal ICollection<object> Result { get; }
    }
}