using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Savannah.Utilities;
using Windows.Storage;

namespace Savannah
{
    public class ObjectStore
    {
        private static readonly IHashProvider _defaultHashProvider = new Md5HashProvider();

        private static async Task<IStorageFolder> _GetDataFolderAsync(string dataFolderName)
        {
            var dataFolder = ApplicationData.Current.LocalFolder;
            if (!string.IsNullOrWhiteSpace(dataFolderName))
                dataFolder = await ApplicationData
                    .Current
                    .LocalFolder
                    .CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists)
                    .AsTask();

            return dataFolder;
        }

        private readonly IHashProvider _hashProvider;
        private readonly Task<IStorageFolder> _dataFolderTask;
        private readonly ConcurrentDictionary<string, ObjectStoreCollection> _collections;

        public ObjectStore(string storageFolderName)
            : this(storageFolderName, _defaultHashProvider)
        {
        }

        internal ObjectStore(string storageFolderName, IHashProvider hashProvider)
        {
#if DEBUG
            if (hashProvider == null)
                throw new ArgumentNullException(nameof(hashProvider));
#endif
            _hashProvider = hashProvider;
            _dataFolderTask = _GetDataFolderAsync(storageFolderName);
            _collections = new ConcurrentDictionary<string, ObjectStoreCollection>(ObjectStoreLimitations.CollectionNameComparer);

            Debug.WriteLine($"Object Store Folder: {Path.Combine(ApplicationData.Current.LocalFolder.Path, storageFolderName)}");
        }

        public ObjectStoreCollection GetCollection(string collectionName)
        {
            ObjectStoreLimitations.CheckCollectionName(collectionName);

            return _collections.GetOrAdd(collectionName, delegate { return new ObjectStoreCollection(_dataFolderTask, _hashProvider, collectionName); });
        }
    }
}