using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a base implementation of the <see cref="ICriteria{TField,TFilter}"/>
  /// class.
  /// </summary>
  public abstract class AbstractCriteria<TField> : ICriteria<TField>
  {
    readonly HashSet<string> fields_;
    readonly Dictionary<string, object> filters_;
    readonly Dictionary<string, string> maps_;

    #region .ctor
    protected AbstractCriteria() {
      fields_ = new HashSet<string>();
      filters_ = new Dictionary<string, object>();
      maps_ = new Dictionary<string, string>();
    }
    #endregion

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
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      fields_.Add(field);
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

    void BaseWhere(string name, object value) {
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      filters_.Add(field, value);
    }

    string GetMemberName<TProperty, T>(
      Expression<Func<T, TProperty>> expression) {
      MemberExpression member;
      if (expression.Body is UnaryExpression) {
        member = ((UnaryExpression) expression.Body).Operand as MemberExpression;
      } else {
        member = expression.Body as MemberExpression;
      }

      if (member == null) {
        throw new ArgumentException("[member] should be a class property");
      }

      return member.Member.Name;
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

    /// <summary>
    /// Gets the list of fields that should be selected from the database.
    /// </summary>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    public ICollection<string> Fields {
      get { return fields_; }
    }

    /// <summary>
    /// Get a <see cref="IDictionary{TKey,TValue}"/> containing the defined
    /// filtering.
    /// </summary>
    /// <remarks>
    /// For SQL-92 compatible repositories, this represents the clause of the
    /// <c>WHERE</c> clause.
    /// </remarks>
    public IDictionary<string, object> Filters {
      get { return filters_; }
    }
  }
}
