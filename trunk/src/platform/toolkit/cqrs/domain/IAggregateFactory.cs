using System;

namespace Nohros.CQRS.Domain
{
  public interface IAggregateFactory<T> where T : AggregateRoot
  {
    T CreateAggregate(Guid id);
  }
}
