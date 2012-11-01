using System;
using Nohros.Configuration;

namespace Nohros.RestQL
{
  /// <summary>
  /// Contains the configuration data related with query definition.
  /// </summary>
  public interface IQuerySettings : IConfiguration
  {
    /// <summary>
    /// Gets a number that indicates how long a query should remain before it
    /// is last accessed in seconds.
    /// </summary>
    long QueryCacheDuration { get; }
  }
}
