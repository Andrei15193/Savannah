using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests.ObjectStoreOperations
{
    [TestClass]
    public class StorageObjectMergerTests
    {
        private static StorageObjectMerger _Merger { get; } = new StorageObjectMerger();

        [TestMethod]
        public void TestObjectPropertiesAreMerged()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var rowKey = Guid.NewGuid().ToString();
            var timestamp = Guid.NewGuid().ToString();

            var oldProperty1 = new StorageObjectProperty(
                "property1",
                "testValue1",
                ValueType.String);
            var newProperty2 = new StorageObjectProperty(
                "property2",
                "testValue2",
                ValueType.String);

            var oldStorageObject = new StorageObject(partitionKey, rowKey, timestamp, oldProperty1);
            var newStorageObject = new StorageObject(partitionKey, rowKey, timestamp, newProperty2);

            var result = _Merger.Merge(oldStorageObject, newStorageObject);

            Assert.IsTrue(
                new[]
                {
                    oldProperty1,
                    newProperty2
                }
                .OrderBy(property => property.Name)
                .SequenceEqual(
                    result
                        .Properties
                        .OrderBy(property => property.Name)));
        }

        [TestMethod]
        public void TestObjectPropertyIsReplacedIfAlreadyExists()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var rowKey = Guid.NewGuid().ToString();
            var timestamp = Guid.NewGuid().ToString();

            var oldProperty1 = new StorageObjectProperty(
                "property1",
                "testValue1",
                ValueType.String);
            var newProperty1 = new StorageObjectProperty(
                oldProperty1.Name,
                "testValue1",
                ValueType.String);

            var oldStorageObject = new StorageObject(partitionKey, rowKey, timestamp, oldProperty1);
            var newStorageObject = new StorageObject(partitionKey, rowKey, timestamp, newProperty1);

            var result = _Merger.Merge(oldStorageObject, newStorageObject);

            Assert.IsTrue(new[] { newProperty1 }.SequenceEqual(result.Properties));
        }

        [TestMethod]
        public void TestObjectTimestampIsReplacedByTheOneInNewObject()
        {
            var partitionKey = Guid.NewGuid().ToString();
            var rowKey = Guid.NewGuid().ToString();
            var oldTimestamp = Guid.NewGuid().ToString();
            var newTimestamp = Guid.NewGuid().ToString();

            var oldProperty1 = new StorageObjectProperty(
                "property1",
                "testValue1",
                ValueType.String);
            var newProperty1 = new StorageObjectProperty(
                oldProperty1.Name,
                "testValue1",
                ValueType.String);

            var oldStorageObject = new StorageObject(partitionKey, rowKey, oldTimestamp, oldProperty1);
            var newStorageObject = new StorageObject(partitionKey, rowKey, newTimestamp, newProperty1);

            var result = _Merger.Merge(oldStorageObject, newStorageObject);

            Assert.AreSame(newTimestamp, result.Timestamp);
        }
    }
}