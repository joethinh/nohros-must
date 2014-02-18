using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nohros.Data.Providers;
using Nohros.Extensions;
using Nohros.Providers;
using Nohros.Resources;

namespace Nohros.Data.SqlCe
{
  /// <summary>
  /// A implementation of the <see cref="IConnectionProviderFactory"/> that
  /// create instances of the <see cref="SqlConnection"/> class.
  /// </summary>
  public class SqlCeConnectionProviderFactory : IConnectionProviderFactory,
                                                IProviderFactory
                                                  <SqlCeConnectionProvider>
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
        return new SqlCeConnectionProvider(connection_string);
      }

      var builder = new SqlConnectionStringBuilder();

      // Relative database paths should be resolved using the calling
      // assembly path as base directory.
      string data_source = GetOption(kServerOption, options);
      if (!IO.Path.IsPathRooted(data_source)) {
        data_source = IO.Path.AbsoluteForCallingAssembly(data_source);
      }
      builder.DataSource = data_source;

      string password;
      if (options.TryGetValue(kPasswordOption, out password)) {
        builder.Password = password;
      }
      return new SqlCeConnectionProvider(builder.ConnectionString);
    }

    /// <inheritdoc/>
    object IProviderFactory.CreateProvider(IDictionary<string, string> options) {
      return CreateProvider(options);
    }

    /// <inheritdoc/>
    SqlCeConnectionProvider IProviderFactory<SqlCeConnectionProvider>.
      CreateProvider(IDictionary<string, string> options) {
      return CreateProvider(options) as SqlCeConnectionProvider;
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
