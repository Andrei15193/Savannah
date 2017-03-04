using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Xml;

namespace Savannah.Tests.Xml
{
    [TestClass]
    public class XmlWriterExtensionsTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPartitionKeyTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSerializesStorageObjectPartitionKey()
        {
            var row = GetRow<XmlObjectPartitionKeyRow>();

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(row.PartitionKey, null, null));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectRowKeyTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSerializesStorageObjectRowKey()
        {
            var row = GetRow<XmlObjectRowKeyRow>();

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, row.RowKey, null));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectTimestampTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSerializesStorageObjectTimestamp()
        {
            var row = GetRow<XmlObjectTimestampRow>();

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, row.Timestamp));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyNames()
        {
            var row = GetRow<XmlObjectPropertyNamesRow>();

            var properties = row
                .PropertyNames
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(propertyName => new StorageObjectProperty(propertyName, null, ValueType.String));

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyValues()
        {
            var row = GetRow<XmlObjectPropertyValuesRow>();

            var properties = row
                .PropertyValues
                ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(
                (propertyValue, propertyIndex) => new StorageObjectProperty(
                    "PropertyName" + (propertyIndex + 1).ToString(),
                    propertyValue,
                    ValueType.String));
            if (properties == null)
                properties = new[] { new StorageObjectProperty("PropertyName", null, ValueType.String) };

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, XmlObjectPropertyTypesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public async Task TestXmlWriterSeializesStorageObjectPropertyTypes()
        {
            var row = GetRow<XmlObjectPropertyTypesRow>();

            var propertyTypes = row
                .PropertyTypes
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(name => (ValueType)Enum.Parse(typeof(ValueType), name));

            var properties = propertyTypes.Cast<ValueType>().Select(
                (propertyType, propertyIndex) => new StorageObjectProperty(
                    "PropertyName" + (propertyIndex + 1).ToString(),
                    null,
                    propertyType));

            var xmlBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xmlBuilder, XmlSettings.WriterSettings))
                await xmlWriter.WriteAsync(new StorageObject(null, null, null, properties));

            var actualXml = xmlBuilder.ToString();
            Assert.AreEqual(row.XmlOut, actualXml, ignoreCase: false);
        }
    }
}