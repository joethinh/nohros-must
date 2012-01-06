using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Represents a string data contains data to be passed to a query processor
  /// in order to obtain data from a data source.
  /// </summary>
  /// <remarks>
  /// A query is composed by a plain text having or not parameters within it.
  /// A parameter is a sequence of characters with no spaces delimited by
  /// anohther string. The default parameter delimiter is the string "$".
  /// <para>
  /// Each query has a type that should be set by the user. The default query
  /// type is <see cref="QeuryType.AdHoc"/>.
  /// </para>
  /// </remarks>
  public class Query : ParameterizedString
  {
    IDictionary<string, string> parms_;
    QueryType query_type_;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string.
    /// </summary>
    /// <param name="query">A string that represents the query.
    /// </param>
    public Query(string query): base(query) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string and paraemter delimiter..
    /// </summary>
    /// <param name="query">A string that represents the query.</param>
    /// <param name="delimiter">A string that is used to delimit the query
    /// parameters.</param>
    /// <remarks>
    /// A parameter is a sequence of characters without spaces
    /// enclosed by the <paramref name="delimiter"/>.
    /// </remarks>
    public Query(string query, string delimiter) : base(query, delimiter) { }

    /// <summary>
    /// Gets or sets the type of the query.
    /// </summary>
    public QueryType Type {
      get { return query_type_; }
    }
  }
}