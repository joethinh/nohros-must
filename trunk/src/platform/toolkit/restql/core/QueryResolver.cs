using System;
using System.Collections.Generic;
using Nohros.Caching;
using Nohros.Collections;
using Nohros.Configuration;
using Nohros.Logging;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The default implementation of the <see cref="IQueryResolver"/> interface.
  /// </summary>
  public partial class QueryResolver : IQueryResolver
  {
    readonly ILoadingCache<QueryExecutorPair> cache_;
    readonly IQueryExecutor[] executors_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResolver"/> class,
    /// using the specified cache.
    /// </summary>
    /// <param name="cache">
    /// A <see cref="ILoadingCache{T}"/> that is used to cache the resolved
    /// queries.
    /// </param>
    /// <param name="executors">
    /// An array of <see cref="IQueryExecutor"/> containing all the query
    /// executors configured for the application.
    /// </param>
    QueryResolver(ILoadingCache<QueryExecutorPair> cache,
      IQueryExecutor[] executors) {
      cache_ = cache;
      executors_ = executors;
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
      QueryExecutorPair query_executor_pair = cache_.Get(query.Name);
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
      Query query = cache_.Get(name).Query;
      query.Parse();
      return query;
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name, IDictionary<string, string> parameters) {
      Query query = cache_.Get(name).Query;
      query.Parse();

      ParameterizedStringPartParameterCollection query_parameters =
        query.Parameters;
      foreach (string parameter in parameters.Keys) {
        int index = query_parameters.IndexOf(parameter);
        if (index == -1) {
          if (MustLogger.ForCurrentProcess.IsWarnEnabled) {
            MustLogger.ForCurrentProcess.Warn("The parameter "
              + parameter + " is not recognized by the query "
                + query.Name);
          }
          return Query.EmptyQuery;
        }
        query_parameters[index].Value = parameter;
      }
      return query;
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name, IDictionary<string, string> parameters,
      IDictionary<string, string> options) {
      IQuery query = GetQuery(name, parameters);
      if (query != Query.EmptyQuery && options.Count != 0) {
        IDictionary<string, string> query_options = query.Options;
        foreach(KeyValuePair<string, string> option in options) {
          query_options[option.Key] = option.Value;
        }
      }
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
