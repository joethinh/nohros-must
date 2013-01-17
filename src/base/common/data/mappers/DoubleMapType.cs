using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="double"/>.
  /// </summary>s
  public class DoubleMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public DoubleMapType(double value) : base(value, TypeMapType.Double) {
    }
    #endregion
  }
}
