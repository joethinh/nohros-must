using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A implementation of the <see cref="IAuthenticationInfo"/> that
  /// represents a failed authentication attempt.
  /// </summary>
  public class FailedAuthenticationInfo : IAuthenticationInfo
  {
    /// <inheritdoc/>
    public bool Authenticated {
      get { return false; }
    }
  }
}
