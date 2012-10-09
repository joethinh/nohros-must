using System;
using Nohros.Caching.Providers;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Defines the common application settings.
  /// </summary>
  public interface ISettings : IConfiguration
  {
    /// <summary>
    /// Gets the application cache provider.
    /// </summary>
    ICacheProvider CacheProvider { get; }

    /// <summary>
    /// Gets the application common data provider.
    /// </summary>
    IQueryDataProvider QueryDataProvider { get; }
  }
}
