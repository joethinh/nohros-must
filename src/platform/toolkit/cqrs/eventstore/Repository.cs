using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI.Exceptions;
using Nohros.CQRS.Domain;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public class Repository<T> : IRepository<T> where T : AggregateRoot
  {
    readonly IAggregateFactory<T> aggregate_factory_;
    readonly IConflictEvaluator conflict_evaluator_;
    readonly IEventSerializer serialzer_;
    readonly IEventStorage storage_;

    #region .ctor
    public Repository(IEventStorage storage, IEventSerializer serializer,
      IAggregateFactory<T> aggregate_factory,
      IConflictEvaluator conflict_evaluator) {
      storage_ = storage;
      serialzer_ = serializer;
      aggregate_factory_ = aggregate_factory;
      conflict_evaluator_ = conflict_evaluator;
    }
    #endregion

    public void Save(AggregateRoot aggregate, int expected_version) {
      var events = aggregate.GetUncommittedChanges().ToArray();

      int current_expected_version = expected_version;
      while (true) {
        try {
          Save(aggregate.ID, events, current_expected_version);
          break;
        } catch (WrongExpectedVersionException e) {
          IList<Event> events_since =
            storage_.GetEventsForAggregate(aggregate.ID, serialzer_,
              expected_version);

          foreach (Event commited_event in events_since) {
            foreach (Event @event in events) {
              if (conflict_evaluator_.ConflictWith(commited_event, @event)) {
                throw new ConcurrencyException();
              }
            }
          }
          current_expected_version = events_since.Last().Version;
        }
      }
    }

    public T GetByID(Guid id) {
      T aggregate = aggregate_factory_.CreateAggregate(id);
      ICollection<Event> events = storage_.GetEventsForAggregate(id, serialzer_);
      aggregate.LoadFromHistory(events);
      return aggregate;
    }

    void Save(Guid id, ICollection<Event> events, int expected_version) {
      storage_.SaveEvents(id, events, expected_version, serialzer_);
    }
  }
}
