using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a Short Message Service(SMS) message.
    /// </summary>
    /// <remarks>The recipients of a SMS message are mobile numbers</remarks>
    public class SmsMessage : Message
    {
        #region .ctor
        /// <summary>
        /// Intiializes a new instance_ of the SmsMessage class.
        /// </summary>
        public SmsMessage():base(MessageType.SmsMessage) { }

        /// <summary>
        /// Initializes a new instance_ of the SmsMessage class by using the message sender.
        /// </summary>
        /// <param name="sender">A string that identifies the message sender</param>
        public SmsMessage(IRecipient sender) : base(MessageType.SmsMessage, sender) { }

        /// <summary>
        /// Initializes a new instance_ of the SmsMessage class by using the message sender and recipients
        /// </summary>
        /// <param name="sender">A string that identifies the message sender.</param>
        /// <param name="receipts">A string array containing the message recipients.</param>
        public SmsMessage(IRecipient sender, IRecipient[] recipients) : base(MessageType.SmsMessage, sender, recipients) { }
        #endregion

        /// <summary>
        /// Gets the SMS message's text body.
        /// </summary>
        public override string Body {
            get { return string.Empty; }
        }
    }
}