using System;
using System.Data.SqlClient;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Provides an interface for getting connection to a Microsoft SQL Server.
  /// </summary>
  public interface ISqlConnectionProvider : IConnectionProvider<SqlConnection>
  {
  }
}
