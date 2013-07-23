using System;
using System.Collections.Generic;

namespace Nohros.Data
{
  /// <summary>
  /// Defines the criteria to be used while resolving a database query.
  /// </summary>
  public interface ICriteria
  {
    /// <summary>
    /// Gets the list of fields that should be selected from the database.
    /// </summary>
    /// <remarks>
    /// This represents the list of columns of the <c>SELECT</c> clause of
    /// the SQL-92 standard
    /// </remarks>
    ICollection<string> Fields { get; }

    /// <summary>
    /// Get a <see cref="IDictionary{TKey,TValue}"/> containing the defined
    /// filtering.
    /// </summary>
    /// <remarks>
    /// This represents the clause of the <c>WHERE</c> clause of the 
    /// the SQL-92 standard
    /// </remarks>
    IDictionary<string, object> Filters { get; }
  }
}
