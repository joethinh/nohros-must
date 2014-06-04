using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Represents a Short Message Service(SMS) message.
  /// </summary>
  /// <remarks>The recipients of a SMS message are mobile numbers</remarks>
  public class SmsMessage: BaseMessage
  {
    #region .ctor
    /// <summary>
    /// Intiializes a new instance of the <see cref="SmsMessage"/> class by 
    /// using the specified message.
    /// </summary>
    public SmsMessage(string message) : base() {
      message_ = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmsMessage"/> class by
    /// using the specified message sender and message.
    /// </summary>
    /// <param name="sender">A <see cref="IAgent"/> object that represents
    /// the message sender.</param>
    public SmsMessage(IAgent sender, string message): base(sender) {
      message_ = message;
    }

    /// <summary>
    /// Initializes a new instance_ of the <see cref="SmsMessage"/> class by
    /// using the message sender, recipients and the message itself.
    /// </summary>
    /// <param name="sender">A <see cref="IAgent"/> that represents
    /// the message sender.
    /// </param>
    /// <param name="receipts">An array of <see cref="IAgent"/> containing the
    /// message recipients.</param>
    public SmsMessage(IAgent sender, IAgent[] recipients, string message)
      : base(sender, recipients) {
      message_ = message;
    }
    #endregion

    /// <summary>
    /// Gets the SMS message's text body.
    /// </summary>
    public override string Message {
      get { return message_; }
    }
  }
}