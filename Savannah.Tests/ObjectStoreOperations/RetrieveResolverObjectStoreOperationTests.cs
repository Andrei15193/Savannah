using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class RetrieveResolverObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        public void TestRetrieveReturnsSameStorageObject()
        {
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(new object(), delegate { return new MockObject(); });
            var storageObject = new StorageObject(null, null, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture));
            var executionContext = new ObjectStoreOperationExectionContext(
                storageObject,
                StorageObjectFactory,
                new List<object>());

            var actualStorageObject = retrieveOperation.GetStorageObjectFrom(executionContext);

            Assert.AreSame(storageObject, actualStorageObject);
        }

        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestRetrieveExistingObject(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var mockObject = new MockObject();
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(@object, delegate { return mockObject; });
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(partitionKey, rowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreSame(mockObject, actualObject);
        }

        [DataTestMethod]
        [DataRow("partitionKey", "rowKey")]
        [DataRow("", "")]
        public void TestRetrieveExistingObjectWithSpecifiedProperties(string partitionKey, string rowKey)
        {
            var @object = new { PartitionKey = partitionKey, RowKey = rowKey };
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(
                @object,
                (partition, row, timestamp, propertyValues) =>
                {
                    Assert.IsNull(row);
                    return new MockObject();
                },
                new[] { nameof(MockObject.PartitionKey) });
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(partitionKey, rowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                new List<object>());

            retrieveOperation.GetStorageObjectFrom(executionContext);
        }

        [TestMethod]
        public void TestTryingToRetrieveNonExistingObjectThrowsException()
        {
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new MockObject(); });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => retrieveOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be retrieved.");
        }

        [DataTestMethod]
        [DataRow(new[] { "test", "property" })]
        public void TestStorageObjectPropertiesAreMadeAvailableInResolver(string[] propertyNames)
        {
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(
                new object(),
                (partition, row, timestamp, propertyValues) =>
                {
                    Assert.IsTrue(propertyNames
                        .OrderBy(propertyName => propertyName)
                        .SequenceEqual(propertyValues.Keys.OrderBy(propertyName => propertyName)));
                    return new MockObject();
                });
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(
                    null,
                    null,
                    DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture),
                    propertyNames
                        .Select(propertyName => new StorageObjectProperty(propertyName, string.Empty, StorageObjectPropertyType.String))
                        .ToList()),
                StorageObjectFactory,
                new List<object>());

            retrieveOperation.GetStorageObjectFrom(executionContext);
        }
    }
}