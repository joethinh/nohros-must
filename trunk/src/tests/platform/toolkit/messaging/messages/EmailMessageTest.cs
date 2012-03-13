using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Toolkit.Messaging;

namespace Nohros.Toolkit.Tests
{
  [TestFixture]
  public class EmailMessageTest
  {
    [Test]
    public void ShouldReturnTheSpecifiedMessage() {
      string mail_message = "mailmessage";
      EmailMessage message = new EmailMessage(mail_message);
      Assert.AreEqual(mail_message, message.Message);
    }

    [Test]
    public void ShouldReturnAnEmptyArrayWhenNoRecipientsWasSpecified() {
      string mail_message = "mailmessage";
      EmailMessage message = new EmailMessage(mail_message);
      Assert.AreEqual(0, message.Recipients.Length);
    }

    [Test]
    public void TheDefaultBodyShouldBeNonHtml() {
      string mail_message = "mailmessage";
      EmailMessage message = new EmailMessage(mail_message);
      Assert.AreEqual(false, message.IsBodyHtml);
    }

    [Test]
    public void ShouldAddTheRecipientToTheRecipientsList() {
      EmailAgent agent = new EmailAgent("mail@adress.com.br", "mailname");
      EmailMessage message = new EmailMessage("mailmessage");

      Assert.AreEqual(0, message.Recipients.Length);
      message.AddRecipient(agent);
      Assert.AreEqual(1, message.Recipients.Length);
      Assert.AreEqual("mailname", message.Recipients[0].Name);
    }
  }
}