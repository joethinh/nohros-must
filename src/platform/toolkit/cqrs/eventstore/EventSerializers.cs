using System;
using EventStore.ClientAPI;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public static class EventSerializers
  {
    /// <summary>
    /// Creates a <see cref="IEventSerializer"/> that uses the
    /// <paramref name="serializer"/> delegate to serialize a event and the
    /// <paramref name="deserializer"/> delegate to deserialize a event.
    /// </summary>
    /// <param name="serializer">
    /// A <see cref="Func{T, TResult}"/> that is used to serialize a event.
    /// </param>
    /// <param name="deserializer">
    /// A <see cref="Func{T, TResult}"/> that is used to deserialize a event.
    /// </param>
    /// <returns>
    /// A <see cref="IEventSerializer"/> that uses the
    /// <paramref name="serializer"/> delegate to serialize a event and the
    /// <paramref name="deserializer"/> delegate to deserialize a event.
    /// </returns>
    public static IEventSerializer FuncToEventSerializer(
      Func<Event, EventData> serializer, Func<EventData, Event> deserializer) {
      return new FuncEventSerializer(serializer, deserializer);
    }
  }
}
