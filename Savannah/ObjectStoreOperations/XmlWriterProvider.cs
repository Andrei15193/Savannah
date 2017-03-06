using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Savannah.FileSystem;
using Savannah.Xml;

namespace Savannah.ObjectStoreOperations
{
    internal sealed class XmlWriterProvider
        : IDisposable
    {
        private volatile IFileSystemFile _temporaryFile;
        private volatile Stream _temporaryFileStream;
        private readonly Lazy<Task<XmlWriter>> _xmlWriter;

        internal XmlWriterProvider(IFileSystem fileSystem)
        {
#if DEBUG
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));
#endif
            _temporaryFile = null;
            _temporaryFileStream = null;
            _xmlWriter = new Lazy<Task<XmlWriter>>(
                async () =>
                {
                    _temporaryFile = await fileSystem.GetTemporaryFileAsync().ConfigureAwait(false);
                    _temporaryFileStream = await _temporaryFile.OpenWriteAsync().ConfigureAwait(false);

                    var xmlWriter = XmlWriter.Create(_temporaryFileStream, XmlSettings.WriterSettings);

                    await xmlWriter.WriteStartDocumentAsync(true).ConfigureAwait(false);
                    await xmlWriter.WriteBucketStartElementAsync().ConfigureAwait(false);

                    return xmlWriter;
                });
        }

        public void Dispose()
        {
            if (_xmlWriter.IsValueCreated)
            {
                if (!_xmlWriter.Value.IsCompleted)
                    Task.Run(() => _xmlWriter.Value).Wait();

                Task.Run(_xmlWriter.Value.Result.WriteEndElementAsync).Wait();
                _xmlWriter.Value.Result.Dispose();

                _temporaryFileStream.Dispose();
            }
        }

        internal Task<XmlWriter> GetXmlWriterAsync()
            => _xmlWriter.Value;

        internal Task<IFileSystemFile> TryGetTemporaryFileAsync()
            => Task.FromResult(_temporaryFile);
    }
}