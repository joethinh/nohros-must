using System;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The default implementation of the <see cref="IQueryResolver"/> interface.
  /// </summary>
  public class QueryResolver : IQueryResolver
  {
    readonly IQueryDataProvider query_data_provider_;
    readonly IQueryExecutor[] executors_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResolver"/> class
    /// </summary>
    /// <param name="executors">
    /// An array of <see cref="IQueryExecutor"/> containing all the query
    /// executors configured for the application.
    /// </param>
    /// <param name="query_data_provider">
    /// A <see cref="IQueryDataProvider"/> object that can be used to search
    /// from a <see cref="IQuery"/>.
    /// </param>
    public QueryResolver(IQueryExecutor[] executors,
      IQueryDataProvider query_data_provider) {
      executors_ = executors;
      query_data_provider_ = query_data_provider;
    }
    #endregion

    /// <inheritdoc/>
    /// <remarks>
    /// If there are no query executors that can execute the specified query
    /// a instance of the <see cref="NoOpQueryExecutor"/> class will be
    /// returned.
    /// </remarks>
    public IQueryExecutor GetQueryExecutor(IQuery query) {
      for (int i = 0, j = executors_.Length; i < j; i++) {
        IQueryExecutor executor = executors_[i];
        if (executor.CanExecute(query)) {
          return executor;
        }
      }
      return NoOpQueryExecutor.StaticNoOpQueryExecutor;
    }

    /// <inheritdoc/>
    public IQuery GetQuery(string name) {
      return query_data_provider_.GetQuery(name);
    }
  }
}
