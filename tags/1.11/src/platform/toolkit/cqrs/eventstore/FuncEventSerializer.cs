using System;
using EventStore.ClientAPI;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  internal class FuncEventSerializer : IEventSerializer
  {
    readonly Func<EventData, Event> deserializer_;
    readonly Func<Event, EventData> serializer_;

    #region .ctor
    public FuncEventSerializer(Func<Event, EventData> serializer,
      Func<EventData, Event> deserializer) {
      deserializer_ = deserializer;
      serializer_ = serializer;
    }
    #endregion

    public EventData Serialize(Event @event) {
      return serializer_(@event);
    }

    public Event Deserialize(EventData serialized_event) {
      return deserializer_(serialized_event);
    }
  }
}
