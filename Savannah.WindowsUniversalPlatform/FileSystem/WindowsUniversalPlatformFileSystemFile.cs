using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;
using Windows.Storage;

namespace Savannah.WindowsUniversalPlatform.FileSystem
{
    internal class WindowsUniversalPlatformFileSystemFile
        : IFileSystemFile
    {
        private readonly IStorageFile _storageFile;

        public WindowsUniversalPlatformFileSystemFile(IStorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        public string Path
            => _storageFile.Path;

        public Task DeleteAsync()
            => DeleteAsync(CancellationToken.None);

        public Task DeleteAsync(CancellationToken cancellationToken)
            => _storageFile.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask(cancellationToken);

        public Task<Stream> OpenReadAsync()
            => OpenReadAsync(CancellationToken.None);

        public async Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _storageFile.OpenStreamForReadAsync().ConfigureAwait(false);
            }
            finally
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public Task<Stream> OpenWriteAsync()
            => OpenWriteAsync(CancellationToken.None);

        public async Task<Stream> OpenWriteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _storageFile.OpenStreamForWriteAsync().ConfigureAwait(false);
            }
            finally
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public Task ReplaceAsync(IFileSystemFile file)
            => ReplaceAsync(file, CancellationToken.None);

        public Task ReplaceAsync(IFileSystemFile file, CancellationToken cancellationToken)
            => _storageFile.MoveAndReplaceAsync(((WindowsUniversalPlatformFileSystemFile)file)._storageFile).AsTask(cancellationToken);
    }
}