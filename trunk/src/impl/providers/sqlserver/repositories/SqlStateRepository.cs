using System;

namespace Nohros.Data.SqlServer
{
  /// <summary>
  /// An implementation of the <see cref="IStateRepository"/> class for the
  /// Microsfot SQL Server Engine.
  /// </summary>
  public class SqlStateRepository : IStateRepository
  {
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IStateRepository"/>
    /// class using the specified <see cref="SqlConnectionProvider"/>
    /// class.
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlConnectionProvider"/> object that can be used to
    /// get a connection to a SQL server.
    /// </param>
    public SqlStateRepository(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
    }
    #endregion

    /// <inheritdoc/>
    public IStateByNameQuery Query(out IStateByNameQuery query) {
      return query = new StateByNameQuery(sql_connection_provider_);
    }
  }
}
