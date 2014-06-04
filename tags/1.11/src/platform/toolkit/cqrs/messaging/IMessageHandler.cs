using System;

namespace Nohros.CQRS.Messaging
{
  internal interface IMessageHandler
  {
    bool TryHandle(IMessage msg);
    bool IsSame<T>(object handler);
  }
}
