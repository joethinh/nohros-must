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

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value against
  /// a specific data provider.
  /// </summary>
  public interface IQueryWithCriteria<in T>
  {
    /// <summary>
    /// Executes the query represented by the
    /// <see cref="IQueryWithCriteria{T}"/> object.
    /// </summary>
    /// <returns>
    /// The result of the query.
    /// </returns>
    void Execute(T criteria);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs a value against a
  /// specific data provider.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type of data in the data provider.
  /// </typeparam>
  /// <typeparam name="TCriteria">
  /// The type of the object that contains the query criteria.
  /// </typeparam>
  public interface IQuery<out TResult, in TCriteria>
  {
    /// <summary>
    /// Executes the query represented by the
    /// <see cref="IQuery{TResult, TCriteria}"/> object.
    /// </summary>
    /// <param name="criteria">
    /// A object that contains the query criteria(i.e. query filters)
    /// </param>
    /// <returns>
    /// The result of the query.
    /// </returns>
    TResult Execute(TCriteria criteria);
  }

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
