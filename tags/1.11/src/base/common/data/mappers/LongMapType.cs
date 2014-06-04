using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="long"/>.
  /// </summary>s
  public class LongMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LongMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public LongMapType(long value) : base(value, TypeMapType.Long) {
    }
    #endregion
  }
}
