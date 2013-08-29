using System;

namespace Nohros.CQRS.Messaging
{
  public interface IHandle<T> where T : IMessage
  {
    void Handle(T msg);
  }
}
