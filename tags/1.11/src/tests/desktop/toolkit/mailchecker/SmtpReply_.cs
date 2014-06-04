using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Toolkit.MailChecker;

namespace Nohros.Test.Toolkit.MailChecker
{
    [TestFixture]
    public class SmtpReply_
    {
        [Test]
        public void SmtpReplyParseByteArray() {
            byte[] message = Encoding.ASCII.GetBytes("205 Smtp Reply BaseMessage");
            SmtpReply smtp = new SmtpReply(message, 0, message.Length);
            Assert.AreEqual(205, smtp.Code);
            Assert.AreEqual(smtp.Message, "Smtp Reply BaseMessage");
        }

        [Test]
        [ExpectedException(typeof(InvalidReplyException))]
        public void InvalidReplyException() {
            byte[] message = Encoding.ASCII.GetBytes("Smtp Reply BaseMessage Without Code");
            SmtpReply smtp = new SmtpReply(message, 0, message.Length);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MessageWithNegativeOffset() {
            byte[] message = Encoding.ASCII.GetBytes("205 Smtp Reply BaseMessage Without Code");
            SmtpReply smtp = new SmtpReply(message, -1, message.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MessageWithBoundsOverload() {
            byte[] message = Encoding.ASCII.GetBytes("205 Smtp Reply BaseMessage Without Code");
            SmtpReply smtp = new SmtpReply(message, 0, message.Length + 10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MessageWithCodeGreaterThanMax() {
            SmtpReply smtp = new SmtpReply(1000, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MessageWithCodeLessThanMin() {
            SmtpReply smtp = new SmtpReply(10, string.Empty);
        }
    }
}