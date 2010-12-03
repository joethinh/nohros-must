using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a e-mail message.
    /// </summary>
    /// <remarks>The recipients of a mail message are email address</remarks>
    public class EmailMessage : Message
    {
        bool is_body_html_;

        #region .ctor
        /// <summary>
        /// Intiializes a new instance of the EmailMessage class by using the specified text message.
        /// </summary>
        public EmailMessage(string message) : base() {
            message_ = message;
            is_body_html_ = false;
        }

        /// <summary>
        /// Initializes a new instance of the EmailMessage class by using the message sender and text message.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender</param>
        public EmailMessage(EmailAgent sender, string message) : base(sender) {
            is_body_html_ = false;
            message_ = message;
        }

        /// <summary>
        /// Initializes a new instance of the EmailMessage class by using the message sender and recipients.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender.</param>
        /// <param name="receipts">A string array containing the message recipients.</param>
        public EmailMessage(IAgent sender, IAgent[] recipients, string message): base(sender, recipients) {
            is_body_html_ = false;
            message_ = message;
        }
        #endregion

        public bool IsBodyHtml {
            get { return is_body_html_; }
            set { is_body_html_ = value; }
        }
    }
}