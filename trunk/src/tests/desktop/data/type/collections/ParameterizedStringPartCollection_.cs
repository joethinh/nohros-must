using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;
using Nohros.Data.Collections;

namespace Nohros.Test.Data.Collections
{
    [TestFixture]
    public class ParameterizedStringPartCollection_
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDuplicate() {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart part = new ParameterizedStringPart("somename", "somedata");
            collection.Add(part);
            collection.Add(part);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNull() {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddLiteral()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("literal-value"));
        }

        [Test]
        public void IndexOfWithNull() {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            Assert.AreEqual(-1, collection.IndexOf(null));
        }

        [Test]
        public void IndexOf()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somename", "somedata"));
            Assert.AreEqual(0, collection.IndexOf("somename"));
        }

        [Test]
        public void Contains()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somedata", "somevalue"));
            collection.Contains(new ParameterizedStringPart("somedata", "somevalue"));
        }

        [Test]
        public void Clear()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somedata", "somevalue"));
            Assert.AreNotEqual(0, collection.Count);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void EmptyCopyTo()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart[] parts = new ParameterizedStringPart[1];
            collection.CopyTo(parts, 0);
            Assert.IsNull(parts[0]);
        }

        [Test]
        public void CopyToZeroIndex()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart[] parts = new ParameterizedStringPart[2];

            collection.Add(new ParameterizedStringPart("somename", "somevalue"));
            collection.Add(new ParameterizedStringPart("othername", "othervalue"));
            collection.CopyTo(parts, 0);
            Assert.IsNotNull(parts[0]);
            Assert.IsNotNull(parts[1]);
            Assert.AreEqual("somename", parts[0].ParameterName);
            Assert.AreEqual("othername", parts[1].ParameterName);
        }

        [Test]
        public void CopyToIndex()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart[] parts = new ParameterizedStringPart[3];

            collection.Add(new ParameterizedStringPart("somename", "somevalue"));
            collection.Add(new ParameterizedStringPart("othername", "othervalue"));
            collection.CopyTo(parts, 1);
            Assert.IsNull(parts[0]);
            Assert.IsNotNull(parts[1]);
            Assert.AreEqual("somename", parts[1].ParameterName);
            Assert.AreEqual("othername", parts[2].ParameterName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToNullArray() {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart[] parts = new ParameterizedStringPart[1];

            collection.CopyTo(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToOverflowIndex()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart[] parts = new ParameterizedStringPart[1];

            collection.Add(new ParameterizedStringPart("somename", "somevalue"));
            collection.Add(new ParameterizedStringPart("othername", "othervalue"));
            collection.CopyTo(parts, 3);
        }

        [Test]
        public void RemovePartNull()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            Assert.IsFalse(collection.Remove((ParameterizedStringPart)null));
        }

        [Test]
        public void RemovePartInexistent()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart part = new ParameterizedStringPart("literal-value");
            Assert.IsFalse(collection.Remove(part));
        }

        [Test]
        public void RemoveStringNull()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            Assert.IsFalse(collection.Remove((string)null));
        }

        [Test]
        public void RemoveStringInexistent()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            Assert.IsFalse(collection.Remove("literal-value"));
        }

        [Test]
        public void RemovePart()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            ParameterizedStringPart part = new ParameterizedStringPart("somename", "somedata");
            collection.Add(part);
            Assert.IsTrue(collection.Remove(part));
        }

        [Test]
        public void RemoveString()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somename", "somedata"));
            Assert.IsTrue(collection.Remove("somename"));
        }

        [Test]
        public void ThisIndex()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somename", "somedata"));
            Assert.AreEqual(1, collection.Count);
            Assert.IsNotNull(collection[0]);
            Assert.AreEqual("somename", collection[0].ParameterName);
        }

        [Test]
        public void ThisName()
        {
            ParameterizedStringPartCollection collection = new ParameterizedStringPartCollection();
            collection.Add(new ParameterizedStringPart("somename", "somedata"));
            Assert.AreEqual(1, collection.Count);
            Assert.IsNotNull(collection[0]);
            Assert.AreEqual("somedata", collection["somename"].LiteralValue);
        }
    }
}