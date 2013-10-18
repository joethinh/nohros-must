using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A factory for thr <see cref="IAuthenticationInfo"/>.
  /// </summary>
  public class AuthenticationInfos
  {
    /// <summary>
    /// Creates a <see cref="IAuthenticationInfo"/> that represents a sucessful
    /// authentication.
    /// </summary>
    /// <returns>
    /// A <see cref="IAuthenticationInfo"/> that represents a sucessful
    /// authentication.
    /// </returns>
    public static IAuthenticationInfo Sucessful() {
      return new AuthenticationInfo(true);
    }

    /// <summary>
    /// Creates a <see cref="IAuthenticationInfo"/> that represents a failed
    /// authentication.
    /// </summary>
    /// <returns>
    /// A <see cref="IAuthenticationInfo"/> that represents a failed
    /// authentication.
    /// </returns>
    public static IAuthenticationInfo Failed() {
      return new AuthenticationInfo(false);
    }
  }
}
