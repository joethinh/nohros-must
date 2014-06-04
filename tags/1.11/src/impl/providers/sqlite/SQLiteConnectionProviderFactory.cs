using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Nohros.Data.Providers;

namespace Nohros.Data.SQLite
{
  public class SQLiteConnectionProviderFactory : IConnectionProviderFactory
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
    /// The key that should be associated with the option that contains the
    /// cache size to use by the sqlite databse connection.
    /// </summary>
    public const string kCacheSizeOption = "cacheSize";

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
    public SQLiteConnectionProviderFactory() {
    }
    #endregion

    /// <inheritdoc/>
    public IConnectionProvider CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      if (options.TryGetValue(kConnectionStringOption, out connection_string)) {
        return new SQLiteConnectionProvider(connection_string);
      }

      SQLiteConnectionStringBuilder builder =
        new SQLiteConnectionStringBuilder();
      builder.DataSource = options[kInitialCatalogOption];

      // Set the optional parameters.
      string option;
      if (options.TryGetValue(kCacheSizeOption, out option)) {
        builder.CacheSize = int.Parse(option);
      }

      if (options.TryGetValue(kPasswordOption, out option)) {
        builder.Password = option;
      }
      return new SQLiteConnectionProvider(builder.ConnectionString);
    }
  }
}
