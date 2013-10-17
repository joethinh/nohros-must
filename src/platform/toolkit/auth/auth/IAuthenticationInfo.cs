using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Provides a way for <see cref="ILoginModule"/> to store transient
  /// authentication status.
  /// </summary>
  public interface IAuthenticationInfo
  {
    /// <summary>
    /// Gets a value indicating if the associated authentication operation
    /// has succeeded.
    /// </summary>
    /// <value>
    /// <c>true</c> if the authentication operation has succeeded; otherwise,
    /// <c>false</c>.
    /// </value>
    bool Authenticated { get; }
  }
}
