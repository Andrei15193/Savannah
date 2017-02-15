using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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

            internal override Task ExecuteAsync(StorageObject existingObject, ObjectStoreOperationContext context, CancellationToken cancellationToken)
                => Task.FromResult<object>(null);
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
        public void TestInsertOperationIsInitializedWithInsertOperationType()
        {
            var insertOperation = ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Insert, insertOperation.OperationType);
        }

        [TestMethod]
        public void TestDeleteOperationIsInitializedWithDeleteOperationType()
        {
            var deleteOperation = ObjectStoreOperation.Delete(new { PartitionKey = string.Empty, RowKey = string.Empty });

            Assert.AreEqual(ObjectStoreOperationType.Delete, deleteOperation.OperationType);
        }
    }
}