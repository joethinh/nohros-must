using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Serves as the base class for custom <see cref="Nohros.Toolkit.IMessage"/>.
    /// </summary>
    public abstract class Message : IMessage
    {
        protected Dictionary<string, IRecipient> recipients_;
        protected IRecipient sender_;
        protected MessageType type_;
        protected DateTime timestamp_;

        #region .ctor
        /// <summary>
        /// Intiializes a new instance_ of the Message class.
        /// </summary>
        public Message(MessageType type) {
            type_ = type;
            timestamp_ = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance_ of the Message class by using the message sender.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender</param>
        public Message(MessageType type, IRecipient sender): this(type) {
            if (sender == null)
                throw new ArgumentNullException("sender");
            sender_ = sender;
        }

        /// <summary>
        /// Initializes a new instance_ of the Message class by using the message sender and recipients
        /// </summary>
        /// <param name="sender">A string that identifies the message sender.</param>
        /// <param name="receipts">A string array containing the message recipients.</param>
        public Message(MessageType type, IRecipient sender, IRecipient[] recipients): this(type, sender) {
            if (recipients == null)
                throw new ArgumentNullException("recipients");

            for (int i = 0, j = recipients.Length; i < j; i++) {
                IRecipient recipient = recipients[i];
                recipients_.Add(recipient.Name, recipient);
            }
        }
        #endregion

        /// <summary>
        /// Adds a recipient to the receipts collection.
        /// </summary>
        /// <param name="recipient">A string that identifies the message recipient.</param>
        public void AddRecipient(IRecipient recipient) {
            if (recipient == null)
                throw new ArgumentNullException("recipient");
            recipients_[recipient.Name] = recipient;
        }

        /// <summary>
        /// Gets the timestamp indicating the date and time the message was sent or received.
        /// </summary>
        public DateTime TimeStamp {
            get { return timestamp_; }
            set { timestamp_ = value; }
        }

        /// <summary>
        /// A string that identifies the message sender.
        /// </summary>
        public IRecipient Sender {
            get { return sender_; }
            set { sender_ = value; }
        }

        /// <summary>
        /// An string array containing the message recipients.
        /// </summary>
        public IRecipient[] Recipients {
            get {
                if (recipients_ == null)
                    return null;
                IRecipient[] recipients = new IRecipient[recipients_.Count];
                recipients_.Values.CopyTo(recipients, 0);
                return recipients;
            }
        }

        /// <summary>
        /// Gets the message's text body.
        /// </summary>
        public abstract string Body { get; }

        /// <summary>
        /// Gets the message's text body.
        /// </summary>
        public MessageType Type {
            get { return type_; }
        }
    }
}
