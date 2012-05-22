using System;

using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Contains the configuration data related with query definition.
  /// </summary>
  public interface IQuerySettings
  {
    /// <summary>
    /// Gets an array containing the configuration for all the query executors
    /// configured for the application.
    /// </summary>
    IProviderNode[] Executors { get; }

    /// <summary>
    /// Gets a number that indicates how long a query should remain before it
    /// is last accessed.
    /// </summary>
    long QueryCacheDuration { get; }
  }
}
