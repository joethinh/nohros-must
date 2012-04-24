using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Defines the common application settings.
  /// </summary>
  public interface ISettings
  {
    /// <summary>
    /// Gets a <see cref="IQuerySettings"/> object containing the query
    /// settings.
    /// </summary>
    IQuerySettings QuerySettings { get; }

    /// <summary>
    /// Gets a <see cref="ITokenPrincipalMapperSettings"/> object containing
    /// the token mapper settings.
    /// </summary>
    ITokenPrincipalMapperSettings TokenPrincipalMapperSettings { get; }
  }
}
