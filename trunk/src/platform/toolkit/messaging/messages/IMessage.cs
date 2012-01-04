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
    /// Gets the timestamp indicating the date and time the message was sent
    /// or received.
    /// </summary>
    DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets an array that contains the recipients of the message.
    /// </summary>
    IAgent[] Recipients { get; }

    /// <summary>
    /// Gets or sets the message's text.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Gets or sets the message's subject.
    /// </summary>
    string Subject { get; set; }
  }
}