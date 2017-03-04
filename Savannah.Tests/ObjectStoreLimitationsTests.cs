using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.ObjectStoreOperations;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreLimitationsTests
        : UnitTest
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

        private static IEnumerable<string> _InvalidKeyCharacters
            => (new[] { '/', '\\', '#', '?', '\t', '\n', '\r' })
            .Concat(Enumerable.Range(0, 0x001F).Select(Convert.ToChar))
            .Concat(Enumerable.Range(0x007F, (0x009F - 0x007F)).Select(Convert.ToChar))
            .Select(@char => @char.ToString());

        private static IEnumerable<string> _ValidKeyCharacters
            => Enumerable
            .Range(0x0020, (0x007E - 0x0020)).Select(Convert.ToChar)
            .Concat(Enumerable.Range(0x00A0, (0x00FF - 0x00A0)).Select(Convert.ToChar))
            .Except(new[] { '/', '\\', '#', '?', '\t', '\n', '\r' })
            .Select(@char => @char.ToString());

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestPartitionKeyCannotContainInvalidCharacter()
        {
            foreach (var invalidKeyCharacter in _InvalidKeyCharacters)
                AssertExtra.ThrowsException<InvalidOperationException>(
                    () => ObjectStoreLimitations.CheckPartitionKey(invalidKeyCharacter),
                    @"A partition key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestValidPartitionKeyCharacter()
        {
            foreach (var validCharacter in _ValidKeyCharacters)
            {
                var unicodeLiteralPartitionKey = _GetInUnicodeLiterals(validCharacter);
                try
                {
                    ObjectStoreLimitations.CheckPartitionKey(validCharacter);
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail($"Unexpected InvalidOperationException for {validCharacter} ({unicodeLiteralPartitionKey}).");
                }
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidKeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestValidPartitionKey()
        {
            var row = GetRow<ValidKeyValuesRow>();

            try
            {
                ObjectStoreLimitations.CheckPartitionKey(row.Value);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail($"Unexpected InvalidOperationException for {row.Value}.");
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestPartitionKeyTooLongThrowsException()
        {
            var row = GetRow<KeyLengthsRow>();

            var partitionKey = new string('t', row.Value);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckPartitionKey(partitionKey),
                "The maximum supported length of a partition key is 512 characters.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestRowKeyCannotContainInvalidCharacter()
        {
            foreach (var invalidKeyCharacter in _InvalidKeyCharacters)
                AssertExtra.ThrowsException<InvalidOperationException>(
                    () => ObjectStoreLimitations.CheckRowKey(invalidKeyCharacter),
                    @"A row key may not contain control characters, including tab (\t), linefeed (\n) and carriage return (\r), nor slash (/), back slash (\), number sign (#) or question mark (?).");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestValidRowKeyCharacter()
        {
            foreach (var validCharacter in _ValidKeyCharacters)
            {
                var unicodeLiteralPartitionKey = _GetInUnicodeLiterals(validCharacter);
                try
                {
                    ObjectStoreLimitations.CheckRowKey(validCharacter);
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail($"Unexpected InvalidOperationException for {validCharacter} ({unicodeLiteralPartitionKey}).");
                }
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidKeyValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestValidRowKey()
        {
            var row = GetRow<ValidKeyValuesRow>();

            try
            {
                ObjectStoreLimitations.CheckRowKey(row.Value);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail($"Unexpected InvalidOperationException for {row.Value}.");
            }
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, KeyLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestRowKeyTooLongThrowsException()
        {
            var row = GetRow<KeyLengthsRow>();

            var rowKey = new string('t', row.Value);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckRowKey(rowKey),
                "The maximum supported length of a row key is 512 characters.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, InvalidDateValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingDateTimeThatDoesNotFallIntoSupportedIntervalThrowsException()
        {
            var row = GetRow<InvalidDateValuesRow>();

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(row.Value),
                "The supported date time range is between 01/01/1601 00:00 and 12/31/9999 23:59.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidDateTimeValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingDateTimeThatFallsIntoSupportedIntervalDoesNotThrowException()
        {
            var row = GetRow<ValidDateTimeValuesRow>();

            ObjectStoreLimitations.CheckValue(row.Value);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, InvalidStringLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForStringLargerThanSupportedLengthThrowsException()
        {
            var row = GetRow<InvalidStringLengthsRow>();

            var value = new string('t', row.Value);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(value),
                "The maximum supported length of a string is 32,768 characters.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidStringLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForStringNotLargerThanSupportedLengthDoesNotThrowException()
        {
            var row = GetRow<ValidStringLengthsRow>();

            var value = new string('t', row.Value);
            ObjectStoreLimitations.CheckValue(value);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, InvalidByteArrayLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForByteArrayLargerThanSupportedLengthThrowsException()
        {
            var row = GetRow<InvalidByteArrayLengthsRow>();

            var byteArray = new byte[row.Value];
            Array.Clear(byteArray, 0, byteArray.Length);

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckValue(byteArray),
                "The maximum supported length of a byte array is 65,536 bytes.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidByteArrayLengthsTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForByteArrayNotLargerThanSupportedLengthDoesNotThrowException()
        {
            var row = GetRow<ValidByteArrayLengthsRow>();

            var byteArray = new byte[row.Value];
            Array.Clear(byteArray, 0, byteArray.Length);
            ObjectStoreLimitations.CheckValue(byteArray);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, BooleanValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForSupportedBoolDoesNotThrowException()
        {
            var row = GetRow<BooleanValuesRow>();

            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, BooleanProperty = row.Value });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, DoubleValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForSupportedDoubleDoesNotThrowException()
        {
            var row = GetRow<DoubleValuesRow>();

            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, DoubleProperty = row.Value });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, GuidValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForSupportedGuidDoesNotThrowException()
        {
            var row = GetRow<GuidValuesRow>();

            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, GuidProperty = row.Value });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, IntValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForSupportedIntDoesNotThrowException()
        {
            var row = GetRow<IntValuesRow>();

            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, IntProperty = row.Value });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, LongValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingForSupportedLongDoesNotThrowException()
        {
            var row = GetRow<LongValuesRow>();

            ObjectStoreLimitations.Check(new { PartitionKey = string.Empty, RowKey = string.Empty, LongProperty = row.Value });
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, UnsupportedTypeNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestCheckingValuesOfNotSupportedPropertyTypesThrowsException()
        {
            var row = GetRow<UnsupportedTypeNamesRow>();

            var type = Type.GetType(row.Value, throwOnError: true);
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

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, InvalidCollectionNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestInvalidCollectionName()
        {
            var row = GetRow<InvalidCollectionNamesRow>();

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckCollectionName(row.Value),
               "Table names can contain only alphanumeric characters. They may not start with a number, are at least 3 characters short and at most 63 charachers long.");

        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidCollectionNamesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestValidCollectionName()
        {
            var row = GetRow<ValidCollectionNamesRow>();

            ObjectStoreLimitations.CheckCollectionName(row.Value);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Owner("Andrei Fangli")]
        public void TestValidatingNullBatchOperationThrowsException()
            => ObjectStoreLimitations.Check(batchOperation: null);

        [TestMethod]
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
        public void TestHavingJustOneRetrieveOperationDoesNotThrowException()
        {
            var batchOperation = new ObjectStoreBatchOperation
            {
                new  ObjectStoreOperationMock(ObjectStoreOperationType.Retrieve,new { PartitionKey = string.Empty, RowKey = string.Empty })
            };

            ObjectStoreLimitations.Check(batchOperation);
        }


        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCheckingObjectWithoutPartitionKeyThrowsException()
        {
            var @object = new { RowKey = Guid.NewGuid().ToString() };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "The given object must expose a readable PartitionKey property of type string.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCheckingObjectWithoutRowKeyThrowsException()
        {
            var @object = new { PartitionKey = Guid.NewGuid().ToString() };

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.Check(@object),
                "The given object must expose a readable RowKey property of type string.");
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
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
        [Owner("Andrei Fangli")]
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

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, InvalidPropertyNameTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestInvalidPropertyName()
        {
            var row = GetRow<InvalidPropertyNameRow>();

            AssertExtra.ThrowsException<InvalidOperationException>(
                () => ObjectStoreLimitations.CheckPropertyName(row.Value),
                "Property names can contain only alphanumeric and underscore characters. They can be only 255 characters long and may not begin with XML (in any casing). Property names themselves are case sensitive.");
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ValidPropertyNameTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestValidPropertyName()
        {
            var row = GetRow<ValidPropertyNameRow>();

            ObjectStoreLimitations.CheckPropertyName(row.Value);
        }

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