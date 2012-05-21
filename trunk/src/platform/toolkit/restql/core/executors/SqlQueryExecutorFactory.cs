using System;
using System.Collections.Generic;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryExecutor : IQueryExecutorFactory
  {
    /// <summary>
    /// Creates a instance of the <see cref="IQueryExecutor"/> class by using
    /// the specified application settings.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the specific
    /// options configured for the query processor.
    /// </param>
    /// <param name="settings">
    /// A <see cref="IQuerySettings"/> containing configuration data for the
    /// query processor.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="IQueryExecutor"/> class.
    /// </returns>
    public IQueryExecutor CreateQueryExecutor(IDictionary<string, string> options, IQuerySettings settings) {
    }
  }
}
