using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Savannah.FileSystem;

namespace Savannah.Tests.Mocks
{
    internal class FileSystemFileMock
        : IFileSystemFile
    {
        private class WriteStreamMock
            : Stream
        {
            private readonly MemoryStream _memoryStream;
            private readonly Action<MemoryStream> _disposeAction;

            internal WriteStreamMock(Action<MemoryStream> disposeAction)
            {
                _memoryStream = new MemoryStream();
                _disposeAction = disposeAction;
            }

            public override bool CanRead
                => false;

            public override bool CanSeek
                => _memoryStream.CanSeek;

            public override bool CanWrite
                => true;

            public override long Length
                => _memoryStream.Length;

            public override long Position
            {
                get
                {
                    return _memoryStream.Position;
                }
                set
                {
                    _memoryStream.Position = value;
                }
            }

            public override void Flush()
                => _memoryStream.Flush();

            public override int Read(byte[] buffer, int offset, int count)
                => _memoryStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin)
                => _memoryStream.Seek(offset, origin);

            public override void SetLength(long value)
                => _memoryStream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count)
                => _memoryStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    var memoryStreamCopy = new MemoryStream();
                    _memoryStream.Seek(0L, SeekOrigin.Begin);
                    _memoryStream.CopyTo(memoryStreamCopy);

                    memoryStreamCopy.Seek(0L, SeekOrigin.Begin);
                    _disposeAction(_memoryStream);
                }
                base.Dispose(disposing);
            }
        }

        private static readonly Task _completedTask = Task.FromResult<object>(null);

        private readonly IDictionary<string, MemoryStream> _files;

        internal FileSystemFileMock(string name, IDictionary<string, MemoryStream> filesInFolder)
        {
            Name = name;
            _files = filesInFolder;
        }

        public string Name { get; }

        public string Path
            => Name;

        public Task DeleteAsync()
        {
            lock (_files)
                _files.Remove(Name);
            return _completedTask;
        }

        public Task DeleteAsync(CancellationToken cancellationToken)
            => DeleteAsync();

        public Task<Stream> OpenReadAsync()
        {
            lock (_files)
            {
                var memoryStreamCopy = new MemoryStream();
                var memoryStream = _files[Name];

                memoryStream.Seek(0L, SeekOrigin.Begin);
                memoryStream.CopyTo(memoryStreamCopy);

                memoryStreamCopy.Seek(0L, SeekOrigin.Begin);
                return Task.FromResult<Stream>(memoryStreamCopy);
            }
        }

        public Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
            => OpenReadAsync();

        public Task<Stream> OpenWriteAsync()
        {
            var writeStreamMock = new WriteStreamMock(
                stream =>
                {
                    lock (_files)
                        _files[Name] = stream;
                });

            return Task.FromResult<Stream>(writeStreamMock);
        }

        public Task<Stream> OpenWriteAsync(CancellationToken cancellationToken)
            => OpenWriteAsync();

        public Task ReplaceAsync(IFileSystemFile file)
        {
            var fileSystemFileMock = (FileSystemFileMock)file;

            if (Name.CompareTo(fileSystemFileMock.Name) <= 0)
                lock (_files)
                {
                    if (!_files.ContainsKey(Name))
                        throw new InvalidOperationException("File no longer exists.");

                    lock (fileSystemFileMock._files)
                        fileSystemFileMock._files[fileSystemFileMock.Name] = _files[Name];
                    _files.Remove(Name);
                }
            else
                lock (fileSystemFileMock._files)
                {
                    if (!_files.ContainsKey(Name))
                        throw new InvalidOperationException("File no longer exists.");

                    lock (_files)
                        fileSystemFileMock._files[fileSystemFileMock.Name] = _files[Name];
                    _files.Remove(Name);
                }

            return _completedTask;
        }

        public Task ReplaceAsync(IFileSystemFile file, CancellationToken cancellationToken)
            => ReplaceAsync(file);
    }
}