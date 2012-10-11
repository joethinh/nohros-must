using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProviderFactory"/> that
  /// create instances of the <see cref="SqlConnection"/> class.
  /// </summary>
  public class SqlConnectionProviderFactory : IConnectionProviderFactory
  {
    /// <summary>
    /// The key that should be associated with the option that contains
    /// the connection string.
    /// </summary>
    /// <remarks>
    /// This option is mutually exclulsive with the others and has the hightest
    /// priority.
    /// </remarks>
    public const string kConnectionStringOption = "connectionString";

    /// <summary>
    /// The key that should be associated with the option that contains
    /// the Sql Server address.
    /// </summary>
    public const string kServerOption = "server";

    /// <summary>
    /// The key that should be associated with the option that contains
    /// the login to be used to connect to the Sql Server.
    /// </summary>
    public const string kLoginOption = "login";

    /// <summary>
    /// The key that should be associated with the option that contains
    /// the password to use to connects to the Sql Server.
    /// </summary>
    public const string kPasswordOption = "password";

    #region .ctor
    /// <summary>
    /// Constructor implied by the interface
    /// <see cref="IConnectionProviderFactory"/>.
    /// </summary>
    public SqlConnectionProviderFactory() {
    }
    #endregion

    /// <inheritdoc/>
    public IConnectionProvider CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      if (options.TryGetValue(kConnectionStringOption, out connection_string)) {
        return new SqlConnectionProvider(connection_string);
      }

      SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
      builder.DataSource = options[kServerOption];
      builder.UserID = options[kLoginOption];
      builder.Password = options[kPasswordOption];
      return new SqlConnectionProvider(builder.ConnectionString);
    }
  }
}
