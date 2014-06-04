using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI.Exceptions;
using Nohros.CQRS.Domain;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public partial class Repository<T> : IRepository<T> where T : AggregateRoot
  {
    readonly IAggregateFactory<T> aggregate_factory_;
    readonly IConflictEvaluator conflict_evaluator_;
    readonly IEventSerializer serialzer_;
    readonly IEventStorage storage_;

    #region .ctor
    protected internal Repository(Builder builder) {
      storage_ = builder.EventStorage;
      serialzer_ = builder.EventSerializer;
      aggregate_factory_ = builder.AggregateFactory;
      conflict_evaluator_ = builder.ConflictEvaluator;
    }
    #endregion

    public virtual void Save(T aggregate, int expected_version) {
      var events = aggregate.GetUncommittedChanges().ToArray();

      int current_expected_version = expected_version;
      while (true) {
        try {
          Save(aggregate.ID, events, current_expected_version);
          break;
        } catch (AggregateException e) {
          if (e.InnerException == null ||
            !(e.InnerException is WrongExpectedVersionException)) {
            throw;
          }

          IList<Event> events_since =
            storage_.GetEventsForAggregateSince(aggregate.ID, serialzer_,
              expected_version);

          if (events_since.Count == 0) {
            throw new AggregateException("The current version is earlier than "
              + " the expected.");
          }

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

    public virtual T GetByID(Guid id) {
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
