using System;
using System.Collections.Generic;
using System.Net;

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
    /// <param name="name">
    /// A string that uniquely identifies the query within an application.
    /// </param>
    /// <param name="options">
    /// A collection of name value pairs containing the user supplied opition (
    /// filter and parameters) for the query.
    /// </param>
    /// <param name="result">
    /// The result of the query processing.
    /// </param>
    /// <returns>
    /// A <see cref="HttpStatusCode"/> repsenting the status of the query
    /// processing.
    /// </returns>
    HttpStatusCode Process(string name, IDictionary<string, string> options,
      out string result);
  }
}
