using System;
using System.Collections.Generic;

namespace Nohros.RestQL
{
  public class QueryProcessor : IQueryProcessor
  {
    readonly IQueryResolver resolver_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryProcessor"/> using
    /// the specified <see cref="IQueryResolver"/> object.
    /// </summary>
    /// <param name="resolver">
    /// A <see cref="IQueryResolver"/> object that is used to resolve queries.
    /// </param>
    public QueryProcessor(IQueryResolver resolver) {
      resolver_ = resolver;
    }
    #endregion

    /// <inheritdoc/>
    public bool Process(string name,
      IDictionary<string, string> data, out string result) {
      IQuery query_to_execute = resolver_.GetQuery(name);
      IQueryExecutor executor = resolver_.GetQueryExecutor(query_to_execute);
      if (!(executor is NoOpQueryExecutor)) {
        try {
          result = executor.Execute(query_to_execute, data);
        } catch(KeyNotFoundException key_not_found_exception) {
          result = key_not_found_exception.Message;
        }
        return true;
      }
      result = string.Format(Resources.QueryProcessor_ProcessorNotFound, name);
      return false;
    }
  }
}
