using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  public class QueryInfo
  {
    Query query_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryInfo"/> class by
    /// using the specified <see cref="Query"/> object.
    /// </summary>
    /// <param name="query">The <see cref="Query"/> object which this instance
    /// has information.</param>
    public QueryInfo(Query query) {
      query_ = query;
    }
    #endregion

    /// <summary>
    /// Gets the <see cref="Query"/> object associated with this instance.
    /// </summary>
    public Query Query {
      get {
        return query_;
      }
    }
  }
}
