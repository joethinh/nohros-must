using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Nohros.Data.SqlServer;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Data.SqlCe
{
  internal class UpdateStateQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.UpdateStateQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlConnectionProvider sql_connection_provider_;

    public UpdateStateQuery(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      SupressTransactions = true;
    }

    public void Execute(string name, string table_name, object state) {
      using (
        new TransactionScope(SupressTransactions
          ? TransactionScopeOption.Suppress
          : TransactionScopeOption.Required)) {
        using (
          SqlConnection conn = sql_connection_provider_.CreateConnection())
        using (var builder = new CommandBuilder(conn)) {
          IDbCommand cmd = builder
            .SetText(@"
update " + table_name + @"
set state = @state" + @"
where name = @name")
            .SetType(CommandType.Text)
            .AddParameter("@name", name)
            .AddParameterWithValue("@state", state)
            .Build();
          try {
            conn.Open();
            cmd.ExecuteNonQuery();
          } catch (SqlException e) {
            throw new ProviderException(e);
          }
        }
      }
    }

    public bool SupressTransactions { get; set; }
  }
}
