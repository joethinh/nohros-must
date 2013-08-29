using System;

namespace Nohros.CRQS.Messaging
{
  public interface ISubscriber
  {
    void Subscribe<T>(IHandle<T> handler) where T : Message;
  }
}
