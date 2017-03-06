using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;

namespace Savannah.Tests.Mocks
{
    internal class FileSystemFolderMock
        : IFileSystemFolder
    {
        private static readonly Task _completedTask = Task.FromResult<object>(null);

        private readonly IDictionary<string, MemoryStream> _files = new Dictionary<string, MemoryStream>(StringComparer.OrdinalIgnoreCase);

        private readonly string _name;
        private readonly IDictionary<string, MemoryStream> _parentFolderFiles;

        internal FileSystemFolderMock(string name, IDictionary<string, MemoryStream> parentFolderFiles)
        {
            _name = name;
            _parentFolderFiles = parentFolderFiles;
        }

        public string Path
            => _name;

        public Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name)
        {
            lock (_files)
            {
                if (!_files.ContainsKey(name))
                    _files.Add(name, new MemoryStream());
                return Task.FromResult<IFileSystemFile>(new FileSystemFileMock(name, _files));
            }
        }

        public Task<IFileSystemFile> CreateFileIfNotExistsAsync(string name, CancellationToken cancellationToken)
            => CreateFileIfNotExistsAsync(name);

        public Task<IFileSystemFolder> CreateFolderAsync(string name)
        {
            lock (_files)
            {
                if (_files.ContainsKey(name))
                    throw new InvalidOperationException("Folder or file already exists.");

                _files.Add(name, null);
                return Task.FromResult<IFileSystemFolder>(new FileSystemFolderMock(name, _files));
            }
        }

        public Task<IFileSystemFolder> CreateFolderAsync(string name, CancellationToken cancellationToken)
            => CreateFolderAsync(name);

        public Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name)
        {
            lock (_files)
            {
                MemoryStream fileMemoryStream;
                if (!_files.TryGetValue(name, out fileMemoryStream))
                    _files.Add(name, null);
                else if (fileMemoryStream != null)
                    throw new InvalidOperationException("File with same name already exists.");

                return Task.FromResult<IFileSystemFolder>(new FileSystemFolderMock(name, _files));
            }
        }

        public Task<IFileSystemFolder> CreateFolderIfNotExistsAsync(string name, CancellationToken cancellationToken)
            => CreateFolderIfNotExistsAsync(name);

        public Task DeleteAsync()
        {
            lock (_parentFolderFiles)
                _parentFolderFiles.Remove(_name);
            return _completedTask;
        }

        public Task DeleteAsync(CancellationToken cancellationToken)
            => DeleteAsync();

        public Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync()
            => Task.FromResult(_files
                .Where(file => file.Value != null)
                .Select(file => new FileSystemFileMock(file.Key, _files))
                .ToList()
                .AsEnumerable<IFileSystemFile>());

        public Task<IEnumerable<IFileSystemFile>> GetAllRootFilesAsync(CancellationToken cancellationToken)
            => GetAllRootFilesAsync();

        public Task<IFileSystemFile> TryGetFile(string name)
        {
            lock (_files)
            {
                MemoryStream fileMemoryStream;
                if (!_files.TryGetValue(name, out fileMemoryStream))
                    return Task.FromResult<IFileSystemFile>(null);
                if (fileMemoryStream == null)
                    throw new InvalidOperationException("A folder with the same name exists.");

                return Task.FromResult<IFileSystemFile>(new FileSystemFileMock(name, _files));
            }
        }

        public Task<IFileSystemFile> TryGetFile(string name, CancellationToken cancellationToken)
            => TryGetFile(name);

        public Task<IFileSystemFile> GetExistingFileAsync(string name)
        {
            lock (_files)
            {
                MemoryStream fileMemoryStream;
                if (!_files.TryGetValue(name, out fileMemoryStream))
                    throw new InvalidOperationException("File does not exist.");
                if (fileMemoryStream == null)
                    throw new InvalidOperationException("A folder with the same name exists.");

                return Task.FromResult<IFileSystemFile>(new FileSystemFileMock(name, _files));
            }
        }

        public Task<IFileSystemFile> GetExistingFileAsync(string name, CancellationToken cancellationToken)
            => GetExistingFileAsync(name);

        public Task<IFileSystemFolder> GetExistingFolderAsync(string name)
        {
            lock (_files)
            {
                MemoryStream fileMemoryStream;
                if (!_files.TryGetValue(name, out fileMemoryStream))
                    throw new InvalidOperationException("Folder does not exist.");
                if (fileMemoryStream != null)
                    throw new InvalidOperationException("A file with the same name exists.");

                return Task.FromResult<IFileSystemFolder>(new FileSystemFolderMock(name, _files));
            }
        }

        public Task<IFileSystemFolder> GetExistingFolderAsync(string name, CancellationToken cancellationToken)
            => GetExistingFolderAsync(name);
    }
}