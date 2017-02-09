using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreOperationTests
    {
        [TestMethod]
        public void TestInsertOperationIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new object());

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestInsertOperationIsInitializedWithSameObject()
        {
            var @object = new object();

            var insertOperation = ObjectStoreOperation.Insert(@object);

            Assert.AreSame(@object, insertOperation.Object);
        }

        [TestMethod]
        public void TestInserOperationCannotBeInitializedWithNullObject()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ObjectStoreOperation.Insert(null));
        }

        [TestMethod]
        public void TestDeleteOperationIsInitializedWithDeleteOperationType()
        {
            var deleteOperation = ObjectStoreOperation.Delete(new object());

            Assert.AreEqual(ObjectStoreOperationType.Delete, deleteOperation.OperationType);
        }

        [TestMethod]
        public void TestDeleteOperationIsInitializedWithSameObject()
        {
            var @object = new object();

            var deleteOperation = ObjectStoreOperation.Insert(@object);

            Assert.AreSame(@object, deleteOperation.Object);
        }

        [TestMethod]
        public void TestDeleteOperationCannotBeInitializedWithNullObject()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ObjectStoreOperation.Delete(null));
        }
    }
}