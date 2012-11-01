using System;
using System.Collections.Generic;
using Nohros.Data.Providers;

namespace Nohros.Metrics
{
  public class SqlMetricsDataProviderFactory : IMetricsDataProviderFactory
  {
    public IMetricsDataProvider CreateMetricsDataProvider(
      IDictionary<string, string> options) {
      var factory = new SqlConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlConnectionProvider;
      return new SqlMetricsDataProvider(sql_connection_provider);
    }
  }
}
