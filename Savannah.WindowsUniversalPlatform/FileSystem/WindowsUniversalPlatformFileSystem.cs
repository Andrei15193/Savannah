using System;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;
using Windows.Storage;

namespace Savannah.WindowsUniversalPlatform.FileSystem
{
    internal class WindowsUniversalPlatformFileSystem
        : IFileSystem
    {
        public string RootPath
            => ApplicationData.Current.LocalFolder.Path;

        public Task<IFileSystemFolder> GetRootFolderAsync()
            => Task.FromResult<IFileSystemFolder>(new WindowsUniversalPlatformFileSystemFolder(ApplicationData.Current.LocalFolder));

        public Task<IFileSystemFile> GetTemporaryFileAsync()
            => GetTemporaryFileAsync(CancellationToken.None);

        public async Task<IFileSystemFile> GetTemporaryFileAsync(CancellationToken cancellationToken)
        {
            var temporaryStorageFile = await ApplicationData
                .Current
                .TemporaryFolder
                .CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.GenerateUniqueName)
                .AsTask(cancellationToken)
                .ConfigureAwait(false);
            return new WindowsUniversalPlatformFileSystemFile(temporaryStorageFile);
        }
    }
}