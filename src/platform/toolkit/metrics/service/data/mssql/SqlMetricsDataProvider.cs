using System;
using Nohros.Data.Providers;

namespace Nohros.Metrics.Data.Sql
{
  public class SqlMetricsDataProvider : IMetricsRepository
  {
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    public SqlMetricsDataProvider(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
    }
    #endregion

    public StoreMetricCommand Query(out StoreMetricCommand query) {
      query = new SqlStoreMetricCommand(sql_connection_provider_);
      return query;
    }
  }
}
