using System;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a base implementation of the <see cref="ICriteria{TField,TFilter}"/>
  /// class.
  /// </summary>
  public abstract class AbstractCriteria<TField, TFilter> :
    AbstractCriteria, ICriteria<TField, TFilter>
  {
    /// <inheritdoc/>
    public virtual ICriteria<TField, TFilter> Where<TProperty>(
      Expression<Func<TFilter, TProperty>> expression, object value) {
      BaseWhere(expression, value);
      return this;
    }

    /// <inheritdoc/>
    public virtual ICriteria<TField, TFilter> Select<TProperty>(
      Expression<Func<TField, TProperty>> expression) {
      BaseSelect(expression);
      return this;
    }

    /// <inheritdoc/>
    public virtual ICriteria<TField, TFilter> Where<TProperty>(
      Expression<Func<TField, TProperty>> expression, object value) {
      BaseWhere(expression, value);
      return this;
    }

    /// <summary>
    /// Add the given property to the list of properties to be returned from
    /// the database.
    /// </summary>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    protected virtual void BaseSelect<TProperty>(
      Expression<Func<TField, TProperty>> expression) {
      string name = GetMemberName(expression);
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      Fields.Add(field);
    }

    /// <summary>
    /// Add the given property/value as a filter.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    protected virtual void BaseWhere<TProperty>(
      Expression<Func<TField, TProperty>> expression, object value) {
      string name = GetMemberName(expression);
      BaseWhere(name, value);
    }

    /// <summary>
    /// Add the given property/value as a filter.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    protected virtual void BaseWhere<TProperty>(
      Expression<Func<TFilter, TProperty>> expression, object value) {
      string name = GetMemberName(expression);
      BaseWhere(name, value);
    }

    /// <summary>
    /// Maps a property to a database field.
    /// </summary>
    /// <typeparam name="TProperty">
    /// The type of property to be mapped
    /// </typeparam>
    /// <param name="expression">
    /// The property to be mapped
    /// </param>
    /// <param name="destination">
    /// The name of the database field that should be mapped to the propertyd
    /// defined by <paramref name="expression"/>.
    /// </param>
    protected void Map<TProperty>(
      Expression<Func<TField, TProperty>> expression, string destination) {
      string name = GetMemberName(expression);
      maps_.Add(name, destination);
    }
  }
}
