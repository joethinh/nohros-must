using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="short"/>.
  /// </summary>
  public class ShortMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ShortMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public ShortMapType(short value) : base(value, TypeMapType.Short) {
    }
    #endregion
  }
}
