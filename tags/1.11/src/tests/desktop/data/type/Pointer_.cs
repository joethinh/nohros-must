using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;
using NUnit.Framework;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class Pointer_
    {
        byte[] message = new byte[] { (byte)'P', (byte)'R', (byte)'U', (byte)'L', (byte)'U', (byte)'L', (byte)'U' };

        [Test]
        public void Pointer_operator() {
            Pointer pointer = new Pointer(message, 0);
            Assert.AreEqual(message[0], pointer[0]);
            Assert.AreEqual(message[1], (++pointer as Pointer).GetByte());
            Assert.AreEqual(message[2], pointer.GetByte());
            Assert.AreEqual(message[3], pointer.GetByte());
            Assert.AreEqual(message[4], pointer.GetByte());
            Assert.AreEqual(message[5], pointer.GetByte());
            Assert.AreEqual(message[6], pointer.GetByte());
        }

        [Test]
        public void Pointer_GetByte() {
            Pointer pointer = new Pointer(message, 0);
            Assert.AreEqual(message[0], pointer.GetByte());
            Assert.AreEqual(message[1], pointer.GetByte());
            Assert.AreEqual(message[2], pointer.GetByte());
            Assert.AreEqual(message[3], pointer.GetByte());
            Assert.AreEqual(message[4], pointer.GetByte());
            Assert.AreEqual(message[5], pointer.GetByte());
            Assert.AreEqual(message[6], pointer.GetByte());
        }

        [Test]
        public void Pointer_GetShort() {
            Pointer pointer = new Pointer(message, 0);
            Assert.AreEqual(message[0] << 8 | message[1], pointer.GetShort());
            Assert.AreEqual(message[2] << 8 | message[3], pointer.GetShort());
            Assert.AreEqual(message[4] << 8 | message[5], pointer.GetShort());
            Assert.Throws<IndexOutOfRangeException>(delegate()
            {
                pointer.GetShort();
            });
        }

        [Test]
        public void Pointer_GetInt() {
            Pointer pointer = new Pointer(message, 0);
            Assert.AreEqual(
                ((ushort)(message[0] << 8 | message[1]) << 16 | (ushort)(message[2] << 8 | message[3])),
                pointer.GetInt()
                );
            Assert.Throws<IndexOutOfRangeException>(delegate()
            {
                pointer.GetInt();
            });
        }

        [Test]
        public void Pointer_GetChar() {
            Pointer pointer = new Pointer(message, 0);
            Assert.AreEqual('P', pointer.GetChar());
            Assert.AreEqual('R', pointer.GetChar());
            Assert.AreEqual('U', pointer.GetChar());
            Assert.AreEqual('L', pointer.GetChar());
            Assert.AreEqual('U', pointer.GetChar());
            Assert.AreEqual('L', pointer.GetChar());
            Assert.AreEqual('U', pointer.GetChar());
            //Assert.Throws<IndexOutOfRangeException>(delegate()
            //{
                //pointer.GetChar();
            //});
        }

        [Test]
        public void Pointer_Increment() {
            Pointer pointer = new Pointer(message, 0);

            Assert.AreEqual('P', pointer.Peek());
            Assert.AreEqual('R', pointer++.Peek());
            Assert.AreEqual('U', pointer++.Peek());
            Assert.AreEqual('L', pointer++.Peek());
            Assert.AreEqual('U', pointer++.Peek());
            Assert.AreEqual('L', pointer++.Peek());
            Assert.AreEqual('U', pointer++.Peek());
            Assert.Throws<IndexOutOfRangeException>(delegate()
            {
                pointer++;
            });
        }

        [Test]
        public void Pointer_Decrement() {
            Pointer pointer = new Pointer(message, 0);
            pointer += message.Length - 1;

            Assert.AreEqual('U', (pointer as Pointer).Peek());
            Assert.AreEqual('L', (--pointer as Pointer).Peek());
            Assert.AreEqual('U', (--pointer as Pointer).Peek());
            Assert.AreEqual('L', (--pointer as Pointer).Peek());
            Assert.AreEqual('U', (--pointer as Pointer).Peek());
            Assert.AreEqual('R', (--pointer as Pointer).Peek());
            Assert.AreEqual('P', (--pointer as Pointer).Peek());
            Assert.Throws<IndexOutOfRangeException>(delegate()
            {
                pointer--;
            });
        }
    }
}
