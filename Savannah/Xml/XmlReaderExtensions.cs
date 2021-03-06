﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Savannah.Xml
{
    internal static class XmlReaderExtensions
    {
        internal static async Task<bool> ReadAsync(this XmlReader reader, CancellationToken cancellationToken)
        {
            try
            {
                return await reader.ReadAsync();
            }
            finally
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        internal static bool IsOnElement(this XmlReader xmlReader, string localName)
        {
#if DEBUG
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
#endif
            return (xmlReader.NodeType == XmlNodeType.Element && ReferenceEquals(localName, xmlReader.LocalName));
        }

        internal static bool IsOnEndElement(this XmlReader xmlReader, string localName)
            => (xmlReader.NodeType == XmlNodeType.EndElement && ReferenceEquals(localName, xmlReader.LocalName));

        internal static bool IsOnBucketElement(this XmlReader xmlReader)
            => IsOnElement(xmlReader, ObjectStoreXmlNameTable.Bucket);

        internal static bool IsOnBucketEndElement(this XmlReader xmlReader)
            => IsOnEndElement(xmlReader, ObjectStoreXmlNameTable.Bucket);

        internal static bool IsOnPartitionElement(this XmlReader xmlReader)
            => IsOnElement(xmlReader, ObjectStoreXmlNameTable.Partition);

        internal static bool IsOnPartitionEndElement(this XmlReader xmlReader)
            => IsOnEndElement(xmlReader, ObjectStoreXmlNameTable.Partition);

        internal static bool IsOnObjectElement(this XmlReader xmlReader)
            => IsOnElement(xmlReader, ObjectStoreXmlNameTable.Object);

        internal static bool IsOnObjectEndElement(this XmlReader xmlReader)
            => IsOnEndElement(xmlReader, ObjectStoreXmlNameTable.Object);

        internal static string GetPartitionKeyAttribute(this XmlReader xmlReader)
            => xmlReader.GetAttribute(ObjectStoreXmlNameTable.PartitionKey);

        internal static string GetRowKeyAttribute(this XmlReader xmlReader)
            => xmlReader.GetAttribute(ObjectStoreXmlNameTable.RowKey);

        internal static Task<StorageObject> ReadStorageObjectAsync(this XmlReader xmlReader)
            => ReadStorageObjectAsync(xmlReader, CancellationToken.None);

        internal static async Task<StorageObject> ReadStorageObjectAsync(this XmlReader xmlReader, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
            if (!xmlReader.IsOnObjectElement())
                throw new InvalidOperationException("Expected the reader to be positioned on an Object element.");
#endif

            var partitionKey = xmlReader.GetAttribute(ObjectStoreXmlNameTable.PartitionKey);
            var rowKey = xmlReader.GetAttribute(ObjectStoreXmlNameTable.RowKey);
            var timestamp = xmlReader.GetAttribute(ObjectStoreXmlNameTable.Timestamp);

            var properties = await xmlReader.ReadStorageObjectPropertiesAsync(cancellationToken).ConfigureAwait(false);
            await xmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);

            var storageObject = new StorageObject(partitionKey, rowKey, timestamp, properties);
            return storageObject;
        }

        internal static async Task<IEnumerable<StorageObjectProperty>> ReadStorageObjectPropertiesAsync(this XmlReader xmlReader, CancellationToken cancellationToken)
        {
            if (xmlReader.IsEmptyElement)
                return Enumerable.Empty<StorageObjectProperty>();

            var properties = new List<StorageObjectProperty>();

            do
            {
                await xmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    var storageProperty = xmlReader.ReadStorageObjectProperty();
                    if (storageProperty != null)
                        properties.Add(storageProperty);
                }
            } while (!xmlReader.IsOnObjectEndElement());

            return properties;
        }

        internal static StorageObjectProperty ReadStorageObjectProperty(this XmlReader xmlReader)
        {
#if DEBUG
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));
#endif
            StorageObjectProperty property = null;

            var propertyName = xmlReader.LocalName;
            var propertyValue = xmlReader.GetAttribute(ObjectStoreXmlNameTable.Value);
            var propertyTypeName = xmlReader.GetAttribute(ObjectStoreXmlNameTable.Type);
            var propertyType = (
                propertyTypeName == null
                ? ValueType.String
                : (ValueType)Enum.Parse(typeof(ValueType), propertyTypeName, ignoreCase: true));

            property = new StorageObjectProperty(propertyName, propertyValue, propertyType);

            return property;
        }
    }
}