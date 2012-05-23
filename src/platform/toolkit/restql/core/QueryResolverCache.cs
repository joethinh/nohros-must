using System;

using Nohros.Caching;
using Nohros.Caching.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryResolver
  {
    #region QueryResolverCache
    /// <summary>
    /// A class that is used to cache the data used by the expensive and
    /// frequently used <see cref="QueryResolverCache"/>.
    /// </summary>
    public class QueryResolverCache
    {
      readonly ICommonDataProvider common_data_provider_;
      ILoadingCache<QueryExecutorPair> cache_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="QueryResolverCache"/> by
      /// using the specified <see cref="ICacheProvider"/>,
      /// <see cref="ICommonDataProvider"/> and <see cref="IQuerySettings"/>
      /// objects.
      /// </summary>
      /// <param name="cache_provider">
      /// A <see cref="ICacheProvider"/> object that can be used to store the
      /// expensive and frequently used objects.
      /// </param>
      /// <param name="common_data_provider">
      /// A <see cref="ICommonDataProvider"/> object that can be used to
      /// access the common data store.
      /// </param>
      /// <param name="settings">
      /// A <see cref="IQuerySettings"/> object containing the query
      /// related configuration information.
      /// </param>
      public QueryResolverCache(ICacheProvider cache_provider,
        ICommonDataProvider common_data_provider, IQuerySettings settings) {
        common_data_provider_ = common_data_provider;

        cache_ = new CacheBuilder<QueryExecutorPair>()
          .ExpireAfterAccess(settings.QueryCacheDuration, TimeUnit.Seconds)
          .Build(cache_provider,
            CacheLoader<QueryExecutorPair>.From(LoadQueryExecutorPair));
      }
      #endregion

      public IQuery GetQuery(string name) {
        return cache_.Get(name).Query;
      }

      /// <summary>
      /// Loads an <see cref="Query"/> from the database.
      /// </summary>
      /// <param name="name">
      /// The name of the query to load.
      /// </param>
      /// <returns>
      /// A <see cref="QueryExecutorPair"/> that contains the loaded query.
      /// </returns>
      QueryExecutorPair LoadQueryExecutorPair(string name) {
        Query query = common_data_provider_.GetQuery(name) as Query ??
          Query.EmptyQuery;
        query.Parse();
        return new QueryExecutorPair(query);
      }
    }
    #endregion
  }
}
