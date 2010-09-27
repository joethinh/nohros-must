using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messenger
{
    /// <summary>
    /// Serves as the base class for custom <see cref="Nohros.Toolkit.IMessage"/>.
    /// </summary>
    public abstract class Message : IMessage
    {
        protected IDictionary<string, string> recipients_;
        protected string sender_;

        #region .ctor
        /// <summary>
        /// Intiializes a new instance of the Message class.
        /// </summary>
        public Message() { }

        /// <summary>
        /// Initializes a new instance of the Message class by using the message sender.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender</param>
        public Message(string sender) {
            if (sender == null)
                throw new ArgumentNullException("sender");
            sender_ = sender;
        }

        /// <summary>
        /// Initializes a new instance of the Message class by using the message sender and recipients
        /// </summary>
        /// <param name="sender">A string that identifies the message sender.</param>
        /// <param name="receipts">A string array containing the message recipients.</param>
        public Message(string sender, string[] recipients):this(sender) {
            if (recipients == null)
                throw new ArgumentNullException("recipients");

            for (int i = 0, j = recipients.Length; i < j; i++) {
                recipients_[recipients[i]] = null;
            }
        }
        #endregion

        /// <summary>
        /// Adds a message recipient.
        /// </summary>
        /// <param name="recipient">A string that identifies the message recipient.</param>
        public void AddRecipient(string recipient) {
            recipients_.Add(recipient);
        }

        /// <summary>
        /// A string that identifies the message sender.
        /// </summary>
        public string Sender {
            get { return sender_; }
        }
    }
}
