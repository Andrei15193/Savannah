﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreLimitationsTests
    {
        private sealed class ObjectStoreOperationMock
            : ObjectStoreOperation
        {
            public ObjectStoreOperationMock(ObjectStoreOperationType operationType, object @object) : base(@object)
            {
                OperationType = operationType;
            }

            public override ObjectStoreOperationType OperationType { get; }

            internal override StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext executionContext)
                => null;
        }

        [DataTestMethod]
        [DataRow("/")]
        [DataRow("\\")]
        [DataRow("#")]
        [DataRow("?")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]

        [DataRow("\x0000")]
        [DataRow("\x0001")]
        [DataRow("\x0002")]
        [DataRow("\x0003")]
        [DataRow("\x0004")]
        [DataRow("\x0005")]
        [DataRow("\x0006")]
        [DataRow("\x0007")]
        [DataRow("\x0008")]
        [DataRow("\x0009")]
        [DataRow("\x000A")]
        [DataRow("\x000B")]
        [DataRow("\x000C")]
        [DataRow("\x000D")]
        [DataRow("\x000E")]
        [DataRow("\x000F")]
        [DataRow("\x0010")]
        [DataRow("\x0011")]
        [DataRow("\x0012")]
        [DataRow("\x0013")]
        [DataRow("\x0014")]
        [DataRow("\x0015")]
        [DataRow("\x0016")]
        [DataRow("\x0017")]
        [DataRow("\x0018")]
        [DataRow("\x0019")]
        [DataRow("\x001A")]
        [DataRow("\x001B")]
        [DataRow("\x001C")]
        [DataRow("\x001D")]
        [DataRow("\x001E")]
        [DataRow("\x001F")]

        [DataRow("\x007F")]
        [DataRow("\x0080")]
        [DataRow("\x0081")]
        [DataRow("\x0082")]
        [DataRow("\x0083")]
        [DataRow("\x0084")]
        [DataRow("\x0085")]
        [DataRow("\x0086")]
        [DataRow("\x0087")]
        [DataRow("\x0088")]
        [DataRow("\x0089")]
        [DataRow("\x008A")]
        [DataRow("\x008B")]
        [DataRow("\x008C")]
        [DataRow("\x008D")]
        [DataRow("\x008E")]
        [DataRow("\x008F")]
        [DataRow("\x0090")]
        [DataRow("\x0091")]
        [DataRow("\x0092")]
        [DataRow("\x0093")]
        [DataRow("\x0094")]
        [DataRow("\x0095")]
        [DataRow("\x0096")]
        [DataRow("\x0097")]
        [DataRow("\x0098")]
        [DataRow("\x0099")]
        [DataRow("\x009A")]
        [DataRow("\x009B")]
        [DataRow("\x009C")]
        [DataRow("\x009D")]
        [DataRow("\x009E")]
        [DataRow("\x009F")]
        public void TestPartitionKeyCannotContainInvalidCharacter(string partitionKey)
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckPartitionKey(partitionKey),
                @"A partition key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        [DataTestMethod]
        [DataRow("partitionKey")]
        [DataRow("partition Key")]
        [DataRow("Partition Key 123")]
        [DataRow("This partition key is 512 characters long.                                                                                                                                                                                                                                                                                                                                                                                                                                                                       It really is...")]

        [DataRow("\x0020")]
        [DataRow("\x0021")]
        [DataRow("\x0022")]
        [DataRow("\x0024")]
        [DataRow("\x0025")]
        [DataRow("\x0026")]
        [DataRow("\x0027")]
        [DataRow("\x0028")]
        [DataRow("\x0029")]
        [DataRow("\x002A")]
        [DataRow("\x002B")]
        [DataRow("\x002C")]
        [DataRow("\x002D")]
        [DataRow("\x002E")]
        [DataRow("\x0030")]
        [DataRow("\x0031")]
        [DataRow("\x0032")]
        [DataRow("\x0033")]
        [DataRow("\x0034")]
        [DataRow("\x0035")]
        [DataRow("\x0036")]
        [DataRow("\x0037")]
        [DataRow("\x0038")]
        [DataRow("\x0039")]
        [DataRow("\x003A")]
        [DataRow("\x003B")]
        [DataRow("\x003C")]
        [DataRow("\x003D")]
        [DataRow("\x003E")]
        [DataRow("\x0040")]
        [DataRow("\x0041")]
        [DataRow("\x0042")]
        [DataRow("\x0043")]
        [DataRow("\x0044")]
        [DataRow("\x0045")]
        [DataRow("\x0046")]
        [DataRow("\x0047")]
        [DataRow("\x0048")]
        [DataRow("\x0049")]
        [DataRow("\x004A")]
        [DataRow("\x004B")]
        [DataRow("\x004C")]
        [DataRow("\x004D")]
        [DataRow("\x004E")]
        [DataRow("\x004F")]
        [DataRow("\x0050")]
        [DataRow("\x0051")]
        [DataRow("\x0052")]
        [DataRow("\x0053")]
        [DataRow("\x0054")]
        [DataRow("\x0055")]
        [DataRow("\x0056")]
        [DataRow("\x0057")]
        [DataRow("\x0058")]
        [DataRow("\x0059")]
        [DataRow("\x005A")]
        [DataRow("\x005B")]
        [DataRow("\x005D")]
        [DataRow("\x005E")]
        [DataRow("\x005F")]
        [DataRow("\x0060")]
        [DataRow("\x0061")]
        [DataRow("\x0062")]
        [DataRow("\x0063")]
        [DataRow("\x0064")]
        [DataRow("\x0065")]
        [DataRow("\x0066")]
        [DataRow("\x0067")]
        [DataRow("\x0068")]
        [DataRow("\x0069")]
        [DataRow("\x006A")]
        [DataRow("\x006B")]
        [DataRow("\x006C")]
        [DataRow("\x006D")]
        [DataRow("\x006E")]
        [DataRow("\x006F")]
        [DataRow("\x0070")]
        [DataRow("\x0071")]
        [DataRow("\x0072")]
        [DataRow("\x0073")]
        [DataRow("\x0074")]
        [DataRow("\x0075")]
        [DataRow("\x0076")]
        [DataRow("\x0077")]
        [DataRow("\x0078")]
        [DataRow("\x0079")]
        [DataRow("\x007A")]
        [DataRow("\x007B")]
        [DataRow("\x007C")]
        [DataRow("\x007D")]
        [DataRow("\x007E")]

        [DataRow("\x00A0")]
        [DataRow("\x00A1")]
        [DataRow("\x00A2")]
        [DataRow("\x00A3")]
        [DataRow("\x00A4")]
        [DataRow("\x00A5")]
        [DataRow("\x00A6")]
        [DataRow("\x00A7")]
        [DataRow("\x00A8")]
        [DataRow("\x00A9")]
        [DataRow("\x00AA")]
        [DataRow("\x00AB")]
        [DataRow("\x00AC")]
        [DataRow("\x00AD")]
        [DataRow("\x00AE")]
        [DataRow("\x00AF")]
        [DataRow("\x00B0")]
        [DataRow("\x00B1")]
        [DataRow("\x00B2")]
        [DataRow("\x00B3")]
        [DataRow("\x00B4")]
        [DataRow("\x00B5")]
        [DataRow("\x00B6")]
        [DataRow("\x00B7")]
        [DataRow("\x00B8")]
        [DataRow("\x00B9")]
        [DataRow("\x00BA")]
        [DataRow("\x00BB")]
        [DataRow("\x00BC")]
        [DataRow("\x00BD")]
        [DataRow("\x00BE")]
        [DataRow("\x00BF")]
        [DataRow("\x00C0")]
        [DataRow("\x00C1")]
        [DataRow("\x00C2")]
        [DataRow("\x00C3")]
        [DataRow("\x00C4")]
        [DataRow("\x00C5")]
        [DataRow("\x00C6")]
        [DataRow("\x00C7")]
        [DataRow("\x00C8")]
        [DataRow("\x00C9")]
        [DataRow("\x00CA")]
        [DataRow("\x00CB")]
        [DataRow("\x00CC")]
        [DataRow("\x00CD")]
        [DataRow("\x00CE")]
        [DataRow("\x00CF")]
        [DataRow("\x00D0")]
        [DataRow("\x00D1")]
        [DataRow("\x00D2")]
        [DataRow("\x00D3")]
        [DataRow("\x00D4")]
        [DataRow("\x00D5")]
        [DataRow("\x00D6")]
        [DataRow("\x00D7")]
        [DataRow("\x00D8")]
        [DataRow("\x00D9")]
        [DataRow("\x00DA")]
        [DataRow("\x00DB")]
        [DataRow("\x00DC")]
        [DataRow("\x00DD")]
        [DataRow("\x00DE")]
        [DataRow("\x00DF")]
        [DataRow("\x00E0")]
        [DataRow("\x00E1")]
        [DataRow("\x00E2")]
        [DataRow("\x00E3")]
        [DataRow("\x00E4")]
        [DataRow("\x00E5")]
        [DataRow("\x00E6")]
        [DataRow("\x00E7")]
        [DataRow("\x00E8")]
        [DataRow("\x00E9")]
        [DataRow("\x00EA")]
        [DataRow("\x00EB")]
        [DataRow("\x00EC")]
        [DataRow("\x00ED")]
        [DataRow("\x00EE")]
        [DataRow("\x00EF")]
        [DataRow("\x00F0")]
        [DataRow("\x00F1")]
        [DataRow("\x00F2")]
        [DataRow("\x00F3")]
        [DataRow("\x00F4")]
        [DataRow("\x00F5")]
        [DataRow("\x00F6")]
        [DataRow("\x00F7")]
        [DataRow("\x00F8")]
        [DataRow("\x00F9")]
        [DataRow("\x00FA")]
        [DataRow("\x00FB")]
        [DataRow("\x00FC")]
        [DataRow("\x00FD")]
        [DataRow("\x00FE")]
        [DataRow("\x00FF")]
        public void TestValidPartitionKey(string partitionKey)
        {
            string unicodeLiteralPartitionKey = _GetInUnicodeLiterals(partitionKey);

            try
            {
                ObjectStoreLimitations.CheckPartitionKey(partitionKey);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail($"Unexpected InvalidOperationException for {partitionKey} ({unicodeLiteralPartitionKey})");
            }
        }

        [DataTestMethod]
        [DataRow(513)]
        [DataRow(1000)]
        public void TestPartitionKeyTooLongThrowsException(int partitionKeyLength)
        {
            var partitionKey = new string('t', partitionKeyLength);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckPartitionKey(partitionKey),
                "The maximum supported length of a partition key is 512 characters.");
        }

        [DataTestMethod]
        [DataRow("/")]
        [DataRow("\\")]
        [DataRow("#")]
        [DataRow("?")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]

        [DataRow("\x0000")]
        [DataRow("\x0001")]
        [DataRow("\x0002")]
        [DataRow("\x0003")]
        [DataRow("\x0004")]
        [DataRow("\x0005")]
        [DataRow("\x0006")]
        [DataRow("\x0007")]
        [DataRow("\x0008")]
        [DataRow("\x0009")]
        [DataRow("\x000A")]
        [DataRow("\x000B")]
        [DataRow("\x000C")]
        [DataRow("\x000D")]
        [DataRow("\x000E")]
        [DataRow("\x000F")]
        [DataRow("\x0010")]
        [DataRow("\x0011")]
        [DataRow("\x0012")]
        [DataRow("\x0013")]
        [DataRow("\x0014")]
        [DataRow("\x0015")]
        [DataRow("\x0016")]
        [DataRow("\x0017")]
        [DataRow("\x0018")]
        [DataRow("\x0019")]
        [DataRow("\x001A")]
        [DataRow("\x001B")]
        [DataRow("\x001C")]
        [DataRow("\x001D")]
        [DataRow("\x001E")]
        [DataRow("\x001F")]

        [DataRow("\x007F")]
        [DataRow("\x0080")]
        [DataRow("\x0081")]
        [DataRow("\x0082")]
        [DataRow("\x0083")]
        [DataRow("\x0084")]
        [DataRow("\x0085")]
        [DataRow("\x0086")]
        [DataRow("\x0087")]
        [DataRow("\x0088")]
        [DataRow("\x0089")]
        [DataRow("\x008A")]
        [DataRow("\x008B")]
        [DataRow("\x008C")]
        [DataRow("\x008D")]
        [DataRow("\x008E")]
        [DataRow("\x008F")]
        [DataRow("\x0090")]
        [DataRow("\x0091")]
        [DataRow("\x0092")]
        [DataRow("\x0093")]
        [DataRow("\x0094")]
        [DataRow("\x0095")]
        [DataRow("\x0096")]
        [DataRow("\x0097")]
        [DataRow("\x0098")]
        [DataRow("\x0099")]
        [DataRow("\x009A")]
        [DataRow("\x009B")]
        [DataRow("\x009C")]
        [DataRow("\x009D")]
        [DataRow("\x009E")]
        [DataRow("\x009F")]
        public void TestRowKeyCannotContainInvalidCharacter(string rowKey)
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckRowKey(rowKey),
                @"A row key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        [DataTestMethod]
        [DataRow("partitionKey")]
        [DataRow("partition Key")]
        [DataRow("Partition Key 123")]
        [DataRow("This partition key is 512 characters long.                                                                                                                                                                                                                                                                                                                                                                                                                                                                       It really is...")]

        [DataRow("\x0020")]
        [DataRow("\x0021")]
        [DataRow("\x0022")]
        [DataRow("\x0024")]
        [DataRow("\x0025")]
        [DataRow("\x0026")]
        [DataRow("\x0027")]
        [DataRow("\x0028")]
        [DataRow("\x0029")]
        [DataRow("\x002A")]
        [DataRow("\x002B")]
        [DataRow("\x002C")]
        [DataRow("\x002D")]
        [DataRow("\x002E")]
        [DataRow("\x0030")]
        [DataRow("\x0031")]
        [DataRow("\x0032")]
        [DataRow("\x0033")]
        [DataRow("\x0034")]
        [DataRow("\x0035")]
        [DataRow("\x0036")]
        [DataRow("\x0037")]
        [DataRow("\x0038")]
        [DataRow("\x0039")]
        [DataRow("\x003A")]
        [DataRow("\x003B")]
        [DataRow("\x003C")]
        [DataRow("\x003D")]
        [DataRow("\x003E")]
        [DataRow("\x0040")]
        [DataRow("\x0041")]
        [DataRow("\x0042")]
        [DataRow("\x0043")]
        [DataRow("\x0044")]
        [DataRow("\x0045")]
        [DataRow("\x0046")]
        [DataRow("\x0047")]
        [DataRow("\x0048")]
        [DataRow("\x0049")]
        [DataRow("\x004A")]
        [DataRow("\x004B")]
        [DataRow("\x004C")]
        [DataRow("\x004D")]
        [DataRow("\x004E")]
        [DataRow("\x004F")]
        [DataRow("\x0050")]
        [DataRow("\x0051")]
        [DataRow("\x0052")]
        [DataRow("\x0053")]
        [DataRow("\x0054")]
        [DataRow("\x0055")]
        [DataRow("\x0056")]
        [DataRow("\x0057")]
        [DataRow("\x0058")]
        [DataRow("\x0059")]
        [DataRow("\x005A")]
        [DataRow("\x005B")]
        [DataRow("\x005D")]
        [DataRow("\x005E")]
        [DataRow("\x005F")]
        [DataRow("\x0060")]
        [DataRow("\x0061")]
        [DataRow("\x0062")]
        [DataRow("\x0063")]
        [DataRow("\x0064")]
        [DataRow("\x0065")]
        [DataRow("\x0066")]
        [DataRow("\x0067")]
        [DataRow("\x0068")]
        [DataRow("\x0069")]
        [DataRow("\x006A")]
        [DataRow("\x006B")]
        [DataRow("\x006C")]
        [DataRow("\x006D")]
        [DataRow("\x006E")]
        [DataRow("\x006F")]
        [DataRow("\x0070")]
        [DataRow("\x0071")]
        [DataRow("\x0072")]
        [DataRow("\x0073")]
        [DataRow("\x0074")]
        [DataRow("\x0075")]
        [DataRow("\x0076")]
        [DataRow("\x0077")]
        [DataRow("\x0078")]
        [DataRow("\x0079")]
        [DataRow("\x007A")]
        [DataRow("\x007B")]
        [DataRow("\x007C")]
        [DataRow("\x007D")]
        [DataRow("\x007E")]

        [DataRow("\x00A0")]
        [DataRow("\x00A1")]
        [DataRow("\x00A2")]
        [DataRow("\x00A3")]
        [DataRow("\x00A4")]
        [DataRow("\x00A5")]
        [DataRow("\x00A6")]
        [DataRow("\x00A7")]
        [DataRow("\x00A8")]
        [DataRow("\x00A9")]
        [DataRow("\x00AA")]
        [DataRow("\x00AB")]
        [DataRow("\x00AC")]
        [DataRow("\x00AD")]
        [DataRow("\x00AE")]
        [DataRow("\x00AF")]
        [DataRow("\x00B0")]
        [DataRow("\x00B1")]
        [DataRow("\x00B2")]
        [DataRow("\x00B3")]
        [DataRow("\x00B4")]
        [DataRow("\x00B5")]
        [DataRow("\x00B6")]
        [DataRow("\x00B7")]
        [DataRow("\x00B8")]
        [DataRow("\x00B9")]
        [DataRow("\x00BA")]
        [DataRow("\x00BB")]
        [DataRow("\x00BC")]
        [DataRow("\x00BD")]
        [DataRow("\x00BE")]
        [DataRow("\x00BF")]
        [DataRow("\x00C0")]
        [DataRow("\x00C1")]
        [DataRow("\x00C2")]
        [DataRow("\x00C3")]
        [DataRow("\x00C4")]
        [DataRow("\x00C5")]
        [DataRow("\x00C6")]
        [DataRow("\x00C7")]
        [DataRow("\x00C8")]
        [DataRow("\x00C9")]
        [DataRow("\x00CA")]
        [DataRow("\x00CB")]
        [DataRow("\x00CC")]
        [DataRow("\x00CD")]
        [DataRow("\x00CE")]
        [DataRow("\x00CF")]
        [DataRow("\x00D0")]
        [DataRow("\x00D1")]
        [DataRow("\x00D2")]
        [DataRow("\x00D3")]
        [DataRow("\x00D4")]
        [DataRow("\x00D5")]
        [DataRow("\x00D6")]
        [DataRow("\x00D7")]
        [DataRow("\x00D8")]
        [DataRow("\x00D9")]
        [DataRow("\x00DA")]
        [DataRow("\x00DB")]
        [DataRow("\x00DC")]
        [DataRow("\x00DD")]
        [DataRow("\x00DE")]
        [DataRow("\x00DF")]
        [DataRow("\x00E0")]
        [DataRow("\x00E1")]
        [DataRow("\x00E2")]
        [DataRow("\x00E3")]
        [DataRow("\x00E4")]
        [DataRow("\x00E5")]
        [DataRow("\x00E6")]
        [DataRow("\x00E7")]
        [DataRow("\x00E8")]
        [DataRow("\x00E9")]
        [DataRow("\x00EA")]
        [DataRow("\x00EB")]
        [DataRow("\x00EC")]
        [DataRow("\x00ED")]
        [DataRow("\x00EE")]
        [DataRow("\x00EF")]
        [DataRow("\x00F0")]
        [DataRow("\x00F1")]
        [DataRow("\x00F2")]
        [DataRow("\x00F3")]
        [DataRow("\x00F4")]
        [DataRow("\x00F5")]
        [DataRow("\x00F6")]
        [DataRow("\x00F7")]
        [DataRow("\x00F8")]
        [DataRow("\x00F9")]
        [DataRow("\x00FA")]
        [DataRow("\x00FB")]
        [DataRow("\x00FC")]
        [DataRow("\x00FD")]
        [DataRow("\x00FE")]
        [DataRow("\x00FF")]
        public void TestValidRowKey(string rowKey)
        {
            string unicodeLiteralRowKey = _GetInUnicodeLiterals(rowKey);

            try
            {
                ObjectStoreLimitations.CheckRowKey(rowKey);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail($"Unexpected InvalidOperationException for {rowKey} ({unicodeLiteralRowKey})");
            }
        }

        [DataTestMethod]
        [DataRow(513)]
        [DataRow(1000)]
        public void TestRowKeyTooLongThrowsException(int rowKeyLength)
        {
            var rowKey = new string('t', rowKeyLength);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckRowKey(rowKey),
                "The maximum supported length of a row key is 512 characters.");
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)]
        [DataRow(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)]
        [DataRow(1600, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)]
        public void TestCheckingDateTimeThatDoesNotFallIntoSupportedIntervalThrowsException(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind)
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(dateTime),
                "The supported date time range is between 01/01/1601 00:00 and 12/31/9999 23:59.");
        }

        [DataTestMethod]
        [DataRow(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Utc)]
        [DataRow(2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Local)]
        [DataRow(2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Unspecified)]
        [DataRow(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)]
        public void TestCheckingDateTimeThatFallsIntoSupportedIntervalDoesNotThrowException(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind)
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            ObjectStoreLimitations.CheckValue(dateTime);
        }

        [DataTestMethod]
        [DataRow(32769)]
        [DataRow(100000)]
        public void TestCheckingForStringLargerThanSupportedLengthThrowsException(int stringLength)
        {
            var value = new string('t', stringLength);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(value),
                "The maximum supported length of a string is 32,768 characters.");
        }

        [DataTestMethod]
        [DataRow(32768)]
        [DataRow(100)]
        [DataRow(0)]
        public void TestCheckingForStringNotLargerThanSupportedLengthDoesNotThrowException(int stringLength)
        {
            var value = new string('t', stringLength);
            ObjectStoreLimitations.CheckValue(value);
        }

        [DataTestMethod]
        [DataRow(65537)]
        [DataRow(100000)]
        public void TestCheckingForByteArrayLargerThanSupportedLengthThrowsException(int byteArrayLength)
        {
            var byteArray = new byte[byteArrayLength];
            Array.Clear(byteArray, 0, byteArray.Length);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(byteArray),
                "The maximum supported length of a byte array is 65,536 bytes.");
        }

        [DataTestMethod]
        [DataRow(65536)]
        [DataRow(100)]
        [DataRow(0)]
        public void TestCheckingForByteArrayNotLargerThanSupportedLengthDoesNotThrowException(int byteArrayLength)
        {
            var byteArray = new byte[byteArrayLength];
            Array.Clear(byteArray, 0, byteArray.Length);
            ObjectStoreLimitations.CheckValue(byteArray);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestCheckingForSupportedBoolDoesNotThrowException(bool value)
        {
            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, BooleanProperty = value });
        }

        [DataTestMethod]
        [DataRow(1D)]
        [DataRow(1.1D)]
        [DataRow(0D)]
        [DataRow(-1D)]
        [DataRow(-1.1D)]
        public void TestCheckingForSupportedDoubleDoesNotThrowException(double value)
        {
            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, DoubleProperty = value });
        }

        [DataTestMethod]
        [DataRow(new byte[16] { 0x30, 0xE0, 0x65, 0x66, 0x70, 0xEE, 0x09, 0x46, 0xB3, 0xE4, 0xFD, 0xDD, 0xF0, 0x82, 0xED, 0x0F })]
        [DataRow(new byte[16] { 0x60, 0xED, 0xAA, 0xD1, 0xB7, 0x65, 0x80, 0x49, 0x90, 0x49, 0xFE, 0x0E, 0x8E, 0x13, 0x5E, 0xFD })]
        public void TestCheckingForSupportedGuidDoesNotThrowException(byte[] guidBytes)
        {
            var guid = new Guid(guidBytes);
            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, GuidProperty = guid });
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(2147483647)]
        [DataRow(-2147483648)]
        public void TestCheckingForSupportedIntDoesNotThrowException(int value)
        {
            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, IntProperty = value });
        }

        [DataTestMethod]
        [DataRow(1L)]
        [DataRow(0L)]
        [DataRow(-1L)]
        [DataRow(2147483647L)]
        [DataRow(-2147483648L)]
        [DataRow(9223372036854775807)]
        [DataRow(-9223372036854775808)]
        public void TestCheckingForSupportedLongDoesNotThrowException(long value)
        {
            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, LongProperty = value });
        }

        [DataTestMethod]
        [DataRow(typeof(sbyte))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(DateTimeOffset))]
        [DataRow(typeof(decimal))]
        [DataRow(typeof(bool[]))]
        [DataRow(typeof(byte[][]))]
        [DataRow(typeof(DateTime[]))]
        [DataRow(typeof(double[]))]
        [DataRow(typeof(Guid[]))]
        [DataRow(typeof(int[]))]
        [DataRow(typeof(long[]))]
        [DataRow(typeof(string[]))]
        [DataRow(typeof(byte[,]))]
        [DataRow(typeof(DateTime[,]))]
        [DataRow(typeof(double[,]))]
        [DataRow(typeof(Guid[,]))]
        [DataRow(typeof(int[,]))]
        [DataRow(typeof(long[,]))]
        [DataRow(typeof(string[,]))]
        public void TestCheckingValuesOfNotSupportedPropertyTypesThrowsException(Type type)
        {
            object value;
            if (type.IsArray)
                value = Array.CreateInstance(type.GetElementType(), Enumerable.Repeat(0, type.GetArrayRank()).ToArray());
            else
                value = Activator.CreateInstance(type);

            var testMethod = (from method in typeof(ObjectStoreLimitationsTests).GetRuntimeMethods()
                              where nameof(_TestCheckingValuesOfNotSupportedPropertyTypesThrowsException).Equals(method.Name)
                              select method).Single();
            try
            {
                testMethod.MakeGenericMethod(type).Invoke(this, new[] { value });
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw targetInvocationException.InnerException;
            }
        }

        private void _TestCheckingValuesOfNotSupportedPropertyTypesThrowsException<T>(T value)
        {
            var @object = new { PartitionKey = string.Empty, RowKey = string.Empty, Property = value };
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "Only properties of type byte[], bool, DateTime, double, Guid, int, long and string are supported.");
        }

        [DataTestMethod]
        [DataRow("t")]
        [DataRow("tt")]
        [DataRow("1tt")]
        [DataRow("test table name")]
        [DataRow("ThisStringHas64CharactersTrustMeItReallyDoesIHaveCheckedTwoTimes")]
        public void TestInvalidCollectionName(string tableName)
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckCollectionName(tableName),
               "Table names can contain only alphanumeric characters. They may not start with a number, are at least 3 characters short and at most 63 charachers long.");

        }

        [DataTestMethod]
        [DataRow("ttt")]
        [DataRow("TableName")]
        [DataRow("ThisStringHas63CharactersTrustMeItReallyDoesIHaveCheckedOneTime")]
        public void TestValidCollectionName(string tableName)
        {
            ObjectStoreLimitations.CheckCollectionName(tableName);
        }

        [TestMethod]
        public void TestValidatingNullBatchOperationThrowsException()
            => Assert.ThrowsException<ArgumentNullException>(() => ObjectStoreLimitations.Check(batchOperation: null));

        [TestMethod]
        public void TestHavingMoreThanSupportedOperationsInABatchThrowsException()
        {
            var batchOperation = new ObjectStoreBatchOperation();
            for (var operationNumber = 0; operationNumber < (ObjectStoreLimitations.MaximumOperationsInBatch + 1); operationNumber++)
                batchOperation.Add(ObjectStoreOperation.Insert(new object()));

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(batchOperation),
                "The maximum number of allowed operations in a batch is 100.");
        }

        [TestMethod]
        public void TestHavingOperationsWhoseObjectsTotalSizeIsAsLargeAsTheMaximumSupportedSizeDoesNotThrowException()
        {
            var batchOperation = new ObjectStoreBatchOperation();
            for (var operationNumber = 0; operationNumber < 4; operationNumber++)
                batchOperation.Add(ObjectStoreOperation.Insert(
                    new
                    {
                        PartitionKey = string.Empty,
                        RowKey = string.Empty,
                        Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize]
                    }));

            ObjectStoreLimitations.Check(batchOperation);
        }

        [TestMethod]
        public void TestHavingOperationsWhoseObjectsExceedTheMaximumAllowedTotalSizeThrowsException()
        {
            var batchOperation = new ObjectStoreBatchOperation();
            for (var operationNumber = 0; operationNumber < 4; operationNumber++)
                batchOperation.Add(ObjectStoreOperation.Insert(
                    new
                    {
                        PartitionKey = string.Empty,
                        RowKey = string.Empty,
                        Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                        Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize]
                    }));
            batchOperation.Add(ObjectStoreOperation.Insert(new { PartitionKey = string.Empty, RowKey = string.Empty, Binary = new byte[1] }));

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(batchOperation),
                "The maximum supported total size of objects in a batch operation is 4,194,304 bytes.");
        }

        [TestMethod]
        public void TestHavingOperationsInABatchThatDoNotOperateInSamePartitionThrowsException()
        {
            var batchOperation = new ObjectStoreBatchOperation
            {
                ObjectStoreOperation.Insert(new { PartitionKey = "1", RowKey = string.Empty }),
                ObjectStoreOperation.Insert(new { PartitionKey = "2", RowKey = string.Empty })
            };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(batchOperation),
                "All operations in a batch must be carried out on objectes having the same PartitionKey.");
        }

        [TestMethod]
        public void TestHavingMultipleRetrieveOperationsThrowsException()
        {
            var batchOperation = new ObjectStoreBatchOperation
            {
                new  ObjectStoreOperationMock(ObjectStoreOperationType.Retrieve, new { PartitionKey = string.Empty, RowKey = string.Empty }),
                new  ObjectStoreOperationMock(ObjectStoreOperationType.Retrieve, new { PartitionKey = string.Empty, RowKey = string.Empty })
            };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(batchOperation),
                "A batch may contain a retrieve operation when it is the only one.");
        }

        [TestMethod]
        public void TestHavingJustOneRetrieveOperationDoesNotThrowException()
        {
            var batchOperation = new ObjectStoreBatchOperation
            {
                new  ObjectStoreOperationMock(ObjectStoreOperationType.Retrieve,new { PartitionKey = string.Empty, RowKey = string.Empty })
            };

            ObjectStoreLimitations.Check(batchOperation);
        }


        [TestMethod]
        public void TestCheckingObjectWithoutPartitionKeyThrowsException()
        {
            var @object = new { RowKey = Guid.NewGuid().ToString() };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        public void TestCheckingObjectWithoutRowKeyThrowsException()
        {
            var @object = new { PartitionKey = Guid.NewGuid().ToString() };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "The given object must expose a readable RowKey property of type string.");
        }

        [TestMethod]
        public void TestCheckingObjectAsLargeAsTheMaximumSupportedSizeDoesNotThrowException()
        {
            var @object =
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize]
                };
            ObjectStoreLimitations.Check(@object);
        }

        [TestMethod]
        public void TestTryingToAddObjectLargerThanSupportedThrowsException()
        {
            var @object =
                new
                {
                    PartitionKey = string.Empty,
                    RowKey = string.Empty,
                    Binary1 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary2 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary3 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary4 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary5 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary6 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary7 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary8 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary9 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary10 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary11 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary12 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary13 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary14 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary15 = new byte[ObjectStoreLimitations.MaximumByteArrayLength],
                    Binary16 = new byte[ObjectStoreLimitations.MaximumByteArrayLength - ObjectStoreLimitations.DateTimeSize + 1]
                };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "The maximum supported size of an object is 1,048,576 bytes.");
        }

        [DataTestMethod]
        [DataRow(default(string))]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("1test")]
        [DataRow("xmlTest")]
        [DataRow("ThisPropertyHas256CharactersForRealBecauseItHasBeenTestedAddingSomeMoreNonSenseLikeTheQuickBrownFoxJumpsOverTheLazyDogBecauseThisPropertyNameHasToBe256CharactersLongAndYetItStillSeemsThereIsMoreToWriteInOrderToGetToThatLimitThisMostDefinitelyIsATinyBitHard")]
        public void TestInvalidPropertyName(string propertyName)
        {
            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckPropertyName(propertyName),
                "Property names can contain only alphanumeric and underscore characters. They can be only 255 characters long and may not begin with XML (in any casing). Property names themselves are case sensitive.");
        }

        [DataTestMethod]
        [DataRow("test")]
        [DataRow("_test")]
        [DataRow("_1test")]
        [DataRow("_xml")]
        [DataRow("_1")]
        [DataRow("_")]
        [DataRow("ThisPropertyHas255CharactersForRealBecauseItHasBeenTestedAddingSomeMoreNonSenseLikeTheQuickBrownFoxJumpsOverTheLazyDogBecauseThisPropertyNameHasToBe255CharactersLongAndYetItStillSeemsThereIsMoreToWriteInOrderToGetToThatLimitThisMostDefinitelyIsALittleHard")]
        public void TestValidPropertyName(string propertyName)
            => ObjectStoreLimitations.CheckPropertyName(propertyName);

        private static string _GetInUnicodeLiterals(string partitionKey)
        {
            var stringBuilder = new StringBuilder(partitionKey.Length * 6);

            foreach (var character in partitionKey)
            {
                stringBuilder.Append("\\x");
                stringBuilder.Append(((short)character).ToString("X4"));
            }

            return stringBuilder.ToString();
        }
    }
}