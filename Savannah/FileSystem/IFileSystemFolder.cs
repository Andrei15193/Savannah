using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Savannah.FileSystem
{
    internal interface IFileSystemFolder
    {
        string Path { get; }

        Task<IFileSystemFolder> GetExistingFolderAsync(string name);

        Task<IFileSystemFolder> GetExistingFolderAsync(string name, CancellationToken cancellationToken);

        Task<IFileSystemFolder> CreateFolderAsync(string name);

        Task<IFileSystemFolder> CreateFolderAsync(string name, CancellationToken cancellationToken);

        Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name);

        Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name, CancellationToken cancellationToken);

        Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync();

        Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync(CancellationToken cancellationToken);

        Task<IFileSystemFile> TryGetFile(string name);

        Task<IFileSystemFile> TryGetFile(string name, CancellationToken cancellationToken);

        Task<IFileSystemFile> GetExistingFileAsync(string name);

        Task<IFileSystemFile> GetExistingFileAsync(string name, CancellationToken cancellationToken);

        Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name);

        Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name, CancellationToken cancellationToken);

        Task DeleteAsync();

        Task DeleteAsync(CancellationToken cancellationToken);
    }
}