using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Specifies supported message types.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// A generic message.
        /// </summary>
        GenericMessage= 0,

        /// <summary>
        /// A mail message
        /// </summary>
        MailMessage = 1,

        /// <summary>
        /// A Short Message Service(SMS) message.
        /// </summary>
        SmsMessage = 2
    }
}
