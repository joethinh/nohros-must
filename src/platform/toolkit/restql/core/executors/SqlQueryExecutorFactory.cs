using System;
using System.Collections.Generic;

using Nohros.Caching;
using Nohros.Configuration;
using Nohros.Data;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryExecutor : IQueryExecutorFactory
  {
    const string kTypeForLogger =
      "[Nohros.Toolkit.RestQL.GetJsonCollectionFactory]";

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
    /// A <see cref="IQuerySettings"/> containing the configuration data for
    /// the query processor.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="IQueryExecutor"/> class.
    /// </returns>
    public IQueryExecutor CreateQueryExecutor(
      IDictionary<string, string> options, IQuerySettings settings) {
      ILoadingCache<IConnectionProvider> connection_provider_cache =
        GetConnectionProviderCache(settings);
      IJsonCollectionFactory json_collection_factory =
        GetJsonCollectionFactory(settings);
      return new SqlQueryExecutor(connection_provider_cache,
        json_collection_factory);
    }
    #endregion

    ILoadingCache<IConnectionProvider> GetConnectionProviderCache(
      IQuerySettings settings) {
      return
        new CacheBuilder<IConnectionProvider>()
          .ExpireAfterAccess(settings.QueryCacheDuration*3, TimeUnit.Seconds)
          .Build(settings.CacheProvider,
            new ConnectionProviderLoader(settings.Providers));
    }

    IJsonCollectionFactory GetJsonCollectionFactory(IQuerySettings settings) {
      IProviderNode provider;
      if (settings.Providers.GetProviderNode(Strings.kJsonCollectionProvider,
        out provider)) {
        try {
          return ProviderFactory<IJsonCollectionFactory>
            .CreateProviderFactoryFallback(provider, settings);
        } catch (Exception exception) {
          // log it and ignore
          RestQLLogger.ForCurrentProcess.Error(kTypeForLogger, exception);
        }
      }
      return new JsonCollectionFactory();
    }
  }
}
