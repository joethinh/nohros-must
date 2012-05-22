using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A implementation of the <see cref="IQueryExecutor"/> that does nothing.
  /// </summary>
  /// <remarks>
  /// </remarks>
  public class NoOpQueryExecutor : IQueryExecutor
  {
    static NoOpQueryExecutor no_op_query_executor_;

    /// <summary>
    /// </summary>
    /// <param name="query">
    /// The query to be processed.
    /// </param>
    /// <remarks>
    /// This method always returns an empty string.
    /// </remarks>
    public string Execute(IQuery query) {
      return string.Empty;
    }

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
    public bool CanExecute(IQuery query) {
      return true;
    }

    /// <summary>
    /// Gets the static <see cref="NoOpQueryExecutor"/> object.
    /// </summary>
    public static NoOpQueryExecutor StaticNoOpQueryExecutor {
      get {
        if (no_op_query_executor_ == null) {
          Interlocked.CompareExchange(
            ref no_op_query_executor_, new NoOpQueryExecutor(), null);
        }
        return no_op_query_executor_;
      }
    }
  }
}
