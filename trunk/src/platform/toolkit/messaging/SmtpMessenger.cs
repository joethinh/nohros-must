using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

using Nohros;
using Nohros.Resources;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Allows applications to send a <see cref="IMessage"/> by using the Simple Mail Transfer Protocol(SMTP).
    /// </summary>
    public class SmtpMessenger : IMessenger
    {
        const string kSMTPHostAddress = "smtp-host";
        const string kSMTPPort = "smtp-port";
        const string kEnableSSL = "enable-ssl";

        SmtpClient smtp_client_;
        string name_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the SmtpMessenger by using the specified messenger's name and options.
        /// </summary>
        /// <param name="name">A string that identifies the messenger.</param>
        /// <param name="options">The options configured for the messenger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="options"/> is a
        /// null reference.</exception>
        /// <exception cref="ArgumentException">The required options could not be found into the <paramref name="options"/>
        /// collections.</exception>
        /// <remarks>The options recognized by this messenger are:
        /// <list type="bullet">
        /// <item>
        /// <term>smtp-host</term>
        /// <description>The address to the SMTP server.</description>
        /// </item>
        /// <item>
        /// <term>smtp-port</term>
        /// <description>The port used to connects to the SMTP server.</description>
        /// </item>
        /// <item>
        /// <term>enable-ssl</term>
        /// <description>A boolean value that indicates if the server requires a SSL connection.</description>
        /// </item>
        /// </list>
        /// <para>
        /// The "smtp-host" and "smtp-port" options are required and the others are optional.
        /// </para>
        /// </remarks>
        public SmtpMessenger(string name, IDictionary<string, string> options) {
            if (name == null)
                throw new ArgumentNullException("name");

            string host = Messenger.GetRequiredOption(kSMTPHostAddress, options);
            string port_str = Messenger.GetRequiredOption(kSMTPPort, options);

            int port;
            if (!int.TryParse(port_str, out port))
                throw new ArgumentException(string.Format(Resources.Messaging_SMTP_InvalidPort, port_str));

            smtp_client_ = new SmtpClient(host, port);
            smtp_client_.EnableSsl = options.ContainsKey(kEnableSSL);
            name_ = name;
        }
        #endregion

        /// <summary>
        /// Sends the specified <paramref name="message"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A <see cref="ResponseMessage"/> object containing the message that the server sent in response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is a null reference.</exception>
        /// <remarks>
        /// The <paramref name="message"/> must be a mail message or a sms message. Any other message type causes
        /// the Send method to returns a error <see cref="ResponseMessage"/> as response.
        /// <para>
        /// If the message is successfully sent the Send method will returns a processed <see cref="ResponseMessage"/> as
        /// response.
        /// </para>
        /// <para>
        /// If a required information is missing this method will returns an error<see cref="ResponseMessage"/> containing
        /// the text describing the cause of the error as response.
        /// </para>
        /// <para>
        /// This method uses the <see cref="SmtpClient"/> class to delivery the <paramref name="message"/> to the SMTP
        /// server and rely on the behavior of this class. The exceptions that are thrown by the <see cref="SmtpClient"/>
        /// class will not be caught by this method and the exception will be propagated to the caller.
        /// </para>
        /// </remarks>
        /// <seealso cref="SmtpClient"/>
        ResponseMessage IMessenger.Send(IMessage message) {
            EmailMessage email_message = message as EmailMessage;
            if (message != null)
                return Send(email_message);
            return new ResponseMessage(ResponseMessageType.NotProcessedMessage);
        }

        /// <summary>
        /// Sends the specified <paramref name="message"/> to an SMTP server for delivery.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A <see cref="ResponseMessage"/> object containing the message that the server sent in response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is a null reference.</exception>
        /// <remarks>
        /// The <paramref name="message"/> must be a mail message or a sms message. Any other message type causes
        /// the Send method to returns a error <see cref="ResponseMessage"/> as response.
        /// <para>
        /// If the message is successfully sent the Send method will returns a processed <see cref="ResponseMessage"/> as
        /// response.
        /// </para>
        /// <para>
        /// If a required information is missing this method will returns an error<see cref="ResponseMessage"/> containing
        /// the text describing the cause of the error as response.
        /// </para>
        /// <para>
        /// This method uses the <see cref="SmtpClient"/> class to delivery the <paramref name="message"/> to the SMTP
        /// server and rely on the behavior of this class. The exceptions that are thrown by the <see cref="SmtpClient"/>
        /// class will not be caught by this method and the exception will be propagated to the caller.
        /// </para>
        /// </remarks>
        /// <seealso cref="SmtpClient"/>
        public ResponseMessage Send(EmailMessage message) {
            if (message == null)
                throw new ArgumentNullException("messsage");

            ResponseMessage response = new ResponseMessage(ResponseMessageType.ErrorMessage);
            if (!ValidateMessage(message, ref response))
                return response;

            MailMessage mail_message = new MailMessage();
            mail_message.From = new MailAddress(message.Sender.Address, message.Sender.Name);
            mail_message.IsBodyHtml = message.IsBodyHtml;
            mail_message.Subject = message.Subject;
            mail_message.Body = message.Body;

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
        /// <param name="error_message">An <see cref="ResponseMessage"/> that contains description of the error
        /// taht cause the validation error.</param>
        /// <returns>true if the message is valid; otherwise false.</returns>
        bool ValidateMessage(IMessage message, ref ResponseMessage error_message) {
            if (message.Sender == null) {
                error_message = new ResponseMessage(Resources.Messaging_smtperr_NoSender, ResponseMessageType.ErrorMessage);
                return false;
            } else if(message.Sender.Address == null) {
                error_message = new ResponseMessage(Resources.Messaging_smtperr_NoSenderAddress, ResponseMessageType.ErrorMessage);
                return false;
            } else if (message.Recipients == null || message.Recipients.Length == 0) {
                error_message = new ResponseMessage(Resources.Messaging_smtperr_NoRecipients, ResponseMessageType.ErrorMessage);
                return false;
            }
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
