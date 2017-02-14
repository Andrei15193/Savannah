using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public abstract class ObjectStoreOperationTestsTemplate
    {
        private StringBuilder _resultBuilder;

        private StringReader _stringReader;

        internal DateTime Timestamp { get; set; }

        internal ObjectStoreOperationContext Context { get; set; }

        internal string Result
        {
            get
            {
                Task.Run(Context.XmlWriter.FlushAsync).Wait();
                return _resultBuilder.ToString();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Timestamp = DateTime.UtcNow;
            _resultBuilder = new StringBuilder();

            var storageObjectFactory = new StorageObjectFactory(Timestamp);

            _stringReader = new StringReader(string.Empty);
            var xmlReader = XmlReader.Create(_stringReader, XmlSettings.ReaderSettings);
            var xmlWriter = XmlWriter.Create(_resultBuilder, XmlSettings.WriterSettings);

            Context = new ObjectStoreOperationContext(storageObjectFactory, xmlReader, xmlWriter);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Context.XmlWriter.Dispose();
            Context.XmlReader.Dispose();
            _stringReader.Dispose();
        }
    }
}