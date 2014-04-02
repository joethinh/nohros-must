using System;

namespace Nohros.Bus
{
  /// <summary>
  /// Synchronously dispatches messages to zero or more subscribers.
  /// Subscribers are responsible for handling exceptions
  /// </summary>
  public class InMemoryBus : IBus
  {
    public InMemoryBus() {
    }

    public void Publish<T>(T @event) where T : IEvent {
    }

    public void Subscribe<T>(IHandle<T> handler) where T : IMessage {
    }

    public void Unsubscribe<T>(IHandle<T> handler) where T : IMessage {
    }
  }
}
