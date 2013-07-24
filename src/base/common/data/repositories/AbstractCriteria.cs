using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a base implementation of the <see cref="ICriteria{TField,TFilter}"/>
  /// class.
  /// </summary>
  public abstract class AbstractCriteria : ICriteria
  {
    readonly HashSet<string> fields_;
    readonly Dictionary<string, object> filters_;
    internal readonly Dictionary<string, string> maps_;

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
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    public ICollection<string> Fields {
      get { return fields_; }
    }

    /// <inheritdoc/>
    public IDictionary<string, string> Map {
      get { return maps_; }
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

    internal void BaseWhere(string name, object value) {
      string field;
      if (!maps_.TryGetValue(name, out field)) {
        field = name;
      }
      filters_.Add(field, value);
    }

    internal string GetMemberName<TProperty, T>(
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
  }
}
