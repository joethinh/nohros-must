using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// The most basic implementation of the <see cref="IAuthenticationInfo"/>.
  /// </summary>
  internal class BasicAuthenticationInfo : IAuthenticationInfo
  {
    #region .ctor
    public BasicAuthenticationInfo(bool authenticated) {
      Authenticated = authenticated;
    }
    #endregion

    /// <inheritdoc/>
    public bool Authenticated { get; private set; }
  }
}
