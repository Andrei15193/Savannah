﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace Savannah
{
    internal sealed class ObjectStoreOperationContext
    {
        internal ObjectStoreOperationContext(StorageObjectFactory storageObjectFactory, XmlReader xmlReader, XmlWriter xmlWriter)
        {
#if DEBUG
            if (storageObjectFactory == null)
                throw new ArgumentNullException(nameof(storageObjectFactory));
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            StorageObjectFactory = storageObjectFactory;
            XmlReader = xmlReader;
            XmlWriter = xmlWriter;
            Result = new List<object>();
        }

        internal StorageObjectFactory StorageObjectFactory { get; }

        internal XmlReader XmlReader { get; }

        internal XmlWriter XmlWriter { get; }

        internal ICollection<object> Result { get; }
    }
}