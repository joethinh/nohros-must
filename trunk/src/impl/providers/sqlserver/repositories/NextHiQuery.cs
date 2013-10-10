using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nohros.Data;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Data.SqlServer
{
  public class NextHiQuery
  {
    const string kClassName = "Nohros.Data.SqlServer.NextHiQuery";
    const string kExecute = ".nohros_hilo_get_next_hi";
    const string kKeyParameter = "@key";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="NextHiQuery"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public NextHiQuery(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
    }
    #endregion

    public long Execute(string key) {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(sql_connection_provider_.Schema + kExecute)
          .SetType(CommandType.StoredProcedure)
          .AddParameter(kKeyParameter, key)
          .Build();
        try {
          conn.Open();
          object obj = cmd.ExecuteScalar();
          if (obj != null) {
            return (long) obj;
          }
          throw new NoResultException();
        } catch (SqlException e) {
          logger_.Error(string.Format(
            StringResources.Log_MethodThrowsException, "Execute", kClassName), e);
          throw new ProviderException(e);
        }
      }
    }
  }
}
