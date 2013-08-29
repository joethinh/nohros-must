using System;
using System.Collections.Generic;
using Nohros.CRQS.Messaging;

namespace Nohros.CQRS.EventSourcing
{
  public interface IEventStore
  {
    /// <summary>
    /// Save the events associated with the given aggregate id ot the event
    /// store.
    /// </summary>
    /// <param name="aggregate_id">
    /// The ID of the aggregate that is associated with the events.
    /// </param>
    /// <param name="events">
    /// The events to be saved.
    /// </param>
    /// <param name="serializer">
    /// A <see cref="IEventSerializer"/> object that can be used to serialize
    /// the event from to an array of bytes.
    /// </param>
    /// <param name="expected_version">
    /// The expected version of the last event stored in the event store.
    /// </param>
    /// <remarks>
    /// </remarks>
    void SaveEvents(Guid aggregate_id, ICollection<Event> events,
      int expected_version, IEventSerializer serializer);

    /// <summary>
    /// Gets a <see cref="IList{T}"/> containing all the events associated with
    /// the given aggregate id.
    /// </summary>
    /// <param name="aggregate_id">
    /// The ID of the aggregate to get the events.
    /// </param>
    /// <param name="serializer">
    /// A <see cref="IEventSerializer"/> object that can be used to deserialize
    /// the event from an array of bytes.
    /// </param>
    /// <returns>
    /// A <see cref="IList{T}"/> containing the events associated with the
    /// given aggregate id.
    /// </returns>
    IList<Event> GetEventsForAggregate(Guid aggregate_id,
      IEventSerializer serializer);
  }
}
