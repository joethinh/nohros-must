using System;
using System.Linq;
using NUnit.Framework;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Domain
{
  public class AggregateRootTests
  {
    class Aggregate : AggregateRoot
    {
      AggregateEntity entity_;
      Guid id_;

      #region .ctor
      public Aggregate() {
        Subscribe<Created>(Apply);
      }

      public Aggregate(Guid id):this() {
        ApplyChange(new Created(id));
      }
      #endregion

      public void GenerateEntityEvent(Event e) {
        entity_.GenerateEntityEvent(e);
      }

      void Apply(Created e) {
        entity_ = new AggregateEntity();

        AddEntity(entity_);
        id_ = e.ID;
      }

      public override Guid ID {
        get { return id_; }
      }
    }

    class AggregateEntity : Entity
    {
      public void GenerateEntityEvent(Event e) {
        ApplyChange(e);
      }
    }

    class Created : Event
    {
      public readonly Guid ID;

      #region .ctor
      public Created(Guid id) {
        ID = id;
      }
      #endregion
    }

    class EntityCreated : Event
    {
      public readonly Guid ID;

      #region .ctor
      public EntityCreated(Guid id) {
        ID = id;
      }
      #endregion
    }

    [Test]
    public void should_replay_events() {
      var aggregate = new Aggregate();
      var guid = Guid.NewGuid();
      aggregate.LoadFromHistory(new Event[] {Deserialize(guid)});
      Assert.That(aggregate.ID, Is.EqualTo(guid));
    }

    [Test]
    public void should_publish_events_from_internal_entities() {
      var aggregate = new Aggregate(Guid.Empty);
      var guid = Guid.NewGuid();
      aggregate.GenerateEntityEvent(new EntityCreated(guid));
      var events = aggregate.GetUncommittedChanges();
      Assert.That(
        events.Any(@event => @event.GetType() == typeof (EntityCreated)));
    }

    [Test]
    public void should_publish_events_from_internal_entities_and_root() {
      var guid = Guid.NewGuid();
      var aggregate = new Aggregate(guid);
      aggregate.GenerateEntityEvent(new EntityCreated(guid));
      var events = aggregate.GetUncommittedChanges();
      Assert.That(
        events.Any(@event => @event.GetType() == typeof (EntityCreated)));
      Assert.That(
        events.Any(@event => @event.GetType() == typeof (Created)));
    }

    Event Deserialize(Guid guid) {
      return new Created(guid);
    }
  }
}
