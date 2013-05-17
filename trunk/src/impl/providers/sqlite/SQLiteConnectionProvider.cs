using System;
using System.Data;
using System.Data.SQLite;
using Nohros.Data.Providers;

namespace Nohros.Data.SQLite
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProvider"/> that provides
  /// connections for a MySql server.
  /// </summary>
  public class SQLiteConnectionProvider : IConnectionProvider
  {
    const string kDefaultSchema = "dbo";

    const string kClassName =
      "Nohros.Data.Providers.SQLite.SQLiteConnectionProvider";

    readonly string connection_string_;
    readonly string schema_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SQLiteConnectionProvider"/>
    /// clas using the specified connection string.
    /// </summary>
    /// <param name="connection_string">
    /// A string that can be used to connect to a MySql server.
    /// </param>
    public SQLiteConnectionProvider(string connection_string)
      : this(connection_string, kDefaultSchema) {
      connection_string_ = connection_string;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SQLiteConnectionProvider"/>
    /// class using the specified connecton string and database schema.
    /// </summary>
    /// <param name="connection_string">
    /// A string that can be used to connect to a MySql server.
    /// </param>
    /// <param name="schema">
    /// The default database schema.
    /// </param>
    public SQLiteConnectionProvider(string connection_string, string schema) {
      connection_string_ = connection_string;
      schema_ = schema;
    }
    #endregion

    /// <summary>
    /// Gets the schema to be used by the connection created by this class.
    /// </summary>
    /// <remarks>
    /// If a schema is not specified, this methos will return the string "dbo".
    /// </remarks>
    public string Schema {
      get { return schema_; }
    }

    IDbConnection IConnectionProvider.CreateConnection() {
      return CreateConnection();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SQLiteConnection"/> class using
    /// the provider connection string.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="SQLiteConnection"/> class.
    /// </returns>
    public SQLiteConnection CreateConnection() {
      return new SQLiteConnection(connection_string_);
    }
  }
}
