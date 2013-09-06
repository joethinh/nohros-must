using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using Nohros.CQRS.Messaging;
using System.Linq;

namespace Nohros.CQRS.EventStore
{
  public class EventStorage : IEventStorage
  {
    const int kWritePageSize = 500;
    const int kReadPageSize = 500;
    readonly IEventStoreConnection connection_;
    readonly IPublisher publisher_;

    #region .ctor
    public EventStorage(IEventStoreConnection connection, IPublisher publisher) {
      connection_ = connection;
      publisher_ = publisher;
    }
    #endregion

    public void SaveEvents(Guid aggregate_id, ICollection<Event> events,
      int expected_version, IEventSerializer serializer) {
      string stream_name = StreamNameForID(aggregate_id);
      if (events.Count < kWritePageSize) {
        SaveEventsAtOnce(stream_name, expected_version, events, serializer);
      } else {
        SaveEventsInBatch(stream_name, expected_version, events, serializer);
      }

      foreach (Event @event in events) {
        publisher_.Publish(@event);
      }
    }

    public IList<Event> GetEventsForAggregate(Guid aggregate_id,
      IEventSerializer serializer, int version) {
      var events = new List<Event>();
      var stream_name = StreamNameForID(aggregate_id);
      int position = 0;
      StreamEventsSlice slice;
      do {
        var count = position + kReadPageSize <= version
          ? kReadPageSize
          : version - position + 1;
        slice = connection_.ReadStreamEventsForward(stream_name, position,
          count, false);
        position = slice.NextEventNumber;
        var serialized_events =
          slice.Events.Select(@event => Deserialize(@event, serializer));
        events.AddRange(serialized_events);
      } while (version > slice.NextEventNumber && !slice.IsEndOfStream);
      return events;
    }

    public IList<Event> GetEventsForAggregateSince(Guid aggregate_id,
      IEventSerializer serializer, int version) {
      var events = new List<Event>();
      var stream_name = StreamNameForID(aggregate_id);
      int position = version;
      StreamEventsSlice slice;
      do {
        slice = connection_.ReadStreamEventsForward(stream_name, position,
          kReadPageSize, false);
        position = slice.NextEventNumber;
        var serialized_events =
          slice.Events.Select(@event => Deserialize(@event, serializer));
        events.AddRange(serialized_events);
      } while (!slice.IsEndOfStream);
      return events;
    }

    public IList<Event> GetEventsForAggregate(Guid aggregate_id,
      IEventSerializer serializer) {
      return GetEventsForAggregate(aggregate_id, serializer, int.MaxValue);
    }

    string StreamNameForID(Guid id) {
      return id.ToString("N");
    }

    void SaveEventsAtOnce(string stream_name, int expected_version,
      IEnumerable<Event> events, IEventSerializer serializer) {
      connection_.AppendToStream(stream_name, expected_version,
        Serialize(events, serializer));
    }

    void SaveEventsInBatch(string stream_name, int expected_version,
      IEnumerable<Event> events, IEventSerializer serializer) {
      EventStoreTransaction trasaction = connection_
        .StartTransaction(stream_name, expected_version);
      EventData[] serialized_events = Serialize(events, serializer).ToArray();

      int position = 0;
      while (position < serialized_events.Length) {
        var page_events = serialized_events
          .Skip(position)
          .Take(kWritePageSize);
        trasaction.Write(page_events);
        position += kWritePageSize;
      }
      trasaction.Commit();
    }

    Event Deserialize(ResolvedEvent @event, IEventSerializer serializer) {
      var e = serializer.Deserialize(
        new EventData(
          @event.OriginalEvent.EventId, @event.OriginalEvent.EventType,
          false, @event.OriginalEvent.Data,
          @event.OriginalEvent.Metadata));
      e.Version = @event.Event.EventNumber;
      return e;
    }

    IEnumerable<EventData> Serialize(IEnumerable<Event> events,
      IEventSerializer serializer) {
      return events.Select(e => {
        var event_id = Guid.NewGuid();
        var type = e.GetType().Name;
        var serialized = serializer.Serialize(e);
        return new EventData(event_id, type, false, serialized.Data,
          serialized.Metadata);
      });
    }
  }
}
