using System;
using System.Data;
using System.Data.SqlClient;
using Nohros.Logging;
using R = Nohros.Resources.StringResources;

namespace Nohros.Data.SqlServer
{
  public class SetStateCommand : ISetStateCommand
  {
    const string kClassName = "Nohros.Data.SqlServer.SetStateCommand";

    /// <summary>
    /// The name of the procedure that will be used to persist the state.
    /// </summary>
    public const string kExecute = ".nohros_state_set";

    /// <summary>
    /// The name of the paramter that contains the name of the state.
    /// </summary>
    public const string kNameParameter = "@name";

    /// <summary>
    /// The name of the parameter that contains the state value.
    /// </summary>
    public const string kStateParameter = "@state";

    readonly MustLogger logger_ = MustLogger.ForCurrentProcess;
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SetStateCommand"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public SetStateCommand(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
    }
    #endregion

    /// <inheritdoc/>
    public void Execute(string name, string state) {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(sql_connection_provider_.Schema + kExecute)
          .SetType(CommandType.StoredProcedure)
          .AddParameter("@name", name)
          .AddParameter("@state", state)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlException e) {
          logger_.Error(
            string.Format(R.Log_MethodThrowsException, "Execute", kClassName), e);
          throw new ProviderException(e);
        }
      }
    }
  }
}
