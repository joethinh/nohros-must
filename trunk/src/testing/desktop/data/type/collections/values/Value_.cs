using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class Value_
    {
        [Test]
        public void Value_CreateString() {
            Value value = Value.CreateStringValue("string");
            Assert.AreEqual(Nohros.Data.ValueType.TYPE_STRING, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_BOOLEAN, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_DICTIONARY, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_INTEGER, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_LIST, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_NULL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_REAL, value.Type);

            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_BOOLEAN));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_DICTIONARY));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_INTEGER));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_LIST));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_NULL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_REAL));
            Assert.AreEqual(true, value.IsType(Nohros.Data.ValueType.TYPE_STRING));

            Assert.AreEqual("string", value.GetAsString());
        }

        [Test]
        public void Value_CreateRealValue() {
            Value value = Value.CreateRealValue(10.5);
            Assert.AreEqual(Nohros.Data.ValueType.TYPE_REAL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_BOOLEAN, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_DICTIONARY, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_INTEGER, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_LIST, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_NULL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_STRING, value.Type);

            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_BOOLEAN));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_DICTIONARY));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_INTEGER));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_LIST));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_NULL));
            Assert.AreEqual(true, value.IsType(Nohros.Data.ValueType.TYPE_REAL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_STRING));
        }

        [Test]
        public void Value_CreateNullValue() {
            Value value = Value.CreateNullValue();
            Assert.AreEqual(Nohros.Data.ValueType.TYPE_NULL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_BOOLEAN, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_DICTIONARY, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_INTEGER, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_LIST, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_REAL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_STRING, value.Type);

            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_BOOLEAN));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_DICTIONARY));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_INTEGER));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_LIST));
            Assert.AreEqual(true, value.IsType(Nohros.Data.ValueType.TYPE_NULL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_REAL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_STRING));
        }

        [Test]
        public void Value_CreateIntegerValue() {
            Value value = Value.CreateIntegerValue(10);
            Assert.AreEqual(Nohros.Data.ValueType.TYPE_INTEGER, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_BOOLEAN, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_DICTIONARY, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_LIST, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_NULL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_REAL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_STRING, value.Type);

            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_BOOLEAN));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_DICTIONARY));
            Assert.AreEqual(true, value.IsType(Nohros.Data.ValueType.TYPE_INTEGER));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_LIST));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_NULL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_REAL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_STRING));
        }

        [Test]
        public void Value_CreateBooleanValue() {
            Value value = Value.CreateBooleanValue(true);
            Assert.AreEqual(Nohros.Data.ValueType.TYPE_BOOLEAN, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_DICTIONARY, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_INTEGER, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_LIST, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_NULL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_REAL, value.Type);
            Assert.AreNotEqual(Nohros.Data.ValueType.TYPE_STRING, value.Type);

            Assert.AreEqual(true, value.IsType(Nohros.Data.ValueType.TYPE_BOOLEAN));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_DICTIONARY));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_INTEGER));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_LIST));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_NULL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_REAL));
            Assert.AreEqual(false, value.IsType(Nohros.Data.ValueType.TYPE_STRING));
        }
    }
}
