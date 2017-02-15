using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreBatchOperationTests
    {
        private ObjectStoreBatchOperation _ObjectStoreBatchOperation { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _ObjectStoreBatchOperation = new ObjectStoreBatchOperation();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _ObjectStoreBatchOperation = null;
        }

        [TestMethod]
        public void TestAddingATableOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());

            _ObjectStoreBatchOperation.Add(operation);

            var actualOperation = _ObjectStoreBatchOperation.Single();
            Assert.AreSame(operation, actualOperation);
        }

        [TestMethod]
        public void TestTryingToAddNullThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ObjectStoreBatchOperation.Add(null));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(10)]
        public void TestCountReturnsNumberOfOperations(int numberOfOperations)
        {
            for (var operationIndex = 0; operationIndex < numberOfOperations; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            Assert.AreEqual(numberOfOperations, _ObjectStoreBatchOperation.Count);
        }

        [TestMethod]
        public void TestClearingTableBatchOperations()
        {
            var operation = ObjectStoreOperation.Insert(new object());

            _ObjectStoreBatchOperation.Add(operation);
            _ObjectStoreBatchOperation.Clear();

            Assert.AreEqual(0, _ObjectStoreBatchOperation.Count);
        }

        [TestMethod]
        public void TestCheckingIfTheBatchOperationContainsAPreviouslyAddedOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsTrue(_ObjectStoreBatchOperation.Contains(operation));
        }

        [TestMethod]
        public void TestCheckingIfTheBatchOperationDoesNotContainAnOperationThatWasNotAdded()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            var newOperation = ObjectStoreOperation.Insert(new object());

            Assert.IsFalse(_ObjectStoreBatchOperation.Contains(newOperation));
        }

        [DataTestMethod]
        [DataRow(10, 0)]
        [DataRow(10, 1)]
        [DataRow(10, 3)]
        [DataRow(10, 6)]
        [DataRow(10, 9)]
        public void TestGetOperationFromIndex(int numberOfOperations, int index)
        {
            ObjectStoreOperation operationToCheck = null;
            for (var operationIndex = 0; operationIndex < numberOfOperations; operationIndex++)
            {
                var operation = ObjectStoreOperation.Insert(new object());
                _ObjectStoreBatchOperation.Add(operation);
                if (operationIndex == index)
                    operationToCheck = operation;
            }

            Assert.AreSame(operationToCheck, _ObjectStoreBatchOperation[index]);
        }

        [DataTestMethod]
        [DataRow(10, 0)]
        [DataRow(10, 1)]
        [DataRow(10, 3)]
        [DataRow(10, 6)]
        [DataRow(10, 9)]
        public void TestSettingANewOperationOnAnExistingIndex(int numberOfOperations, int index)
        {
            for (var operationIndex = 0; operationIndex < numberOfOperations; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation[index] = operation;

            Assert.AreSame(operation, _ObjectStoreBatchOperation[index]);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(3)]
        [DataRow(6)]
        [DataRow(9)]
        public void TestTryingToSetNullOnAnIndexThrowsException(int index)
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ObjectStoreBatchOperation[index] = null);
        }

        [DataTestMethod]
        [DataRow(10, 0)]
        [DataRow(10, 1)]
        [DataRow(10, 3)]
        [DataRow(10, 6)]
        [DataRow(10, 9)]
        public void TestInsertingANewOperation(int numberOfOperations, int index)
        {
            for (var operationIndex = 0; operationIndex < numberOfOperations; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Insert(index, operation);

            Assert.AreSame(operation, _ObjectStoreBatchOperation[index]);
        }

        [TestMethod]
        public void TestRemovingAnExistingOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            _ObjectStoreBatchOperation.Remove(operation);

            Assert.IsFalse(_ObjectStoreBatchOperation.Any());
        }

        [TestMethod]
        public void TestRemovingAnExistingOperationReturnsTrue()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsTrue(_ObjectStoreBatchOperation.Remove(operation));
        }

        [TestMethod]
        public void TestTryingToRemoveAnObjectThatDoesNotExistDoesNotChangeTheCollection()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            _ObjectStoreBatchOperation.Remove(ObjectStoreOperation.Insert(new object()));

            Assert.AreSame(operation, _ObjectStoreBatchOperation.Single());
        }

        [TestMethod]
        public void TestTryingToRemoveAnObjectThatDoesNotExistReturnsFalse()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsFalse(_ObjectStoreBatchOperation.Remove(ObjectStoreOperation.Insert(new object())));
        }

        [DataTestMethod]
        [DataRow(10, 0)]
        [DataRow(10, 1)]
        [DataRow(10, 3)]
        [DataRow(10, 6)]
        [DataRow(10, 9)]
        public void TestRemovingAnObjectFromAnIndexEliminatesItFromTheBatch(int numberOfOperations, int index)
        {
            ObjectStoreOperation removedOperation = null;
            for (var operationIndex = 0; operationIndex < numberOfOperations; operationIndex++)
            {
                var operation = ObjectStoreOperation.Insert(new object());
                _ObjectStoreBatchOperation.Add(operation);
                if (operationIndex == index)
                    removedOperation = operation;
            }

            _ObjectStoreBatchOperation.RemoveAt(index);

            Assert.IsFalse(_ObjectStoreBatchOperation.Contains(removedOperation));
        }

        [TestMethod]
        public void TestInsertAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.Insert(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Insert, operation.OperationType);
        }

        [TestMethod]
        public void TestDeleteAddsANewDeleteOperation()
        {
            _ObjectStoreBatchOperation.Delete(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Delete, operation.OperationType);
        }
    }
}