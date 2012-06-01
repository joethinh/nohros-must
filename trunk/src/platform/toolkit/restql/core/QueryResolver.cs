using System;

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
      return cache_.GetQueryEecutor(query, executors_);
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name) {
      return cache_.GetQuery(name);
    }
    #endregion
  }
}
