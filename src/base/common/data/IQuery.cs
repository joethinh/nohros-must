using System;

namespace Nohros.Data
{
  /// <summary>
  /// Provides functionality to execute queries that retunrs no value against
  /// a specific data provider.
  /// </summary>
  public interface IQuery
  {
    /// <summary>
    /// Executes the query represented by the <see cref="IQuery{T}"/> object.
    /// </summary>
    /// <returns>
    /// The result of the query.
    /// </returns>
    void Execute();
  }
}
