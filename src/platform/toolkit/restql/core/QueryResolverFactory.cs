using System;

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
      QueryResolverCache query_resolver_cache =
        new QueryResolverCache(settings.CacheProvider,
          settings.CommonDataProvider, settings);
      return new QueryResolver(GetQueryExecutors(settings), query_resolver_cache);
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
