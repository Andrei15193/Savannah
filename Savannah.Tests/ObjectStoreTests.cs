using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreTests
    {
        private const string _objectStoreFolderName = nameof(ObjectStoreTests);

        private ObjectStore _ObjectStore { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _ObjectStore = new ObjectStore(_objectStoreFolderName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _ObjectStore = null;
            var localTestFolder = ApplicationData.Current.LocalFolder;
            var objectStoreFolder = Task.Run(localTestFolder.CreateFolderAsync(_objectStoreFolderName, CreationCollisionOption.OpenIfExists).AsTask).Result;

            Task.Run(objectStoreFolder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask).Wait();
        }

        [DataTestMethod]
        [DataRow("collectionName")]
        [DataRow("test")]
        public void TestGettingCollectionWithSameNameReturnsExactSameInstance(string collectionName)
        {
            var collection1 = _ObjectStore.GetCollection(collectionName.ToUpperInvariant());
            var collection2 = _ObjectStore.GetCollection(collectionName.ToLowerInvariant());

            Assert.AreSame(collection1, collection2);
        }
    }
}