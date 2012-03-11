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
    const string kDefaultDelimiter = "$";

    string name_;
    string processor_factory_type_;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string.
    /// </summary>
    /// <param name="name">A string that uniquely identifies the query within
    /// the main data store.</param>
    /// <param name="processor_factory_type">The fully qualified assembly's
    /// name of the class fatory that is used to create instances of the class
    /// that is able to process the query.</param>
    /// <param name="query">A string that represents the query.
    /// <param name="processor_factory_type">The assembly's fully qualified
    /// name of the factory class that is responsible to create instances
    /// of the class that can execute the query.</param>
    /// </param>
    public Query(string name, string processor_factory_type, string query)
      : this(name, query, processor_factory_type, kDefaultDelimiter) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string and paraemter delimiter..
    /// </summary>
    /// <param name="name">A string that uniquely identifies the query within
    /// the main data store.</param>
    /// <param name="query">A string that represents the query.</param>
    /// <param name="delimiter">A string that is used to delimit the query
    /// parameters.</param>
    /// <param name="processor_factory_type">The assembly's fully qualified
    /// name of the factory class that is responsible to create instances
    /// of the class that can execute the query.</param>
    /// <remarks>
    /// A parameter is a sequence of characters without spaces
    /// enclosed by the <paramref name="delimiter"/>.
    /// </remarks>
    public Query(string name, string query, string processor_factory_type,
      string delimiter) : base(query, delimiter) {
      processor_factory_type_ = processor_factory_type;
      name_ = name;
    }

    /// <summary>
    /// Gets the name of the query.
    /// </summary>
    public string Name {
      get { return name_; }
    }

    /// <summary>
    /// Gets the assembly's fully qualified name of the factory class that can
    /// create instances of the class that can execute the query.
    /// </summary>
    /// <value>The assembly's fully qualified name of the factory class that
    /// can create instances of the class that is able to process the query.
    /// </value>
    public string ProcessorFactoryType {
      get { return processor_factory_type_; }
    }
  }
}