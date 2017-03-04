using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreTests
        : UnitTest
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
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreCollectionNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGettingCollectionWithSameNameReturnsExactSameInstance()
        {
            var row = GetRow<ObjectStoreCollectionNamesRow>();

            var collection1 = _ObjectStore.GetCollection(row.Value.ToUpperInvariant());
            var collection2 = _ObjectStore.GetCollection(row.Value.ToLowerInvariant());

            Assert.AreSame(collection1, collection2);
        }
    }
}