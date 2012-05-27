using System;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Represents a method to be called when a <see cref="Mailbox{T}"/> receives
  /// a message.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the message.
  /// </typeparam>
  /// <param name="message">
  /// The message that was sent to the <see cref="Mailbox{T}"/>.
  /// </param>
  /// <remarks>
  /// <see cref="MailboxReceiveCallback{T}"/> represents a callback that you
  /// want to execute when a message is received by a
  /// <see cref="Mailbox{T}"/>. The delegate is executed for each received
  /// message respecting the arrival order.
  /// <para>
  /// The callback runs in a thread from the thread pool.
  /// </para>
  /// </remarks>
  public delegate void MailboxReceiveCallback<T>(T message);
}
