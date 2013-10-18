using System;

namespace Nohros.Security.Auth
{
  public class AuthenticationToken : IAuthenticationInfo
  {
    #region .ctor
    public AuthenticationToken(string token, bool authenticated) {
      Token = token;
      Authenticated = authenticated;
    }
    #endregion

    public bool Authenticated { get; private set; }
    public string Token { get; private set; }
  }
}
