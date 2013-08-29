using System;
using System.Collections.Generic;
using Nohros.Concurrent;

namespace Nohros.CRQS.Messaging
{
  public class InMemoryBus : IBus
  {
    readonly Dictionary<Type, List<Action<IMessage>>> routes_;

    #region .ctor
    public InMemoryBus() {
      routes_ = new Dictionary<Type, List<Action<IMessage>>>();
    }
    #endregion

    public void Publish<T>(T @event) where T : IMessage {
      Publish(@event, Executors.SameThreadExecutor());
    }

    public void Publish<T>(T @event, IExecutor executor) where T : IMessage {
      List<Action<IMessage>> handlers;
      if (routes_.TryGetValue(@event.GetType(), out handlers)) {
        return;
      }

      foreach (Action<IMessage> handler in handlers) {
        var h = handler;
        executor.Execute(() => h(@event));
      }
    }

    public void Send<T>(T msg) where T : IMessage {
      List<Action<IMessage>> handlers;
      if (routes_.TryGetValue(typeof (T), out handlers)) {
        if (handlers.Count != 1) {
          throw new InvalidOperationException("\"" + typeof (T)
            + "\" command has more than one handlers.");
        }

        if (handlers.Count == 0) {
          throw new InvalidOperationException("There is no handler for the"
            + " command\"" + typeof (T) + "\"");
        }
        handlers[0](msg);
      }
    }

    public void Subscribe<T>(IHandle<T> handler) where T : IMessage {
      List<Action<IMessage>> handlers;
      if (!routes_.TryGetValue(typeof (T), out handlers)) {
        handlers = new List<Action<IMessage>>();
        routes_.Add(typeof (T), handlers);
      }
      handlers.Add(msg => handler.Handle((T) msg));
    }
  }
}
