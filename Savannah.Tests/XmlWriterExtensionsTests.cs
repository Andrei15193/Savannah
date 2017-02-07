using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class XmlWriterExtensionsTests
    {
        [DataTestMethod]
        [DataRow(
            "PartitionKey",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object PartitionKey=\"PartitionKey\" />")]
        [DataRow(
            "",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object PartitionKey=\"\" />")]
        [DataRow(
            default(string),
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        public async Task TestXmlWriterSerializesStorageObjectPartitionKey(string partitionKey, string expectedXml)
        {
            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(partitionKey, null, null));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(
            "RowKey",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object RowKey=\"RowKey\" />")]
        [DataRow(
            "",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object RowKey=\"\" />")]
        [DataRow(
            default(string),
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        public async Task TestXmlWriterSerializesStorageObjectRowKey(string rowKey, string expectedXml)
        {
            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, rowKey, null));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(
            "Timestamp",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object Timestamp=\"Timestamp\" />")]
        [DataRow(
            "",
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object Timestamp=\"\" />")]
        [DataRow(
            default(string),
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        public async Task TestXmlWriterSerializesStorageObjectTimestamp(string timestamp, string expectedXml)
        {
            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, timestamp));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(
            new string[0],
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        [DataRow(
            new string[1] {
                "PropertyName"
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName Type=\"String\" />"
            + "</Object>")]
        [DataRow(
            new string[2] {
                "PropertyName1",
                "PropertyName2"
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" />"
                + "<PropertyName2 Type=\"String\" />"
            + "</Object>")]
        [DataRow(
            new string[2] {
                "PropertyName2",
                "PropertyName1"
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName2 Type=\"String\" />"
                + "<PropertyName1 Type=\"String\" />"
            + "</Object>")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyNames(string[] propertyNames, string expectedXml)
        {
            var properties = propertyNames.Select(propertyName => new StorageObjectProperty(propertyName, null, StorageObjectPropertyType.String));

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(
            new string[0],
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        [DataRow(
            new string[1] {
                "PropertyValue"
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" Value=\"PropertyValue\" />"
            + "</Object>")]
        [DataRow(
            new string[2] {
                "PropertyValue1",
                "PropertyValue2",
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" Value=\"PropertyValue1\" />"
                + "<PropertyName2 Type=\"String\" Value=\"PropertyValue2\" />"
            + "</Object>")]
        [DataRow(
            new string[2] {
                "PropertyValue2",
                "PropertyValue1"
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" Value=\"PropertyValue2\" />"
                + "<PropertyName2 Type=\"String\" Value=\"PropertyValue1\" />"
            + "</Object>")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyValues(string[] propertyValues, string expectedXml)
        {
            var properties = propertyValues.Select(
                (propertyValue, propertyIndex) => new StorageObjectProperty(
                    "PropertyName" + (propertyIndex + 1).ToString(),
                    propertyValue,
                    StorageObjectPropertyType.String));

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(
            new object[0],
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object />")]
        [DataRow(
            new object[1] {
                StorageObjectPropertyType.String
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" />"
            + "</Object>")]
        [DataRow(
            new object[2] {
                StorageObjectPropertyType.String,
                StorageObjectPropertyType.Int
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" />"
                + "<PropertyName2 Type=\"Int\" />"
            + "</Object>")]
        [DataRow(
            new object[2] {
                StorageObjectPropertyType.Int,
                StorageObjectPropertyType.String
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"Int\" />"
                + "<PropertyName2 Type=\"String\" />"
            + "</Object>")]
        [DataRow(
            new object[8] {
                StorageObjectPropertyType.String,
                StorageObjectPropertyType.Binary,
                StorageObjectPropertyType.Boolean,
                StorageObjectPropertyType.DateTime,
                StorageObjectPropertyType.Double,
                StorageObjectPropertyType.Guid,
                StorageObjectPropertyType.Int,
                StorageObjectPropertyType.Long
            },
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
            + "<Object>"
                + "<PropertyName1 Type=\"String\" />"
                + "<PropertyName2 Type=\"Binary\" />"
                + "<PropertyName3 Type=\"Boolean\" />"
                + "<PropertyName4 Type=\"DateTime\" />"
                + "<PropertyName5 Type=\"Double\" />"
                + "<PropertyName6 Type=\"Guid\" />"
                + "<PropertyName7 Type=\"Int\" />"
                + "<PropertyName8 Type=\"Long\" />"
            + "</Object>")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyValues(object[] propertyTypes, string expectedXml)
        {
            var properties = propertyTypes.Cast<StorageObjectPropertyType>().Select(
                (propertyType, propertyIndex) => new StorageObjectProperty(
                    "PropertyName" + (propertyIndex + 1).ToString(),
                    null,
                    propertyType));

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(expectedXml, actualXml, ignoreCase: false);
        }
    }
}