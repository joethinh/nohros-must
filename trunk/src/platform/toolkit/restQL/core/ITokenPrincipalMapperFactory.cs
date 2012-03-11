using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Configuration;
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
    /// specified mapper options.
    /// </summary>
    /// <returns>A <see cref="ITokenPrincipalMapper"/> object.</returns>
    ITokenPrincipalMapper CreateTokenPrincipalMapper(
      IDictionary<string, string> options);
  }
}
