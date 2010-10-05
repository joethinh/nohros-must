using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class DictionaryValueT_
    {
        [Test]
        public void Add() {
            DictionaryValue<StringValue> dict = new DictionaryValue<StringValue>();
            dict.Add("bu.x", new StringValue("x"));

            StringValue x = dict["bu.x"];
            Assert.IsNotNull(x);
            Assert.AreEqual(x, "x");
        }

        [Test]
        public void Get() {
            DictionaryValue<StringValue> dict = new DictionaryValue<StringValue>();
            dict.Add("bu.x", new StringValue("x"));

            StringValue x = dict["bu"];
            Assert.IsNull(x);

            x = dict["bu.x"];
            Assert.IsNotNull(x);
            Assert.AreEqual(x, "x");
        }
    }
}