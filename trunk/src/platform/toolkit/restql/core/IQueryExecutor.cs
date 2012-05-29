using System;
using System.Collections.Generic;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A <see cref="IQueryExecutor"/> is used to process the submitted
  /// rest queries.
  /// </summary>
  public interface IQueryExecutor
  {
    /// <summary>
    /// Computes the specified query and returns the computation results as a
    /// string.
    /// </summary>
    /// <param name="query">
    /// The query to be processed.
    /// </param>
    /// <param name="paramters">
    /// A <see cref="IDictionary{TKey,TValue}"/> object containing the
    /// parameters that should be binded to the query.
    /// </param>
    /// <remarks>
    /// The format of the returned string is processor dependant. Callers
    /// should check the documentation of each processor to understand the
    /// meaning of the returned string.
    /// </remarks>
    string Execute(IQuery query, IDictionary<string, string> paramters);

    /// <summary>
    /// Gets a value indicating if a <see cref="IQueryExecutor"/> can execute
    /// the specified query.
    /// </summary>
    /// <param name="query">
    /// The <seealso cref="Query"/> object to check.
    /// </param>
    /// <returns>
    /// <c>true</c> If the executor can execute the query
    /// <paramref name="query"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <seealso cref="Query"/>
    bool CanExecute(IQuery query);
  }
}