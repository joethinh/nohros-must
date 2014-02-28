using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using Nohros.Data.Providers;

namespace Nohros.Data.SqlCe
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProvider"/> that
  /// provides connections for the Microsoft Sql Server.
  /// </summary>
  public class SqlCeConnectionProvider : IConnectionProvider
  {
    const string kDefaultSchema = "dbo";
    const string kClassName = "Nohros.Data.Providers.SqlCeConnectionProvider";

    readonly string connection_string_;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SqlCeConnectionProvider"/> class using the specified
    /// connection string.
    /// </summary>
    public SqlCeConnectionProvider(string connection_string) {
      connection_string_ = connection_string;
    }

    string IConnectionProvider.Schema {
      get { return "dbo"; }
    }

    /// <inheritdoc/>
    IDbConnection IConnectionProvider.CreateConnection() {
      return CreateConnection();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SqlConnection"/> class using
    /// the provider connection string.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="SqlConnection"/> class.
    /// </returns>
    /// <remarks>
    /// If a <see cref="ITransactionContext"/> exists this, the connection
    /// that is associated with it will be returned.
    /// </remarks>
    public SqlCeConnection CreateConnection() {
      return new SqlCeConnection(connection_string_);
    }

    /// <summary>
    /// Gets the associated connection string.
    /// </summary>
    /// <value>
    /// The string that is used to connect to a SQLCE database.
    /// </value>
    public string ConnectionString {
      get { return connection_string_; }
    }
  }
}
