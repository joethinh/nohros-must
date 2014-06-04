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

namespace Nohros.RestQL
{
  public class SqlQueryExecutorFactory : IQueryExecutorFactory
  {
    const string kClassName = "Nohros.RestQL.AbstractSqlQueryExecutor";
    readonly IQuerySettings settings_;

    #region .ctor
    public SqlQueryExecutorFactory(IQuerySettings settings) {
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
      ICacheProvider cache_provider = GetCacheProvider();
      ICache<IConnectionProvider> cache = new CacheBuilder<IConnectionProvider>()
        .ExpireAfterAccess(settings_.QueryCacheDuration*3, TimeUnit.Seconds)
        .Build(cache_provider);
      IJsonCollectionFactory json_collection_factory =
        GetJsonCollectionFactory();
      return new SqlQueryExecutor(json_collection_factory, cache);
    }

    ICacheProvider GetCacheProvider() {
      IProviderNode provider =
        settings_.Providers.GetProviderNode(Strings.kCacheProviderName);
      return RuntimeTypeFactory<ICacheProviderFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateCacheProvider(provider.Options.ToDictionary());
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
