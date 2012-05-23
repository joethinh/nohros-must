using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A <see cref="IQueryResolver"/> is used to perform query resolution.
  /// </summary>
  /// <remarks>
  /// A <see cref="IQueryResolver"/> process a given string and produces a
  /// <see cref="Query"/> object that can be sent to a
  /// <see cref="IQueryExecutor"/> to be executed.
  /// </remarks>
  public interface IQueryResolver
  {
    /// <summary>
    /// Resolves a query using its unique name.
    /// </summary>
    /// <param name="name">
    /// The name of the query to resolve.
    /// </param>
    /// <returns>
    /// The resolved query or <see cref="Query.EmptyQuery"/> if the query could
    /// not be resolved.
    /// </returns>
    IQuery GetQuery(string name);

    /// <summary>
    /// Resolves a query using its unique name and bound the speicifed
    /// parameters to it.
    /// </summary>
    /// <param name="name">
    /// The query's unique name.
    /// </param>
    /// <param name="parameters">
    /// A <see cref="IDictionary{TKey,TValue}"/> object containing the
    /// parameters to be bound to the resolved query.
    /// </param>
    /// <returns>
    /// The resolved query or <see cref="Query.EmptyQuery"/> if the query could
    /// not be resolved.
    /// </returns>
    /// <remarks>
    /// The given parameters will be bound to the query if it can be resolved
    /// using its name.
    /// </remarks>
    IQuery GetQuery(string name, IDictionary<string, string> parameters);

    /// <summary>
    /// Gets a <see cref="IQueryExecutor"/> that is capable to resolve the
    /// specified query.
    /// </summary>
    /// <returns>
    /// A <see cref="IQueryExecutor"/> object that is capable to execute the
    /// given query.
    /// </returns>
    /// <remarks>
    /// If there are no query executors that can execute the specified query
    /// a instance of the <see cref="NoOpQueryExecutor"/> class will be
    /// returned.
    /// </remarks>
    IQueryExecutor GetQueryExecutor(IQuery query);
  }
}
