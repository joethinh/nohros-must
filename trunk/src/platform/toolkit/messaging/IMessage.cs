using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a message that is sent or received from messaging systems.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the recipient who sent the message.
        /// </summary>
        IRecipient Sender { get; set; }

        /// <summary>
        /// Gets the timestamp indicating the date and time the message was sent or received.
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        MessageType Type { get; }

        /// <summary>
        /// Gets the collection of recipients for the message.
        /// </summary>
        IRecipient[] Recipients { get; }

        /// <summary>
        /// Gets the message's text body.
        /// </summary>
        string Body { get; }
    }
}