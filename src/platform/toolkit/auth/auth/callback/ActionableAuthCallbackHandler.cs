using System;

namespace Nohros.Security.Auth
{
  internal class ActionableAuthCallbackHandler : IAuthCallbackHandler
  {
    readonly Action<IAuthCallback[]> handler_;

    #region .ctor
    internal ActionableAuthCallbackHandler(Action<IAuthCallback[]> handler) {
      handler_ = handler;
    }
    #endregion

    public void Handle(IAuthCallback[] callback) {
      handler_(callback);
    }
  }
}
