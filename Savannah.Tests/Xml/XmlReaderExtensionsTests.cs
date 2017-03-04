using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Xml;

namespace Savannah.Tests.Xml
{
    [TestClass]
    public class XmlReaderExtensionsTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPartitionKeyTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectPartitionKey()
        {
            var row = GetRow<XmlObjectPartitionKeyRow>();

            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(row.PartitionKey, storageObject.PartitionKey);
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectRowKeyTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectRowKey()
        {
            var row = GetRow<XmlObjectRowKeyRow>();

            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(row.RowKey, storageObject.RowKey);
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectTimestampTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectTimestamp()
        {
            var row = GetRow<XmlObjectTimestampRow>();

            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(row.Timestamp, storageObject.Timestamp);
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectPropertyNamesAccrodingly()
        {
            var row = GetRow<XmlObjectPropertyNamesRow>();

            var expectedPropertyNames = row.PropertyNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            StorageObject storageObject;
            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                storageObject = await xmlReader.ReadStorageObjectAsync();
            }

            Assert.IsTrue(storageObject
                .Properties
                .Select(property => property.Name)
                .SequenceEqual(expectedPropertyNames));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectPropertyValuesAccordingly()
        {
            var row = GetRow<XmlObjectPropertyValuesRow>();

            var expectedPropertyValues = row.PropertyValues?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (expectedPropertyValues == null)
                expectedPropertyValues = new string[] { null };

            StorageObject storageObject;
            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                storageObject = await xmlReader.ReadStorageObjectAsync();
            }

            Assert.IsTrue(storageObject
                .Properties
                .Select(property => property.Value)
                .SequenceEqual(expectedPropertyValues));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyTypesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsStorageObjectPropertyTypesAccordingly()
        {
            var row = GetRow<XmlObjectPropertyTypesRow>();

            var expectedPropertyTypes = row
                .PropertyTypes
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(name => (ValueType)Enum.Parse(typeof(ValueType), name));

            StorageObject storageObject;
            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                storageObject = await xmlReader.ReadStorageObjectAsync();
            }

            Assert.IsTrue(storageObject
                .Properties
                .Select(property => property.Type)
                .SequenceEqual(expectedPropertyTypes));
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectsInPartitionTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlReaderReadsConsecutiveObjects()
        {
            var row = GetRow<XmlObjectsInPartitionRow>();

            var storageObjects = new List<StorageObject>();
            using (var stringReader = new StringReader(row.Xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                await xmlReader.ReadAsync();

                while (xmlReader.IsOnObjectElement())
                    storageObjects.Add(await xmlReader.ReadStorageObjectAsync());
            }

            Assert.AreEqual(row.NumberOfObjects, storageObjects.Distinct().Count());
        }
    }
}