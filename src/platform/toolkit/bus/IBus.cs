using System;

namespace Nohros.Bus
{
  public interface IBus : IPublisher
  {
    void Send<T>(T msg);
  }
}
