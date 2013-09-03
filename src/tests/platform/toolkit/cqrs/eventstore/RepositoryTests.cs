using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using NUnit.Framework;
using Nohros.CQRS.Domain;
using Nohros.CQRS.Messaging;
using Telerik.JustMock;

namespace Nohros.CQRS.EventStore
{
  public class RepositoryTests
  {
    class Created : Event
    {
      #region .ctor
      public Created() {
        Version = 0;
      }
      #endregion
    }

    class DummyAggregate : AggregateRoot
    {
      Guid id_;

      #region .ctor
      public DummyAggregate(Guid id) {
        ApplyChange(new Created());
      }
      #endregion

      public override Guid ID {
        get { return id_; }
      }
    }

    class Modified : Event
    {
      #region .ctor
      public Modified() {
        Version = 2;
      }
      #endregion
    }

    IEventSerializer serializer_;

    [SetUp]
    public void SetUp() {
      serializer_ = EventSerializers.FuncToEventSerializer(
        @event =>
          new EventData(Guid.Empty, typeof (Created).FullName, false,
            new byte[0], new byte[0]),
        data => (Event) Activator.CreateInstance(Type.GetType(data.Type)));
    }

    [Test]
    public void should_store_events_if_they_not_conclict() {
      var storage = Mock.Create<IEventStorage>();

      var id = Guid.Empty;
      Mock
        .Arrange(() => storage.GetEventsForAggregate(id, serializer_, 1))
        .Returns(new List<Event> {
          new Modified()
        });

      Mock
        .Arrange(() => storage
          .SaveEvents(Arg.IsAny<Guid>(), Arg.IsAny<ICollection<Event>>(), 1,
            serializer_))
        .Throws(new WrongExpectedVersionException(string.Empty));

      var repository = new Repository<DummyAggregate>(storage, serializer_,
        AggregateFactories.FromFunc(guid => new DummyAggregate(guid)),
        ConflictEvaluators.FromFunc((@event, event1) => false));
      var aggregate = new DummyAggregate(Guid.Empty);

      Assert.That(() => repository.Save(aggregate, 1), Throws.Nothing);
    }

    [Test]
    public void should_throw_concurrency_exception_if_events_conflict() {
      var storage = Mock.Create<IEventStorage>();

      var id = Guid.Empty;
      Mock
        .Arrange(() => storage.GetEventsForAggregate(id, serializer_, 1))
        .Returns(new List<Event> {
          new Modified()
        });

      Mock
        .Arrange(() => storage
          .SaveEvents(Arg.IsAny<Guid>(), Arg.IsAny<ICollection<Event>>(), 1,
            serializer_))
        .Throws(new WrongExpectedVersionException(string.Empty));

      var repository = new Repository<DummyAggregate>(storage, serializer_,
        AggregateFactories.FromFunc(guid => new DummyAggregate(guid)),
        ConflictEvaluators.FromFunc((@event, event1) => true));
      var aggregate = new DummyAggregate(Guid.Empty);

      Assert.That(() => repository.Save(aggregate, 1),
        Throws.TypeOf<ConcurrencyException>());
    }
  }
}
