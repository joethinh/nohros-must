using System;
using System.Linq;
using System.Collections.Generic;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Domain
{
  public abstract class AggregateRoot : Entity
  {
    readonly List<Entity> entities_;

    #region .ctor
    protected AggregateRoot() {
      entities_ = new List<Entity>();
    }
    #endregion

    protected void AddEntity(Entity entity) {
      entities_.Add(entity);
    }

    public override IEnumerable<Event> GetUncommittedChanges() {
      return base.GetUncommittedChanges()
                 .Concat(GetEntityUncommittedChanges());
    }

    IEnumerable<Event> GetEntityUncommittedChanges() {
      return entities_.SelectMany(entity => entity.GetUncommittedChanges());
    }

    public override void MarkChangesAsCommited() {
      entities_.ForEach(entity => entity.MarkChangesAsCommited());
      base.MarkChangesAsCommited();
    }

    public abstract Guid ID { get; }
    public int Version { get; internal set; }
  }
}
