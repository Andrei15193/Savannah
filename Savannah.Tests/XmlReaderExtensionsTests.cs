using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class XmlReaderExtensionsTests
    {
        [DataTestMethod]
        [DataRow("<Object PartitionKey=\"PartitionKey\" />", "PartitionKey")]
        [DataRow("<Object PartitionKey=\"\" />", "")]
        [DataRow("<Object />", default(string))]
        [DataRow("<Object RowKey=\"RowKey\" />", default(string))]
        [DataRow("<Object PartitionKey=\"PartitionKey\" RowKey=\"RowKey\" />", "PartitionKey")]
        [DataRow("<Object RowKey=\"RowKey\" PartitionKey=\"PartitionKey\" />", "PartitionKey")]
        public async Task TestXmlReaderReadsStorageObjectPartitionKey(string xml, string expectedPartitionKey)
        {
            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(expectedPartitionKey, storageObject.PartitionKey);
            }
        }

        [DataTestMethod]
        [DataRow("<Object RowKey=\"RowKey\" />", "RowKey")]
        [DataRow("<Object RowKey=\"\" />", "")]
        [DataRow("<Object />", default(string))]
        [DataRow("<Object Timestamp=\"Timestamp\" />", default(string))]
        [DataRow("<Object RowKey=\"RowKey\" Timestamp=\"Timestamp\" />", "RowKey")]
        [DataRow("<Object Timestamp=\"Timestamp\" RowKey=\"RowKey\" />", "RowKey")]
        public async Task TestXmlReaderReadsStorageObjectRowKey(string xml, string expectedRowKey)
        {
            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(expectedRowKey, storageObject.RowKey);
            }
        }

        [DataTestMethod]
        [DataRow("<Object Timestamp=\"Timestamp\" />", "Timestamp")]
        [DataRow("<Object Timestamp=\"\" />", "")]
        [DataRow("<Object />", default(string))]
        [DataRow("<Object RowKey=\"RowKey\" />", default(string))]
        [DataRow("<Object Timestamp=\"Timestamp\" RowKey=\"RowKey\" />", "Timestamp")]
        [DataRow("<Object RowKey=\"RowKey\" Timestamp=\"Timestamp\" />", "Timestamp")]
        public async Task TestXmlReaderReadsStorageObjectTimestamp(string xml, string expectedTimestamp)
        {
            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                var storageObject = await xmlReader.ReadStorageObjectAsync();

                Assert.AreEqual(expectedTimestamp, storageObject.Timestamp);
            }
        }

        [DataTestMethod]
        [DataRow(
            "<Object />",
            new object[] { new string[0] {
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName />
                </Object>",
            new object[] { new string[1] {
                "PropertyName"
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName1 />
                    <PropertyName2 />
                </Object>",
            new object[] { new string[2] {
                "PropertyName1",
                "PropertyName2"
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName2 />
                    <PropertyName1 />
                </Object>",
            new object[] { new string[2] {
                "PropertyName2",
                "PropertyName1"
            } })]
        public async Task TestXmlReaderReadsStorageObjectPropertyNamesAccrodingly(string xml, string[] expectedPropertyNames)
        {
            StorageObject storageObject;

            using (var stringReader = new StringReader(xml))
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

        [DataTestMethod]
        [DataRow(
            "<Object />",
            new object[] { new string[0] {
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName />
                </Object>",
            new object[] { new string[1] {
                null
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName Value=""PropertyValue"" />
                </Object>",
            new object[] { new string[1] {
                "PropertyValue"
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName1 Value=""PropertyValue1"" />
                    <PropertyName2 Value=""PropertyValue2"" />
                </Object>",
            new object[] { new string[2] {
                "PropertyValue1",
                "PropertyValue2"
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName2 Value=""PropertyValue2"" />
                    <PropertyName1 Value=""PropertyValue1"" />
                </Object>",
            new object[] { new string[2] {
                "PropertyValue2",
                "PropertyValue1"
            } })]
        public async Task TestXmlReaderReadsStorageObjectPropertyValuesAccordingly(string xml, string[] expectedPropertyValues)
        {
            StorageObject storageObject;

            using (var stringReader = new StringReader(xml))
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

        [DataTestMethod]
        [DataRow(
            "<Object />",
            new object[] { new object[0] {
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName />
                </Object>",
            new object[] { new object[1] {
                StorageObjectPropertyType.String
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName Type=""String"" />
                </Object>",
            new object[] { new object[1] {
                StorageObjectPropertyType.String
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName1 Type=""String"" />
                    <PropertyName2 Type=""Int"" />
                </Object>",
            new object[] { new object[2] {
                StorageObjectPropertyType.String,
                StorageObjectPropertyType.Int
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName2 Type=""Int"" />
                    <PropertyName1 Type=""String"" />
                </Object>",
            new object[] { new object[2] {
                StorageObjectPropertyType.Int,
                StorageObjectPropertyType.String
            } })]
        [DataRow(
            @"  <Object>
                    <PropertyName1 Type=""String"" />
                    <PropertyName2 Type=""Binary"" />
                    <PropertyName3 Type=""Boolean"" />
                    <PropertyName4 Type=""DateTime"" />
                    <PropertyName5 Type=""Double"" />
                    <PropertyName6 Type=""Guid"" />
                    <PropertyName7 Type=""Int"" />
                    <PropertyName8 Type=""Long"" />
                </Object>",
            new object[] { new object[8] {
                StorageObjectPropertyType.String,
                StorageObjectPropertyType.Binary,
                StorageObjectPropertyType.Boolean,
                StorageObjectPropertyType.DateTime,
                StorageObjectPropertyType.Double,
                StorageObjectPropertyType.Guid,
                StorageObjectPropertyType.Int,
                StorageObjectPropertyType.Long
            } })]
        public async Task TestXmlReaderReadsStorageObjectPropertyTypesAccordingly(string xml, object[] expectedPropertyTypes)
        {
            StorageObject storageObject;

            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                storageObject = await xmlReader.ReadStorageObjectAsync();
            }

            Assert.IsTrue(storageObject
                .Properties
                .Select(property => property.Type)
                .SequenceEqual(expectedPropertyTypes.Cast<StorageObjectPropertyType>()));
        }

        [DataTestMethod]
        [DataRow(@"
            <Partition>
                <Object />
                <Object />
            </Partition>", 2)]
        public async Task TestXmlReaderReadsConsecutiveObjects(string xml, int numberOfObjects)
        {
            var storageObjects = new List<StorageObject>();

            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                await xmlReader.ReadAsync();

                while (xmlReader.IsOnObjectElement())
                    storageObjects.Add(await xmlReader.ReadStorageObjectAsync());
            }

            Assert.AreEqual(numberOfObjects, storageObjects.Distinct().Count());
        }

        [DataTestMethod]
        [DataRow(
            @"  <Object>
                    <PropertyName2 />
                    <PropertyName1 />
                </Object>",
            new string[2]
            {
                "PropertyName1",
                "PropertyName2"
            },
            new string[2] {
                "PropertyName2",
                "PropertyName1"
            })]
        [DataRow(
            @"  <Object>
                    <PropertyName2 />
                    <PropertyName1 />
                </Object>",
            new string[1]
            {
                "PropertyName1"
            },
            new string[1] {
                "PropertyName1"
            })]
        public async Task TestXmlReaderReadsOnlyStorageObjectPropertyWhoseNamesHaveBeenSpecified(string xml, string[] propertiesToRead, string[] expectedPropertyNames)
        {
            StorageObject storageObject;

            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, XmlSettings.ReaderSettings))
            {
                await xmlReader.ReadAsync();
                storageObject = await xmlReader.ReadStorageObjectAsync(propertiesToRead);
            }

            Assert.IsTrue(storageObject
                .Properties
                .Select(property => property.Name)
                .SequenceEqual(expectedPropertyNames));
        }
    }
}