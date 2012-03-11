using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A class used to process a restQL query.
  /// </summary>
  public class QueryProcessor
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryProcessor"/> class
    /// by using the specified query string.
    /// </summary>
    /// <param name="query_key">A string that uniquely identifies a query
    /// within the main data store.</param>
    /// <param name="query_string">A <see cref="IDictionary"/> object
    /// containing the parameters that will be used by the query associated
    /// with the given <paramref name="query_key"/>.</param>
    public QueryProcessor(QueryInfo query) {
    }
    #endregion

    /// <summary>
    /// Retrieve the query information from the main datastore and execute it,
    /// using the supplied parameters.
    /// </summary>
    /// <returns></returns>
    public string Process() {
    }
  }
}
