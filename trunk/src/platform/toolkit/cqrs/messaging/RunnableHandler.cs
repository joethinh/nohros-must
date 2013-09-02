using System;

namespace Nohros.CQRS.Messaging
{
  internal class RunnableHandler<T> : IHandle<T> where T : IMessage
  {
    readonly Action<T> handler_;

    #region .ctor
    public RunnableHandler(Action<T> handler) {
      handler_ = handler;
    }
    #endregion

    /// <inheritdoc/>
    public void Handle(T msg) {
      handler_(msg);
    }
  }
}
