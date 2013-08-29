using System;
using System.Collections.Generic;
using Nohros.CRQS.Messaging;

namespace Nohros.CQRS.Domain
{
  public abstract class AggregateRoot
  {
    readonly List<Event> changes_;
    readonly IDictionary<Type, Action<Event>> handlers_;

    #region .ctor
    protected AggregateRoot() {
      changes_ = new List<Event>();
      handlers_ = new Dictionary<Type, Action<Event>>();
    }
    #endregion

    public IEnumerable<Event> GetUncommittedChanges() {
      return changes_;
    }

    public void MarkChangesAsCommited() {
      changes_.Clear();
    }

    public void LoadFromHistory(IEnumerable<Event> events) {
      foreach (Event @event in events) {
        ApplyChange(@event, false);
      }
    }

    protected void Subscribe<T>(Action<T> handler) where T : Event {
      handlers_.Add(typeof (T), @event => handler(@event as T));
    }

    protected void ApplyChange(Event @event) {
      ApplyChange(@event, true);
    }

    void ApplyChange(Event @event, bool is_new) {
      Action<Event> action;
      if (handlers_.TryGetValue(@event.GetType(), out action)) {
        action(@event);
      }
      if (is_new) {
        changes_.Add(@event);
      }
    }

    public abstract Guid ID { get; }
    public int Version { get; internal set; }
  }
}
