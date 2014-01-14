using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nohros.Providers;

namespace Nohros.Data.SqlCe
{
  /// <summary>
  /// A factory for the <see cref="SqlCeAppState"/> class.
  /// </summary>
  public class SqlAppStateFactory : IProviderFactory<SqlCeAppState>
  {
    object IProviderFactory.CreateProvider(IDictionary<string, string> options) {
      return CreateProvider(options);
    }

    public SqlCeAppState CreateProvider(IDictionary<string, string> options) {
      return CreateAppState(options) as SqlCeAppState;
    }

    public IAppState CreateAppState(IDictionary<string, string> options) {
      var factory = new SqlCeConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlCeConnectionProvider;
      var states = new SqlCeAppState(sql_connection_provider);
      states.Initialize();
      return states;
    }
  }
}
