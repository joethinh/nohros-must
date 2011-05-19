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
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendNullMessage() {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.acao.net.br";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            ResponseMessage response = messenger.Send(null);
        }

        [Test]
        public void SendNoEmailMessage() {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.acao.net.br";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            ResponseMessage response = ((IMessenger)messenger).Send(new ResponseMessage("somemessage", ResponseMessageType.ErrorMessage));
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

        [Test]
        public void SendTextAsHtml() {
            EmailMessage message = new EmailMessage("somemessage");
            message.Sender = new EmailAgent("somename@somedomain.com", "somename");
            message.Subject = "somesubject";
            message.IsBodyHtml = false;
            message.Body = "<html><title>Text as HTML</title><body>sometext</body></html>";
            message.AddRecipient(new EmailAgent("neylor@acaoassessoria.com.br", "Neylor Ohmaly"));

            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.acao.net.br";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            ResponseMessage response = messenger.Send(message);
            Assert.AreEqual(ResponseMessageType.ProcessedMessage, response.Type);
        }

        [Test]
        public void SendHtml() {
            EmailMessage message = new EmailMessage("somemessage");
            message.Sender = new EmailAgent("somename@somedomain.com", "somename");
            message.Subject = "somesubject";
            message.IsBodyHtml = true;
            message.Body = "<html><title>HTML mail</title><body>sometext</body></html>";
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
