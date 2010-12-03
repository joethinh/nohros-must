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
    /// <remarks>
    /// All messengers must have a constructor that accepts a string and a <see cref="IDictionary&gt;string, string&lt;"/>
    /// object. The string parameter represents the name of the provider and the dictionary parameter represents the
    /// options configured for the provider. Note that the options parameter could be a null reference, but the name
    /// parameter could not. If the name parameter is null the constructor should throw an <see cref="ArgumentNullException"/> must be raised.
    /// exception.
    /// </remarks>
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
        ResponseMessage Send(IMessage message);
    }
}