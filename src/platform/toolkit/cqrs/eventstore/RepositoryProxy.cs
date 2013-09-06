using System;
using Nohros.Caching;
using Nohros.CQRS.Domain;

namespace Nohros.CQRS.EventStore
{
  internal class RepositoryProxy<T> : Repository<T> where T : AggregateRoot
  {
    readonly ICache<T> cache_;

    #region .ctor
    protected internal RepositoryProxy(Builder builder) : base(builder) {
      cache_ = builder.Cache;
    }
    #endregion

    public override void Save(T aggregate, int expected_version) {
      base.Save(aggregate, expected_version);
      cache_.Put(Key(aggregate.ID), aggregate);
    }

    public override T GetByID(Guid id) {
      return cache_.Get(Key(id),
        CacheLoader<T>.From(key => base.GetByID(id)));
    }

    string Key(Guid id) {
      return id.ToString("N");
    }
  }
}
