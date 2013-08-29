using System;
using Nohros.Concurrent;

namespace Nohros.CRQS.Messaging
{
  public interface IPublisher
  {
    /// <summary>
    /// Synchronously punlish the event.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the event to publish.
    /// </typeparam>
    /// <param name="event">
    /// The event to publish.
    /// </param>
    void Publish<T>(T @event) where T : IMessage;

    /// <summary>
    /// Publish a event using the given executor.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the event to publish.
    /// </typeparam>
    /// <param name="event">
    /// The event to publish.
    /// </param>
    /// <param name="executor">
    /// A <see cref="IExecutor"/> to use to publish the event.
    /// </param>
    void Publish<T>(T @event, IExecutor executor) where T : IMessage;
  }
}
