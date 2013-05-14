using System;
using System.Data;
using MySql.Data.MySqlClient;
using Nohros.Data.Providers;

namespace Nohros.Data.Providers.MySql
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProvider"/> that provides
  /// connections for a MySql server.
  /// </summary>
  public class MySqlConnectionProvider : IConnectionProvider
  {
    const string kDefaultSchema = "dbo";
    const string kClassName = "Nohros.Data.MySql.MySqlConnectionProvider";

    readonly string connection_string_;
    readonly string schema_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlConnectionProvider"/>
    /// clas using the specified connection string.
    /// </summary>
    /// <param name="connection_string">
    /// A string that can be used to connect to a MySql server.
    /// </param>
    public MySqlConnectionProvider(string connection_string)
      : this(connection_string, kDefaultSchema) {
      connection_string_ = connection_string;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlConnectionProvider"/>
    /// class using the specified connecton string and database schema.
    /// </summary>
    /// <param name="connection_string">
    /// A string that can be used to connect to a MySql server.
    /// </param>
    /// <param name="schema">
    /// The default database schema.
    /// </param>
    public MySqlConnectionProvider(string connection_string, string schema) {
      connection_string_ = connection_string;
      schema_ = schema;
    }
    #endregion

    IDbConnection IConnectionProvider.CreateConnection() {
      return CreateConnection();
    }

    /// <summary>
    /// Gets the schema to be used by the connection created by this class.
    /// </summary>
    /// <remarks>
    /// If a schema is not specified, this methos will return the string "dbo".
    /// </remarks>
    public string Schema {
      get { return schema_; }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MySqlConnection"/> class using
    /// the provider connection string.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="MySqlConnection"/> class.
    /// </returns>
    public MySqlConnection CreateConnection() {
      return new MySqlConnection(connection_string_);
    }
  }
}
