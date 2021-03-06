﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Tests.Mocks;
using Savannah.Xml;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class RetrieveResolverObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveReturnsSameStorageObject()
        {
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(new object(), delegate { return new MockObject(); });
            var storageObject = new StorageObject(null, null, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture));
            var executionContext = new ObjectStoreOperationExectionContext(
                storageObject,
                StorageObjectFactory,
                DateTime.UtcNow,
                new List<object>());

            var actualStorageObject = retrieveOperation.GetStorageObjectFrom(executionContext);

            Assert.AreSame(storageObject, actualStorageObject);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRetrieveExistingObject()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var mockObject = new MockObject();
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(@object, delegate { return mockObject; });
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                DateTime.UtcNow,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (MockObject)result.Single();

            Assert.AreSame(mockObject, actualObject);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRetrieveExistingObjectWithSpecifiedProperties()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(
                @object,
                (partitionKey, rowKey, timestamp, propertyValues) =>
                {
                    Assert.IsNull(rowKey);
                    return new MockObject();
                },
                new[] { nameof(MockObject.PartitionKey) });
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                DateTime.UtcNow,
                new List<object>());

            retrieveOperation.GetStorageObjectFrom(executionContext);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToRetrieveNonExistingObjectThrowsException()
        {
            var retrieveOperation = new RetrieveDelegateObjectStoreOperation<MockObject>(new { PartitionKey = string.Empty, RowKey = string.Empty }, delegate { return new MockObject(); });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => retrieveOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be retrieved.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestStorageObjectPropertiesAreMadeAvailableInResolver()
        {
            var propertyNames = new[] { "test", "property" };

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
                        .Select(propertyName => new StorageObjectProperty(propertyName, string.Empty, ValueType.String))
                        .ToList()),
                StorageObjectFactory,
                DateTime.UtcNow,
                new List<object>());

            retrieveOperation.GetStorageObjectFrom(executionContext);
        }
    }
}