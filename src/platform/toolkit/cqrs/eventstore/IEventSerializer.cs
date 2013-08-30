using System;
using EventStore.ClientAPI;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  public interface IEventSerializer
  {
    /// <summary>
    /// Serializes the <paramref name="event"/> into a <see cref="EventData"/>
    /// structure.
    /// </summary>
    /// <param name="event">
    /// The event to be serialized.
    /// </param>
    /// <returns>
    /// A <see cref="EventData"/> object containing the the serialized version
    /// of the <see cref="Event"/>.
    /// </returns>
    EventData Serialize(Event @event);

    /// <summary>
    /// Deserializes the <paramref name="@event"/> from the given
    /// <see cref="EventData"/> structure.
    /// </summary>
    /// <param name="event">
    /// A <see cref="EventData"/> that contains the serialized version of the
    /// event to be deserialized.
    /// </param>
    /// <returns>
    /// </returns>
    Event Deserialize(EventData @event);
  }
}
