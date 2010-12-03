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
        /// Gets the agent who sent the message.
        /// </summary>
        IAgent Sender { get; set; }

        /// <summary>
        /// Gets the timestamp indicating the date and time the message was sent or received.
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets the collection of recipients for the message.
        /// </summary>
        IAgent[] Recipients { get; }

        /// <summary>
        /// Gets or sets the message's text body.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets the message's subject.
        /// </summary>
        string Subject { get; set; }
    }
}