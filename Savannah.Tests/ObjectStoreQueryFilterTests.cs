using System;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Query;
using Savannah.Xml;

namespace Savannah.Tests
{
    [TestClass]
    public class ObjectStoreQueryFilterTests
    {
        [TestMethod]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithBooleanValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithDoubleValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithGuidValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithIntValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithLongValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingEqualWithStringValueReturnOperatorOfTypeEqual()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithIntValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithLongValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithStringValueReturnOperatorOfTypeNotEqual()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingNotEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithGuidValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithIntValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithLongValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanWithStringValueReturnOperatorOfTypeLessThan()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorOfTypeLessThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingLessThanOrEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorOfTypeGreaterThan()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", new byte[0]);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, new byte[0]);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithByteArrayValueReturnOperatorWithSameValue()
        {
            var value = new byte[0];
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", true);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, true);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithBooleanValueReturnOperatorWithSameValue()
        {
            var value = true;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", DateTime.UtcNow);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, DateTime.UtcNow);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDateTimeValueReturnOperatorWithSameValue()
        {
            var value = DateTime.UtcNow;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1.0D);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1.0D);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithDoubleValueReturnOperatorWithSameValue()
        {
            var value = 1.0D;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", Guid.NewGuid());

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, Guid.NewGuid());

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithGuidValueReturnOperatorWithSameValue()
        {
            var value = Guid.NewGuid();
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithIntValueReturnOperatorWithSameValue()
        {
            var value = 1;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", 1L);

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, 1L);

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithLongValueReturnOperatorWithSameValue()
        {
            var value = 1L;
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreEqual(value, filter.Value);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorOfTypeGreaterThanOrEqual()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, filter.Operator);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorWithSamePropertyName()
        {
            var propertyName = "testProperty";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual(propertyName, "testValue");

            Assert.AreSame(propertyName, filter.PropertyName);
        }

        [TestMethod]
        public void TestCreatingGreaterThanOrEqualWithStringValueReturnOperatorWithSameValue()
        {
            var value = "testValue";
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", value);

            Assert.AreSame(value, filter.Value);
        }

        [TestMethod]
        public void TestEqualOperatorReturnsNotEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.NotEqual, notFilter.Operator);
        }

        [TestMethod]
        public void TestEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestNotEqualOperatorReturnsEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.Equal, notFilter.Operator);
        }

        [TestMethod]
        public void TestNotEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestNotEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.NotEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestLessThanOperatorReturnsGreaterThanOrEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThanOrEqual, notFilter.Operator);
        }

        [TestMethod]
        public void TestLessThanOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestLessThanOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestLessThanOrEqualOperatorReturnsGreaterThanOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.GreaterThan, notFilter.Operator);
        }

        [TestMethod]
        public void TestLessThanOrEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestLessThanOrEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.LessThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestGreaterThanOperatorReturnsLessThanOrEqualOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.LessThanOrEqual, notFilter.Operator);
        }

        [TestMethod]
        public void TestGreaterThanOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestGreaterThanOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThan("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestGreaterThanOrEqualOperatorReturnsLessThanOperatorOnNot()
        {
            var filter = ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = filter.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.LessThan, notFilter.Operator);
        }

        [TestMethod]
        public void TestGreaterThanOrEqualOperatorReturnsOperatorWithSameProperty()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.PropertyName, notFilter.PropertyName);
        }

        [TestMethod]
        public void TestGreaterThanOrEqualOperatorReturnsOperatorWithSameValue()
        {
            var filter = (ObjectStoreQueryValueFilter)ObjectStoreQueryFilter.GreaterThanOrEqual("testProperty", "testValue");

            var notFilter = (ObjectStoreQueryValueFilter)filter.Not();

            Assert.AreSame(filter.Value, notFilter.Value);
        }

        [TestMethod]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = filter1.And(filter2);

            Assert.AreEqual(ObjectStoreQueryOperator.And, andFilter.Operator);
        }

        [TestMethod]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilterHavingTheSameLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = (ObjectStoreQueryLogicalFilter)filter1.And(filter2);

            Assert.AreSame(filter1, andFilter.LeftOperand);
        }

        [TestMethod]
        public void TestCombiningTwoFiltersWithAndProducesAnAndFilterHavingTheSameRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var andFilter = (ObjectStoreQueryLogicalFilter)filter1.And(filter2);

            Assert.AreSame(filter2, andFilter.RightOperand);
        }


        [TestMethod]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = filter1.Or(filter2);

            Assert.AreEqual(ObjectStoreQueryOperator.Or, orFilter.Operator);
        }

        [TestMethod]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilterHavingTheSameLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = (ObjectStoreQueryLogicalFilter)filter1.Or(filter2);

            Assert.AreSame(filter1, orFilter.LeftOperand);
        }

        [TestMethod]
        public void TestCombiningTwoFiltersWithOrProducesAnOrFilterHavingTheSameRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");

            var orFilter = (ObjectStoreQueryLogicalFilter)filter1.Or(filter2);

            Assert.AreSame(filter2, orFilter.RightOperand);
        }

        [TestMethod]
        public void TestNegatingAnAndFilterReturnsAnOrFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = andOperator.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.Or, notAndFilter.Operator);
        }

        [TestMethod]
        public void TestNegatingAnAndFilterReturnsTheNegatedLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = (ObjectStoreQueryLogicalFilter)andOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notAndFilter.LeftOperand.Operator);
        }

        [TestMethod]
        public void TestNegatingAnAndFilterReturnsTheNegatedRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var andOperator = filter1.And(filter2);

            var notAndFilter = (ObjectStoreQueryLogicalFilter)andOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notAndFilter.RightOperand.Operator);
        }

        [TestMethod]
        public void TestNegatingAnOrFilterReturnsAnAndFilter()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = orOperator.Not();

            Assert.AreEqual(ObjectStoreQueryOperator.And, notOrFilter.Operator);
        }

        [TestMethod]
        public void TestNegatingAnOrFilterReturnsTheNegatedLeftOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = (ObjectStoreQueryLogicalFilter)orOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notOrFilter.LeftOperand.Operator);
        }

        [TestMethod]
        public void TestNegatingAnOrFilterReturnsTheNegatedRightOperand()
        {
            var filter1 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var filter2 = ObjectStoreQueryFilter.Equal("testProperty", "testValue");
            var orOperator = filter1.Or(filter2);

            var notOrFilter = (ObjectStoreQueryLogicalFilter)orOperator.Not();

            Assert.AreEqual(filter1.Not().Operator, notOrFilter.RightOperand.Operator);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, new byte[] { 0xFF, 0x99 }, "FF99")]
        [DataRow(ObjectStoreQueryOperator.NotEqual, new byte[] { 0xFF, 0x99 }, "FF99")]
        [DataRow(ObjectStoreQueryOperator.LessThan, new byte[] { 0xFF, 0x99 }, "FF99")]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, new byte[] { 0xFF, 0x99 }, "FF99")]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, new byte[] { 0xFF, 0x99 }, "FF99")]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, new byte[] { 0xFF, 0x99 }, "FF99")]
        public void TestToStringForByteArrayValueFilter(ObjectStoreQueryOperator operatorType, byte[] byteArray, string byteArrayString)
        {
            var propertyName = "propertyName";
            var value = byteArray;

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Binary, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Binary] {byteArrayString}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, true)]
        [DataRow(ObjectStoreQueryOperator.NotEqual, false)]
        [DataRow(ObjectStoreQueryOperator.LessThan, true)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, false)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, true)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, false)]
        public void TestToStringForBooleanValueFilter(ObjectStoreQueryOperator operatorType, bool value)
        {
            var propertyName = "propertyName";

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Boolean, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Boolean] {(value ? bool.TrueString : bool.FalseString)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, 1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)]
        [DataRow(ObjectStoreQueryOperator.NotEqual, 2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Utc)]
        [DataRow(ObjectStoreQueryOperator.LessThan, 2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Local)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, 2016, 1, 24, 12, 20, 10, 14, DateTimeKind.Unspecified)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, 9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, 9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)]
        public void TestToStringForDateTimeValueFilter(ObjectStoreQueryOperator operatorType, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind dateTimeKind)
        {
            var propertyName = "propertyName";
            var value = new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.DateTime, value, operatorType);

            Assert.AreEqual(
                    $"{propertyName} {operatorType} [DateTime] {value.ToString(XmlSettings.DateTimeFormat, CultureInfo.InvariantCulture)}",
                    filter.ToString(),
                    ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, 1.0D)]
        [DataRow(ObjectStoreQueryOperator.NotEqual, 1.0D)]
        [DataRow(ObjectStoreQueryOperator.LessThan, 1.0D)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, 1.0D)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, 1.0D)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, 1.0D)]
        public void TestToStringForDoubleValueFilter(ObjectStoreQueryOperator operatorType, double value)
        {
            var propertyName = "propertyName";

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Double, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Double] {value.ToString("e", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, new byte[16] { 0x0B, 0x3C, 0x29, 0x59, 0x12, 0xFB, 0xAD, 0x44, 0xA8, 0x77, 0x15, 0xF5, 0x69, 0x22, 0xE0, 0x7F })]
        [DataRow(ObjectStoreQueryOperator.NotEqual, new byte[16] { 0x6E, 0xEE, 0xD2, 0x6F, 0x20, 0xF4, 0x85, 0x46, 0xA7, 0x5C, 0x8E, 0xAF, 0xE6, 0x3A, 0x81, 0xD6 })]
        [DataRow(ObjectStoreQueryOperator.LessThan, new byte[16] { 0x9F, 0xF3, 0x9D, 0xFC, 0xF7, 0xE8, 0x24, 0x4E, 0xB2, 0xFA, 0x91, 0xDC, 0xD6, 0xFE, 0x8B, 0x71 })]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, new byte[16] { 0xB3, 0x3B, 0xB0, 0xB5, 0xA9, 0xBF, 0x10, 0x48, 0x88, 0x97, 0x59, 0x13, 0x6A, 0xFA, 0xC8, 0x5C })]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, new byte[16] { 0xEF, 0xA1, 0xD0, 0x26, 0xC0, 0xCF, 0x3C, 0x46, 0x93, 0x9D, 0x75, 0x3C, 0x69, 0x18, 0x31, 0xB9 })]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, new byte[16] { 0xF2, 0x25, 0x4E, 0x9D, 0xA5, 0x19, 0xAC, 0x44, 0xAA, 0x92, 0x13, 0xEA, 0xCF, 0x82, 0xD0, 0x0B })]
        public void TestToStringForGuidValueFilter(ObjectStoreQueryOperator operatorType, byte[] guidBytes)
        {
            var propertyName = "propertyName";
            var value = new Guid(guidBytes);

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Guid, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Guid] {value}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, 12345)]
        [DataRow(ObjectStoreQueryOperator.NotEqual, 12345)]
        [DataRow(ObjectStoreQueryOperator.LessThan, 12345)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, 12345)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, 12345)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, 12345)]
        public void TestToStringForIntValueFilter(ObjectStoreQueryOperator operatorType, int value)
        {
            var propertyName = "propertyName";

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Int, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Int] {value.ToString("n0", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal, 12345)]
        [DataRow(ObjectStoreQueryOperator.NotEqual, 12345)]
        [DataRow(ObjectStoreQueryOperator.LessThan, 12345)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual, 12345)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan, 12345)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual, 12345)]
        public void TestToStringForLongValueFilter(ObjectStoreQueryOperator operatorType, long value)
        {
            var propertyName = "propertyName";

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.Long, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [Long] {value.ToString("n0", CultureInfo.InvariantCulture)}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.Equal)]
        [DataRow(ObjectStoreQueryOperator.NotEqual)]
        [DataRow(ObjectStoreQueryOperator.LessThan)]
        [DataRow(ObjectStoreQueryOperator.LessThanOrEqual)]
        [DataRow(ObjectStoreQueryOperator.GreaterThan)]
        [DataRow(ObjectStoreQueryOperator.GreaterThanOrEqual)]
        public void TestToStringForStringValueFilter(ObjectStoreQueryOperator operatorType)
        {
            var propertyName = "propertyName";
            var value = "testValue";

            var filter = new ObjectStoreQueryValueFilter(propertyName, operatorType, ValueType.String, value, operatorType);

            Assert.AreEqual(
                $"{propertyName} {operatorType} [String] {value}",
                filter.ToString(),
                ignoreCase: false);
        }

        [DataTestMethod]
        [DataRow(ObjectStoreQueryOperator.And)]
        [DataRow(ObjectStoreQueryOperator.Or)]
        public void TestToStringForLogicalFilter(ObjectStoreQueryOperator operatorType)
        {
            var filter1 = ObjectStoreQueryFilter.Equal("property1", "value1");
            var filter2 = ObjectStoreQueryFilter.Equal("property2", "value2");

            var filter = new ObjectStoreQueryLogicalFilter(filter1, operatorType, filter2, operatorType);

            Assert.AreEqual(
                $"({filter1} {operatorType} {filter2})",
                filter.ToString(),
                ignoreCase: false);
        }
    }
}