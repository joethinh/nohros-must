using System;
using System.Data.SqlClient;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProvider{T}"/> that
  /// provides connections for the Microsoft Sql Server.
  /// </summary>
  public partial class SqlConnectionProvider : ISqlConnectionProvider
  {
    const string kConnectionStringOption = "connection-string";
    const string kServerOption = "server";
    const string kLoginOption = "login";
    const string kPasswordOption = "password";

    readonly string connection_string_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SqlConnectionProvider"/> class using the specified
    /// connection string.
    /// </summary>
    public SqlConnectionProvider(string connection_string) {
      connection_string_ = connection_string;
    }
    #endregion

    #region ISqlConnectionProvider Members
    /// <inheritdoc/>
    public SqlConnection CreateConnection() {
      return new SqlConnection(connection_string_);
    }
    #endregion
  }
}
