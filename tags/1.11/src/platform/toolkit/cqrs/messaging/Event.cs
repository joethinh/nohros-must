using System;

namespace Nohros.CQRS.Messaging
{
  public class Event : Message
  {
    /// <summary>
    /// Gets the version in which the aggregate was when the event was
    /// generated.
    /// </summary>
    public int Version { get; set; }
  }
}
