using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a e-mail message.
    /// </summary>
    /// <remarks>The recipients of a mail message are email address</remarks>
    public class EmailMessage : Message
    {
        #region .ctor
        /// <summary>
        /// Intiializes a new instance_ of the EmailMessage class.
        /// </summary>
        public EmailMessage() : base(MessageType.MailMessage) { }

        /// <summary>
        /// Initializes a new instance_ of the EmailMessage class by using the message sender.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender</param>
        public EmailMessage(IRecipient sender) : base(MessageType.MailMessage, sender) { }

        /// <summary>
        /// Initializes a new instance_ of the EmailMessage class by using the message sender and recipients.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender.</param>
        /// <param name="receipts">A string array containing the message recipients.</param>
        public EmailMessage(IRecipient sender, IRecipient[] recipients) : base(MessageType.MailMessage, sender, recipients) { }
        #endregion

        /// <summary>
        /// Gets the e-mail message's text body.
        /// </summary>
        public override string Body {
            get { return null; }
        }
    }
}