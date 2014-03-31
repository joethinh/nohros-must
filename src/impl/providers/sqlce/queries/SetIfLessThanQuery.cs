using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Transactions;
using Nohros.Data;
using Nohros.Data.SqlCe.Extensions;
using Nohros.Logging;
using Nohros.Resources;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Data.SqlCe
{
  public class SetIfLessThanQuery
  {
    const string kClassName = "Nohros.Data.SqlCe.SetIfGreaterThanQuery";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetIfGreaterThanQuery"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlCeConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public SetIfLessThanQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      SupressTransactions = true;
    }

    public bool Execute(string name, string table_name, long state,
      long comparand) {
      using (
        new TransactionScope(SupressTransactions
          ? TransactionScopeOption.Suppress
          : TransactionScopeOption.Required)) {
        using (
          SqlCeConnection conn = sql_connection_provider_.CreateConnection())
        using (var builder = new CommandBuilder(conn)) {
          IDbCommand cmd = builder
            .SetText(@"
update " + table_name + @"
set state = @state" + @"
where name = @name
  and state < @comparand")
            .SetType(CommandType.Text)
            .AddParameter("@name", name)
            .AddParameterWithValue("@state", state)
            .AddParameterWithValue("@comparand", comparand)
            .Build();
          try {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          } catch (SqlCeException e) {
            throw e.AsProviderException();
          }
        }
      }
    }

    public bool SupressTransactions { get; set; }
  }
}
