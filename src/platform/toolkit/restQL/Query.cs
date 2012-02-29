using System;
using System.Collections.Generic;
using System.Text;

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
  /// Each query has a type that should be set by the user. The default query
  /// type is <see cref="QeuryType.AdHoc"/>.
  /// </para>
  /// </remarks>
  public class Query : ParameterizedString
  {
    string name_;
    string executor_factory_;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string.
    /// </summary>
    /// <param name="name">A string that uniquely identifies the query within
    /// the main data store.</param>
    /// <param name="query">A string that represents the query.
    /// <param name="executor_factory">The assembly's fully qualified
    /// name of the factory class that is responsible to create instances
    /// of the class that can execute the query.</param>
    /// </param>
    public Query(string name, string executor_factory, string query)
      : this(name, query, executor_factory, kDefaultDelimiter) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class by using
    /// the specified query string and paraemter delimiter..
    /// </summary>
    /// <param name="name">A string that uniquely identifies the query within
    /// the main data store.</param>
    /// <param name="query">A string that represents the query.</param>
    /// <param name="delimiter">A string that is used to delimit the query
    /// parameters.</param>
    /// <param name="executor_factory">The assembly's fully qualified
    /// name of the factory class that is responsible to create instances
    /// of the class that can execute the query.</param>
    /// </param>
    /// <remarks>
    /// A parameter is a sequence of characters without spaces
    /// enclosed by the <paramref name="delimiter"/>.
    /// </remarks>
    public Query(string name, string query, string executor_factory,
      string delimiter) : base(query, delimiter) {
      executor_factory_ = executor_factory;
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
    public string ExecutorFactory {
      get { return executor_factory_; }
    }
  }
}