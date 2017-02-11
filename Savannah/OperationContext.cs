using System;
using System.Xml;

namespace Savannah
{
    internal sealed class OperationContext
    {
        internal OperationContext(StorageObject storageObject, XmlReader xmlReader, XmlWriter xmlWriter)
        {
#if DEBUG
            if (storageObject == null)
                throw new ArgumentNullException(nameof(storageObject));
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            StorageObject = storageObject;
            XmlReader = xmlReader;
            XmlWriter = xmlWriter;
        }

        internal StorageObject StorageObject { get; }

        internal XmlReader XmlReader { get; }

        internal XmlWriter XmlWriter { get; }
    }
}