using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nohros.Data.Providers;

namespace Nohros.Data.SqlServer
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
    const string kLoginOption = "login";

    /// <summary>
    /// The key that should be associated with the option that contains
    /// the login to be used to connect to the Sql Server.
    /// </summary>
    public const string kUserNameOption = "username";

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

      // We try to get the user name information using the "login" key for
      // backward compatibility.
      string user_id;
      if (!options.TryGetValue(kLoginOption, out user_id)) {
        user_id = options[kUserNameOption];
      }
      builder.UserID = user_id;
      builder.Password = options[kPasswordOption];
      builder.InitialCatalog = options[kInitialCatalogOption];
      return new SqlConnectionProvider(builder.ConnectionString);
    }
  }
}
