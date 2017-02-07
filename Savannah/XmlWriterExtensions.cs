using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Savannah
{
    internal static class XmlWriterExtensions
    {
        internal static Task WriteStartElementAsync(this XmlWriter xmlWriter, string localName)
            => WriteStartElementAsync(xmlWriter, localName, CancellationToken.None);

        internal static async Task WriteStartElementAsync(this XmlWriter xmlWriter, string localName, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            await xmlWriter.WriteStartElementAsync(null, localName, null).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        internal static Task WriteAttributeStringAsync(this XmlWriter xmlWriter, string localName, string value)
            => WriteAttributeStringAsync(xmlWriter, localName, value, CancellationToken.None);

        internal static async Task WriteAttributeStringAsync(this XmlWriter xmlWriter, string localName, string value, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            await xmlWriter.WriteAttributeStringAsync(null, localName, null, value).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        internal static Task WriteBucketStartElementAsync(this XmlWriter xmlWriter)
            => WriteBucketStartElementAsync(xmlWriter, CancellationToken.None);

        internal static Task WriteBucketStartElementAsync(this XmlWriter xmlWriter, CancellationToken cancellationToken)
            => WriteStartElementAsync(xmlWriter, ObjectStoreXmlNameTable.Bucket);

        internal static Task WritePartitionStartElementAsync(this XmlWriter xmlWriter)
            => WritePartitionStartElementAsync(xmlWriter, CancellationToken.None);

        internal static Task WritePartitionStartElementAsync(this XmlWriter xmlWriter, CancellationToken cancellationToken)
            => WriteStartElementAsync(xmlWriter, ObjectStoreXmlNameTable.Partition);

        internal static Task WriteObjectStartElementAsync(this XmlWriter xmlWriter)
            => WriteObjectStartElementAsync(xmlWriter, CancellationToken.None);

        internal static Task WriteObjectStartElementAsync(this XmlWriter xmlWriter, CancellationToken cancellationToken)
            => WriteStartElementAsync(xmlWriter, ObjectStoreXmlNameTable.Object);

        internal static Task WritePartitionKeyAttriuteAsync(this XmlWriter xmlWriter, string partitionKey)
            => WritePartitionKeyAttriuteAsync(xmlWriter, partitionKey, CancellationToken.None);

        internal static Task WritePartitionKeyAttriuteAsync(this XmlWriter xmlWriter, string partitionKey, CancellationToken cancellationToken)
            => WriteAttributeStringAsync(xmlWriter, ObjectStoreXmlNameTable.PartitionKey, partitionKey, cancellationToken);

        internal static Task WriteRowKeyAttriuteAsync(this XmlWriter xmlWriter, string rowKey)
            => WriteRowKeyAttriuteAsync(xmlWriter, rowKey, CancellationToken.None);

        internal static Task WriteRowKeyAttriuteAsync(this XmlWriter xmlWriter, string rowKey, CancellationToken cancellationToken)
            => WriteAttributeStringAsync(xmlWriter, ObjectStoreXmlNameTable.RowKey, rowKey, cancellationToken);

        internal static Task WriteTimestampKeyAttriuteAsync(this XmlWriter xmlWriter, string timestamp)
            => WriteTimestampKeyAttriuteAsync(xmlWriter, timestamp, CancellationToken.None);

        internal static Task WriteTimestampKeyAttriuteAsync(this XmlWriter xmlWriter, string timestamp, CancellationToken cancellationToken)
            => WriteAttributeStringAsync(xmlWriter, ObjectStoreXmlNameTable.Timestamp, timestamp, cancellationToken);

        internal static Task WriteStorageObjectPropertyTypeAttriuteAsync(this XmlWriter xmlWriter, StorageObjectPropertyType type)
            => WriteStorageObjectPropertyTypeAttriuteAsync(xmlWriter, type, CancellationToken.None);

        internal static Task WriteStorageObjectPropertyTypeAttriuteAsync(this XmlWriter xmlWriter, StorageObjectPropertyType type, CancellationToken cancellationToken)
            => WriteAttributeStringAsync(xmlWriter, ObjectStoreXmlNameTable.Type, type.ToString(), cancellationToken);

        internal static Task WriteStorageObjectPropertyValueAttriuteAsync(this XmlWriter xmlWriter, string value)
            => WriteStorageObjectPropertyValueAttriuteAsync(xmlWriter, value, CancellationToken.None);

        internal static Task WriteStorageObjectPropertyValueAttriuteAsync(this XmlWriter xmlWriter, string value, CancellationToken cancellationToken)
            => WriteAttributeStringAsync(xmlWriter, ObjectStoreXmlNameTable.Value, value.ToString(), cancellationToken);

        internal static Task WriteNodeAsync(this XmlWriter xmlWriter, XmlReader xmlReader)
            => WriteNodeAsync(xmlWriter, xmlReader, CancellationToken.None);

        internal static async Task WriteNodeAsync(this XmlWriter xmlWriter, XmlReader xmlReader, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            await xmlWriter.WriteNodeAsync(xmlReader, true).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        internal static async Task WriteEndElementAsync(this XmlWriter xmlWriter, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
#endif
            await xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        internal static Task WriteAsync(this XmlWriter xmlWriter, StorageObject storageObject)
            => WriteAsync(xmlWriter, storageObject, CancellationToken.None);

        internal static async Task WriteAsync(this XmlWriter xmlWriter, StorageObject storageObject, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (storageObject == null)
                throw new ArgumentNullException(nameof(storageObject));
#endif
            await xmlWriter.WriteObjectStartElementAsync(cancellationToken).ConfigureAwait(false);

            if (storageObject.PartitionKey != null)
                await xmlWriter.WritePartitionKeyAttriuteAsync(storageObject.PartitionKey, cancellationToken).ConfigureAwait(false);
            if (storageObject.RowKey != null)
                await xmlWriter.WriteRowKeyAttriuteAsync(storageObject.RowKey, cancellationToken).ConfigureAwait(false);
            if (storageObject.Timestamp != null)
                await xmlWriter.WriteTimestampKeyAttriuteAsync(storageObject.Timestamp, cancellationToken).ConfigureAwait(false);

            foreach (var storageObjectProperty in storageObject.Properties)
                await xmlWriter.WriteAsync(storageObjectProperty, cancellationToken).ConfigureAwait(false);

            await xmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        internal static Task WriteAsync(this XmlWriter xmlWriter, StorageObjectProperty storageObjectProperty)
            => WriteAsync(xmlWriter, storageObjectProperty, CancellationToken.None);

        internal static async Task WriteAsync(this XmlWriter xmlWriter, StorageObjectProperty storageObjectProperty, CancellationToken cancellationToken)
        {
#if DEBUG
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (storageObjectProperty == null)
                throw new ArgumentNullException(nameof(storageObjectProperty));
#endif
            await xmlWriter.WriteStartElementAsync(storageObjectProperty.Name, cancellationToken).ConfigureAwait(false);

            await xmlWriter.WriteStorageObjectPropertyTypeAttriuteAsync(storageObjectProperty.Type, cancellationToken).ConfigureAwait(false);
            if (storageObjectProperty.Value != null)
                await xmlWriter.WriteStorageObjectPropertyValueAttriuteAsync(storageObjectProperty.Value, cancellationToken).ConfigureAwait(false);

            await xmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}