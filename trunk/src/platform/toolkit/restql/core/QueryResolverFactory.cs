using System;

using Nohros.Caching;
using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryResolver
  {
    /// <summary>
    /// Creates an instance of the <see cref="QueryResolver"/> object using the
    /// specified cache provider, common data provider and query settings.
    /// </summary>
    /// <returns>
    /// The created <see cref="QueryResolver"/> object.
    /// </returns>
    public static QueryResolver CreateQueryResolver(IQuerySettings settings) {
      CacheBuilder<QueryExecutorPair> cache =
        new CacheBuilder<QueryExecutorPair>();
      cache.ExpireAfterAccess(settings.QueryCacheDuration, TimeUnit.Minutes);
      ILoadingCache<QueryExecutorPair> loading_cache =
        cache.Build(settings.CacheProvider,
          CacheLoader<QueryExecutorPair>.From(
            delegate(string key)
            {
              return
                new QueryExecutorPair(
                  settings.CommonDataProvider.GetQuery(key));
            }));
      return new QueryResolver(loading_cache, GetQueryExecutors(settings));
    }

    static IQueryExecutor[] GetQueryExecutors(IQuerySettings settings) {
      IProviderNode[] providers = settings.Executors;
      int length = providers.Length;
      IQueryExecutor[] executors = new IQueryExecutor[length];
      for (int i = 0, j = length; i < j; i++) {
        IProviderNode provider = providers[i];
        executors[i] = ProviderFactory<IQueryExecutorFactory>
          .CreateProviderFactory(provider)
          .CreateQueryExecutor(provider.Options, settings);
      }
      return executors;
    }
  }
}
