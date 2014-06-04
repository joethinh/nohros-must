using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="float"/>.
  /// </summary>s
  public class FloatMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="FloatMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public FloatMapType(float value) : base(value, TypeMapType.Float) {
    }
    #endregion
  }
}
