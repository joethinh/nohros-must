using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A <see cref="IQueryProcessor"/> is used to process a query.
  /// </summary>
  public interface IQueryProcessor
  {
    /// <summary>
    /// Process the given query.
    /// </summary>
    /// <param name="query">
    /// A query string to be processed.
    /// </param>
    /// <returns></returns>
    HttpStatusCode Process(string query, out string result);
  }
}
