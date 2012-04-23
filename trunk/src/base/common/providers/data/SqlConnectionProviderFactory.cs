using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nohros.Configuration;

namespace Nohros.Data.Providers
{
  public partial class SqlConnectionProvider : IConnectionProviderFactory
  {
    #region .ctor
    /// <summary>
    /// Constructor implied by the interface
    /// <see cref="IConnectionProviderFactory"/>.
    /// </summary>
    SqlConnectionProvider() {
    }
    #endregion

    #region IConnectionProviderFactory Members
    /// <inheritdoc/>
    IConnectionProvider IConnectionProviderFactory.CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      SqlConnectionStringBuilder builder;
      if (options.TryGetValue(kConnectionStringOption, out connection_string)) {
        builder = new SqlConnectionStringBuilder(connection_string);
      } else {
        string[] data = ProviderOptions.ThrowIfNotExists(options, kServerOption,
          kLoginOption, kPasswordOption);

        const int kServer = 0;
        const int kLogin = 1;
        const int kPassword = 2;

        builder = new SqlConnectionStringBuilder {
          DataSource = data[kServer],
          UserID = data[kLogin],
          Password = data[kPassword]
        };
      }
      return new SqlConnectionProvider(builder.ConnectionString);
    }
    #endregion
  }
}
