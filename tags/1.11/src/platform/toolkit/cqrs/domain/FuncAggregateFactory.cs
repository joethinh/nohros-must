using System;

namespace Nohros.CQRS.Domain
{
  internal class FuncAggregateFactory<T> : IAggregateFactory<T>
    where T : AggregateRoot
  {
    readonly Func<Guid, T> func_;

    #region .ctor
    public FuncAggregateFactory(Func<Guid, T> func) {
      func_ = func;
    }
    #endregion

    public T CreateAggregate(Guid id) {
      return func_(id);
    }
  }
}
