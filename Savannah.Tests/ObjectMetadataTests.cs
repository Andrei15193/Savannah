using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Tests.Mocks;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectMetadataTests
    {
        private class TestObject
        {
            public static string StaticPublicProperty { get; set; }

            public static string StaticPublicReadOnlyProperty { get; }

            public static string StaticPublicWriteOnlyProperty { set { } }

            internal protected static string StaticInternalProtectedProperty { get; set; }

            internal protected static string StaticInternalProtectedReadOnlyProperty { get; }

            internal protected static string StaticInternalProtectedWriteOnlyProperty { set { } }

            internal static string StaticInternalProperty { get; set; }

            internal static string StaticInternalReadOnlyProperty { get; }

            internal static string StaticInternalWriteOnlyProperty { set { } }

            protected static string StaticProtectedProperty { get; set; }

            protected static string StaticProtectedReadOnlyProperty { get; }

            protected static string StaticProtectedWriteOnlyProperty { set { } }

            private static string _StaticPrivateProperty { get; set; }

            private static string _StaticPrivateReadOnlyProperty { get; }

            private static string _StaticPrivateWriteOnlyProperty { set { } }

            public string PartitionKey { get; set; }

            public string RowKey { get; set; }

            public string PublicProperty { get; set; }

            public string PublicReadOnlyProperty { get; }

            public string PublicWriteOnlyProperty { set { } }

            internal protected string InternalProtectedProperty { get; set; }

            internal protected string InternalProtectedReadOnlyProperty { get; }

            internal protected string InternalProtectedWriteOnlyProperty { set { } }

            internal string InternalProperty { get; set; }

            internal string InternalReadOnlyProperty { get; }

            internal string InternalWriteOnlyProperty { set { } }

            protected string ProtectedProperty { get; set; }

            protected string ProtectedReadOnlyProperty { get; }

            protected string ProtectedWriteOnlyProperty { set { } }

            private string _PrivateProperty { get; set; }

            private string _PrivateReadOnlyProperty { get; }

            private string _PrivateWriteOnlyProperty { set { } }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsAllPublicNonStaticReadableProperties()
        {
            var metadata = ObjectMetadata.GetFor<TestObject>();
            var expectedPropertyNames =
                new[]
                {
                    nameof(TestObject.PartitionKey),
                    nameof(TestObject.RowKey),
                    nameof(TestObject.PublicProperty),
                    nameof(TestObject.PublicReadOnlyProperty)
                }.OrderBy(propertyName => propertyName);

            var actualPropertyNames = metadata.ReadableProperties.Select(property => property.Name).OrderBy(propertyName => propertyName);

            Assert.IsTrue(
                expectedPropertyNames.SequenceEqual(actualPropertyNames),
                $"Expected {{{string.Join(", ", expectedPropertyNames)}}} and actaully received {{{string.Join(", ", actualPropertyNames)}}}.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsAllPublicNonStaticWritableProperties()
        {
            var metadata = ObjectMetadata.GetFor<TestObject>();
            var expectedPropertyNames =
                new[]
                {
                    nameof(TestObject.PartitionKey),
                    nameof(TestObject.RowKey),
                    nameof(TestObject.PublicProperty),
                    nameof(TestObject.PublicWriteOnlyProperty)
                }.OrderBy(propertyName => propertyName);

            var actualPropertyNames = metadata.WritableProperties.Select(property => property.Name).OrderBy(propertyName => propertyName);

            Assert.IsTrue(
                expectedPropertyNames.SequenceEqual(actualPropertyNames),
                $"Expected {{{string.Join(", ", expectedPropertyNames)}}} and actaully received {{{string.Join(", ", actualPropertyNames)}}}.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataHasPartitionKeyPropertyListedInReadableProperties()
        {
            var metadata = ObjectMetadata.GetFor<TestObject>();

            Assert.IsTrue(metadata.ReadableProperties.Contains(metadata.PartitionKeyProperty));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataHasRowKeyPropertyListedInReadableProperties()
        {
            var metadata = ObjectMetadata.GetFor<TestObject>();

            Assert.IsTrue(metadata.ReadableProperties.Contains(metadata.RowKeyProperty));
        }

        private sealed class ObjectWithKeysAndTimestamp
        {
            public string PartitionKey { get; set; }

            public string RowKey { get; set; }

            public DateTime Timestamp { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsPartitionKeyPropertyIfItIsPublicNonStaticReadableAndWritable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithKeysAndTimestamp.PartitionKey), metadata.PartitionKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsRowKeyPropertyIfItIsPublicNonStaticReadableAndWritable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithKeysAndTimestamp.RowKey), metadata.RowKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsTimestampPropertyIfItIsPublicNonStaticReadableAndWritable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithKeysAndTimestamp.Timestamp), metadata.TimestampProperty.Name, ignoreCase: false);
        }

        private sealed class ObjectWithReadOnlyKeysAndTimestamp
        {
            public string PartitionKey { get; }

            public string RowKey { get; }

            public DateTime Timestamp { get; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsPartitionKeyPropertyIfItIsPublicNonStaticReadable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithReadOnlyKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithReadOnlyKeysAndTimestamp.PartitionKey), metadata.PartitionKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsRowKeyPropertyIfItIsPublicNonStaticReadable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithReadOnlyKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithReadOnlyKeysAndTimestamp.RowKey), metadata.RowKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsTimestampPropertyIfItIsPublicNonStaticReadable()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithReadOnlyKeysAndTimestamp>();

            Assert.AreEqual(nameof(ObjectWithReadOnlyKeysAndTimestamp.Timestamp), metadata.TimestampProperty.Name, ignoreCase: false);
        }

        private sealed class ObjectWithWriteOnlyKeysAndTimestamp
        {
            public string PartitionKey { set { } }

            public string RowKey { set { } }

            public DateTime Timestamp { set { } }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsPartitionKeyPropertyIfItIsPublicNonStaticWriteOnly()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithWriteOnlyKeysAndTimestamp>();


            Assert.AreEqual(nameof(ObjectWithWriteOnlyKeysAndTimestamp.PartitionKey), metadata.PartitionKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsRowKeyPropertyIfItIsPublicNonStaticWriteOnly()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithWriteOnlyKeysAndTimestamp>();


            Assert.AreEqual(nameof(ObjectWithWriteOnlyKeysAndTimestamp.RowKey), metadata.RowKeyProperty.Name, ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsTimestampPropertyIfItIsPublicNonStaticWriteOnly()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithWriteOnlyKeysAndTimestamp>();


            Assert.AreEqual(nameof(ObjectWithWriteOnlyKeysAndTimestamp.Timestamp), metadata.TimestampProperty.Name, ignoreCase: false);
        }

        private sealed class ObjectWithStaticKeysTimestamp
        {
            public static string PartitionKey { get; set; }

            public static string RowKey { get; set; }

            public static DateTime Timestamp { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapPartitionKeyPropertyIfItIsStatic()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithStaticKeysTimestamp>();

            Assert.IsNull(metadata.PartitionKeyProperty);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapRowKeyPropertyIfItIsStatic()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithStaticKeysTimestamp>();

            Assert.IsNull(metadata.RowKeyProperty);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapTimestampPropertyIfItIsStatic()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithStaticKeysTimestamp>();

            Assert.IsNull(metadata.TimestampProperty);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataMapsPropertiesFromAnonymousType()
        {
            var @object =
                new
                {
                    Property1 = "value1",
                    Property2 = "value2"
                };
            var expectedPropertyNames =
                new[]
                {
                    nameof(@object.Property1),
                    nameof(@object.Property2)
                }.OrderBy(propertyName => propertyName);
            var metadata = ObjectMetadata.GetFor(@object.GetType());

            var actualPropertyNames = metadata.ReadableProperties.Select(property => property.Name).OrderBy(propertyName => propertyName);

            Assert.IsTrue(expectedPropertyNames.SequenceEqual(actualPropertyNames));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapPartitionKeyIfItNotDefined()
        {
            var metadata = ObjectMetadata.GetFor<object>();

            Assert.IsNull(metadata.PartitionKeyProperty);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapRowKeyIfItNotDefined()
        {
            var metadata = ObjectMetadata.GetFor<object>();

            Assert.IsNull(metadata.RowKeyProperty);
        }

        private sealed class ObjectWithIndexer
        {
            public string this[int index]
            {
                get
                {
                    return default(string);
                }
                set
                {
                }
            }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapReadableIndexer()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithIndexer>();

            Assert.IsFalse(metadata.ReadableProperties.Any());
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotMapWritableIndexer()
        {
            var metadata = ObjectMetadata.GetFor<ObjectWithIndexer>();

            Assert.IsFalse(metadata.WritableProperties.Any());
        }

        private sealed class ObjectHavingPropertiesOfUnsupportedType
        {
            public decimal PartitionIndex { get; set; }

            public DateTimeOffset RowIndex { get; set; }

            public float StringBuilder { get; set; }

            public char StringComparer { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToGetObjectMetadataForTypeContainingPublicPropertiesOfUnsupportedTypesThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectHavingPropertiesOfUnsupportedType>(),
                "Only properties of type byte[], bool, DateTime, double, Guid, int, long and string are supported.");
        }

        private sealed class ObjectWithPartitionKeyOfGuidType
        {
            public Guid PartitionKey { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotSupportPartitionKeyOfDifferentTypeThanString()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectWithPartitionKeyOfGuidType>(),
                "The PartitionKey property must be of type string.");
        }

        private sealed class ObjectWithRowKeyOfGuidType
        {
            public Guid RowKey { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotSupportRowKeyOfDifferentTypeThanString()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectWithRowKeyOfGuidType>(),
                "The RowKey property must be of type string.");
        }

        private sealed class ObjectWithTimestampOfIntType
        {
            public int Timestamp { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataDoesNotSupportTimestampOfDifferentTypeThanDateTime()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectWithTimestampOfIntType>(),
               "The Timestamp property must be of type DateTime.");
        }

        private sealed class ObjectWith256Properties
        {
            public string Property1 { get; set; }

            public string Property2 { get; set; }

            public string Property3 { get; set; }

            public string Property4 { get; set; }

            public string Property5 { get; set; }

            public string Property6 { get; set; }

            public string Property7 { get; set; }

            public string Property8 { get; set; }

            public string Property9 { get; set; }

            public string Property10 { get; set; }

            public string Property11 { get; set; }

            public string Property12 { get; set; }

            public string Property13 { get; set; }

            public string Property14 { get; set; }

            public string Property15 { get; set; }

            public string Property16 { get; set; }

            public string Property17 { get; set; }

            public string Property18 { get; set; }

            public string Property19 { get; set; }

            public string Property20 { get; set; }

            public string Property21 { get; set; }

            public string Property22 { get; set; }

            public string Property23 { get; set; }

            public string Property24 { get; set; }

            public string Property25 { get; set; }

            public string Property26 { get; set; }

            public string Property27 { get; set; }

            public string Property28 { get; set; }

            public string Property29 { get; set; }

            public string Property30 { get; set; }

            public string Property31 { get; set; }

            public string Property32 { get; set; }

            public string Property33 { get; set; }

            public string Property34 { get; set; }

            public string Property35 { get; set; }

            public string Property36 { get; set; }

            public string Property37 { get; set; }

            public string Property38 { get; set; }

            public string Property39 { get; set; }

            public string Property40 { get; set; }

            public string Property41 { get; set; }

            public string Property42 { get; set; }

            public string Property43 { get; set; }

            public string Property44 { get; set; }

            public string Property45 { get; set; }

            public string Property46 { get; set; }

            public string Property47 { get; set; }

            public string Property48 { get; set; }

            public string Property49 { get; set; }

            public string Property50 { get; set; }

            public string Property51 { get; set; }

            public string Property52 { get; set; }

            public string Property53 { get; set; }

            public string Property54 { get; set; }

            public string Property55 { get; set; }

            public string Property56 { get; set; }

            public string Property57 { get; set; }

            public string Property58 { get; set; }

            public string Property59 { get; set; }

            public string Property60 { get; set; }

            public string Property61 { get; set; }

            public string Property62 { get; set; }

            public string Property63 { get; set; }

            public string Property64 { get; set; }

            public string Property65 { get; set; }

            public string Property66 { get; set; }

            public string Property67 { get; set; }

            public string Property68 { get; set; }

            public string Property69 { get; set; }

            public string Property70 { get; set; }

            public string Property71 { get; set; }

            public string Property72 { get; set; }

            public string Property73 { get; set; }

            public string Property74 { get; set; }

            public string Property75 { get; set; }

            public string Property76 { get; set; }

            public string Property77 { get; set; }

            public string Property78 { get; set; }

            public string Property79 { get; set; }

            public string Property80 { get; set; }

            public string Property81 { get; set; }

            public string Property82 { get; set; }

            public string Property83 { get; set; }

            public string Property84 { get; set; }

            public string Property85 { get; set; }

            public string Property86 { get; set; }

            public string Property87 { get; set; }

            public string Property88 { get; set; }

            public string Property89 { get; set; }

            public string Property90 { get; set; }

            public string Property91 { get; set; }

            public string Property92 { get; set; }

            public string Property93 { get; set; }

            public string Property94 { get; set; }

            public string Property95 { get; set; }

            public string Property96 { get; set; }

            public string Property97 { get; set; }

            public string Property98 { get; set; }

            public string Property99 { get; set; }

            public string Property100 { get; set; }

            public string Property101 { get; set; }

            public string Property102 { get; set; }

            public string Property103 { get; set; }

            public string Property104 { get; set; }

            public string Property105 { get; set; }

            public string Property106 { get; set; }

            public string Property107 { get; set; }

            public string Property108 { get; set; }

            public string Property109 { get; set; }

            public string Property110 { get; set; }

            public string Property111 { get; set; }

            public string Property112 { get; set; }

            public string Property113 { get; set; }

            public string Property114 { get; set; }

            public string Property115 { get; set; }

            public string Property116 { get; set; }

            public string Property117 { get; set; }

            public string Property118 { get; set; }

            public string Property119 { get; set; }

            public string Property120 { get; set; }

            public string Property121 { get; set; }

            public string Property122 { get; set; }

            public string Property123 { get; set; }

            public string Property124 { get; set; }

            public string Property125 { get; set; }

            public string Property126 { get; set; }

            public string Property127 { get; set; }

            public string Property128 { get; set; }

            public string Property129 { get; set; }

            public string Property130 { get; set; }

            public string Property131 { get; set; }

            public string Property132 { get; set; }

            public string Property133 { get; set; }

            public string Property134 { get; set; }

            public string Property135 { get; set; }

            public string Property136 { get; set; }

            public string Property137 { get; set; }

            public string Property138 { get; set; }

            public string Property139 { get; set; }

            public string Property140 { get; set; }

            public string Property141 { get; set; }

            public string Property142 { get; set; }

            public string Property143 { get; set; }

            public string Property144 { get; set; }

            public string Property145 { get; set; }

            public string Property146 { get; set; }

            public string Property147 { get; set; }

            public string Property148 { get; set; }

            public string Property149 { get; set; }

            public string Property150 { get; set; }

            public string Property151 { get; set; }

            public string Property152 { get; set; }

            public string Property153 { get; set; }

            public string Property154 { get; set; }

            public string Property155 { get; set; }

            public string Property156 { get; set; }

            public string Property157 { get; set; }

            public string Property158 { get; set; }

            public string Property159 { get; set; }

            public string Property160 { get; set; }

            public string Property161 { get; set; }

            public string Property162 { get; set; }

            public string Property163 { get; set; }

            public string Property164 { get; set; }

            public string Property165 { get; set; }

            public string Property166 { get; set; }

            public string Property167 { get; set; }

            public string Property168 { get; set; }

            public string Property169 { get; set; }

            public string Property170 { get; set; }

            public string Property171 { get; set; }

            public string Property172 { get; set; }

            public string Property173 { get; set; }

            public string Property174 { get; set; }

            public string Property175 { get; set; }

            public string Property176 { get; set; }

            public string Property177 { get; set; }

            public string Property178 { get; set; }

            public string Property179 { get; set; }

            public string Property180 { get; set; }

            public string Property181 { get; set; }

            public string Property182 { get; set; }

            public string Property183 { get; set; }

            public string Property184 { get; set; }

            public string Property185 { get; set; }

            public string Property186 { get; set; }

            public string Property187 { get; set; }

            public string Property188 { get; set; }

            public string Property189 { get; set; }

            public string Property190 { get; set; }

            public string Property191 { get; set; }

            public string Property192 { get; set; }

            public string Property193 { get; set; }

            public string Property194 { get; set; }

            public string Property195 { get; set; }

            public string Property196 { get; set; }

            public string Property197 { get; set; }

            public string Property198 { get; set; }

            public string Property199 { get; set; }

            public string Property200 { get; set; }

            public string Property201 { get; set; }

            public string Property202 { get; set; }

            public string Property203 { get; set; }

            public string Property204 { get; set; }

            public string Property205 { get; set; }

            public string Property206 { get; set; }

            public string Property207 { get; set; }

            public string Property208 { get; set; }

            public string Property209 { get; set; }

            public string Property210 { get; set; }

            public string Property211 { get; set; }

            public string Property212 { get; set; }

            public string Property213 { get; set; }

            public string Property214 { get; set; }

            public string Property215 { get; set; }

            public string Property216 { get; set; }

            public string Property217 { get; set; }

            public string Property218 { get; set; }

            public string Property219 { get; set; }

            public string Property220 { get; set; }

            public string Property221 { get; set; }

            public string Property222 { get; set; }

            public string Property223 { get; set; }

            public string Property224 { get; set; }

            public string Property225 { get; set; }

            public string Property226 { get; set; }

            public string Property227 { get; set; }

            public string Property228 { get; set; }

            public string Property229 { get; set; }

            public string Property230 { get; set; }

            public string Property231 { get; set; }

            public string Property232 { get; set; }

            public string Property233 { get; set; }

            public string Property234 { get; set; }

            public string Property235 { get; set; }

            public string Property236 { get; set; }

            public string Property237 { get; set; }

            public string Property238 { get; set; }

            public string Property239 { get; set; }

            public string Property240 { get; set; }

            public string Property241 { get; set; }

            public string Property242 { get; set; }

            public string Property243 { get; set; }

            public string Property244 { get; set; }

            public string Property245 { get; set; }

            public string Property246 { get; set; }

            public string Property247 { get; set; }

            public string Property248 { get; set; }

            public string Property249 { get; set; }

            public string Property250 { get; set; }

            public string Property251 { get; set; }

            public string Property252 { get; set; }

            public string Property253 { get; set; }

            public string Property254 { get; set; }

            public string Property255 { get; set; }

            public string Property256 { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToGetObjectMetadataForTypeContainingMoreThanTheSupportedNumberOfPropertiesThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectWith256Properties>(),
               "The maximum number of allowed properties on a single object is 255. This includes PartitionKey, RowKey and Timestamp properties.");
        }

        private sealed class ObjectHavingPropertyStartingWithXML
        {
            public string XmlProperty { get; set; }
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestTryingToGetObjectMetadataForTypeContainingPropertiesStrtingWithXMLThrowsException()
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectMetadata.GetFor<ObjectHavingPropertyStartingWithXML>(),
               "Property names can contain only alphanumeric and underscore characters. They can be only 255 characters long and may not begin with XML (in any casing). Property names themselves are case sensitive.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataListsReadablePropertiesSortedByName()
        {
            var metadata = ObjectMetadata.GetFor<MockObject>();

            Assert.IsTrue(
                new[]
                {
                    nameof(MockObject.PartitionKey),
                    nameof(MockObject.RowKey)
                }.SequenceEqual(metadata.ReadableProperties.Select(property => property.Name)));
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestObjectMetadataListsWritablePropertiesSortedByName()
        {
            var metadata = ObjectMetadata.GetFor<MockObject>();

            Assert.IsTrue(
                new[]
                {
                    nameof(MockObject.PartitionKey),
                    nameof(MockObject.RowKey)
                }.SequenceEqual(metadata.WritableProperties.Select(property => property.Name)));
        }
    }
}