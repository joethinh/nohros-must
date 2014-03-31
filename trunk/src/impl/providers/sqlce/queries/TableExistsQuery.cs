using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using Nohros.Data;
using Nohros.Data.SqlCe.Extensions;
using Nohros.Logging;
using Nohros.Resources;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Data.SqlCe
{
  internal class TableExistsQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.TableExistsQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    public TableExistsQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
    }

    public bool Execute(string table_name) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(@"
select table_name
from information_schema.tables
where table_name = @name")
          .SetType(CommandType.Text)
          .AddParameter("@name", table_name)
          .Build();
        try {
          conn.Open();
          return cmd.ExecuteScalar() != null;
        } catch (SqlCeException e) {
          throw e.AsProviderException();
        }
      }
    }
  }
}
