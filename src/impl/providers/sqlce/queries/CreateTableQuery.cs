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
  internal class CreateTableQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.CreateTableQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;
    readonly TableExistsQuery table_exists_;

    public CreateTableQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      table_exists_ = new TableExistsQuery(sql_connection_provider);
    }

    public void Execute(string table_name, string state_type) {
      if (!table_exists_.Execute(table_name)) {
        ExecuteInternal(table_name, state_type);
      }
    }

    void ExecuteInternal(string table_name, string state_type) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(@"
create table " + table_name + @"(
  name nvarchar(1024), state " + state_type + ")")
          .SetType(CommandType.Text)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlCeException e) {
          e.AsProviderException();
        }
      }
    }
  }
}
