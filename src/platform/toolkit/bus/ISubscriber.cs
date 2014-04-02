using System;

namespace Nohros.Bus
{
  public interface ISubscriber
  {
    void Subscribe<T>(IHandle<T> handler) where T: IMessage;
    void Unsubscribe<T>(IHandle<T> handler) where T : IMessage;
  }
}
