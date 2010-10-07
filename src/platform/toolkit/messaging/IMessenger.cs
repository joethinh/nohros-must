using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a special-purpose class whose primary function is to delivery messages
    /// to foreign messaging systems, and also, to translate <see cref="IMessage"/> objects
    /// into the format used by a foreign messaging system, as well as to translate the returned
    /// data back into a <see cref="IMessage"/> class.
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Gets the name of the messenger.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>A <see cref="IMessage"/> containing the response from the messaging system.</returns>
        IMessage Send(IMessage message);

        /// <summary>
        /// Process the message response sent from the messaging system.
        /// </summary>
        /// <param name="message">The response message to process</param>
        /// <remarks>This method is used to process response message that could be sent
        /// from messaging system after a message is sent, when a applications needs to
        /// performs some post processing operation(ex. store the response into a database).</remarks>
        void ProcessResponse(IMessage message);
    }
}