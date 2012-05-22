using System;
using Nohros.Caching.Providers;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Defines the common application settings.
  /// </summary>
  public interface ISettings : IMustConfiguration
  {
    /// <summary>
    /// Gets a <see cref="IQuerySettings"/> object containing the query
    /// settings.
    /// </summary>
    //IQuerySettings QuerySettings { get; }

    /// <summary>
    /// Gets a <see cref="ITokenPrincipalMapperSettings"/> object containing
    /// the token mapper settings.
    /// </summary>
    //ITokenPrincipalMapperSettings TokenPrincipalMapperSettings { get; }

    /// <summary>
    /// Gets the application cache provider.
    /// </summary>
    ICacheProvider CacheProvider { get; }

    /// <summary>
    /// Gets the application common data provider.
    /// </summary>
    ICommonDataProvider CommonDataProvider { get; }
  }
}
