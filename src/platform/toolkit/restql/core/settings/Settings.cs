using System;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public partial class Settings : MustConfiguration, ISettings
  {
    IQuerySettings query_settings_;
    ITokenPrincipalMapperSettings token_principal_mapper_settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    Settings() { }
    #endregion

    /// <inheritdoc/>
    public IQuerySettings QuerySettings {
      get { return query_settings_; }
    }

    /// <inheritdoc/>
    public ITokenPrincipalMapperSettings TokenPrincipalMapperSettings {
      get { return token_principal_mapper_settings_; }
    }
  }
}
