using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using NUnit.Framework;
using Nohros.Toolkit.MailChecker;

namespace Nohros.Test.Toolkit.MailChecker
{
    [TestFixture]
    public class MailChecker_
    {
        [Test]
        public void EmailAddressWithoutLocalPart() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("@submarino.com", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreNotEqual(error_message, null);
        }

        [Test]
        public void EmailAddressWithoutDomain() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("submarino@", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreNotEqual(error_message, null);
        }

        [Test]
        public void EmailAddressWithoutAtSign() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("submarino=submarino.com", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreNotEqual(error_message, null);
        }

        [Test]
        public void BadDomainName() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("submarino@!£$%^&*()", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreNotEqual(error_message, null);
        }

        [Test]
        public void GoodMailSyntax() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("!£$%^&*()@submarino.com", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreEqual(error_message, null);
        }

        [Test]
        public void GoodMail() {
            string error_message = null;
            Nohros.Toolkit.MailChecker.MailChecker.CheckMail("submarino@submarino.com", IPAddress.Parse("192.168.203.1"), out error_message);
            Assert.AreEqual(error_message, null);
        }
    }
}