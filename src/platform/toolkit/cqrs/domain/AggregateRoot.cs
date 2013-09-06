using System;
using System.Linq;
using System.Collections.Generic;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Domain
{
  public abstract class AggregateRoot : Entity
  {
    readonly List<Event> changes_;
    readonly List<Entity> entities_;

    #region .ctor
    protected AggregateRoot() {
      changes_ = new List<Event>();
      entities_ = new List<Entity>();
    }
    #endregion

    protected void AddEntity(Entity entity) {
      entities_.Add(entity);
    }

    public override IEnumerable<Event> GetUncommittedChanges() {
      return changes_.Concat(GetEntityUncommittedChanges());
    }

    IEnumerable<Event> GetEntityUncommittedChanges() {
      return entities_.SelectMany(entity => entity.GetUncommittedChanges());
    }

    public override void MarkChangesAsCommited() {
      entities_.ForEach(entity => entity.MarkChangesAsCommited());
      changes_.Clear();
    }

    public abstract Guid ID { get; }
    public int Version { get; internal set; }
  }
}
