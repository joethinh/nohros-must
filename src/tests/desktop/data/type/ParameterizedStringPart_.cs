using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class ParameterizedStringPart_
    {
        [Test]
        public void CtorString() {
            ParameterizedStringPart part = new ParameterizedStringPart("literal-text");
            Assert.IsNotNull(part);
            Assert.Pass();
        }

        [Test]
        public void CtorStringString()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somename", "somevalue");
            Assert.IsNotNull(part);
            Assert.Pass();
        }

        [Test]
        public void CtorStringEmpty()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somename", string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNull()
        {
            ParameterizedStringPart part = new ParameterizedStringPart(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullString()
        {
            ParameterizedStringPart part = new ParameterizedStringPart(null, "somevalue");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CtorEmptyString()
        {
            ParameterizedStringPart part = new ParameterizedStringPart(string.Empty, "somevalue");
        }

        [Test]
        public void IsParameter()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("string", "string");
            Assert.IsTrue(part.IsParameter);
        }

        [Test]
        public void IsNotParameter()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("sometext");
            Assert.IsFalse(part.IsParameter);
        }

        [Test]
        public void LiteralValue()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("literal-value");
            Assert.AreEqual("literal-value", part.LiteralValue);
        }

        [Test]
        public void EmptyLiteralValue()
        {
            ParameterizedStringPart part = new ParameterizedStringPart(string.Empty);
            Assert.AreEqual(string.Empty, part.LiteralValue);
        }

        [Test]
        public void ParameterName()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somename", "somevalue");
            Assert.AreEqual(part.ParameterName, "somename");
        }

        [Test]
        public void Equals()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somedata");
            Assert.IsTrue(part.Equals(part));
            Assert.IsTrue(part == part);

            ParameterizedStringPart part2 = new ParameterizedStringPart("somedata");
            Assert.IsTrue(part.Equals(part2));
            Assert.IsTrue(part == part2);

            part = new ParameterizedStringPart("somename", "somevalue");
            Assert.IsTrue(part.Equals(part));
            Assert.IsTrue(part == part);

            part2 = new ParameterizedStringPart("somename", "somevalue");
            Assert.IsTrue(part.Equals(part2));
            Assert.IsTrue(part == part2);
        }

        [Test]
        public void NullEqualsNull()
        {
            ParameterizedStringPart part = null;
            Assert.IsTrue(part == null);
        }

        [Test]
        public void NotEqualsNull()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somedata");
            Assert.IsFalse(part.Equals(null));
            Assert.IsTrue(part != null);

            part = new ParameterizedStringPart("somename", "somedata");
            Assert.IsFalse(part.Equals(null));
            Assert.IsTrue(part != null);
        }

        [Test]
        public void NotEquals()
        {
            ParameterizedStringPart part = new ParameterizedStringPart("somedata");
            ParameterizedStringPart part2 = new ParameterizedStringPart("somedata2");
            Assert.IsFalse(part.Equals(part2));
            Assert.IsTrue(part != part2);
        }
    }
}
