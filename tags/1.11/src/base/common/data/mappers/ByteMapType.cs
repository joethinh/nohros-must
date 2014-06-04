using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="byte"/>.
  /// </summary>s
  public class ByteMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ByteMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public ByteMapType(byte value) : base(value, TypeMapType.Byte) {
    }
    #endregion
  }
}
