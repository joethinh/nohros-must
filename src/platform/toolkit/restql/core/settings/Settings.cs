using System;
using Nohros.Caching.Providers;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public partial class Settings : MustConfiguration, ISettings
  {
    ICacheProvider cache_provider_;
    ICommonDataProvider common_data_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    protected Settings() {
    }
    #endregion

    #region ISettings Members
    /// <inheritdoc/>
    public ICacheProvider CacheProvider {
      get { return cache_provider_; }
    }

    /// <inheritdoc/>
    public ICommonDataProvider CommonDataProvider {
      get { return common_data_provider_; }
    }
    #endregion
  }
}
