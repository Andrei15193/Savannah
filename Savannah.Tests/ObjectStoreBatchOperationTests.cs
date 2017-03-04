using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Tests.Mocks;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreBatchOperationTests
        : UnitTest
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
        [Owner("Andrei Fangli")]
        public void TestAddingATableOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());

            _ObjectStoreBatchOperation.Add(operation);

            var actualOperation = _ObjectStoreBatchOperation.Single();
            Assert.AreSame(operation, actualOperation);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public void TestTryingToAddNullThrowsException()
            => _ObjectStoreBatchOperation.Add(null);

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PositiveIntegerValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCountReturnsNumberOfOperations()
        {
            var row = GetRow<PositiveIntegerValuesRow>();

            for (var operationIndex = 0; operationIndex < row.Value; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            Assert.AreEqual(row.Value, _ObjectStoreBatchOperation.Count);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestClearingTableBatchOperations()
        {
            var operation = ObjectStoreOperation.Insert(new object());

            _ObjectStoreBatchOperation.Add(operation);
            _ObjectStoreBatchOperation.Clear();

            Assert.AreEqual(0, _ObjectStoreBatchOperation.Count);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCheckingIfTheBatchOperationContainsAPreviouslyAddedOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsTrue(_ObjectStoreBatchOperation.Contains(operation));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCheckingIfTheBatchOperationDoesNotContainAnOperationThatWasNotAdded()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            var newOperation = ObjectStoreOperation.Insert(new object());

            Assert.IsFalse(_ObjectStoreBatchOperation.Contains(newOperation));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, OperationTestSetTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGetOperationFromIndex()
        {
            var row = GetRow<OperationTestSetRow>();

            ObjectStoreOperation operationToCheck = null;
            for (var operationIndex = 0; operationIndex < row.NumberOfOperations; operationIndex++)
            {
                var operation = ObjectStoreOperation.Insert(new object());
                _ObjectStoreBatchOperation.Add(operation);
                if (operationIndex == row.Index)
                    operationToCheck = operation;
            }

            Assert.AreSame(operationToCheck, _ObjectStoreBatchOperation[row.Index]);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, OperationTestSetTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestSettingANewOperationOnAnExistingIndex()
        {
            var row = GetRow<OperationTestSetRow>();

            for (var operationIndex = 0; operationIndex < row.NumberOfOperations; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation[row.Index] = operation;

            Assert.AreSame(operation, _ObjectStoreBatchOperation[row.Index]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, PositiveIntegerValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestTryingToSetNullOnAnIndexThrowsException()
        {
            var row = GetRow<PositiveIntegerValuesRow>();

            _ObjectStoreBatchOperation[row.Value] = null;
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, OperationTestSetTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestInsertingANewOperation()
        {
            var row = GetRow<OperationTestSetRow>();

            for (var operationIndex = 0; operationIndex < row.NumberOfOperations; operationIndex++)
                _ObjectStoreBatchOperation.Add(ObjectStoreOperation.Insert(new object()));

            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Insert(row.Index, operation);

            Assert.AreSame(operation, _ObjectStoreBatchOperation[row.Index]);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRemovingAnExistingOperation()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            _ObjectStoreBatchOperation.Remove(operation);

            Assert.IsFalse(_ObjectStoreBatchOperation.Any());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRemovingAnExistingOperationReturnsTrue()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsTrue(_ObjectStoreBatchOperation.Remove(operation));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToRemoveAnObjectThatDoesNotExistDoesNotChangeTheCollection()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            _ObjectStoreBatchOperation.Remove(ObjectStoreOperation.Insert(new object()));

            Assert.AreSame(operation, _ObjectStoreBatchOperation.Single());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToRemoveAnObjectThatDoesNotExistReturnsFalse()
        {
            var operation = ObjectStoreOperation.Insert(new object());
            _ObjectStoreBatchOperation.Add(operation);

            Assert.IsFalse(_ObjectStoreBatchOperation.Remove(ObjectStoreOperation.Insert(new object())));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, OperationTestSetTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRemovingAnObjectFromAnIndexEliminatesItFromTheBatch()
        {
            var row = GetRow<OperationTestSetRow>();

            ObjectStoreOperation removedOperation = null;
            for (var operationIndex = 0; operationIndex < row.NumberOfOperations; operationIndex++)
            {
                var operation = ObjectStoreOperation.Insert(new object());
                _ObjectStoreBatchOperation.Add(operation);
                if (operationIndex == row.Index)
                    removedOperation = operation;
            }

            _ObjectStoreBatchOperation.RemoveAt(row.Index);

            Assert.IsFalse(_ObjectStoreBatchOperation.Contains(removedOperation));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestDeleteAddsANewDeleteOperation()
        {
            _ObjectStoreBatchOperation.Delete(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Delete, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.Insert(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Insert, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertWithEchoContentAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.Insert(new object(), echoContent: true);

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Insert, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOrMergeAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.InsertOrMerge(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.InsertOrMerge, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOrReplaceAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.InsertOrReplace(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.InsertOrReplace, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestMergeAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.Merge(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Merge, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestReplaceAddsANewInsertOperation()
        {
            _ObjectStoreBatchOperation.Replace(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Replace, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicObjectAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicObjectWithPropertyEnumerableFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object(), Enumerable.Empty<string>());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicObjectWithPropertyArrayFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object(), new string[0]);

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoObjectAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve<MockObject>(new object());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoObjectWithPropertyEnumerableFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve<MockObject>(new object(), Enumerable.Empty<string>());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoObjectWithPropertyArrayFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve<MockObject>(new object(), new string[0]);

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolvedObjectAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object(), delegate { return new object(); });

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolvedObjectWithPropertyEnumerableFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object(), delegate { return new object(); }, Enumerable.Empty<string>());

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolvedObjectWithPropertyArrayFilterAddsANewRetrieveOperation()
        {
            _ObjectStoreBatchOperation.Retrieve(new object(), delegate { return new object(); }, new string[0]);

            var operation = _ObjectStoreBatchOperation.Single();
            Assert.AreEqual(ObjectStoreOperationType.Retrieve, operation.OperationType);
        }
    }
}