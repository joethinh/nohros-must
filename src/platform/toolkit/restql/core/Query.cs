using Nohros.Data;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Represents a string that contains data to be passed to a query processor
  /// in order to obtain results from a data source.
  /// </summary>
  /// <remarks>
  /// A query is composed by a plain text having or not parameters within it.
  /// A parameter is a sequence of characters with no spaces delimited by
  /// anohther string. The default parameter delimiter is the string "$".
  /// <para>
  /// Each query should have a unique name.
  /// </para>
  /// </remarks>
  public class Query : ParameterizedString
  {
    static readonly Query empty_query_;

    readonly string name_;
    readonly string type_;

    bool is_parsed_;

    #region .ctor
    /// <summary>
    /// Initialize the static members.
    /// </summary>
    static Query() {
      empty_query_ = new Query(string.Empty, string.Empty, string.Empty);
    }

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
      : this(name, type, query, "@") {
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
    }
    #endregion

    /// <inheritdoc/>
    public override void Parse() {
      if (!is_parsed_) {
        base.Parse();
        is_parsed_ = true;
      }
    }

    /// <summary>
    /// Gets an instance of the <see cref="Query"/> class that represents an
    /// empty query, that is a query whose name, type and query string is an
    /// empty string.
    /// </summary>
    public static Query EmptyQuery {
      get { return empty_query_; }
    }

    /// <summary>
    /// Gets the name of the query.
    /// </summary>
    public string Name {
      get { return name_; }
    }

    /// <summary>
    /// </summary>
    public string Type {
      get { return type_; }
    }
  }
}
