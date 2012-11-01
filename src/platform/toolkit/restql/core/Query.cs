using System;
using System.Collections.Generic;
using System.Linq;

namespace Nohros.RestQL
{
  /// <summary>
  /// The default implementation of the <see cref="IQuery"/> interface.
  /// </summary>
  public class Query : ParameterizedString, IQuery
  {
    static readonly Query empty_query_;

    readonly string name_;
    readonly IDictionary<string, string> options_;
    readonly string type_;

    bool is_parsed_;

    #region .ctor
    /// <summary>
    /// Initialize the static members.
    /// </summary>
    static Query() {
      empty_query_ = new Query(string.Empty, string.Empty, string.Empty);
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string, name and type.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the query within the main data
    /// store.
    /// </param>
    /// <param name="type">
    /// A string that identifies the type of the query. This paramter is used
    /// to locate a query processor that can process the query.
    /// </param>
    /// <param name="query">
    /// A string that represents the query itself.
    /// </param>
    /// <remarks>
    /// The default parameter delimiter "$" will be used when instantiating
    /// a <see cref="Query"/> object through this constructor.
    /// </remarks>
    public Query(string name, string type, string query)
      : this(name, type, query, "$") {
      UseSpaceAsTerminator = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string and paraemter delimiter..
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the query within the main data
    /// store.
    /// </param>
    /// <param name="type">
    /// A string that identifies the type of the query. This paramter is used
    /// to locate a query processor that can process the query.
    /// </param>
    /// <param name="query">
    /// A string that represents the query itself.
    /// </param>
    /// <param name="delimiter">
    /// A string to be used as a parameter delimiter.
    /// </param>
    /// <remarks>
    /// A parameter is a sequence of characters without spaces enclosed by
    /// <paramref name="delimiter"/>.
    /// </remarks>
    public Query(string name, string type, string query, string delimiter)
      : base(query, delimiter) {
      type_ = type;
      name_ = name;
      is_parsed_ = false;
      options_ = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
    #endregion

    /// <inheritdoc/>
    public string Name {
      get { return name_; }
    }

    /// <inheritdoc/>
    public string Type {
      get { return type_; }
    }

    /// <inheritdoc/>
    string[] IQuery.Parameters {
      get {
        var names =
          from parameter in Parameters
          where parameter.IsParameter
          select parameter.Name;
        return names.ToArray();
      }
    }

    /// <inheritdoc/>
    public string QueryText {
      get { return flat_string; }
    }

    /// <summary>
    /// Gets or sets the query's method.
    /// </summary>
    /// <remarks>
    /// The <see cref=" QueryMethod"/> identifies the type of operation that
    /// will be performed on the data base ( retrieve value, modify data,
    /// modify and retrieve data).
    /// </remarks>
    public QueryMethod QueryMethod { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, string> Options {
      get { return options_; }
    }

    /// <inheritdoc/>
    public override void Parse() {
      if (!is_parsed_) {
        base.Parse();
        is_parsed_ = true;
      }
    }

    public override bool Equals(object obj) {
      Query query = obj as Query;
      if ((object) obj == null) {
        return false;
      }
      return Equals(query);
    }

    public bool Equals(Query query) {
      return query.name_ == name_;
    }

    public override int GetHashCode() {
      return name_.GetHashCode();
    }

    public static bool operator ==(Query query_a, Query query_b) {
      if (ReferenceEquals(query_a, query_b)) {
        return true;
      }
      if ((object) query_a == null || (object) query_b == null) {
        return false;
      }
      return query_a.name_ == query_b.name_;
    }

    public static bool operator !=(Query query_a, Query query_b) {
      return !(query_a == query_b);
    }

    /// <summary>
    /// Gets an instance of the <see cref="Query"/> class that represents an
    /// empty query, that is a query whose name, type and query string is an
    /// empty string.
    /// </summary>
    public static Query EmptyQuery {
      get { return empty_query_; }
    }
  }
}
