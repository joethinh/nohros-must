using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Defines the configuration settings for the classes associated with the
  /// <see cref="ITokenPrincipalMapper"/>.
  /// </summary>
  public interface ITokenPrincipalMapperSettings: ISettings
  {
    /// <summary>
    /// Gets the token associated with the "anonymous" user.
    /// </summary>
    string AnonymousToken { get; }
  }
}
