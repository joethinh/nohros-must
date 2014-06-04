using System;
using System.Collections.Generic;
using Nohros.Data.Providers;

namespace Nohros.RestQL
{
  public class SqlQueryDataProviderFactory : IQueryDataProviderFactory
  {
    public IQueryDataProvider CreateCommonDataProvider(
      IDictionary<string, string> options) {
      var factory = new SqlConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlConnectionProvider;
      return new SqlQueryDataProvider(sql_connection_provider);
    }
  }
}
