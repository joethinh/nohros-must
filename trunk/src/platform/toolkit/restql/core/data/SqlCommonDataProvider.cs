using System;
using System.Data;
using System.Data.SqlClient;
using Nohros.Data;
using Nohros.Data.Providers;

namespace Nohros.Toolkit.RestQL
{
  public class SqlCommonDataProvider : CommonDataProvider
  {
    readonly IConnectionProvider connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IConnectionProvider"/>
    /// using the specified <see cref="IConnectionProvider"/> connection
    /// provider.
    /// </summary>
    /// <param name="connection_provider">
    /// A <see cref="IConnectionProvider"/> that is used to create connections
    /// and query a SQL server.
    /// </param>
    public SqlCommonDataProvider(IConnectionProvider connection_provider) {
#if DEBUG
      if (connection_provider == null)
        throw new ArgumentException("connection_provider");
#endif
      connection_provider_ = connection_provider;
    }
    #endregion

    /// <inheritdoc/>
    public override Query GetQuery(string name) {
      const string kGetQuery = "rql_get_query";

      using (var conn = connection_provider_.CreateConnection())
      using (var cmd = conn.CreateCommand()) {
        cmd.CommandText = connection_provider_.Schema + kGetQuery;
        cmd.CommandType = CommandType.StoredProcedure;

        DataParameters.CreateParameter(cmd, "name", DbType.String, 260).Value =
          name;

        conn.Open();

        using(IDataReader dr = cmd.ExecuteReader()) {
          return CreatedQueryFromDataReader(dr);
        }
      }
    }
  }
}
