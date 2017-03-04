using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;
using Savannah.Xml;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class RetrieveDynamicObjectStoreOperationTests
        : ObjectStoreOperationTestsTemplate
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRetrieveReturnsSameStorageObject()
        {
            var retrieveOperation = new RetrieveDynamicObjectStoreOperation(new object());
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
            var retrieveOperation = new RetrieveDynamicObjectStoreOperation(@object);
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                DateTime.UtcNow,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (dynamic)result.Single();

            Assert.AreEqual(
                @object,
                new { PartitionKey = (string)actualObject.PartitionKey, RowKey = (string)actualObject.RowKey });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectKeysTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRetrieveExistingObjectWithSpecifiedProperties()
        {
            var row = GetRow<ObjectKeysRow>();

            var @object = new { row.PartitionKey, row.RowKey };
            var retrieveOperation = new RetrieveDynamicObjectStoreOperation(@object, new[] { nameof(StorageObject.PartitionKey) });
            var result = new List<object>();
            var executionContext = new ObjectStoreOperationExectionContext(
                new StorageObject(row.PartitionKey, row.RowKey, DateTime.UtcNow.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)),
                StorageObjectFactory,
                DateTime.UtcNow,
                result);

            retrieveOperation.GetStorageObjectFrom(executionContext);
            var actualObject = (dynamic)result.Single();

            Assert.AreEqual(
                new { @object.PartitionKey, RowKey = default(string) },
                new { PartitionKey = (string)actualObject.PartitionKey, RowKey = default(string) });
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToRetrieveNonExistingObjectThrowsException()
        {
            var retrieveOperation = new RetrieveDynamicObjectStoreOperation(new { PartitionKey = string.Empty, RowKey = string.Empty });
            var executionContext = new ObjectStoreOperationExectionContext(null, StorageObjectFactory, DateTime.UtcNow, new List<object>());

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => retrieveOperation.GetStorageObjectFrom(executionContext),
                "The object does not exist, it cannot be retrieved.");
        }
    }
}