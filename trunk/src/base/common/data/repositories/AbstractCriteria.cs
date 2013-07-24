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
    const string kFilterMapPrefix = "FL";
    const string kFieldMapPrefix = "FD";

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
    /// For SQL-92 compatible repositories, this represents the list of columns
    /// of the <c>SELECT</c> clause.
    /// </remarks>
    public ICollection<string> Fields {
      get { return fields_; }
    }

    /// <inheritdoc/>
    public string GetFieldMap(string key) {
      string map;
      if (TryGetMap(kFieldMapPrefix + key, out map)) {
        return map;
      }
      return key;
    }

    /// <inheritdoc/>
    public string GetFilterMap(string key) {
      string map;
      if (TryGetMap(kFilterMapPrefix + key, out map)) {
        return map;
      }
      return key;
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

    /// <inheritdoc/>
    bool TryGetMap(string key, out string map) {
      return maps_.TryGetValue(key, out map);
    }

    /// <summary>
    /// Maps a property to a field.
    /// </summary>
    public void MapField(string key, string map) {
      Map(kFieldMapPrefix + key, map);
    }

    /// <summary>
    /// Maps a property to a filter field.
    /// </summary>
    public void MapFilter(string key, string map) {
      Map(kFilterMapPrefix + key, map);
    }

    void Map(string key, string map) {
      maps_[key] = map;
    }

    internal void Where(string name, object value) {
      string key = GetFilterMap(name);
      filters_.Add(key, name);
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
