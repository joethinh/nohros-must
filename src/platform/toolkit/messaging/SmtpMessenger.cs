using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

using Nohros;
using Nohros.Resources;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Allows applications to send a <see cref="IMessage"/> by using the Simple
  /// Mail Transfer Protocol(SMTP).
  /// </summary>
  public class SmtpMessenger: IMessenger
  {
    SmtpClient smtp_client_;
    string name_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpMessenger"/> class by
    /// using the specified messenger's name.
    /// </summary>
    /// <param name="name">A string that identifies the messenger.</param>
    /// <param name="smtp_client">A <see cref="SmtpClient"/> object that is
    /// used to deliver the message to a SMTP server.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is a
    /// null reference.</exception>
    public SmtpMessenger(string name, SmtpClient smtp_client) {
      if (name == null)
        throw new ArgumentNullException("name");

      name_ = name;
      smtp_client_ = smtp_client;
    }
    #endregion

    /// <summary>
    /// Sends the specified <paramref name="message"/> to an SMTP server for
    /// delivery.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>A <see cref="IMessage"/> object containing the message that
    /// the server sent in response.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is
    /// a null reference.</exception>
    /// <remarks>
    /// The <paramref name="message"/> must be a object of the type
    /// <see cref="EmailMessage"/> or an object that derives from that type.
    /// Any other message type causes the Send method to returns a
    /// error <see cref="ResponseMessage"/> as response.
    /// <para>
    /// If the message is successfully sent the this method will returns a
    /// processed <see cref="ResponseMessage"/> as response.
    /// </para>
    /// <para>
    /// If a required information is missing this method will returns an
    /// error <see cref="ResponseMessage"/> containing the text describing the
    /// cause of the error as response.
    /// </para>
    /// <para>
    /// This method uses the <see cref="SmtpClient"/> class to delivery the
    /// <paramref name="message"/> to the SMTP server and rely on the behavior
    /// of this class. The exceptions that are thrown by the
    /// <see cref="SmtpClient"/> class will not be caught by this method and
    /// the exception will be propagated to the caller.
    /// </para>
    /// </remarks>
    /// <seealso cref="SmtpClient"/>
    IMessage IMessenger.Send(IMessage message) {
      EmailMessage email_message = message as EmailMessage;
      if (email_message != null)
        return Send(email_message);
      return new ResponseMessage(ResponseMessageType.NotProcessedMessage);
    }

    /// <summary>
    /// Sends the specified <paramref name="message"/> to an SMTP server for
    /// delivery.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>A <see cref="ResponseMessage"/> object containing the message
    /// that the server sent in response.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is a
    /// null reference.</exception>
    /// <remarks>
    /// If the message is successfully sent the Send method will returns a
    /// processed <see cref="ResponseMessage"/> as response.
    /// <para>
    /// If a required information is missing this method will returns an
    /// error<see cref="ResponseMessage"/> containing the text describing the
    /// cause of the error as response.
    /// </para>
    /// <para>
    /// This method uses the <see cref="SmtpClient"/> class to delivery the
    /// <paramref name="message"/> to the SMTP server and rely on the behavior
    /// of this class. The exceptions that are thrown by the
    /// <see cref="SmtpClient"/> class will not be caught by this method and
    /// the exception will be propagated to the caller.
    /// </para>
    /// </remarks>
    /// <seealso cref="SmtpClient"/>
    public IMessage Send(EmailMessage message) {
      if (message == null)
        throw new ArgumentNullException("messsage");

      ResponseMessage response;
      if (!ValidateMessage(message, out response))
        return response;

      MailMessage mail_message = new MailMessage();
      mail_message.From =
        new MailAddress(message.Sender.Address, message.Sender.Name);

      mail_message.IsBodyHtml = message.IsBodyHtml;
      mail_message.Subject = message.Subject;
      mail_message.Body = message.Message;

      MailAddressCollection addresses = mail_message.To;
      for (int i = 0, j = message.Recipients.Length; i < j; i++) {
        IAgent recipient = message.Recipients[i];
        if (recipient.Address != null && recipient.Name != null)
          addresses.Add(new MailAddress(recipient.Address, recipient.Name));
      }

      smtp_client_.Send(mail_message);

      return new ResponseMessage(ResponseMessageType.ProcessedMessage);
    }

    /// <summary>
    /// Validate the message parameters.
    /// </summary>
    /// <param name="message">The message to validate.</param>
    /// <param name="error_message">An <see cref="ResponseMessage"/> that
    /// contains description of the error that cause the validation error.
    /// </param>
    /// <returns>true if the message is valid; otherwise false.</returns>
    protected bool ValidateMessage(IMessage message,
      out ResponseMessage error_message) {
      if (message.Sender == null) {
        error_message = new ResponseMessage(
          Resources.Messaging_smtperr_NoSender,
          ResponseMessageType.ErrorMessage);
        return false;
      } else if (message.Sender.Address == null) {
        error_message = new ResponseMessage(
          Resources.Messaging_smtperr_NoSenderAddress,
          ResponseMessageType.ErrorMessage);
        return false;
      } else if (message.Recipients == null
        || message.Recipients.Length == 0) {
        error_message = new ResponseMessage(
          Resources.Messaging_smtperr_NoRecipients,
          ResponseMessageType.ErrorMessage);
        return false;
      }
      error_message = null;
      return true;
    }

    /// <summary>
    /// Gets the name of the messenger.
    /// </summary>
    public string Name {
      get { return name_; }
    }
  }
}