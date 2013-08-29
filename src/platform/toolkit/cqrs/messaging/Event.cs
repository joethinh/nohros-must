using System;

namespace Nohros.CQRS.Messaging
{
  public class Event : Message
  {
    public int Version { get; set; }
  }
}
