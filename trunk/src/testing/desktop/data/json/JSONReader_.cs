using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Nohros.Data;

namespace Nohros.Test.Data.JSon
{
    [TestFixture]
    public class JSONReader_
    {
        [Test]
        public void ParseInt()
        {
            JSONReader reader = new JSONReader();
            Value value = reader.JsonToValue("{ \"number\":10, \"numbere\":10e5, \"real\":10.01, \"frac\":-10.00E-5, \"string\":\"5478\n965\", /* string value with comment */\"string_comment\":\"teste50125\" }", true, true);

            Assert.IsInstanceOf<DictionaryValue>(value);
            DictionaryValue dict = value as DictionaryValue;

            // int parse
            int i;
            Assert.AreEqual(true, dict.GetInteger("number", out i));
            Assert.AreEqual(i, 10);

            // exponential int
            Assert.AreEqual(true, dict.GetInteger("numbere", out i));
            Assert.AreEqual(i, 10e5);

            // real number
            double d;
            Assert.AreEqual(true, dict.GetReal("real", out d));
            Assert.AreEqual(d, 10.01);

            // fractional
            Assert.AreEqual(true, dict.GetReal("frac", out d));
            Assert.AreEqual(d, -10.00e-5);

            // string parse
            string s;
            Assert.AreEqual(true, dict.GetString("string", out s));
            Assert.AreEqual(s, "5478\n965");

            // comment
            Assert.AreEqual(true, dict.GetString("string_comment", out s));
            Assert.AreEqual(s, "teste50125");

            // invalid numbers
            // valid number is: [minus] int [frac] [exp]
            Assert.Throws<ArgumentException>( delegate() {
                reader.JsonToValue("{ \"number\":10e }", true, true);
            });

            Assert.Throws<ArgumentException>(delegate() {
                reader.JsonToValue("{ \"number\":+10 }", true, true);
            });

            // unquoted strings
            Assert.Throws<ArgumentException>(delegate() {
                reader.JsonToValue("{ \"string\":string }", true, true);
            });

            Assert.Throws<ArgumentException>(delegate() {
                reader.JsonToValue("{ string:string }", true, true);
            });

            // trailing comma
            Assert.Throws<ArgumentException>(delegate() {
                reader.JsonToValue("{ \"string\":\"string\", }", true, false);
            });
        }
    }
}
