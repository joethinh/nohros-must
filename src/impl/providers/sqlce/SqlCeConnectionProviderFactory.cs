using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nohros.Data.Providers;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Data.SqlServer
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProviderFactory"/> that
  /// create instances of the <see cref="SqlConnection"/> class.
  /// </summary>
  public class SqlCeConnectionProviderFactory : IConnectionProviderFactory
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
    /// the password to use to connects to the Sql Server.
    /// </summary>
    public const string kPasswordOption = "password";

    /// <inheritdoc/>
    public IConnectionProvider CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      if (options.TryGetValue(kConnectionStringOption, out connection_string)) {
        return new SqlConnectionProvider(connection_string);
      }

      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = GetOption(kServerOption, options);

      string password;
      if (options.TryGetValue(kPasswordOption, out password)) {
        builder.Password = password;
      }
      return new SqlConnectionProvider(builder.ConnectionString);
    }

    string GetOption(string name, IDictionary<string, string> options) {
      string option;
      if (!options.TryGetValue(name, out option)) {
        throw new KeyNotFoundException(
          string.Format(StringResources.Arg_KeyNotFound, name));
      }
      return option;
    }
  }
}
