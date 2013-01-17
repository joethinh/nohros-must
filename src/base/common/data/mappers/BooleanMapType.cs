using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="bool"/>.
  /// </summary>s
  public class BooleanMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public BooleanMapType(bool value) : base(value, TypeMapType.Boolean) {
    }
    #endregion
  }
}
