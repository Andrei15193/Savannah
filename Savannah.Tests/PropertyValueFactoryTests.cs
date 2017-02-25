using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    [TestClass]
    public class PropertyValueFactoryTests
    {
        private static PropertyValueFactory _Factory { get; } = new PropertyValueFactory();

        [DataTestMethod]
        [DataRow("1F4D", new byte[] { 0x1F, 0x4D })]
        [DataRow("1F4DA4", new byte[] { 0x1F, 0x4D, 0xA4 })]
        [DataRow("0F", new byte[] { 0x0F })]
        [DataRow("F", new byte[] { 0x0F })]
        [DataRow("", new byte[0])]
        [DataRow(" ", new byte[0])]
        [DataRow("\n", new byte[0])]
        [DataRow("\r", new byte[0])]
        [DataRow("\t", new byte[0])]
        [DataRow(default(string), default(byte[]))]
        [DataRow("00", new byte[] { 0x0 })]
        [DataRow("01", new byte[] { 0x1 })]
        [DataRow("02", new byte[] { 0x2 })]
        [DataRow("03", new byte[] { 0x3 })]
        [DataRow("04", new byte[] { 0x4 })]
        [DataRow("05", new byte[] { 0x5 })]
        [DataRow("06", new byte[] { 0x6 })]
        [DataRow("07", new byte[] { 0x7 })]
        [DataRow("08", new byte[] { 0x8 })]
        [DataRow("09", new byte[] { 0x9 })]
        [DataRow("0A", new byte[] { 0xA })]
        [DataRow("0B", new byte[] { 0xB })]
        [DataRow("0C", new byte[] { 0xC })]
        [DataRow("0D", new byte[] { 0xD })]
        [DataRow("0E", new byte[] { 0xE })]
        [DataRow("0F", new byte[] { 0xF })]
        [DataRow("10", new byte[] { 0x10 })]
        [DataRow("11", new byte[] { 0x11 })]
        [DataRow("12", new byte[] { 0x12 })]
        [DataRow("13", new byte[] { 0x13 })]
        [DataRow("14", new byte[] { 0x14 })]
        [DataRow("15", new byte[] { 0x15 })]
        [DataRow("16", new byte[] { 0x16 })]
        [DataRow("17", new byte[] { 0x17 })]
        [DataRow("18", new byte[] { 0x18 })]
        [DataRow("19", new byte[] { 0x19 })]
        [DataRow("1A", new byte[] { 0x1A })]
        [DataRow("1B", new byte[] { 0x1B })]
        [DataRow("1C", new byte[] { 0x1C })]
        [DataRow("1D", new byte[] { 0x1D })]
        [DataRow("1E", new byte[] { 0x1E })]
        [DataRow("1F", new byte[] { 0x1F })]
        [DataRow("20", new byte[] { 0x20 })]
        [DataRow("21", new byte[] { 0x21 })]
        [DataRow("22", new byte[] { 0x22 })]
        [DataRow("23", new byte[] { 0x23 })]
        [DataRow("24", new byte[] { 0x24 })]
        [DataRow("25", new byte[] { 0x25 })]
        [DataRow("26", new byte[] { 0x26 })]
        [DataRow("27", new byte[] { 0x27 })]
        [DataRow("28", new byte[] { 0x28 })]
        [DataRow("29", new byte[] { 0x29 })]
        [DataRow("2A", new byte[] { 0x2A })]
        [DataRow("2B", new byte[] { 0x2B })]
        [DataRow("2C", new byte[] { 0x2C })]
        [DataRow("2D", new byte[] { 0x2D })]
        [DataRow("2E", new byte[] { 0x2E })]
        [DataRow("2F", new byte[] { 0x2F })]
        [DataRow("30", new byte[] { 0x30 })]
        [DataRow("31", new byte[] { 0x31 })]
        [DataRow("32", new byte[] { 0x32 })]
        [DataRow("33", new byte[] { 0x33 })]
        [DataRow("34", new byte[] { 0x34 })]
        [DataRow("35", new byte[] { 0x35 })]
        [DataRow("36", new byte[] { 0x36 })]
        [DataRow("37", new byte[] { 0x37 })]
        [DataRow("38", new byte[] { 0x38 })]
        [DataRow("39", new byte[] { 0x39 })]
        [DataRow("3A", new byte[] { 0x3A })]
        [DataRow("3B", new byte[] { 0x3B })]
        [DataRow("3C", new byte[] { 0x3C })]
        [DataRow("3D", new byte[] { 0x3D })]
        [DataRow("3E", new byte[] { 0x3E })]
        [DataRow("3F", new byte[] { 0x3F })]
        [DataRow("40", new byte[] { 0x40 })]
        [DataRow("41", new byte[] { 0x41 })]
        [DataRow("42", new byte[] { 0x42 })]
        [DataRow("43", new byte[] { 0x43 })]
        [DataRow("44", new byte[] { 0x44 })]
        [DataRow("45", new byte[] { 0x45 })]
        [DataRow("46", new byte[] { 0x46 })]
        [DataRow("47", new byte[] { 0x47 })]
        [DataRow("48", new byte[] { 0x48 })]
        [DataRow("49", new byte[] { 0x49 })]
        [DataRow("4A", new byte[] { 0x4A })]
        [DataRow("4B", new byte[] { 0x4B })]
        [DataRow("4C", new byte[] { 0x4C })]
        [DataRow("4D", new byte[] { 0x4D })]
        [DataRow("4E", new byte[] { 0x4E })]
        [DataRow("4F", new byte[] { 0x4F })]
        [DataRow("50", new byte[] { 0x50 })]
        [DataRow("51", new byte[] { 0x51 })]
        [DataRow("52", new byte[] { 0x52 })]
        [DataRow("53", new byte[] { 0x53 })]
        [DataRow("54", new byte[] { 0x54 })]
        [DataRow("55", new byte[] { 0x55 })]
        [DataRow("56", new byte[] { 0x56 })]
        [DataRow("57", new byte[] { 0x57 })]
        [DataRow("58", new byte[] { 0x58 })]
        [DataRow("59", new byte[] { 0x59 })]
        [DataRow("5A", new byte[] { 0x5A })]
        [DataRow("5B", new byte[] { 0x5B })]
        [DataRow("5C", new byte[] { 0x5C })]
        [DataRow("5D", new byte[] { 0x5D })]
        [DataRow("5E", new byte[] { 0x5E })]
        [DataRow("5F", new byte[] { 0x5F })]
        [DataRow("60", new byte[] { 0x60 })]
        [DataRow("61", new byte[] { 0x61 })]
        [DataRow("62", new byte[] { 0x62 })]
        [DataRow("63", new byte[] { 0x63 })]
        [DataRow("64", new byte[] { 0x64 })]
        [DataRow("65", new byte[] { 0x65 })]
        [DataRow("66", new byte[] { 0x66 })]
        [DataRow("67", new byte[] { 0x67 })]
        [DataRow("68", new byte[] { 0x68 })]
        [DataRow("69", new byte[] { 0x69 })]
        [DataRow("6A", new byte[] { 0x6A })]
        [DataRow("6B", new byte[] { 0x6B })]
        [DataRow("6C", new byte[] { 0x6C })]
        [DataRow("6D", new byte[] { 0x6D })]
        [DataRow("6E", new byte[] { 0x6E })]
        [DataRow("6F", new byte[] { 0x6F })]
        [DataRow("70", new byte[] { 0x70 })]
        [DataRow("71", new byte[] { 0x71 })]
        [DataRow("72", new byte[] { 0x72 })]
        [DataRow("73", new byte[] { 0x73 })]
        [DataRow("74", new byte[] { 0x74 })]
        [DataRow("75", new byte[] { 0x75 })]
        [DataRow("76", new byte[] { 0x76 })]
        [DataRow("77", new byte[] { 0x77 })]
        [DataRow("78", new byte[] { 0x78 })]
        [DataRow("79", new byte[] { 0x79 })]
        [DataRow("7A", new byte[] { 0x7A })]
        [DataRow("7B", new byte[] { 0x7B })]
        [DataRow("7C", new byte[] { 0x7C })]
        [DataRow("7D", new byte[] { 0x7D })]
        [DataRow("7E", new byte[] { 0x7E })]
        [DataRow("7F", new byte[] { 0x7F })]
        [DataRow("80", new byte[] { 0x80 })]
        [DataRow("81", new byte[] { 0x81 })]
        [DataRow("82", new byte[] { 0x82 })]
        [DataRow("83", new byte[] { 0x83 })]
        [DataRow("84", new byte[] { 0x84 })]
        [DataRow("85", new byte[] { 0x85 })]
        [DataRow("86", new byte[] { 0x86 })]
        [DataRow("87", new byte[] { 0x87 })]
        [DataRow("88", new byte[] { 0x88 })]
        [DataRow("89", new byte[] { 0x89 })]
        [DataRow("8A", new byte[] { 0x8A })]
        [DataRow("8B", new byte[] { 0x8B })]
        [DataRow("8C", new byte[] { 0x8C })]
        [DataRow("8D", new byte[] { 0x8D })]
        [DataRow("8E", new byte[] { 0x8E })]
        [DataRow("8F", new byte[] { 0x8F })]
        [DataRow("90", new byte[] { 0x90 })]
        [DataRow("91", new byte[] { 0x91 })]
        [DataRow("92", new byte[] { 0x92 })]
        [DataRow("93", new byte[] { 0x93 })]
        [DataRow("94", new byte[] { 0x94 })]
        [DataRow("95", new byte[] { 0x95 })]
        [DataRow("96", new byte[] { 0x96 })]
        [DataRow("97", new byte[] { 0x97 })]
        [DataRow("98", new byte[] { 0x98 })]
        [DataRow("99", new byte[] { 0x99 })]
        [DataRow("9A", new byte[] { 0x9A })]
        [DataRow("9B", new byte[] { 0x9B })]
        [DataRow("9C", new byte[] { 0x9C })]
        [DataRow("9D", new byte[] { 0x9D })]
        [DataRow("9E", new byte[] { 0x9E })]
        [DataRow("9F", new byte[] { 0x9F })]
        [DataRow("A0", new byte[] { 0xA0 })]
        [DataRow("A1", new byte[] { 0xA1 })]
        [DataRow("A2", new byte[] { 0xA2 })]
        [DataRow("A3", new byte[] { 0xA3 })]
        [DataRow("A4", new byte[] { 0xA4 })]
        [DataRow("A5", new byte[] { 0xA5 })]
        [DataRow("A6", new byte[] { 0xA6 })]
        [DataRow("A7", new byte[] { 0xA7 })]
        [DataRow("A8", new byte[] { 0xA8 })]
        [DataRow("A9", new byte[] { 0xA9 })]
        [DataRow("AA", new byte[] { 0xAA })]
        [DataRow("AB", new byte[] { 0xAB })]
        [DataRow("AC", new byte[] { 0xAC })]
        [DataRow("AD", new byte[] { 0xAD })]
        [DataRow("AE", new byte[] { 0xAE })]
        [DataRow("AF", new byte[] { 0xAF })]
        [DataRow("B0", new byte[] { 0xB0 })]
        [DataRow("B1", new byte[] { 0xB1 })]
        [DataRow("B2", new byte[] { 0xB2 })]
        [DataRow("B3", new byte[] { 0xB3 })]
        [DataRow("B4", new byte[] { 0xB4 })]
        [DataRow("B5", new byte[] { 0xB5 })]
        [DataRow("B6", new byte[] { 0xB6 })]
        [DataRow("B7", new byte[] { 0xB7 })]
        [DataRow("B8", new byte[] { 0xB8 })]
        [DataRow("B9", new byte[] { 0xB9 })]
        [DataRow("BA", new byte[] { 0xBA })]
        [DataRow("BB", new byte[] { 0xBB })]
        [DataRow("BC", new byte[] { 0xBC })]
        [DataRow("BD", new byte[] { 0xBD })]
        [DataRow("BE", new byte[] { 0xBE })]
        [DataRow("BF", new byte[] { 0xBF })]
        [DataRow("C0", new byte[] { 0xC0 })]
        [DataRow("C1", new byte[] { 0xC1 })]
        [DataRow("C2", new byte[] { 0xC2 })]
        [DataRow("C3", new byte[] { 0xC3 })]
        [DataRow("C4", new byte[] { 0xC4 })]
        [DataRow("C5", new byte[] { 0xC5 })]
        [DataRow("C6", new byte[] { 0xC6 })]
        [DataRow("C7", new byte[] { 0xC7 })]
        [DataRow("C8", new byte[] { 0xC8 })]
        [DataRow("C9", new byte[] { 0xC9 })]
        [DataRow("CA", new byte[] { 0xCA })]
        [DataRow("CB", new byte[] { 0xCB })]
        [DataRow("CC", new byte[] { 0xCC })]
        [DataRow("CD", new byte[] { 0xCD })]
        [DataRow("CE", new byte[] { 0xCE })]
        [DataRow("CF", new byte[] { 0xCF })]
        [DataRow("D0", new byte[] { 0xD0 })]
        [DataRow("D1", new byte[] { 0xD1 })]
        [DataRow("D2", new byte[] { 0xD2 })]
        [DataRow("D3", new byte[] { 0xD3 })]
        [DataRow("D4", new byte[] { 0xD4 })]
        [DataRow("D5", new byte[] { 0xD5 })]
        [DataRow("D6", new byte[] { 0xD6 })]
        [DataRow("D7", new byte[] { 0xD7 })]
        [DataRow("D8", new byte[] { 0xD8 })]
        [DataRow("D9", new byte[] { 0xD9 })]
        [DataRow("DA", new byte[] { 0xDA })]
        [DataRow("DB", new byte[] { 0xDB })]
        [DataRow("DC", new byte[] { 0xDC })]
        [DataRow("DD", new byte[] { 0xDD })]
        [DataRow("DE", new byte[] { 0xDE })]
        [DataRow("DF", new byte[] { 0xDF })]
        [DataRow("E0", new byte[] { 0xE0 })]
        [DataRow("E1", new byte[] { 0xE1 })]
        [DataRow("E2", new byte[] { 0xE2 })]
        [DataRow("E3", new byte[] { 0xE3 })]
        [DataRow("E4", new byte[] { 0xE4 })]
        [DataRow("E5", new byte[] { 0xE5 })]
        [DataRow("E6", new byte[] { 0xE6 })]
        [DataRow("E7", new byte[] { 0xE7 })]
        [DataRow("E8", new byte[] { 0xE8 })]
        [DataRow("E9", new byte[] { 0xE9 })]
        [DataRow("EA", new byte[] { 0xEA })]
        [DataRow("EB", new byte[] { 0xEB })]
        [DataRow("EC", new byte[] { 0xEC })]
        [DataRow("ED", new byte[] { 0xED })]
        [DataRow("EE", new byte[] { 0xEE })]
        [DataRow("EF", new byte[] { 0xEF })]
        [DataRow("F0", new byte[] { 0xF0 })]
        [DataRow("F1", new byte[] { 0xF1 })]
        [DataRow("F2", new byte[] { 0xF2 })]
        [DataRow("F3", new byte[] { 0xF3 })]
        [DataRow("F4", new byte[] { 0xF4 })]
        [DataRow("F5", new byte[] { 0xF5 })]
        [DataRow("F6", new byte[] { 0xF6 })]
        [DataRow("F7", new byte[] { 0xF7 })]
        [DataRow("F8", new byte[] { 0xF8 })]
        [DataRow("F9", new byte[] { 0xF9 })]
        [DataRow("FA", new byte[] { 0xFA })]
        [DataRow("FB", new byte[] { 0xFB })]
        [DataRow("FC", new byte[] { 0xFC })]
        [DataRow("FD", new byte[] { 0xFD })]
        [DataRow("FE", new byte[] { 0xFE })]
        [DataRow("FF", new byte[] { 0xFF })]
        public void TestBinaryPropertyIsConvertedAccordingly(string byteArrayString, byte[] expectedByteArray)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                byteArrayString,
                ValueType.Binary);

            var result = (byte[])_Factory.GetPropertyValueFrom(storageObjectProperty);

            if (expectedByteArray == null)
                Assert.IsNull(result);
            else
                Assert.IsTrue(expectedByteArray.SequenceEqual(result));
        }

        [DataTestMethod]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void TestBooleanPropertyIsSetAccordingly(string booleanString, bool expectedBooleanValue)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                booleanString,
                ValueType.Boolean);

            var result = (bool)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedBooleanValue, result);
        }

        [DataTestMethod]
        [DataRow("2015/01/22 01:05:09:0130000Z", 2015, 1, 22, 1, 5, 9, 13, DateTimeKind.Utc)]
        [DataRow("2016/02/23 02:06:10:0140000+02:00", 2016, 2, 23, 2, 6, 10, 14, DateTimeKind.Local)]
        [DataRow("2017/03/24 04:07:11:0150000+03:00", 2017, 3, 24, 3, 7, 11, 15, DateTimeKind.Local)]
        [DataRow("2018/04/25 04:08:12:0160000", 2018, 4, 25, 4, 8, 12, 16, DateTimeKind.Unspecified)]
        public void TestDateTimePropertyIsSetAccordingly(string dateTimeString, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSecond, int expectedMillisecond, DateTimeKind expectedDateTimeKind)
        {
            var expectedTimestamp = new DateTime(expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSecond, expectedMillisecond, expectedDateTimeKind);
            var storageObjectProperty = new StorageObjectProperty(
                null,
                dateTimeString,
                ValueType.DateTime);

            var result = (DateTime)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedTimestamp, result);
        }

        [DataTestMethod]
        [DataRow("1", 1D)]
        [DataRow("1.1", 1.1D)]
        [DataRow("-1", -1D)]
        [DataRow("-1.1", -1.1D)]
        [DataRow("0", 0D)]
        public void TestDoublePropertyIsSetAccordingly(string doubleString, double expectedDoubleValue)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                doubleString,
                ValueType.Double);

            var result = (double)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedDoubleValue, result);
        }

        [DataTestMethod]
        [DataRow("54df4e54-d850-4a55-ac8f-3cc2b25cb101", new byte[16] { 0x54, 0x4E, 0xDF, 0x54, 0x50, 0xD8, 0x55, 0x4A, 0xAC, 0x8F, 0x3C, 0xC2, 0xB2, 0x5C, 0xB1, 0x01 })]
        [DataRow("E4650D21-0036-4E79-B912-D327ACE2AA6F", new byte[16] { 0x21, 0x0D, 0x65, 0xE4, 0x36, 0x00, 0x79, 0x4E, 0xB9, 0x12, 0xD3, 0x27, 0xAC, 0xE2, 0xAA, 0x6F })]
        [DataRow("00000000-0000-0000-0000-000000000000", new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        public void TestGuidPropertyIsSetAccordingly(string guidString, byte[] expectedGuidBytes)
        {
            var expectedGuid = new Guid(expectedGuidBytes);
            var storageObjectProperty = new StorageObjectProperty(
                null,
                guidString,
                ValueType.Guid);

            var result = (Guid)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedGuid, result);
        }

        [DataTestMethod]
        [DataRow("1", 1)]
        [DataRow("-1", -1)]
        [DataRow("2147483647", 2147483647)]
        [DataRow("-2147483648", -2147483648)]
        [DataRow("0", 0)]
        public void TestIntPropertyIsSetAccordingly(string intString, int expectedIntValue)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                intString,
                ValueType.Int);

            var result = (int)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedIntValue, result);
        }

        [DataTestMethod]
        [DataRow("1", 1L)]
        [DataRow("-1", -1L)]
        [DataRow("2147483647", 2147483647L)]
        [DataRow("-2147483648", -2147483648L)]
        [DataRow("9223372036854775807", 9223372036854775807)]
        [DataRow("-9223372036854775808", -9223372036854775808)]
        [DataRow("0", 0L)]
        public void TestLongPropertyIsSetAccordingly(string longString, long expectedLongValue)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                longString,
                ValueType.Long);

            var result = (long)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedLongValue, result);
        }

        [DataTestMethod]
        [DataRow("testValue", "testValue")]
        [DataRow(" ", " ")]
        [DataRow("\t", "\t")]
        [DataRow("\r", "\r")]
        [DataRow("\n", "\n")]
        [DataRow(default(string), default(string))]
        public void TestStringPropertyIsSetAccordingly(string @string, string expectedString)
        {
            var storageObjectProperty = new StorageObjectProperty(
                null,
                @string,
                ValueType.String);
            var storageObject = new StorageObject(null, null, null, storageObjectProperty);

            var result = (string)_Factory.GetPropertyValueFrom(storageObjectProperty);

            Assert.AreEqual(expectedString, result, ignoreCase: false);
        }
    }
}