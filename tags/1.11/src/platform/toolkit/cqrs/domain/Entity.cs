using System;
using System.Collections.Generic;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Domain
{
  public abstract class Entity
  {
    readonly List<Event> changes_;
    readonly IDictionary<Type, Action<Event>> handlers_;

    #region .ctor
    protected Entity() {
      changes_ = new List<Event>();
      handlers_ = new Dictionary<Type, Action<Event>>();
    }
    #endregion

    public virtual IEnumerable<Event> GetUncommittedChanges() {
      return changes_;
    }

    public virtual void MarkChangesAsCommited() {
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
  }
}
