using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public abstract class ObjectStoreOperationTestsTemplate
    {
        internal DateTime Timestamp { get; private set; }

        internal StorageObjectFactory StorageObjectFactory { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Timestamp = DateTime.UtcNow;
            StorageObjectFactory = new StorageObjectFactory(Timestamp);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StorageObjectFactory = null;
        }
    }
}