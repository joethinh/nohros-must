using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Security.Auth;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Perform mapping between a token and a principal.
  /// </summary>
  /// <remarks>
  /// A principal is a representation of any entity such as an individual, a
  /// corporation, a login id and a group name.
  /// </remarks>
  public interface ITokenPrincipalMapper
  {
    /// <summary>
    /// Map a token to a principal.
    /// </summary>
    /// <param name="token">The token to map to a <see cref="IPrincipal"/>
    /// </param>
    /// <returns>If the token could be mapped, returns the mapped principal;
    /// otherwise, returns a string that maps to the anonymous user.
    /// <para>The string that maps to the anonymous user is defined on
    /// the restql application settings.</para>
    /// <para>When the specified token is a null reference or a empty string it
    /// should be mapped to the "anonymous" principal.</para>
    /// </returns>
    string MapTokenToPrincipal(string token);
  }
}
