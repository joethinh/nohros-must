using System;
using System.Collections.Generic;
using Nohros.Caching;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A implementation of <see cref="IQueryResolver"/> class that caches
  /// resolved <see cref="IQuery"/> objects.
  /// </summary>
  /// <remarks>
  /// When a object if not found in cache, the <see cref="QueryResolverProxy"/>
  /// forwards the resolve operation to another <see cref="IQueryResolver"/>.
  /// </remarks>
  public class QueryResolverProxy : IQueryResolver
  {
    class QueryTuple : IQuery
    {
      readonly IQuery query_;
      IQueryExecutor executor_;

      #region .ctor
      public QueryTuple(IQuery query, IQueryExecutor executor) {
        query_ = query;
        executor_ = executor;
      }
      #endregion

      public IDictionary<string, string> Options {
        get { return query_.Options; }
      }

      public QueryMethod QueryMethod {
        get { return query_.QueryMethod; }
      }

      public string Type {
        get { return query_.Type; }
      }

      public string Name {
        get { return query_.Name; }
      }

      public string[] Parameters {
        get { return query_.Parameters; }
      }

      public string QueryText {
        get { return query_.QueryText; }
      }

      public IQueryExecutor Executor {
        get { return executor_; }
        set { executor_ = value; }
      }
    }

    readonly ICache<IQuery> cache_;
    readonly IQueryResolver resolver_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResolverProxy"/>
    /// class using the specified <see cref="IQueryResolver"/>.
    /// </summary>
    /// <param name="resolver">
    /// A <see cref="IQueryResolver"/> object that is used to resolve a query
    /// when it is not found in cache.
    /// </param>
    /// <param name="cache">
    /// A <see cref="ICache{T}"/> object that can be used to cache resolved
    /// <see cref="IQuery"/>.
    /// </param>
    public QueryResolverProxy(IQueryResolver resolver, ICache<IQuery> cache) {
      resolver_ = resolver;
      cache_ = cache;
    }
    #endregion

    /// <inheritdoc/>
    public IQuery GetQuery(string name) {
      return cache_.Get(name,
        CacheLoader<IQuery>.From(query => GetQueryTupple(name)));
    }

    /// <inheritdoc/>
    public IQueryExecutor GetQueryExecutor(IQuery query) {
      var tuple = query as QueryTuple;
      if (tuple == null) {
        return resolver_.GetQueryExecutor(query);
      }
      return tuple.Executor;
    }

    QueryTuple GetQueryTupple(string name) {
      IQuery query = resolver_.GetQuery(name);
      IQueryExecutor executor = resolver_.GetQueryExecutor(query);
      return new QueryTuple(query, executor);
    }
  }
}
