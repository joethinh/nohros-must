using System;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a base implementation of the <see cref="ICriteria{TField,TFilter}"/>
  /// class.
  /// </summary>
  public abstract class AbstractCriteria<TField> : AbstractCriteria,
                                                   ICriteria<TField>
  {
    /// <inheritdoc/>
    public virtual ICriteria<TField> Select<TProperty>(
      Expression<Func<TField, TProperty>> expression) {
      BaseSelect(expression);
      return this;
    }

    /// <inheritdoc/>
    public virtual ICriteria<TField> Where<TProperty>(
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
      string field = GetFieldMap(name);
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
      Where(name, value);
    }

    /// <summary>
    /// Maps a property to query field.
    /// </summary>
    public void Map<TProperty>(
      Expression<Func<TField, TProperty>> expression, string map) {
      string name = GetMemberName(expression);
      MapField(name, map);
    }
  }
}
