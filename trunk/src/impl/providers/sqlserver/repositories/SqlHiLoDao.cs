using System;

namespace Nohros.Data.SqlServer
{
  public class SqlHiLoDao : IHiLoDao
  {
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlHiLoDao"/> class
    /// by using the given <see cref="SqlConnectionProvider"/> class.
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlConnectionProvider"/> object that can be used to
    /// access the SQL Server engine where the HiLo values are stored.
    /// </param>
    public SqlHiLoDao(SqlConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
    }
    #endregion

    /// <inheritdoc/>
    public long GetNextHi(string key) {
      return new NextHiQuery(sql_connection_provider_).Execute(key);
    }
  }
}
