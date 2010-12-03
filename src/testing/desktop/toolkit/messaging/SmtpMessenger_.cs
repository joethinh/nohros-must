using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Toolkit.Messaging;

namespace Nohros.Test.Toolkit.Messaging
{
    [TestFixture]
    public class SmtpMessenger_
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullOptions() {
            Dictionary<string, string> options = new Dictionary<string,string>();
            options["someoption"] = "someoptionvalue";
            SmtpMessenger messenger = new SmtpMessenger(null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorStringNull() {
            SmtpMessenger messenger = new SmtpMessenger("somename", null);
            Assert.Pass();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CtorStringOptionMissingRequired() {
            Dictionary<string, string> options = new Dictionary<string,string>();
            options["someoption"] = "someoptionvalue";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
        }

        [Test]
        public void CtorStringOptions() {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.nohros.com";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            Assert.Pass();
        }

        [Test]
        public void SendText() {
            EmailMessage message = new EmailMessage("somemessage");
            message.Sender = new EmailAgent("somename@somedomain.com", "somename");
            message.Subject = "somesubject";
            message.IsBodyHtml = false;
            message.Body = "sometext";
            message.AddRecipient(new EmailAgent("neylor@acaoassessoria.com.br", "Neylor Ohmaly"));

            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.acao.net.br";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            ResponseMessage response = messenger.Send(message);
            Assert.AreEqual(ResponseMessageType.ProcessedMessage, response.Type);
        }
    }
}
