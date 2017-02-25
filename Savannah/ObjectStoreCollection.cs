using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Savannah.ObjectStoreOperations;
using Savannah.Query;
using Savannah.Utilities;
using Savannah.Xml;
using Windows.Storage;

namespace Savannah
{
    public class ObjectStoreCollection
    {
        private class BucketFilter
        {
            private readonly Func<StorageObject, bool> _objectPredicate;

            internal BucketFilter(IStorageFile file, IEnumerable<string> partitionKeys, IEnumerable<string> rowKeys, Func<StorageObject, bool> objectPredicate)
            {
#if DEBUG
                if (file == null)
                    throw new ArgumentNullException(nameof(file));
                if (objectPredicate == null)
                    throw new ArgumentNullException(nameof(objectPredicate));
#endif
                File = file;
                PartitionKeys = partitionKeys;
                RowKeys = rowKeys;
                _objectPredicate = objectPredicate;
            }

            internal IStorageFile File { get; }

            internal IEnumerable<string> PartitionKeys { get; }

            internal IEnumerable<string> RowKeys { get; }

            internal bool IsMatch(StorageObject storageObject)
                => _objectPredicate(storageObject);
        }

        private static readonly string _EmptyBucketFileContents = $"<?xml version=\"1.0\" encoding=\"utf-16\"?><{ObjectStoreXmlNameTable.Bucket} />";

        private static dynamic _ExpandoObjectFactory(StorageObject storageObject, IEnumerable<string> properties)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();

            var propertyValueFactory = new PropertyValueFactory();
            var propertyNamesToRetrieve = (properties == null ? null : new HashSet<string>(properties, ObjectStoreLimitations.StringComparer));

            if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.PartitionKey)) ?? true)
                expandoObject[nameof(StorageObject.PartitionKey)] = storageObject.PartitionKey;
            if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.RowKey)) ?? true)
                expandoObject[nameof(StorageObject.RowKey)] = storageObject.RowKey;
            if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.Timestamp)) ?? true)
            {
                var timestampProperty = new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, ValueType.DateTime);
                expandoObject[nameof(StorageObject.Timestamp)] = propertyValueFactory.GetPropertyValueFrom(timestampProperty);
            }

            foreach (var property in storageObject.Properties)
                if (propertyNamesToRetrieve?.Contains(property.Name) ?? true)
                    expandoObject.Add(property.Name, propertyValueFactory.GetPropertyValueFrom(property));

            return (expandoObject.Keys.Any() ? (dynamic)expandoObject : null);
        }

        private Func<StorageObject, IEnumerable<string>, T> _GetObjectFactoryFrom<T>(ObjectResolver<T> objectResolver)
            => (storageObject, properties) =>
            {
                var propertyNamesToRetrieve = (properties == null ? null : new HashSet<string>(properties, ObjectStoreLimitations.StringComparer));

                var partitionKey = default(string);
                var rowKey = default(string);
                var timestamp = default(DateTime);

                var propertyValueFactory = new PropertyValueFactory();

                if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.PartitionKey)) ?? true)
                    partitionKey = storageObject.PartitionKey;
                if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.RowKey)) ?? true)
                    rowKey = storageObject.RowKey;
                if (propertyNamesToRetrieve?.Contains(nameof(StorageObject.Timestamp)) ?? true)
                {
                    var timestampProperty = new StorageObjectProperty(nameof(StorageObject.Timestamp), storageObject.Timestamp, ValueType.DateTime);
                    timestamp = (DateTime)propertyValueFactory.GetPropertyValueFrom(timestampProperty);
                }

                var storageObjectProperties = storageObject.Properties;
                if (propertyNamesToRetrieve != null)
                    storageObjectProperties = storageObjectProperties.Where(property => propertyNamesToRetrieve.Contains(property.Name));

                return objectResolver(
                    partitionKey,
                    rowKey,
                    timestamp,
                    storageObjectProperties.ToDictionary(property => property.Name, propertyValueFactory.GetPropertyValueFrom));
            };

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

        public Task<IEnumerable<object>> ExecuteAsync(ObjectStoreOperation operation)
            => ExecuteAsync(operation, CancellationToken.None);

        public async Task<IEnumerable<object>> ExecuteAsync(ObjectStoreOperation operation, CancellationToken cancellationToken)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return await _ExecuteAsync(new ObjectStoreBatchOperation { operation }, cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<object>> ExecuteAsync(ObjectStoreBatchOperation batchOperation)
            => ExecuteAsync(batchOperation, CancellationToken.None);

        public async Task<IEnumerable<object>> ExecuteAsync(ObjectStoreBatchOperation batchOperation, CancellationToken cancellationToken)
        {
            if (batchOperation == null)
                throw new ArgumentNullException(nameof(batchOperation));

            return await _ExecuteAsync(batchOperation, cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(ObjectStoreQuery query)
            => QueryAsync(query, CancellationToken.None);
        public async Task<IEnumerable<dynamic>> QueryAsync(ObjectStoreQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var objects = await _ExecuteQueryAsync(query, _ExpandoObjectFactory, cancellationToken).ConfigureAwait(false);
            return objects;
        }

        public Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query)
            where T : new()
            => QueryAsync<T>(query, CancellationToken.None);

        public async Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query, CancellationToken cancellationToken)
            where T : new()
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var objectFactory = new ObjectFactory<T>();
            var objects = await _ExecuteQueryAsync(query, objectFactory.CreateFrom, cancellationToken).ConfigureAwait(false);
            return objects;
        }

        public Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query, ObjectResolver<T> objectResolver)
            => QueryAsync(query, objectResolver, CancellationToken.None);

        public async Task<IEnumerable<T>> QueryAsync<T>(ObjectStoreQuery query, ObjectResolver<T> objectResolver, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (objectResolver == null)
                throw new ArgumentNullException(nameof(objectResolver));

            var objects = await _ExecuteQueryAsync(query, _GetObjectFactoryFrom(objectResolver), cancellationToken).ConfigureAwait(false);
            return objects;
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

        private async Task<IEnumerable<object>> _ExecuteAsync(ObjectStoreBatchOperation batchOperation, CancellationToken cancellationToken)
        {
            ObjectStoreLimitations.Check(batchOperation);

            if (batchOperation.Count == 0)
                return Enumerable.Empty<object>();

            var timestamp = DateTime.UtcNow;
            var batchPartitionKey = batchOperation.First().PartitionKey;
            var storageObjectFactory = new StorageObjectFactory(timestamp);

            IEnumerable<object> result;
            var bucketFile = await _GetBucketFileForAsync(batchPartitionKey, cancellationToken).ConfigureAwait(false);
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
                        var context = new ObjectStoreOperationContext(storageObjectFactory, timestamp, xmlReader, xmlWriter);
                        await _ExecuteAsync(batchOperation, context, cancellationToken).ConfigureAwait(false);
                        result = context.Result;
                    }
                }

                await temporaryFile.MoveAndReplaceAsync(bucketFile).AsTask().ConfigureAwait(false);
            }
            catch
            {
                await temporaryFile.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().ConfigureAwait(false);
                throw;
            }
            finally
            {
                var isBucketFileEmpty = true;
                using (var bucketFileStream = await bucketFile.OpenStreamForReadAsync().ConfigureAwait(false))
                    isBucketFileEmpty = (bucketFileStream.Length == 0);
                if (isBucketFileEmpty)
                    await bucketFile.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().ConfigureAwait(false);
            }

            return result;
        }

        private static async Task _ExecuteAsync(ObjectStoreBatchOperation batchOperation, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
            var batchPartitionKey = batchOperation.First().PartitionKey;

            await context.XmlWriter.WriteStartDocumentAsync(true).ConfigureAwait(false);
            await context.XmlWriter.WriteBucketStartElementAsync(cancellationToken).ConfigureAwait(false);

            var foundPartition = false;
            while (!foundPartition && context.XmlReader.ReadState != ReadState.EndOfFile)
                if (context.XmlReader.IsOnPartitionElement())
                {
                    var partitionKey = context.XmlReader.GetPartitionKeyAttribute();

                    var partitionKeyComparisonResult = ObjectStoreLimitations.StringComparer.Compare(partitionKey, batchPartitionKey);
                    if (partitionKeyComparisonResult < 0)
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                    else
                    {
                        foundPartition = true;
                        await _ExecuteInPartitionAsync(batchOperation, context, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                    await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);

            if (foundPartition)
                while (context.XmlReader.IsOnPartitionElement())
                    await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            else
                await _ExecuteInPartitionAsync(batchOperation, context, cancellationToken).ConfigureAwait(false);

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private static async Task _ExecuteInPartitionAsync(ObjectStoreBatchOperation batchOperation, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
            var batchPartitionKey = batchOperation.First().PartitionKey;

            await context.XmlWriter.WritePartitionStartElementAsync(cancellationToken).ConfigureAwait(false);
            await context.XmlWriter.WritePartitionKeyAttriuteAsync(batchPartitionKey).ConfigureAwait(false);

            using (var operation = batchOperation.OrderBy(storeOperation => storeOperation.RowKey, ObjectStoreLimitations.StringComparer).GetEnumerator())
            {
                var operationHasValue = operation.MoveNext();
                while (operationHasValue && !context.XmlReader.IsOnPartitionEndElement() && context.XmlReader.ReadState != ReadState.EndOfFile)
                    if (context.XmlReader.IsOnObjectElement())
                    {
                        var rowKey = context.XmlReader.GetRowKeyAttribute();

                        var rowKeyComparisonResult = ObjectStoreLimitations.StringComparer.Compare(rowKey, operation.Current.RowKey);
                        if (rowKeyComparisonResult < 0)
                            await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
                        else
                        {
                            var existingObject = (
                                rowKeyComparisonResult == 0
                                ? await context.XmlReader.ReadStorageObjectAsync(cancellationToken).ConfigureAwait(false)
                                : null
                            );

                            operationHasValue = await _ExecuteOnSameRowKey(operation, existingObject, context, cancellationToken).ConfigureAwait(false);
                        }
                    }
                    else
                        await context.XmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);

                if (operationHasValue)
                    do
                        operationHasValue = await _ExecuteOnSameRowKey(operation, null, context, cancellationToken).ConfigureAwait(false);
                    while (operationHasValue);
                else
                    while (context.XmlReader.IsOnObjectElement())
                        await context.XmlWriter.WriteNodeAsync(context.XmlReader, cancellationToken).ConfigureAwait(false);
            }

            await context.XmlWriter.WriteEndElementAsync(cancellationToken).ConfigureAwait(false);
        }

        private static async Task<bool> _ExecuteOnSameRowKey(IEnumerator<ObjectStoreOperation> operation, StorageObject existingObject, ObjectStoreOperationContext context, CancellationToken cancellationToken)
        {
            var hasValue = true;
            string previousRowKey;

            do
            {
                previousRowKey = operation.Current.RowKey;
                existingObject = operation.Current.GetStorageObjectFrom(
                    new ObjectStoreOperationExectionContext(
                        existingObject,
                        context.StorageObjectFactory,
                        context.Timestamp,
                        context.Result));
                hasValue = operation.MoveNext();
            } while (hasValue && ObjectStoreLimitations.StringComparer.Equals(previousRowKey, operation.Current.RowKey));

            if (existingObject != null)
                await context.XmlWriter.WriteAsync(existingObject).ConfigureAwait(false);

            return hasValue;
        }

        private async Task<IEnumerable<BucketFilter>> _GetBucketFiltersAsync(ObjectStoreQuery query, CancellationToken cancellationToken)
        {
            var predicate = ObjectStoreQueryPredicate.CreateFrom(query.Filter);

            if (predicate.PartitionKeys == null)
            {
                var bucketFiles = await _collectionFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
                return bucketFiles.Select(bucketFile => new BucketFilter(bucketFile, null, predicate.RowKeys, predicate.IsMatch));
            }
            else
                return await Task.WhenAll(predicate
                    .PartitionKeys
                    .GroupBy(_hashProvider.GetHashFor, StringComparer.OrdinalIgnoreCase)
                    .Select(async partitionKeysByBucketName => new BucketFilter(
                        await _collectionFolder.GetFileAsync(partitionKeysByBucketName.Key).AsTask(cancellationToken).ConfigureAwait(false),
                        partitionKeysByBucketName.AsEnumerable(),
                        predicate.RowKeys,
                        predicate.IsMatch)));
        }

        private async Task<IEnumerable<T>> _ExecuteQueryAsync<T>(ObjectStoreQuery query, Func<StorageObject, IEnumerable<string>, T> objectFactory, CancellationToken cancellationToken)
        {
#if DEBUG
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (objectFactory == null)
                throw new ArgumentNullException(nameof(objectFactory));
#endif
            await _EnsureCollectionExists(cancellationToken).ConfigureAwait(false);

            var bucketFilters = await _GetBucketFiltersAsync(query, cancellationToken).ConfigureAwait(false);
            var storageObjects = await _ExecuteQueryAsync(bucketFilters, query.Take, cancellationToken).ConfigureAwait(false);

            var objects = storageObjects
                .Select(storageObject => objectFactory(storageObject, query.Properties))
                .Where(@object => @object != null)
                .ToList();
            return objects;
        }

        private async Task<IEnumerable<StorageObject>> _ExecuteQueryAsync(IEnumerable<BucketFilter> bucketFilters, int? take, CancellationToken cancellationToken)
        {
            var bucketQueryContextList = bucketFilters.ToList();

            IResultBuilder resultBuilder;
            if (bucketQueryContextList.Count == 1)
                resultBuilder = new ResultBuilder(take);
            else
                resultBuilder = new SynchronizedResultBuilder(take);

            await Task
                .WhenAll(bucketFilters.Select(bucketQueryContext => _ExecuteQueryAsync(bucketQueryContext, resultBuilder, cancellationToken)))
                .ConfigureAwait(false);

            return resultBuilder.Result;
        }

        private async Task _ExecuteQueryAsync(BucketFilter bucketFilter, IResultBuilder resultBuilder, CancellationToken cancellationToken)
        {
            var partitionKeys = (bucketFilter.PartitionKeys == null ? null : new HashSet<string>(bucketFilter.PartitionKeys, ObjectStoreLimitations.StringComparer));
            var rowKeys = (bucketFilter.RowKeys == null ? null : new HashSet<string>(bucketFilter.RowKeys, ObjectStoreLimitations.StringComparer));

            var canFindObjects = true;

            using (var bucketStream = await bucketFilter.File.OpenStreamForReadAsync().ConfigureAwait(false))
            using (var xmlReader = XmlReader.Create(bucketStream, XmlSettings.ReaderSettings))
                do
                    if (!xmlReader.IsEmptyElement && xmlReader.IsOnPartitionElement()
                        && (partitionKeys?.Contains(xmlReader.GetPartitionKeyAttribute()) ?? true))
                        do
                            if (xmlReader.IsOnObjectElement() && (rowKeys?.Contains(xmlReader.GetRowKeyAttribute()) ?? true))
                            {
                                var storageObject = await xmlReader.ReadStorageObjectAsync(cancellationToken).ConfigureAwait(false);
                                if (bucketFilter.IsMatch(storageObject))
                                    canFindObjects = resultBuilder.TryAdd(storageObject);
                            }
                            else
                                await xmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
                        while (canFindObjects && !xmlReader.IsOnPartitionEndElement());
                    else
                        await xmlReader.ReadAsync(cancellationToken).ConfigureAwait(false);
                while (canFindObjects && xmlReader.ReadState != ReadState.EndOfFile);
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