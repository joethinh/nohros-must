using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="string"/>.
  /// </summary>s
  public class ConstStringMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstStringMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public ConstStringMapType(string value)
      : base(value, TypeMapType.ConstString) {
    }
    #endregion
  }
}
