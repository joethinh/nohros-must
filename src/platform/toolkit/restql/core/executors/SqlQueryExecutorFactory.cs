using System;
using System.Collections.Generic;
using Nohros.Caching;
using Nohros.Configuration;
using Nohros.Data;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Providers;
using Nohros.Resources;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryExecutor : IQueryExecutorFactory
  {
    const string kClassName = "Nohros.Toolkit.RestQL.SqlQueryExecutor";

    /// <summary>
    /// Constructor implied by the <see cref="IQueryExecutorFactory"/>
    /// interface.
    /// </summary>
    protected SqlQueryExecutor() {
    }

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
      ICommonDataProvider common_data_provider = settings.CommonDataProvider;

      // Merges the application configured connection provider with the
      // list of connection providers fetched from the common data provider.
      List<IProviderNode> providers = new List<IProviderNode>(settings.Providers);
      providers.AddRange(common_data_provider.GetConnectionProviders());

      return
        new CacheBuilder<IConnectionProvider>()
          .ExpireAfterAccess(settings.QueryCacheDuration*3, TimeUnit.Seconds)
          .Build(settings.CacheProvider,
            new ConnectionProviderLoader(providers));
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
          RestQLLogger.ForCurrentProcess.Error(
            string.Format(StringResources.Log_MethodThrowsException,
              "GetJsonCollectionFactory", kClassName), exception);
        }
      }
      return new JsonCollectionFactory();
    }
  }
}
