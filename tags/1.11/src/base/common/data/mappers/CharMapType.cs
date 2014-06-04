using System;

namespace Nohros.Data
{
  /// <summary>
  /// Maps a class property to constant <see cref="char"/>.
  /// </summary>s
  public class CharMapType : TypeMap
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CharMapType"/> class
    /// using the specified column name.
    /// </summary>
    /// <param name="value">
    /// The constant value to be mapped.
    /// </param>
    public CharMapType(char value) : base(value, TypeMapType.Char) {
    }
    #endregion
  }
}
