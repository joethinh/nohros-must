using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A class that contains informations about a particular query.
  /// </summary>
  public class Query : ParameterizedString
  {
    ParameterizedString query_;
    IDictionary<string, string> parms_;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryProcessor"/> class
    /// by using the specified query string.
    /// </summary>
    /// <param name="query_key">A string that uniquely identifies a query
    /// within the main data store.</param>
    /// <param name="parms">A <see cref="IDictionary&lt;TKey, TValue>"/> object
    /// containing the parameters that should be bound to the query.</param>
    public Query(): base() {
      query_ = query;
    }
  }
}