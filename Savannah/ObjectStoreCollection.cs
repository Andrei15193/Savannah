using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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

            await _EnsureCollectionExists(cancellationToken).ConfigureAwait(false);
            var bucketFiles = await _collectionFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
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
            var temporaryFile = await _GetTemporaryFolderAsync(cancellationToken);

            try
            {
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var temporaryFileStream = await temporaryFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var xmlReader = _GetXmlReaderFor(bucketFileStream))
                    using (var xmlWriter = XmlWriter.Create(temporaryFileStream, XmlSettings.WriterSettings))
                    {
                        var storageObject = new StorageObject(partitionKey, rowKey, null);
                        var context = new OperationContext(storageObject, xmlReader, xmlWriter);
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
            var temporaryFile = await _GetTemporaryFolderAsync(cancellationToken);

            try
            {
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                using (var temporaryFileStream = await temporaryFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var xmlReader = _GetXmlReaderFor(bucketFileStream))
                    using (var xmlWriter = XmlWriter.Create(temporaryFileStream, XmlSettings.WriterSettings))
                    {
                        var context = new OperationContext(storageObject, xmlReader, xmlWriter);
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