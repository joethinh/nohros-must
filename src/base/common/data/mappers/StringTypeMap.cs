using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to a value of a column from a
  /// <see cref="IDataReader"/>.
  /// </summary>
  public class StringTypeMap : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="StringTypeMap"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The name of the column to use as data source for the map operation.
    /// </param>
    public StringTypeMap(string value) : base(value, TypeMapType.String) {
    }
    #endregion
  }
}
