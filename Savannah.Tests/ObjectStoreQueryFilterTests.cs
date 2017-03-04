using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah.Query;
using Savannah.Xml;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreQueryFilterTests
        : UnitTest
    {
        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithBooleanValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDoubleValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithGuidValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithIntValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithLongValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithStringValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithIntValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithLongValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithStringValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingNotEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithGuidValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithIntValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithLongValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithStringValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestEqualOperatorReturnsNotEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNotEqualOperatorReturnsEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNotEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNotEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOperatorReturnsGreaterThanOrEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualOperatorReturnsGreaterThanOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestLessThanOrEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOperatorReturnsLessThanOrEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualOperatorReturnsLessThanOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, notFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestGreaterThanOrEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = filter1.And(filter2);

            Assert.AreEqual(ObjectStoreQueryOperator.And, andFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilterHavingTheSameLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = (ObjectStoreQueryLogicalFilter)filter1.And(filter2);

            Assert.AreSame(filter1, andFilter.LeftOperand);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilterHavingTheSameRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = (ObjectStoreQueryLogicalFilter)filter1.And(filter2);

            Assert.AreSame(filter2, andFilter.RightOperand);
        }


        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = filter1.Or(filter2);

            Assert.AreEqual(ObjectStoreQueryOperator.Or, orFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilterHavingTheSameLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = (ObjectStoreQueryLogicalFilter)filter1.Or(filter2);

            Assert.AreSame(filter1, orFilter.LeftOperand);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilterHavingTheSameRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = (ObjectStoreQueryLogicalFilter)filter1.Or(filter2);

            Assert.AreSame(filter2, orFilter.RightOperand);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnAndFilterReturnsAnOrFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = andOperator.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.Or, notAndFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnAndFilterReturnsTheNegatedLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = (ObjectStoreQueryLogicalFilter)andOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notAndFilter.LeftOperand.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnAndFilterReturnsTheNegatedRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = (ObjectStoreQueryLogicalFilter)andOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notAndFilter.RightOperand.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnOrFilterReturnsAnAndFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = orOperator.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.And, notOrFilter.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnOrFilterReturnsTheNegatedLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = (ObjectStoreQueryLogicalFilter)orOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notOrFilter.LeftOperand.Operator);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestNegatingAnOrFilterReturnsTheNegatedRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = (ObjectStoreQueryLogicalFilter)orOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notOrFilter.RightOperand.Operator);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForByteArrayValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);
            var byteArrayString = string.Join(string.Empty, row.BinaryValue.Select(@byte => @byte.ToString("X2")));

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Binary, row.BinaryValue, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Binary] {byteArrayString}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForBooleanValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Boolean, row.BooleanValue, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Boolean] {(row.BooleanValue ? bool.TrueString : bool.FalseString)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForDateTimeValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.DateTime, row.DateTimeValue, operatorType);

            Assert.AreEqual(
                    $"{propertyName} {operatorType} [DateTime] {row.DateTimeValue.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)}",
                    filter.ToString(),
                    ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForDoubleValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Double, row.DoubleValue, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Double] {row.DoubleValue.ToString("e", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }


        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForGuidValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Guid, row.GuidValue, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Guid] {row.GuidValue}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForIntValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Int, row.Int32Value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Int] {row.Int32Value.ToString("n0", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForLongValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Long, row.Int64Value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Long] {row.Int64Value.ToString("n0", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, ObjectStoreValueFileterQueryOperatorTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestToStringForStringValueFilter()
        {
            var row = GetRow<ObjectStoreValueFileterQueryOperatorRow>();

            var propertyName = "propertyName";
            var operatorType = (ObjectStoreQueryOperator)Enum.Parse(typeof(ObjectStoreQueryOperator), row.Name);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.String, row.StringValue, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [String] {row.StringValue}",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestToStringForLogicalAndFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("property1", "value1");
            var filter2 = ObjectStoreQueryFilter.Equal("property2", "value2");

            var filter = new ObjectStoreQueryLogicalFilter(filter1, ObjectStoreQueryOperator.And, filter2, ObjectStoreQueryOperator.And);

            Assert.AreEqual(
                $"({filter1} And {filter2})",
                filter.ToString(),
                ignoreCase: false);
        }

        [TestMethod]
        [Owner("Andrei Fangli")]
        public void TestToStringForLogicalOrFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("property1", "value1");
            var filter2 = ObjectStoreQueryFilter.Equal("property2", "value2");

            var filter = new ObjectStoreQueryLogicalFilter(filter1, ObjectStoreQueryOperator.Or, filter2, ObjectStoreQueryOperator.Or);

            Assert.AreEqual(
                $"({filter1} Or {filter2})",
                filter.ToString(),
                ignoreCase: false);
        }
    }
}