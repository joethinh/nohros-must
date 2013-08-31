using System;
using System.Collections.Generic;
using System.Linq;
using Nohros.CQRS.Domain;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public class Repository<T> : IRepository<T> where T : AggregateRoot
  {
    readonly Func<Guid, T> aggregate_factory_;
    readonly IEventSerializer serialzer_;
    readonly EventStorage storage_;

    #region .ctor
    public Repository(EventStorage storage, IEventSerializer serializer,
      Func<Guid, T> aggregate_factory) {
      storage_ = storage;
      serialzer_ = serializer;
      aggregate_factory_ = aggregate_factory;
    }
    #endregion

    public void Save(AggregateRoot aggregate, int expected_version) {
      var events = aggregate.GetUncommittedChanges().ToArray();
      storage_.SaveEvents(aggregate.ID, events, expected_version, serialzer_);
    }

    public T GetByID(Guid id) {
      T aggregate = aggregate_factory_(id);
      ICollection<Event> events = storage_.GetEventsForAggregate(id, serialzer_);
      aggregate.LoadFromHistory(events);
      return aggregate;
    }
  }
}
