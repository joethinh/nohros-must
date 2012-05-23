using System;
using System.Collections.Generic;

using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The default implementation of the <see cref="IQueryResolver"/> interface.
  /// </summary>
  public partial class QueryResolver : IQueryResolver
  {
    const int kDefaultExpirationInMinutes = 30;

    readonly QueryResolverCache cache_;
    readonly IQueryExecutor[] executors_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResolver"/> class
    /// </summary>
    /// <param name="executors">
    /// An array of <see cref="IQueryExecutor"/> containing all the query
    /// executors configured for the application.
    /// </param>
    /// <param name="cache">
    /// A <see cref="QueryResolverCache"/> object that can be used to cache
    /// expensive and frequently used data.
    /// </param>
    /// <param name="">
    /// </param>
    public QueryResolver(IQueryExecutor[] executors, QueryResolverCache cache) {
      executors_ = executors;
      cache_ = cache;
    }
    #endregion

    #region IQueryResolver Members
    /// <inheritdoc/>
    /// <remarks>
    /// If there are no query executors that can execute the specified query
    /// a instance of the <see cref="NoOpQueryExecutor"/> class will be
    /// returned.
    /// </remarks>
    public IQueryExecutor GetQueryExecutor(IQuery query) {
      QueryExecutorPair query_executor_pair = cache_.LoadingCache.Get(query.Name);
      IQueryExecutor query_executor = query_executor_pair.QueryExecutor;
      if (query_executor == null) {
        query_executor = FindQueryExecutor(query);
        query_executor_pair.QueryExecutor = query_executor;
        cache_.Put(query.Name, query_executor_pair);
      }
      return query_executor;
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name) {
      return cache_.GetQuery(name);
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name, IDictionary<string, string> parameters) {
      IQuery query = cache_.Get(name).Query;
      query.BindParameters(parameters);
      return query;
    }
    #endregion

    IQueryExecutor FindQueryExecutor(IQuery query) {
      for (int i = 0, j = executors_.Length; i < j; i++) {
        IQueryExecutor executor = executors_[i];
        if (executor.CanExecute(query)) {
          return executor;
        }
      }
      return NoOpQueryExecutor.StaticNoOpQueryExecutor;
    }
  }
}
