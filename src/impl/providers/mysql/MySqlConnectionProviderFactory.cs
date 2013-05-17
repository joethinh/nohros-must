using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Nohros.Data.Providers;

namespace Nohros.Data.MySql
{
  public class MySqlConnectionProviderFactory : IConnectionProviderFactory
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
    public const string kLoginOption = "username";

    /// <summary>
    /// The key that should be associated with the option that contains
    /// the password to use to connects to the Sql Server.
    /// </summary>
    public const string kPasswordOption = "password";

    /// <summary>
    /// The key that should be associated with the options that contains the
    /// initial catalog
    /// </summary>
    public const string kInitialCatalogOption = "database";

    #region .ctor
    public MySqlConnectionProviderFactory() {
    }
    #endregion

    /// <inheritdoc/>
    public IConnectionProvider CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      if (options.TryGetValue(kConnectionStringOption, out connection_string)) {
        return new SqlConnectionProvider(connection_string);
      }

      MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
      builder.Database = options[kInitialCatalogOption];
      builder.Password = options[kPasswordOption];
      builder.UserID = options[kLoginOption];
      builder.Server = options[kServerOption];
      return new MySqlConnectionProvider(builder.ConnectionString);
    }
  }
}
