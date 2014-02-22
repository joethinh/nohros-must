using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Transactions;
using Nohros.Data;
using Nohros.Logging;
using Nohros.Resources;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Data.SqlCe
{
  public class GetStateQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.GetStateQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    public GetStateQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      SupressTransactions = true;
    }

    public bool Execute<T>(string state_name, string table_name, out T state) {
      using (
        new TransactionScope(SupressTransactions
          ? TransactionScopeOption.Suppress
          : TransactionScopeOption.Required)) {
        using (
          SqlCeConnection conn = sql_connection_provider_.CreateConnection())
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
          } catch (SqlCeException e) {
            logger_.Error(
              StringResources.Log_MethodThrowsException.Fmt("Execute",
                kClassName),
              e);
            throw new ProviderException(e);
          }
        }
      }
    }

    public bool SupressTransactions { get; set; }
  }
}
