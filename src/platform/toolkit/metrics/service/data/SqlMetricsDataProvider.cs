using System;
using System.Data;
using System.Data.SqlClient;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Resources;

namespace Nohros.Metrics
{
  public class SqlMetricsDataProvider : IMetricsDataProvider
  {
    const string kClassName = "Nohros.Metrics.SqlMetricsDataProvider";
    const string kStoreQuery = ".mtrc_metric_store";

    readonly MetricsLogger logger_;
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    public SqlMetricsDataProvider(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MetricsLogger.ForCurrentProcess;
    }
    #endregion

    public void Store(string name, double value, long timestamp) {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kStoreQuery)
          .AddParameter("@name", name, DbType.String)
          .AddParameter("@timestamp", timestamp, DbType.Int64)
          .AddParameter("@value", value, DbType.Double)
          .SetType(CommandType.StoredProcedure)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlException e) {
          logger_.Error(string.Format(
            StringResources.Log_MethodThrowsException, kClassName,
            "Store"));
        }
      }
    }
  }
}
