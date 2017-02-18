using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Tests.Mocks;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreOperationTests
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

        [DataTestMethod]
        [DataRow("partitionKey")]
        [DataRow("test")]
        [DataRow("")]
        public void TestOperationExtractsPartitionKey(string partitionKey)
        {
            var operation = new ObjectStoreOperationMock(new { PartitionKey = partitionKey, RowKey = string.Empty });
            Assert.AreEqual(partitionKey, operation.PartitionKey, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow("rowKey")]
        [DataRow("test")]
        [DataRow("")]
        public void TestOperationExtractsRowKey(string rowKey)
        {
            var operation = new ObjectStoreOperationMock(new { PartitionKey = string.Empty, RowKey = rowKey });
            Assert.AreEqual(rowKey, operation.RowKey, ignoreCase: false);
        }

        [TestMethod]
        public void TestTryingToCreateOperationWithNullObjectThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ObjectStoreOperation.Insert(null));
        }

        [TestMethod]
        public void TestOperationExposesSameObject()
        {
            var @object = new { PartitionKey = string.Empty, RowKey = string.Empty };
            var operation = new ObjectStoreOperationMock(@object);
            Assert.AreSame(@object, operation.Object);
        }

        [TestMethod]
        public void TestDeleteOperationIsInitializedWithDeleteOperationType()
        {
            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Delete, deleteOperation.OperationType);
        }

        [TestMethod]
        public void TestInsertOperationIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestInsertOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty }, echoContent: true);

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestInsertOrMergeOperationIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.InsertOrMerge(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.InsertOrMerge, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveDynamicOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveDynamicWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveDynamicWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrievePocoOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrievePocoWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrievePocoWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveResolverOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; });

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveResolverWithEnumerableSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; }, Enumerable.Empty<string>());

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestRetrieveResolverWithArraybleSelectPropertiesOperationWithEchoIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Retrieve(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new { }; }, new string[0]);

            Assert.AreEqual(ObjectStoreOperationType.Retrieve, insertOperation.OperationType);
        }
    }
}