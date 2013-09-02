using System;

namespace Nohros.CQRS.Messaging
{
  /// <summary>
  /// A factory for the <see cref="IHandle{T}"/> class.
  /// </summary>
  public static class Handlers
  {
    /// <summary>
    /// Returns a <see cref="IHandle{T}"/> that executed the given action
    /// while handling the message.
    /// </summary>
    /// <typeparam name="T">
    /// The type of message to handle.
    /// </typeparam>
    /// <param name="handler">
    /// A <see cref="Action{T}"/> object that is used to handle a message.
    /// </param>
    /// <returns>
    /// A <see cref="IHandle{T}"/> object that used the given
    /// <see cref="Action{T}"/> to handle messages.
    /// </returns>
    public static IHandle<T> Runnable<T>(Action<T> handler) where T : IMessage {
      return new RunnableHandler<T>(handler);
    }
  }
}
