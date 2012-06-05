using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// <param name="result">
    /// The result of the query processing.
    /// </param>
    /// <returns>
    /// A <see cref="HttpStatusCode"/> repsenting the status of the query
    /// processing.
    /// </returns>
    HttpStatusCode Process(string query, out string result);

    /// <summary>
    /// Process the given query.
    /// </summary>
    /// <param name="query">
    /// A collection of name value pairs that represents the query.
    /// </param>
    /// <param name="result">
    /// The result of the query processing.
    /// </param>
    /// <returns>
    /// A <see cref="HttpStatusCode"/> repsenting the status of the query
    /// processing.
    /// </returns>
    HttpStatusCode Process(NameValueCollection query, out string result);

    /// <summary>
    /// Process the given query.
    /// </summary>
    /// <param name="query">
    /// A collection of name value pairs that represents the query.
    /// </param>
    /// <param name="result">
    /// The result of the query processing.
    /// </param>
    /// <returns>
    /// A <see cref="HttpStatusCode"/> repsenting the status of the query
    /// processing.
    /// </returns>
    HttpStatusCode Process(IDictionary<string, string> query, out string result);
  }
}
