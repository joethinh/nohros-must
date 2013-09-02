using System;
using NUnit.Framework;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Domain
{
  public class AggregateRootTests
  {
    class Aggregate : AggregateRoot
    {
      Guid id_;

      #region .ctor
      public Aggregate(Guid id) {
        id_ = id;
        Subscribe<Created>(Apply);
      }
      #endregion

      void Apply(Created e) {
        id_ = e.ID;
      }

      public override Guid ID {
        get { return id_; }
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

    [Test]
    public void should_replay_events() {
      var aggregate = new Aggregate(Guid.Empty);
      var guid = Guid.NewGuid();
      aggregate.LoadFromHistory(new Event[] {Deserialize(guid)});
      Assert.That(aggregate.ID, Is.EqualTo(guid));
    }

    Event Deserialize(Guid guid) {
      return new Created(guid);
    }
  }
}
