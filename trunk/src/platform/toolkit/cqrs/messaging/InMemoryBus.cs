using System;
using System.Collections.Generic;
using Nohros.Concurrent;

namespace Nohros.CQRS.Messaging
{
  public class InMemoryBus : IBus
  {
    readonly Dictionary<Type, List<IMessageHandler>> routes_;

    #region .ctor
    public InMemoryBus() {
      routes_ = new Dictionary<Type, List<IMessageHandler>>();
    }
    #endregion

    public void Publish<T>(T @event) where T : IMessage {
      Publish(@event, Executors.SameThreadExecutor());
    }

    public void Publish<T>(T @event, IExecutor executor) where T : IMessage {
      List<IMessageHandler> handlers;
      if (!routes_.TryGetValue(@event.GetType(), out handlers)) {
        return;
      }

      foreach (IMessageHandler handler in handlers) {
        var h = handler;
        executor.Execute(() => h.TryHandle(@event));
      }
    }

    public void Send<T>(T msg) where T : IMessage {
      List<IMessageHandler> handlers;
      if (routes_.TryGetValue(typeof (T), out handlers)) {
        if (handlers.Count != 1) {
          throw new InvalidOperationException("\"" + typeof (T)
            + "\" command has more than one handlers.");
        }

        if (handlers.Count == 0) {
          throw new InvalidOperationException("There is no handler for the"
            + " command\"" + typeof (T) + "\"");
        }
        handlers[0].TryHandle(msg);
      }
    }

    public void Unsubscribe<T>(IHandle<T> handler) where T : IMessage {
      if (handler == null) {
        throw new ArgumentNullException("handler");
      }

      List<IMessageHandler> handlers;
      if (routes_.TryGetValue(typeof (T), out handlers)) {
        foreach (IMessageHandler h in handlers) {
          if (h.IsSame<T>(handler)) {
            handlers.Remove(h);
            break;
          }
        }
      }
    }

    public void Subscribe<T>(IHandle<T> handler) where T : IMessage {
      if (handler == null) {
        throw new ArgumentNullException("handler");
      }

      List<IMessageHandler> handlers;
      if (!routes_.TryGetValue(typeof (T), out handlers)) {
        handlers = new List<IMessageHandler>();
        routes_.Add(typeof (T), handlers);
      }

      foreach (IMessageHandler h in handlers) {
        if (h.IsSame<T>(h)) {
          return;
        }
      }
      handlers.Add(new MessageHandler<T>(handler));
    }
  }
}
