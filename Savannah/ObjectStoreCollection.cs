using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Savannah.ObjectStoreOperations;
using Savannah.Utilities;
using Windows.Storage;

namespace Savannah
{
    public class ObjectStoreCollection
    {
        private static readonly string _EmptyBucketFileContents = $"<?xml version=\"1.0\" encoding=\"utf-16\"?><{ObjectStoreXmlNameTable.Bucket} />";

        private static XmlReader _GetXmlReaderFor(Stream bucketFileStream)
        => (bucketFileStream.Length == 0
            ? XmlReader.Create(new StringReader(_EmptyBucketFileContents), XmlSettings.ReaderSettings)
            : XmlReader.Create(bucketFileStream, XmlSettings.ReaderSettings));

        private static async Task<IStorageFile> _GetTemporaryFolderAsync(CancellationToken cancellationToken)
        {
            return await ApplicationData
                .Current
                .TemporaryFolder
                .CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.GenerateUniqueName)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
        }

        private readonly Task<IStorageFolder> _dataFolderTask;
        private readonly string _collectionName;
        private readonly IHashProvider _hashProvider;
        private IStorageFolder _collectionFolder;

        internal ObjectStoreCollection(Task<IStorageFolder> dataFolderTask, IHashProvider hashProvider, string collectionName)
        {
#if DEBUG
            if (dataFolderTask == null)
                throw new ArgumentNullException(nameof(dataFolderTask));
            if (hashProvider == null)
                throw new ArgumentNullException(nameof(hashProvider));
            ObjectStoreLimitations.CheckCollectionName(collectionName);
#endif
            _dataFolderTask = dataFolderTask;
            _collectionName = collectionName;
            _hashProvider = hashProvider;
            _collectionFolder = null;
        }

        public string Name
            => _collectionName;

        public Task CreateAsync()
            => CreateAsync(CancellationToken.None);

        public async Task CreateAsync(CancellationToken cancellationToken)
        {
            var dataFolder = await _dataFolderTask.ConfigureAwait(false);
            try
            {
                _collectionFolder = await dataFolder.CreateFolderAsync(_collectionName, CreationCollisionOption.FailIfExists).AsTask(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("The Object Store Collection already exists. To ensure that a collection exists call one of the CreateIfNotExistsAsync overloads.");
            }
        }

        public Task CreateIfNotExistsAsync()
            => CreateIfNotExistsAsync(CancellationToken.None);

        public async Task CreateIfNotExistsAsync(CancellationToken cancellationToken)
        {
            if (_collectionFolder == null)
            {
                var dataFolder = await _dataFolderTask.ConfigureAwait(false);
                _collectionFolder = await dataFolder.CreateFolderAsync(_collectionName, CreationCollisionOption.OpenIfExists).AsTask(cancellationToken).ConfigureAwait(false);
            }
        }

        public Task DeleteAsync()
            => DeleteAsync(CancellationToken.None);

        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            if (_collectionFolder == null)
            {
                IStorageFolder collectionFolder;
                var dataFolder = await _dataFolderTask.ConfigureAwait(false);
                try
                {
                    collectionFolder = await dataFolder.GetFolderAsync(_collectionName).AsTask(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("The Object Store Collection does not exists. To ensure that a collection is removed call one of the DeleteIfExistsAsync overloads.");
                }
                await collectionFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _collectionFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken).ConfigureAwait(false);
                _collectionFolder = null;
            }
        }

        public Task DeleteIfExists()
            => DeleteIfExists(CancellationToken.None);

        public async Task DeleteIfExists(CancellationToken cancellationToken)
        {
            if (_collectionFolder == null)
            {
                var dataFolder = await _dataFolderTask.ConfigureAwait(false);
                var collectionFolder = await dataFolder.CreateFolderAsync(_collectionName, CreationCollisionOption.OpenIfExists).AsTask(cancellationToken).ConfigureAwait(false);
                await collectionFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _collectionFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken).ConfigureAwait(false);
                _collectionFolder = null;
            }
        }

        public Task ExecuteAsync(ObjectStoreOperation operation)
            => ExecuteAsync(operation, CancellationToken.None);

        public async Task ExecuteAsync(ObjectStoreOperation operation, CancellationToken cancellationToken)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
#if DEBUG
            try
            {
                ObjectStoreLimitations.Check(operation.Object);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    "Expected object to follow limitations.",
                    exception);
            }
#endif
            await _ExecuteAsync(operation, cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query) where T : new()
            => QueryAsync<T>(query, CancellationToken.None);

        public async Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query, CancellationToken cancellationToken) where T : new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (query != ObjectStoreQuery.All)
                throw new InvalidOperationException($"Querying is not yet supported, please use {nameof(ObjectStoreQuery)}.{nameof(ObjectStoreQuery.All)} for now.");

            await _EnsureCollectionExists(cancellationToken).ConfigureAwait(false);
            var bucketFiles = await _collectionFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
            var objectsByPartition = await Task.WhenAll(from bucketFile in bucketFiles
                                                        select _GetAllObjectsFromAsync<T>(bucketFile, cancellationToken)).ConfigureAwait(false);

            return objectsByPartition.SelectMany(Enumerable.AsEnumerable).ToList();
        }

        private async Task _EnsureCollectionExists(CancellationToken cancellationToken)
        {
            if (_collectionFolder == null)
            {
                var dataFolder = await _dataFolderTask.ConfigureAwait(false);
                try
                {
                    _collectionFolder = await dataFolder.GetFolderAsync(_collectionName).AsTask(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("The Object Store Collection does not exist. Call one of the CreateAsync or CreateIfNotExistsAsync overloads first.");
                }
            }
        }

        private async Task _ExecuteAsync(ObjectStoreOperation operation, CancellationToken cancellationToken)
        {
            var storageObjectFactory = new StorageObjectFactory(DateTime.UtcNow);

            var bucketFile = await _GetBucketFileForAsync(operation.PartitionKey, cancellationToken).ConfigureAwait(false);
            var temporaryFile = await _GetTemporaryFolderAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var temporaryFileStream = await temporaryFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var xmlReader = _GetXmlReaderFor(bucketFileStream))
                    using (var xmlWriter = XmlWriter.Create(temporaryFileStream, XmlSettings.WriterSettings))
                    {
                        var context = new ObjectStoreOperationContext(storageObjectFactory, xmlReader, xmlWriter);
                        await _ExecuteAsync(operation, context, cancellationToken).ConfigureAwait(false);
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

        private async Task _ExecuteAsync(ObjectStoreOperation operation, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
            await context.XmlWriter.WriteStartDocumentAsync(true).ConfigureAwait(false);
            await context.XmlWriter.WriteBucketStartElementAsync(cancellationToken).ConfigureAwait(false);

            var foundPartition = false;
            while (!foundPartition && context.XmlReader.ReadState != ReadState.EndOfFile)
                if (context.XmlReader.IsOnPartitionElement())
                {
                    var partitionKey = context.XmlReader.GetPartitionKeyAttribute();

                    var partitionKeyComparisonResult = ObjectStoreLimitations.StringComparer.Compare(partitionKey, operation.PartitionKey);
                    if (partitionKeyComparisonResult < 0)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        foundPartition = true;
                        await _ExecuteInPartitionAsync(operation, context, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);

            if (foundPartition)
                while (context.XmlReader.IsOnPartitionElement())
                    await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            else
                await _ExecuteInPartitionAsync(operation, context, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private static async Task _ExecuteInPartitionAsync(ObjectStoreOperation operation, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
            await context.XmlWriter.WritePartitionStartElementAsync(cancellationToken).ConfigureAwait(false);
            await context.XmlWriter.WritePartitionKeyAttriuteAsync(operation.PartitionKey).ConfigureAwait(false);

            var foundRow = false;
            while (!foundRow && !context.XmlReader.IsOnPartitionEndElement() && context.XmlReader.ReadState != ReadState.EndOfFile)
                if (context.XmlReader.IsOnObjectElement())
                {
                    var rowKey = context.XmlReader.GetRowKeyAttribute();

                    var rowKeyComparisonResult = ObjectStoreLimitations.StringComparer.Compare(rowKey, operation.RowKey);
                    if (rowKeyComparisonResult < 0)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        foundRow = true;
                        var existingObject = (
                            rowKeyComparisonResult == 0
                            ? await context.XmlReader.ReadStorageObjectAsync(cancellationToken).ConfigureAwait(false)
                            : null
                        );

                        await operation.ExecuteAsync(existingObject, context, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);

            if (foundRow)
                while (context.XmlReader.IsOnObjectElement())
                    await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            else
                await operation.ExecuteAsync(null, context, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<IEnumerable<T>> _GetAllObjectsFromAsync<T>(StorageFile bucketFile, CancellationToken cancellationToken) where T : new()
        {
            var objects = new List<T>();
            var objectFactory = new ObjectFactory<T>();

            using (var bucketStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
            using (var xmlReader = XmlReader.Create(bucketStream, XmlSettings.ReaderSettings))
                while (xmlReader.ReadState != ReadState.EndOfFile)
                    if (xmlReader.IsOnElement(ObjectStoreXmlNameTable.Object))
                    {
                        var storageObject = await xmlReader.ReadStorageObjectAsync(cancellationToken).ConfigureAwait(false);
                        objects.Add(objectFactory.CreateFrom(storageObject));
                    }
                    else
                        await xmlReader.ReadAsync(cancellationToken);

            return objects;
        }

        private async Task<IStorageFile> _GetBucketFileForAsync(string partitionKey, CancellationToken cancellationToken)
        {
            await _EnsureCollectionExists(cancellationToken).ConfigureAwait(false);
            var partitionKeyHash = _hashProvider.GetHashFor(partitionKey);
            var partitionFile = await _collectionFolder
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