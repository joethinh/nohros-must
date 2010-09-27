using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messenger
{
    /// <summary>
    /// Defines methods and properties used to manage messages.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Adds a message recipient.
        /// </summary>
        /// <param name="recipient">A string that identifies the message recipient.</param>
        void AddRecipient(string recipient);

        /// <summary>
        /// A string that identifies the message sender.
        /// </summary>
        string Sender { get; set; }

        /// <summary>
        /// An string array containing one or more message recipients.
        /// </summary>
        string[] Recipients { get; }

        /// <summary>
        /// Gets the message to send.
        /// </summary>
        string Message { get; }
    }
}