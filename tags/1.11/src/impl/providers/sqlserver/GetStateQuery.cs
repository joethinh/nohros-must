using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Nohros.Data.SqlServer.Extensions;
using Nohros.Logging;
using Nohros.Resources;
using Nohros.Extensions;

namespace Nohros.Data.SqlServer
{
  public class GetStateQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.GetStateQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlConnectionProvider sql_connection_provider_;

    public GetStateQuery(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      SupressTransactions = true;
    }

    public bool Execute<T>(string state_name, string table_name, out T state) {
      using (
        new TransactionScope(SupressTransactions
          ? TransactionScopeOption.Suppress
          : TransactionScopeOption.Required)) {
        using (SqlConnection conn = sql_connection_provider_.CreateConnection())
        using (var builder = new CommandBuilder(conn)) {
          IDbCommand cmd = builder
            .SetText("select state from " + table_name + " where name=@name")
            .SetType(CommandType.Text)
            .AddParameter("@name", state_name)
            .Build();
          try {
            conn.Open();
            object obj = cmd.ExecuteScalar();
            if (obj == null) {
              state = default(T);
              return false;
            }
            state = (T) obj;
            return true;
          } catch (SqlException e) {
            logger_.Error(
              StringResources.Log_MethodThrowsException.Fmt("Execute",
                kClassName),
              e);
            throw e.AsProviderException();
          }
        }
      }
    }

    public bool SupressTransactions { get; set; }
  }
}
