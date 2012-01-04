using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Represents a special-purpose class whose primary function is to
  /// delivery messages to foreign messaging systems, and also, to translate
  /// the <see cref="IMessage"/> objects into the format used by the
  /// foreign messaging system, as well as translate the returned
  /// data back into a <see cref="IMessage"/> object.
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
    /// <returns>A <see cref="IMessage"/> containing the response from the
    /// messaging system.</returns>
    /// <exception cref="ArgumentNullException">message is null.</exception>
    /// <remarks>A messenger can send a message or not</remarks>
    IMessage Send(IMessage message);
  }
}