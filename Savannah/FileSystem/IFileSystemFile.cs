using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Savannah.FileSystem
{
    internal interface IFileSystemFile
    {
        string Path { get; }

        Task<Stream> OpenReadAsync();

        Task<Stream> OpenReadAsync(CancellationToken cancellationToken);

        Task<Stream> OpenWriteAsync();

        Task<Stream> OpenWriteAsync(CancellationToken cancellationToken);

        Task ReplaceAsync(IFileSystemFile file);

        Task ReplaceAsync(IFileSystemFile file, CancellationToken cancellationToken);

        Task DeleteAsync();

        Task DeleteAsync(CancellationToken cancellationToken);
    }
}