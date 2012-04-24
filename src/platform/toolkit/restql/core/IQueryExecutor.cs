using System;
using System.Collections.Generic;
using System.Text;

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
    /// <remarks>
    /// The format of the returned string is processor dependant. Callers
    /// should check the documentation of each processor to understand the
    /// meaning of the returned string.
    /// </remarks>
    string Execute(Query query);
  }
}