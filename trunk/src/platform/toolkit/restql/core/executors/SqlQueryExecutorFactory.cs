using System;
using System.Collections.Generic;
using Nohros.Caching;
using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Data;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryExecutor : IQueryExecutorFactory
  {
    const string kClassName = "Nohros.Toolkit.RestQL.SqlQueryExecutor";
    readonly IQuerySettings settings_;

    #region .ctor
    public SqlQueryExecutor(IQuerySettings settings) {
      settings_ = settings;
    }
    #endregion

    /// <summary>
    /// Creates a instance of the <see cref="IQueryExecutor"/> class by using
    /// the specified application settings.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the specific
    /// options configured for the query processor.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="IQueryExecutor"/> class.
    /// </returns>
    public IQueryExecutor CreateQueryExecutor(
      IDictionary<string, string> options) {
      IQueryDataProvider query_data_provider = GetQueryDataProvider();
      ICacheProvider cache_provider = GetCacheProvider();
      ILoadingCache<IConnectionProvider> connection_provider_cache =
        GetConnectionProviderCache(query_data_provider, cache_provider);
      IJsonCollectionFactory json_collection_factory =
        GetJsonCollectionFactory();
      return new SqlQueryExecutor(connection_provider_cache,
        json_collection_factory);
    }

    IQueryDataProvider GetQueryDataProvider() {
      IProviderNode provider =
        settings_.Providers.GetProviderNode(Strings.kQueryDataProviderName);
      return RuntimeTypeFactory<IQueryDataProviderFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateCommonDataProvider(provider.Options.ToDictionary());
    }

    ICacheProvider GetCacheProvider() {
      IProviderNode provider =
        settings_.Providers.GetProviderNode(Strings.kCacheProviderName);
      return RuntimeTypeFactory<ICacheProviderFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateCacheProvider(provider.Options.ToDictionary());
    }

    ILoadingCache<IConnectionProvider> GetConnectionProviderCache(
      IQueryDataProvider query_data_provider, ICacheProvider cache_provider) {
      // Merges the application configured connection provider with the
      // list of connection providers fetched from the common data provider.
      var providers = new List<IProviderNode>(
        settings_.Providers[Strings.kQueryExecutorsGroup]);
      providers.AddRange(query_data_provider.GetConnectionProviders());

      return
        new CacheBuilder<IConnectionProvider>()
          .ExpireAfterAccess(settings_.QueryCacheDuration*3, TimeUnit.Seconds)
          .Build(cache_provider, new ConnectionProviderLoader(providers));
    }

    IJsonCollectionFactory GetJsonCollectionFactory() {
      IProviderNode provider;
      if (settings_.Providers.GetProviderNode(Strings.kJsonCollectionProvider,
        out provider)) {
        try {
          return RuntimeTypeFactory<IJsonCollectionFactory>
            .CreateInstanceFallback(provider, settings_);
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
