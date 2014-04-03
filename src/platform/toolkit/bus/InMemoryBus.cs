using System;
using System.Collections.Generic;
using System.Linq;

namespace Nohros.Bus
{
  /// <summary>
  /// Synchronously dispatches messages to zero or more subscribers.
  /// Subscribers are responsible for handling exceptions
  /// </summary>
  public class InMemoryBus : IBus
  {
    readonly ICreateHandlers builder_;

    public InMemoryBus(ICreateHandlers builder) {
      if (builder == null) {
        throw new ArgumentNullException("builder");
      }
      builder_ = builder;
    }

    /// <summary>
    /// Sends the <paramref name="command"/> to the registered command handler.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the command to be published.
    /// </typeparam>
    /// <param name="command">
    /// The command to be sent.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="command"/> is not a value type and is a <c>null</c>
    /// reference.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The <typeparamref name="T"/> has more than one registered command
    /// handler.
    /// </exception>
    public void Send<T>(T command) {
      if (!typeof (T).IsValueType && command == null) {
        throw new ArgumentNullException("command");
      }

      var handlers = builder_.CreateHandlersOf<T>().ToArray();
      if (handlers.Length > 1) {
        throw new InvalidOperationException(
          string.Format(Resources.MultipleCommandHandlers, (typeof (T).Name)));
      }

      Dispatch(command, handlers);
    }

    /// <summary>
    /// Publish the <paramref name="event"/> to all registered subscribers.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the event to be published.
    /// </typeparam>
    /// <param name="event">
    /// The event to be published.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="@event"/> is not a value type and is a <c>null</c>
    /// reference.
    /// </exception>
    public void Publish<T>(T @event) {
      if (!typeof (T).IsValueType && @event == null) {
        throw new ArgumentNullException("event");
      }

      var handlers = builder_.CreateHandlersOf<T>().ToArray();
      Dispatch(@event, handlers);
    }

    void Dispatch<T>(T msg, IEnumerable<IHandle<T>> handlers) {
      foreach (IHandle<T> handler in handlers) {
        try {
          handler.Handle(msg);
        } finally {
          var disposable = handler as IDisposable;
          if (disposable != null) {
            disposable.Dispose();
          }
        }
      }
    }
  }
}
