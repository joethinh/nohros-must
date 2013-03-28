using System;
using System.Data;
using System.Data.SQLite;

namespace Nohros.Data.Providers.SQLite
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProvider"/> that provides
  /// connections to the SQLite engine.
  /// </summary>
  public class InMemoryConnectionProvider : IConnectionProvider
  {
    const string kDefaultSchema = "dbo";

    const string kClassName =
      "Nohros.Data.Providers.SQLite.InMemoryConnectionProvider";

    const string kInMemoryConnectionString = ":memory";

    SQLiteConnection sqlite_connection_;
    bool opened_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="InMemoryConnectionProvider"/> class.
    /// </summary>
    public InMemoryConnectionProvider() {
      opened_ = false;
    }
    #endregion

    /// <summary>
    /// Gets the schema to be used by the connection created by this class.
    /// </summary>
    /// <remarks>
    /// If a schema is not specified, this methos will return the string "dbo".
    /// </remarks>
    public string Schema {
      get { return string.Empty; }
    }

    IDbConnection IConnectionProvider.CreateConnection() {
      return CreateConnection();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SQLiteConnection"/> class
    /// using the provider connection string.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="SQLiteConnection"/> class.
    /// </returns>
    public SQLiteConnection CreateConnection() {
      if (!opened_) {
        sqlite_connection_ = new SQLiteConnection(kInMemoryConnectionString);
      }
      return sqlite_connection_;
    }
  }
}
