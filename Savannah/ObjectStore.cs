using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Savannah.FileSystem;
using Savannah.Utilities;

namespace Savannah
{
    public partial class ObjectStore
    {
        private static async Task<IFileSystemFolder> _GetDataFolderAsync(string dataFolderName, IFileSystem fileSystem)
        {
            var dataFolder = await fileSystem.GetRootFolderAsync().ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(dataFolderName))
                dataFolder = await dataFolder.CreateFolderIfNotExistsAsync(dataFolderName).ConfigureAwait(false);

            return dataFolder;
        }

        private readonly IHashValueProvider _hashValueProvider;
        private readonly IFileSystem _fileSystem;
        private readonly Task<IFileSystemFolder> _dataFolderTask;
        private readonly ConcurrentDictionary<string, ObjectStoreCollection> _collections;

        internal ObjectStore(string storageFolderName, IHashValueProvider hashValueProvider, IFileSystem fileSystem)
        {
#if DEBUG
            if (hashValueProvider == null)
                throw new ArgumentNullException(nameof(hashValueProvider));
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));
#endif
            _hashValueProvider = hashValueProvider;
            _fileSystem = fileSystem;
            _dataFolderTask = _GetDataFolderAsync(storageFolderName, fileSystem);
            _collections = new ConcurrentDictionary<string, ObjectStoreCollection>(ObjectStoreLimitations.CollectionNameComparer);

            Debug.WriteLine($"Object Store Folder: {Path.Combine(fileSystem.RootPath, storageFolderName)}");
        }

        public ObjectStoreCollection GetCollection(string collectionName)
        {
            ObjectStoreLimitations.CheckCollectionName(collectionName);

            return _collections.GetOrAdd(collectionName, delegate { return new ObjectStoreCollection(_fileSystem, _dataFolderTask, _hashValueProvider, collectionName); });
        }
    }
}