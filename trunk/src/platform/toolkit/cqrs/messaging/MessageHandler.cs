using System;

namespace Nohros.CQRS.Messaging
{
  internal class MessageHandler<T> : IMessageHandler where T : IMessage
  {
    readonly IHandle<T> handler_;

    #region .ctor
    public MessageHandler(IHandle<T> handler) {
      handler_ = handler;
    }
    #endregion

    public bool TryHandle(IMessage msg) {
      try {
        handler_.Handle((T) msg);
        return true;
      } catch (InvalidCastException) {
        return false;
      }
    }

    public bool IsSame<T2>(object handler) {
      return ReferenceEquals(handler_, handler) && typeof (T) == typeof (T2);
    }
  }
}
