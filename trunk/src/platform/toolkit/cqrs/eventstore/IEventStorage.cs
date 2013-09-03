using System;
using System.Collections.Generic;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public interface IEventStorage
  {
    IList<Event> GetEventsForAggregate(Guid aggregate_id,
      IEventSerializer serializer);

    void SaveEvents(Guid aggregate_id, ICollection<Event> events,
      int expected_version, IEventSerializer serializer);

    IList<Event> GetEventsForAggregate(Guid aggregate_id,
      IEventSerializer serializer, int version);
  }
}
