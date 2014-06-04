using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nohros.Extensions;
using Nohros.Providers;

namespace Nohros.Data.SqlCe
{
  /// <summary>
  /// A factory for the <see cref="SqlCeAppState"/> class.
  /// </summary>
  public class SqlAppStateFactory : IProviderFactory<SqlCeAppState>
  {
    /// <summary>
    /// The key that should be associated with the option that caontains the
    /// flag that indicates if transactions should be suppressed.
    /// </summary>
    public const string kSupressTransactions = "supressTransactions";

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
      bool supress_dtc =
        bool.Parse(options.GetString(kSupressTransactions, "false"));
      var states = new SqlCeAppState(sql_connection_provider, supress_dtc);
      states.Initialize();
      return states;
    }
  }
}
