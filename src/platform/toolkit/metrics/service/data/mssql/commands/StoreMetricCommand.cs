using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Resources;

namespace Nohros.Metrics.Data.Sql
{
  public class SqlStoreMetricCommand : StoreMetricCommand
  {
    const string kClassName = "Nohros.Metrics.StoreMetricCommand";
    const string kExecute = "";

    readonly MetricsLogger logger_ = MetricsLogger.ForCurrentProcess;
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="StoreMetricCommand"/>
    /// using the specified sql connection provider.
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public SqlStoreMetricCommand(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MetricsLogger.ForCurrentProcess;
    }
    #endregion

    /// <inheritdoc/>
    public override void Execute() {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kExecute)
          .AddParameter("@name", Name)
          .AddParameter("@timestamp", Timestamp)
          .AddParameter("@value", Value, DbType.Double)
          .SetType(CommandType.StoredProcedure)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlException e) {
          logger_.Error(string.Format(
            StringResources.Log_MethodThrowsException, kClassName,
            "Store"), e);
          throw new ProviderException(e);
        }
      }
    }
  }
}
