using System;
using System.Collections.Generic;

namespace Nohros.Data.SqlServer
{
  /// <summary>
  /// An implementation of the <see cref="IStateDaoFactory"/> class for the
  /// SQL Server.
  /// </summary>
  public class SqlStateDaoFactory : IStateDaoFactory
  {
    public IStateDao CreateStateDao(IDictionary<string, string> options) {
      var factory = new SqlConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlConnectionProvider;
      return new SqlStateDao(sql_connection_provider);
    }
  }
}
