using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;
using Windows.Storage;

namespace Savannah.WindowsUniversalPlatform.FileSystem
{
    internal class WindowsUniversalPlatformFileSystemFolder
        : IFileSystemFolder
    {
        private readonly IStorageFolder _storageFolder;

        internal WindowsUniversalPlatformFileSystemFolder(IStorageFolder localFolder)
        {
#if DEBUG
            if (localFolder == null)
                throw new ArgumentNullException(nameof(localFolder));
#endif
            _storageFolder = localFolder;
        }

        public string Path
            => _storageFolder.Path;

        public Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name)
            => CreateFileIfNotExistsAsync(name, CancellationToken.None);

        public async Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name, CancellationToken cancellationToken)
        {
            var storageFile = await _storageFolder
                .CreateFileAsync(name, CreationCollisionOption.OpenIfExists)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFile(storageFile);
        }

        public Task<IFileSystemFolder> CreateFolderAsync(string name)
            => CreateFolderAsync(name, CancellationToken.None);

        public async Task<IFileSystemFolder> CreateFolderAsync(string name, CancellationToken cancellationToken)
        {
            var storageFolder = await _storageFolder
                .CreateFolderAsync(name, CreationCollisionOption.FailIfExists)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFolder(storageFolder);
        }

        public Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name)
            => CreateFolderIfNotExistsAsync(name, CancellationToken.None);

        public async Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name, CancellationToken cancellationToken)
        {
            var storageFolder = await _storageFolder
                .CreateFolderAsync(name, CreationCollisionOption.OpenIfExists)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFolder(storageFolder);
        }

        public Task DeleteAsync()
            => DeleteAsync(CancellationToken.None);

        public Task DeleteAsync(CancellationToken cancellationToken)
            => _storageFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken);

        public Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync()
            => GetAllRootFilesAsync(CancellationToken.None);

        public async Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync(CancellationToken cancellationToken)
        {
            var storageFiles = await _storageFolder.GetFilesAsync().AsTask(cancellationToken).ConfigureAwait(false);
            return storageFiles.Select(storageFile => new WindowsUniversalPlatformFileSystemFile(storageFile)).ToList();
        }

        public Task<IFileSystemFile> GetExistingFileAsync(string name)
            => GetExistingFileAsync(name, CancellationToken.None);

        public async Task<IFileSystemFile> GetExistingFileAsync(string name, CancellationToken cancellationToken)
        {
            var storageFile = await _storageFolder
                .GetFileAsync(name)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFile(storageFile);
        }

        public Task<IFileSystemFolder> GetExistingFolderAsync(string name)
            => GetExistingFolderAsync(name, CancellationToken.None);

        public async Task<IFileSystemFolder> GetExistingFolderAsync(string name, CancellationToken cancellationToken)
        {
            var storageFolder = await _storageFolder
                .GetFolderAsync(name)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFolder(storageFolder);
        }
    }
}