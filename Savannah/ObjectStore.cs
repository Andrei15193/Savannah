using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Savannah.Utilities;
using Windows.Storage;

namespace Savannah
{
    public class ObjectStore
    {
        private static readonly string _EmptyBucketFileContents = $"<?xml version=\"1.0\" encoding=\"utf-16\"?><{ObjectStoreXmlNameTable.Bucket} />";

        private static XmlReader _GetXmlReaderFor(Stream bucketFileStream)
        => (bucketFileStream.Length == 0
            ? XmlReader.Create(new StringReader(_EmptyBucketFileContents), XmlSettings.ReaderSettings)
            : XmlReader.Create(bucketFileStream, XmlSettings.ReaderSettings));

        private sealed class OperationContext
        {
            internal StorageObject StorageObject { get; set; }

            internal XmlReader XmlReader { get; set; }

            internal XmlWriter XmlWriter { get; set; }
        }

        private readonly string _storageFolderName;
        private readonly IHashProvider _hashProvider;

        public ObjectStore(string storageFolderName)
            : this(storageFolderName, new Md5HashProvider())
        {
        }

        internal ObjectStore(string storageFolderName, IHashProvider hashProvider)
        {
            if (hashProvider == null)
                throw new ArgumentNullException(nameof(hashProvider));

            _storageFolderName = storageFolderName;
            _hashProvider = hashProvider;

            Debug.WriteLine($"Object Store Folder: {Path.Combine(ApplicationData.Current.LocalFolder.Path, _storageFolderName)}");
        }

        public Task ExecuteAsync(ObjectStoreOperation operation)
            => ExecuteAsync(operation, CancellationToken.None);

        public async Task ExecuteAsync(ObjectStoreOperation operation, CancellationToken cancellationToken)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            ObjectStoreLimitations.Check(operation.Object);

            switch (operation.OperationType)
            {
                case ObjectStoreOperationType.Insert:
                    await _InsertAsync(operation.Object, cancellationToken).ConfigureAwait(false);
                    break;

                case ObjectStoreOperationType.Delete:
                    await _DeleteAsync(operation.Object, cancellationToken).ConfigureAwait(false);
                    break;

                default:
                    throw _InvalidOperationExceptionFor(operation);
            }
        }

        public Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query) where T : new()
            => QueryAsync<T>(query, CancellationToken.None);

        public async Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query, CancellationToken cancellationToken) where T : new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (query != ObjectStoreQuery.All)
                throw new InvalidOperationException($"Querying is not yet supported, please use {nameof(ObjectStoreQuery)}.{nameof(ObjectStoreQuery.All)} for now.");

            var dataFolder = ApplicationData
                .Current
                .LocalFolder;
            if (!string.IsNullOrWhiteSpace(_storageFolderName))
                dataFolder = await dataFolder
                    .CreateFolderAsync(_storageFolderName, CreationCollisionOption.OpenIfExists)
                    .AsTask(cancellationToken)
                    .ConfigureAwait(false);

            var bucketFiles = await dataFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
            var objectsByPartition = await Task.WhenAll(from bucketFile in bucketFiles
                                                        select _GetAllObjectsFromAsync<T>(bucketFile, cancellationToken)).ConfigureAwait(false);

            return objectsByPartition.SelectMany(Enumerable.AsEnumerable).ToList();
        }

        private static InvalidOperationException _InvalidOperationExceptionFor(ObjectStoreOperation operation)
        {
            string exceptionMessage;

            if (Enum.IsDefined(typeof(ObjectStoreOperationType), operation.OperationType))
                exceptionMessage = $"The {operation.OperationType} operation is not supported.";
            else
                exceptionMessage = $"The given {nameof(ObjectStoreOperationType)} does not exist.";

            return new InvalidOperationException(exceptionMessage);
        }

        private async Task<IEnumerable<T>> _GetAllObjectsFromAsync<T>(StorageFile bucketFile, CancellationToken cancellationToken) where T : new()
        {
            var objects = new List<T>();
            var objectFactory = new ObjectFactory<T>();

            using (var bucketStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
            using (var xmlReader = XmlReader.Create(bucketStream, XmlSettings.ReaderSettings))
                do
                    if (xmlReader.IsOnElement(ObjectStoreXmlNameTable.Object))
                    {
                        var storageObject = await xmlReader.ReadStorageObjectAsync(cancellationToken).ConfigureAwait(false);
                        objects.Add(objectFactory.CreateFrom(storageObject));
                    }
                    else
                        await xmlReader.ReadAsync(cancellationToken);
                while (xmlReader.ReadState != ReadState.EndOfFile);

            return objects;
        }

        private async Task _DeleteAsync(object @object, CancellationToken cancellationToken)
        {
            var metadata = ObjectMetadata.GetFor(@object.GetType());
            var partitionKey = (string)metadata.PartitionKeyProperty.GetValue(@object);
            var rowKey = (string)metadata.RowKeyProperty.GetValue(@object);

            var bucketFile = await _GetBucketFileForAsync(partitionKey, cancellationToken).ConfigureAwait(false);
            var temporaryFile = await ApplicationData
                .Current
                .TemporaryFolder
                .CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.ReplaceExisting)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);

            try
            {
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var temporaryFileStream = await temporaryFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var xmlReader = _GetXmlReaderFor(bucketFileStream))
                    using (var xmlWriter = XmlWriter.Create(temporaryFileStream, XmlSettings.WriterSettings))
                    {
                        var context = new OperationContext
                        {
                            StorageObject = new StorageObject(partitionKey, rowKey, null),
                            XmlReader = xmlReader,
                            XmlWriter = xmlWriter
                        };
                        await _DeleteAsync(context, cancellationToken).ConfigureAwait(false);
                    }
                }

                await temporaryFile.MoveAndReplaceAsync(bucketFile).AsTask().ConfigureAwait(false);
            }
            catch
            {
                await temporaryFile.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().ConfigureAwait(false);
                throw;
            }
        }
        private async Task _DeleteAsync(OperationContext context, CancellationToken cancellationToken)
        {
            await context.XmlWriter.WriteBucketStartElementAsync(cancellationToken).ConfigureAwait(false);

            await _DeleteFromBucketAsync(context, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private static async Task _DeleteFromBucketAsync(OperationContext context, CancellationToken cancellationToken)
        {
            var deleted = false;
            do
                if (context.XmlReader.IsOnPartitionElement())
                {
                    var partitionKey = context.XmlReader.GetPartitionKeyAttribute();
                    var partitionKeyComparison = string.CompareOrdinal(partitionKey, context.StorageObject.PartitionKey);

                    if (partitionKeyComparison == 0)
                    {
                        await context.XmlWriter.WritePartitionStartElementAsync(cancellationToken).ConfigureAwait(false);
                        await _DeleteFromPartitionAsync(context, cancellationToken).ConfigureAwait(false);
                        await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);

                        deleted = true;
                    }
                    else
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
            while (context.XmlReader.ReadState != ReadState.EndOfFile);

            if (!deleted)
                throw new InvalidOperationException("The object does not exist, it cannot be removed.");
        }

        private static async Task _DeleteFromPartitionAsync(OperationContext context, CancellationToken cancellationToken)
        {
#if DEBUG
            if (!context.XmlReader.IsOnPartitionElement())
                throw new InvalidOperationException("Expected reader to be on Partition element.");
#endif
            if (context.XmlReader.IsEmptyElement)
                throw new InvalidOperationException("The object does not exist, it cannot be removed.");

            var deleted = false;
            do
                if (context.XmlReader.IsOnObjectElement())
                    if (deleted)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        var rowKey = context.XmlReader.GetRowKeyAttribute();
                        var rowKeyComparison = string.CompareOrdinal(rowKey, context.StorageObject.RowKey);
                        if (rowKeyComparison == 0)
                        {
                            deleted = true;
                            await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
                        }
                        else if (rowKeyComparison > 0)
                            throw new InvalidOperationException("The object does not exist, it cannot be removed.");
                        else
                            await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
            while (!context.XmlReader.IsOnPartitionEndElement());
        }

        private async Task _InsertAsync(object @object, CancellationToken cancellationToken)
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.UtcNow);
            var storageObject = storageObjectFactory.CreateFrom(@object);

            var bucketFile = await _GetBucketFileForAsync(storageObject.PartitionKey, cancellationToken).ConfigureAwait(false);
            var temporaryFile = await ApplicationData
                .Current
                .TemporaryFolder
                .CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.ReplaceExisting)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);

            try
            {
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var temporaryFileStream = await temporaryFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var xmlReader = _GetXmlReaderFor(bucketFileStream))
                    using (var xmlWriter = XmlWriter.Create(temporaryFileStream, XmlSettings.WriterSettings))
                    {
                        var context = new OperationContext
                        {
                            StorageObject = storageObject,
                            XmlReader = xmlReader,
                            XmlWriter = xmlWriter
                        };
                        await _InsertAsync(context, cancellationToken).ConfigureAwait(false);
                    }
                }

                await temporaryFile.MoveAndReplaceAsync(bucketFile).AsTask().ConfigureAwait(false);
            }
            catch
            {
                await temporaryFile.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().ConfigureAwait(false);
                throw;
            }
        }

        private async Task _InsertAsync(OperationContext context, CancellationToken cancellationToken)
        {
            await context.XmlWriter.WriteStartElementAsync(null, ObjectStoreXmlNameTable.Bucket, null).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            await _InsertInBucketAsync(context, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        private async Task _InsertInBucketAsync(OperationContext context, CancellationToken cancellationToken)
        {
            var inserted = false;
            do
                if (context.XmlReader.IsOnPartitionElement())
                {
                    var partitionKey = context.XmlReader.GetPartitionKeyAttribute();
                    var partitionKeyComparison = string.CompareOrdinal(partitionKey, context.StorageObject.PartitionKey);

                    if (partitionKeyComparison < 0)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        inserted = true;
                        if (partitionKeyComparison > 0)
                            await _WritePartitionAsync(context, cancellationToken).ConfigureAwait(false);
                        else if (context.XmlReader.IsEmptyElement)
                        {
                            await _WritePartitionAsync(context, cancellationToken).ConfigureAwait(false);
                            await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            await context.XmlWriter.WritePartitionStartElementAsync(cancellationToken).ConfigureAwait(false);
                            await context.XmlWriter.WritePartitionKeyAttriuteAsync(partitionKey, cancellationToken).ConfigureAwait(false);

                            await _InsertInPartitionAsync(context, cancellationToken);

                            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
            while (!inserted && context.XmlReader.ReadState != ReadState.EndOfFile);

            if (inserted)
                while (context.XmlReader.IsOnPartitionElement())
                    await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            else
                await _WritePartitionAsync(context, cancellationToken).ConfigureAwait(false);
        }

        private async Task _InsertInPartitionAsync(OperationContext context, CancellationToken cancellationToken)
        {
#if DEBUG
            if (!context.XmlReader.IsOnPartitionElement())
                throw new InvalidOperationException("Expected to be on a Partition element.");
#endif
            var inserted = false;

            do
                if (context.XmlReader.IsOnObjectElement())
                {
                    var rowKey = context.XmlReader.GetRowKeyAttribute();

                    var rowKeyComparison = string.CompareOrdinal(rowKey, context.StorageObject.RowKey);
                    if (rowKeyComparison < 0)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        inserted = true;
                        if (rowKeyComparison > 0)
                            await context.XmlWriter.WriteAsync(context.StorageObject, cancellationToken).ConfigureAwait(false);
                        else
                            throw new InvalidOperationException(
                                "Duplicate PartitionKey and RowKey pair. Any stored object must be uniquely identifiable by its partition and row keys.");
                    }
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
            while (!inserted && !context.XmlReader.IsOnPartitionEndElement());

            if (inserted)
                while (context.XmlReader.IsOnObjectElement())
                    await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            else
                await context.XmlWriter.WriteAsync(context.StorageObject, cancellationToken).ConfigureAwait(false);

#if DEBUG
            if (!context.XmlReader.IsOnPartitionEndElement())
                throw new InvalidOperationException("Expected to be on Partition end element.");
#endif
            await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task _WritePartitionAsync(OperationContext context, CancellationToken cancellationToken)
        {
            await context.XmlWriter.WritePartitionStartElementAsync(cancellationToken).ConfigureAwait(false);
            await context.XmlWriter.WritePartitionKeyAttriuteAsync(context.StorageObject.PartitionKey, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteAsync(context.StorageObject, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<StorageFile> _GetBucketFileForAsync(string partitionKey, CancellationToken cancellationToken)
        {
            var dataFolder = ApplicationData
                .Current
                .LocalFolder;
            if (!string.IsNullOrWhiteSpace(_storageFolderName))
                dataFolder = await dataFolder
                      .CreateFolderAsync(_storageFolderName, CreationCollisionOption.OpenIfExists)
                      .AsTask(cancellationToken)
                      .ConfigureAwait(false);

            var partitionKeyHash = _hashProvider.GetHashFor(partitionKey);
            var partitionFile = await dataFolder
                .CreateFileAsync(partitionKeyHash, CreationCollisionOption.OpenIfExists)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);

            return partitionFile;
        }

        private static async Task _WriteStorageObjectInPartitionAsync(XmlWriter xmlWriter, StorageObject storageObject, CancellationToken cancellationToken)
        {
            await xmlWriter.WriteStartElementAsync(null, ObjectStoreXmlNameTable.Partition, null).ConfigureAwait(false);
            await xmlWriter.WriteAttributeStringAsync(null, ObjectStoreXmlNameTable.PartitionKey, null, storageObject.PartitionKey).ConfigureAwait(false);

            await xmlWriter.WriteAsync(storageObject, cancellationToken).ConfigureAwait(false);

            await xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}