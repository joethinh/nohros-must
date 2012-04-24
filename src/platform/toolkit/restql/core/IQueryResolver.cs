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
    Query GetQuery(string name);

    /// <summary>
    /// Resolves a query using its unique name and parameters.
    /// </summary>
    /// <param name="name">
    /// The query's unique name.
    /// </param>
    /// <param name="parameters">
    /// The query's parameters.
    /// </param>
    /// <returns>
    /// The resolved query or <see cref="Query.EmptyQuery"/> if the query could
    /// not be resolved.
    /// </returns>
    /// <remarks>
    /// The given parameters will be bound to the query if it can be resolved
    /// using its name.
    /// </remarks>
    Query GetQuery(string name, IDictionary<string, string> parameters);
  }
}
