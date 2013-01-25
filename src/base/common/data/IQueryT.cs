using System;

namespace Nohros.Data
{
  /// <summary>
  /// Provides functionality to execute queries that retunrs a value against a
  /// specific data provider.
  /// </summary>
  /// <typeparam name="T">
  /// The type of data in the data provider.
  /// </typeparam>
  public interface IQuery<out T>
  {
    /// <summary>
    /// Executes the query represented by the <see cref="IQuery{T}"/> object.
    /// </summary>
    /// <returns>
    /// The result of the query.
    /// </returns>
    T Execute();
  }
}
