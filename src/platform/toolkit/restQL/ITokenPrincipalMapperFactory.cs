using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// <para>
  /// This interface implies a constructor with no parameters.
  /// </para>
  /// </summary>
  public interface ITokenPrincipalMapperFactory
  {
    /// <summary>
    /// Creates a <see cref="ITokenPrincipalMapper"/> object using the
    /// specified settings object.
    /// </summary>
    /// <returns>A <see cref="ITokenPrincipalMapper"/> object.</returns>
    /// <exception cref=""
    ITokenPrincipalMapper CreateTokenPrincipalMapper(RestQLSettings settings);
  }
}
