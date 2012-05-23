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
    public HttpStatusCode Process(string query, out string result) {
      IDictionary<string, string> entries = ParseQueryString(query);
      string name;
      if (entries.TryGetValue(Strings.kQueryStringQueryName, out name)) {
        IQuery query_to_execute = resolver_.GetQuery(name, entries);
        IQueryExecutor executor = resolver_.GetQueryExecutor(query_to_execute);
        if (!(executor is NoOpQueryExecutor)) {
          executor.Execute(query_to_execute);
        }
      }
      result = null;
      return HttpStatusCode.NotFound;
    }

    static IDictionary<string, string> ParseQueryString(string s) {
      IDictionary<string, string> entries = new Dictionary<string, string>();
      int length = s != null ? s.Length : 0;
      for (int i = 0; i < length; i++) {
        int start_index = i;
        int end_index = -1;
        while (i < length) {
          char ch = s[i];
          if (ch == '=') {
            if (end_index < 0) {
              end_index = i;
            }
          } else if (ch == '&') {
            break;
          }
          i++;
        }

        string name, value = string.Empty;
        if (end_index > 0) {
          name = s.Substring(start_index, end_index - start_index);
          value = s.Substring(end_index + 1, (i - end_index) - 1);
        } else {
          name = s.Substring(start_index, i - start_index);
        }
        entries.Add(name, value);
      }
      return entries;
    }
  }
}
