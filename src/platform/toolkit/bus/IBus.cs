using System;

namespace Nohros.Bus
{
  public interface IBus : IPublisher, ISubscriber
  {
    void Send<T>() where T : ICommand;
  }
}
