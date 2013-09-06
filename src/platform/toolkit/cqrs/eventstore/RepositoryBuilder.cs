using System;
using Nohros.CQRS.Domain;
using Nohros.CQRS.Messaging;
using Nohros.Caching;

namespace Nohros.CQRS.EventStore
{
  public partial class Repository<T>
  {
    public class Builder
    {
      ICache<T> cache_;
      IConflictEvaluator conflict_evaluator_;
      IAggregateFactory<T> factory_;
      IEventSerializer serializer_;
      EventStorage storage_;
      TimeSpan ttl_;

      #region .ctor
      public Builder() {
        conflict_evaluator_ = ConflictEvaluators.NoConflict();
        ttl_ = TimeSpan.MaxValue;
      }
      #endregion

      public Builder WithCache(ICache<T> cache) {
        cache_ = cache;
        return this;
      }

      public Builder WithEventStorage(EventStorage storage) {
        storage_ = storage;
        return this;
      }

      public Builder WithSerializer(IEventSerializer serializer) {
        serializer_ = serializer;
        return this;
      }

      public Builder WithConflictEvaluator(IConflictEvaluator conflict_evaluator) {
        if (conflict_evaluator_ == null) {
          throw new ArgumentNullException("conflict_evaluator");
        }
        conflict_evaluator_ = conflict_evaluator;
        return this;
      }

      public Builder WithAggregateFactory(IAggregateFactory<T> factory) {
        factory_ = factory;
        return this;
      }

      public Builder ExpiresAggregateAfter(TimeSpan ttl) {
        ttl_ = ttl;
        return this;
      }

      public Repository<T> Build() {
        if (storage_ == null) {
          throw new InvalidOperationException("The EventStorage is not defined");
        }

        if (factory_ == null) {
          throw new InvalidOperationException(
            "The AggregateFactory is not defined");
        }

        if (serializer_ == null) {
          throw new InvalidOperationException(
            "The AggregateFactory is not defined");
        }

        return (cache_ != null)
          ? new RepositoryProxy<T>(this)
          : new Repository<T>(this);
      }

      public EventStorage EventStorage {
        get { return storage_; }
      }

      public IEventSerializer EventSerializer {
        get { return serializer_; }
      }

      public IConflictEvaluator ConflictEvaluator {
        get { return conflict_evaluator_; }
      }

      public ICache<T> Cache {
        get { return cache_; }
      }

      public IAggregateFactory<T> AggregateFactory {
        get { return factory_; }
      }

      public TimeSpan CacheTTL {
        get { return ttl_; }
      }
    }
  }
}
