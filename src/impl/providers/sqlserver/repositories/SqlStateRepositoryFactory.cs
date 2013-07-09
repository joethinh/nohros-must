using System;
using System.Collections.Generic;

namespace Nohros.Data.SqlServer
{
  public class SqlStateRepositoryFactory : IStateRepositoryFactory
  {
    public IStateRepository CreateStateRepository(
      IDictionary<string, string> options) {
      var factory = new SqlConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlConnectionProvider;
      return new SqlStateRepository(sql_connection_provider);
    }
  }
}
