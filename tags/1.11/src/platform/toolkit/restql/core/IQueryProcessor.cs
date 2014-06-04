using System;
using System.Collections.Generic;

namespace Nohros.RestQL
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
    /// A collection of name value pairs containing the user supplied options (
    /// filters and parameters) for the query.
    /// </param>
    /// <param name="result">
    /// The result of the query processing, or <see cref="string.Empty"/> if
    /// the query processing operation fails.
    /// </param>
    /// <returns>
    /// <c>true</c> if the query was successfully processed; otherwise,
    /// <c>false</c>.
    /// </returns>
    bool Process(string name, IDictionary<string, string> options,
      out string result);
  }
}
