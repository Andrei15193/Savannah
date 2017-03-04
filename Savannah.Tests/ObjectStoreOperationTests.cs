using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreOperationTests
        : UnitTest
    {
        private sealed class ObjectStoreOperationMock
            : ObjectStoreOperation
        {
            internal ObjectStoreOperationMock(object @object)
                : base(@object)
            {
            }

            public override ObjectStoreOperationType OperationType
                => default(ObjectStoreOperationType);

            internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext executionContext)
                => null;
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestOperationExtractsPartitionKey()
        {
            var row = GetRow<KeyValuesRow>();

            var operation = new ObjectStoreOperationMock(new { PartitionKey = row.Value, RowKey = string.Empty });
            Assert.AreEqual(row.Value, operation.PartitionKey, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestOperationExtractsRowKey()
        {
            var row = GetRow<KeyValuesRow>();

            var operation = new ObjectStoreOperationMock(new { PartitionKey = string.Empty, RowKey = row.Value });
            Assert.AreEqual(row.Value, operation.RowKey, ignoreCase: false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public void TestTryingToCreateOperationWithNullObjectThrowsException()
            => ObjectStoreOperation.Insert(null);

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestOperationExposesSameObject()
        {
            var @object = new { PartitionKey = string.Empty, RowKey = string.Empty };
            var operation = new ObjectStoreOperationMock(@object);
            Assert.AreSame(@object, operation.Object);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestDeleteOperationIsInitializedWithDeleteOperationType()
        {
            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Delete, deleteOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOperationIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty }, echoContent: true);

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOrMergeOperationIsInitializedWithInsertOperationType()
        {
            var insertOrMergeOperation = ObjectStoreOperation.InsertOrMerge(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.InsertOrMerge, insertOrMergeOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestInsertOrReplaceOperationIsInitializedWithInsertOperationType()
        {
            var insertOrReplaceOperation = ObjectStoreOperation.InsertOrReplace(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.InsertOrReplace, insertOrReplaceOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestMergeOperationIsInitializedWithInsertOperationType()
        {
            var insertOrReplaceOperation = ObjectStoreOperation.Merge(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Merge, insertOrReplaceOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveDynamicWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrievePocoWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolverOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolverWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveResolverWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }
    }
}