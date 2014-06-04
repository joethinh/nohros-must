using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Specifies supported message types.
  /// </summary>
  public enum ResponseMessageType
  {
    /// <summary>
    /// The default response message. That is a response message that has an
    /// associated text.
    /// </summary>
    TextMessage = 0,

    /// <summary>
    /// A error message. Used internally.
    /// </summary>
    ErrorMessage = 3,

    /// <summary>
    /// A message used to signal that a message is successfully sent. This type
    /// of message does not have a text associated.
    /// </summary>
    /// <remarks>When a provider successfully sends a message and does not want
    /// to send a text response it should returns a
    /// <see cref="ProcessedMessage"/> to inform the caller that the message
    /// was sent.</remarks>
    ProcessedMessage = 5,

    /// <summary>
    /// A message used to signal that a message cannot be send through a
    /// messenger provider. This type of message does not have a text
    /// associated.
    /// </summary>
    /// <remarks>When a provider cannot knows how to send a particular type of
    /// message, it should do nothing and returns a
    /// <see cref="NotProcessedMessage"/> message to the caller.</remarks>
    NotProcessedMessage = 6
  }
}
