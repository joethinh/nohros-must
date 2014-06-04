using System;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Defines the criteria to be used while resolving a database query.
  /// </summary>
  /// <typeparam name="TField">
  /// The type of object(s) that will be returned by the query.
  /// </typeparam>
  public interface ICriteria<TField> : ICriteria
  {
    /// <summary>
    /// Add the given property to the list of properties to be returned from
    /// the database.
    /// </summary>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    ICriteria<TField> Select<TProperty>(
      Expression<Func<TField, TProperty>> expression);

    /// <summary>
    /// Add the given property/value as a filter.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    ICriteria<TField> Where<TProperty>(
      Expression<Func<TField, TProperty>> expression, object value);
  }

  /// <summary>
  /// Defines the criteria to be used while resolving a database query.
  /// </summary>
  /// <typeparam name="TField">
  /// The type of object(s) that will be returned by the query.
  /// </typeparam>
  public interface ICriteria<TField, TFilter> : ICriteria
  {
    /// <summary>
    /// Add the given property to the list of properties to be returned from
    /// the database.
    /// </summary>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    ICriteria<TField, TFilter> Select<TProperty>(
      Expression<Func<TField, TProperty>> expression);

    /// <summary>
    /// Add the given property/value as a filter.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    ICriteria<TField, TFilter> Where<TProperty>(
      Expression<Func<TField, TProperty>> expression, object value);

    /// <summary>
    /// Add the given property/value as a filter.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    ICriteria<TField, TFilter> Where<TProperty>(
      Expression<Func<TFilter, TProperty>> expression, object value);
  }
}
