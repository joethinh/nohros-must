using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.MessageQueue
{
  /// <summary>
  /// Defines the concept of a message queue middleware. That is the place
  /// where the messages are send to and received from.
  /// </summary>
  public interface IMessageMiddleware<T>: IMessageMiddleware where T: IMessage
  {
    /// <summary>
    /// Put a message into the queue.
    /// </summary>
    /// <param name="data">The parameters needed to put a message in the
    /// queue.</param>
    /// <returns>true if the message is successfully enqueued; otherwise,
    /// false. If the middleware does not know if the message was enqueued or
    /// not, this method should return true.</returns>
    bool Enqueue(T message, IOperationData data);

    /// <summary>
    /// Removes and returns a message from the queue.
    /// </summary>
    /// <param name="data">The parameters needed to remove the message from
    /// the queue.</param>
    /// <returns>true if a message is removed from the queue; otherwise,
    /// false.</returns>
    T Dequeue(IOperationData data);
  }
}
