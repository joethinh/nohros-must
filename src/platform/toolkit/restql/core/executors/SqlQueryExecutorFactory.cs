using System;
using System.Collections.Generic;
using Nohros.Caching;
using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Data.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryExecutor : IQueryExecutorFactory
  {
    #region IQueryExecutorFactory Members
    /// <summary>
    /// Creates a instance of the <see cref="IQueryExecutor"/> class by using
    /// the specified application settings.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the specific
    /// options configured for the query processor.
    /// </param>
    /// <param name="settings">
    /// A <see cref="IQuerySettings"/> containing configuration data for the
    /// query processor.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="IQueryExecutor"/> class.
    /// </returns>
    public IQueryExecutor CreateQueryExecutor(
      IDictionary<string, string> options, IQuerySettings settings) {
      IProviderNode provider = settings.Providers[Strings.kCacheProviderName];
      CacheBuilder<IConnectionProvider> builder =
        new CacheBuilder<IConnectionProvider>()
          .ExpireAfterAccess(settings.QueryCacheDuration*3, TimeUnit.Seconds)
          .Build();
    }
    #endregion
  }
}
