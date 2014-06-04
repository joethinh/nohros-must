using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class ParameterizedString_
    {
        [Test]
        public void Ctor() {
            ParameterizedString ps = new ParameterizedString();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullString()
        {
            ParameterizedString ps = new ParameterizedString((string)null, "delimiter");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorStringNull()
        {
            ParameterizedString ps = new ParameterizedString("string with paramters", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorEnumerableNull()
        {
            List<ParameterizedStringPart> parts;
            parts = new List<ParameterizedStringPart>();
            parts.Add(new ParameterizedStringPart("string with paramters"));
            parts.Add(new ParameterizedStringPart("paramter1"));

            ParameterizedString ps = new ParameterizedString(parts, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullEnumerableString()
        {
            ParameterizedString ps = new ParameterizedString((IEnumerable<ParameterizedStringPart>)null, "delimiter");
        }

        [Test]
        public void NoParameter()
        {
            ParameterizedString ps = new ParameterizedString("string with no paramter", "$");
            Assert.IsNotNull(ps.Parameters);
            Assert.AreEqual(0, ps.Parameters.Count);
            Assert.AreEqual("string with no paramter", ps.ToString());
        }

        [Test]
        public void Parameters()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: $paramter1$ and parameter: $paramter2$", "$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("paramter1", ps.Parameters[0].ParameterName);
            Assert.AreEqual("paramter2", ps.Parameters[1].ParameterName);

            ps.Parameters[0].LiteralValue = "value1";
            ps.Parameters[1].LiteralValue = "value2";
            Assert.AreEqual("string with paramter: value1 and parameter: value2", ps.ToString());
        }

        [Test]
        public void LeadingDelimiter()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: $paramter1$ and parameter:$paramter2$$$", "$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("string with paramter:  and parameter:$$", ps.ToString());
        }

        [Test]
        public void NotClosedDelimiter()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: $paramter1$ and parameter:$paramter2$ and $literal-opened-delimiter", "$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("string with paramter:  and parameter: and $literal-opened-delimiter", ps.ToString());
        }

        [Test]
        public void OnlyDelimiters()
        {
            ParameterizedString ps = new ParameterizedString("$$$$$$$$$$$$$$$$$", "$");
            Assert.AreEqual(0, ps.Parameters.Count);
            Assert.AreEqual("$$$$$$$$$$$$$$$$$", ps.ToString());
        }

        [Test]
        public void OnlyParameters()
        {
            ParameterizedString ps = new ParameterizedString("$p1$$p2$$p3$", "$");
            Assert.AreEqual(3, ps.Parameters.Count);
            Assert.AreEqual(string.Empty, ps.ToString());
        }

        [Test]
        public void StartsWithDelimiter()
        {
            ParameterizedString ps = new ParameterizedString("$P1 parameter is at the begging of string", "$");
            Assert.AreEqual(0, ps.Parameters.Count);
            Assert.AreEqual("$P1 parameter is at the begging of string", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterNoParameter()
        {
            ParameterizedString ps = new ParameterizedString("string with no paramter", "D$");
            Assert.IsNotNull(ps.Parameters);
            Assert.AreEqual(0, ps.Parameters.Count);
            Assert.AreEqual("string with no paramter", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterParameters()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: D$paramter1D$ and parameter: D$paramter2D$", "D$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("paramter1", ps.Parameters[0].ParameterName);
            Assert.AreEqual("paramter2", ps.Parameters[1].ParameterName);

            ps.Parameters[0].LiteralValue = "value1";
            ps.Parameters[1].LiteralValue = "value2";
            Assert.AreEqual("string with paramter: value1 and parameter: value2", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterLeadingDelimiter()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: D$paramter1D$ and parameter:D$paramter2D$D$D$", "D$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("string with paramter:  and parameter:D$D$", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterNotClosedDelimiter()
        {
            ParameterizedString ps = new ParameterizedString("string with paramter: D$paramter1D$ and parameter:D$paramter2D$ and D$literal-opened-delimiter", "D$");
            Assert.AreEqual(2, ps.Parameters.Count);
            Assert.AreEqual("string with paramter:  and parameter: and D$literal-opened-delimiter", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterOnlyDelimiters()
        {
            ParameterizedString ps = new ParameterizedString("D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$", "D$");
            Assert.AreEqual(0, ps.Parameters.Count);
            Assert.AreEqual("D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$D$", ps.ToString());
        }

        [Test]
        public void MultiCharDelimiterOnlyParameters()
        {
            ParameterizedString ps = new ParameterizedString("D$p1D$D$p2D$D$p3D$", "D$");
            Assert.AreEqual(3, ps.Parameters.Count);
            Assert.AreEqual(string.Empty, ps.ToString());
        }
    }
}