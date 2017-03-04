using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;

namespace Savannah.Tests.Mocks
{
    internal class FileSystemMock
        : IFileSystem
    {
        private readonly IDictionary<string, MemoryStream> _temporaryFiles = new Dictionary<string, MemoryStream>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, MemoryStream> _rootFiles = new Dictionary<string, MemoryStream>(StringComparer.OrdinalIgnoreCase);

        public string RootPath
            => ".";

        public Task<IFileSystemFolder> GetRootFolderAsync()
            => Task.FromResult<IFileSystemFolder>(new FileSystemFolderMock(".", _rootFiles));

        public Task<IFileSystemFile> GetTemporaryFileAsync()
            => Task.FromResult<IFileSystemFile>(new FileSystemFileMock(Guid.NewGuid().ToString(), _temporaryFiles));

        public Task<IFileSystemFile> GetTemporaryFileAsync(CancellationToken cancellationToken)
            => GetTemporaryFileAsync();
    }
}