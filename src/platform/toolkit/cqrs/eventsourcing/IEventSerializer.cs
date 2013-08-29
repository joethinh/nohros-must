using System;
using Nohros.CRQS.Messaging;

namespace Nohros.CQRS.EventSourcing
{
  public interface IEventSerializer
  {
    SerializedEvent Serialize(Event @event);
    Event Deserialize(SerializedEvent serialized);
  }
}
