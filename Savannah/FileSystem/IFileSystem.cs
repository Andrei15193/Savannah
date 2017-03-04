using System.Threading;
using System.Threading.Tasks;

namespace Savannah.FileSystem
{
    internal interface IFileSystem
    {
        string RootPath { get; }

        Task<IFileSystemFolder> GetRootFolderAsync();

        Task<IFileSystemFile> GetTemporaryFileAsync();

        Task<IFileSystemFile> GetTemporaryFileAsync(CancellationToken cancellationToken);
    }
}