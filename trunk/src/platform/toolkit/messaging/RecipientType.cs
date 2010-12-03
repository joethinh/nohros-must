using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    internal enum RecipientType
    {
        /// <summary>
        /// A generic sender recipient.
        /// </summary>
        Sender = 0,

        /// <summary>
        /// A e-mail recipient
        /// </summary>
        Email = 1,

        /// <summary>
        /// A short message service recipient.
        /// </summary>
        Sms = 2
    }
}
