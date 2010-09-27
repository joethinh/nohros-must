using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Defines a recipient for messages.
    /// </summary>
    public interface IRecipient
    {
        /// <summary>
        /// Gets the recipient's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the recipient address.
        /// </summary>
        /// <remarks>The meaning of this parameter depends on the type of the related message.</remarks>
        string Address { get; set; }

        /// <summary>
        /// The type of the recipient.
        /// </summary>
        RecipientType Type { get; }
    }
}