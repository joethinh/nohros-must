using System;

namespace Nohros.CRQS.Messaging
{
  public interface IHandle<T> where T : IMessage
  {
    void Handle(T msg);
  }
}
