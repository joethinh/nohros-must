using System;
using System.Collections.Generic;
using System.Net;

namespace Nohros.Toolkit.RestQL
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
    public HttpStatusCode Process(string name,
      IDictionary<string, string> data, out string result) {
      IQuery query_to_execute = resolver_.GetQuery(name);
      IQueryExecutor executor = resolver_.GetQueryExecutor(query_to_execute);
      if (!(executor is NoOpQueryExecutor)) {
        result = executor.Execute(query_to_execute, data);
        return HttpStatusCode.OK;
      }
      result = string.Empty;
      return HttpStatusCode.NotFound;
    }
  }
}
