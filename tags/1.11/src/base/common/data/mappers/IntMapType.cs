using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="int"/>.
  /// </summary>s
  public class IntMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IntMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public IntMapType(int value) : base(value, TypeMapType.Int) {
    }
    #endregion
  }
}
