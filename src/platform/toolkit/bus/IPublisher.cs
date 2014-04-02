using System;

namespace Nohros.Bus
{
  public interface IPublisher
  {
    void Publish<T>(T @event) where T : IEvent;
  }
}
