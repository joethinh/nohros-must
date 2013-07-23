using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Defines the criteria to be used while resolving a database query.
  /// </summary>
  /// <typeparam name="TPoco">
  /// The type of object(s) that will be returned by the query.
  /// </typeparam>
  public abstract class AbstractCriteria<TPoco> : ICriteria
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

    /// <summary>
    /// Gets the list of fields that should be selected from the database.
    /// </summary>
    /// <remarks>
    /// This represents the list of columns of the <c>SELECT</c> clause of
    /// the SQL-92 standard
    /// </remarks>
    public ICollection<string> Fields {
      get { return fields_; }
    }

    /// <summary>
    /// Get a <see cref="IDictionary{TKey,TValue}"/> containing the defined
    /// filtering.
    /// </summary>
    /// <remarks>
    /// This represents the clause of the <c>WHERE</c> clause of the 
    /// the SQL-92 standard
    /// </remarks>
    public IDictionary<string, object> Filters {
      get { return filters_; }
    }

    /// <summary>
    /// Add the given property to the list of properties to be returned from
    /// the database.
    /// </summary>
    protected virtual void BaseSelect<TProperty>(
      Expression<Func<TPoco, TProperty>> expression) {
      string name = GetMemberName(expression);
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      fields_.Add(field);
    }

    protected virtual void BaseWhere<TProperty>(
      Expression<Func<TPoco, TProperty>> expression, object value) {
      string name = GetMemberName(expression);
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      filters_.Add(field, value);
    }

    string GetMemberName<TProperty>(
      Expression<Func<TPoco, TProperty>> expression) {
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
      Expression<Func<TPoco, TProperty>> expression, string destination) {
      string name = GetMemberName(expression);
      maps_.Add(name, destination);
    }
  }
}
