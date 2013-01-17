using System;

namespace Nohros.Data
{
  /// <summary>
  /// Defines the type that a <see cref="ITypeMap"/> maps to.
  /// </summary>
  public enum TypeMapType
  {
    /// <summary>
    /// Maps to a column from the datareader.
    /// </summary>
    String = 0,

    /// <summary>
    /// Maps to a constant string.
    /// </summary>
    ConstString = 1,

    /// <summary>
    /// Maps to a constant <see cref="int"/>.
    /// </summary>
    Int = 2,

    /// <summary>
    /// Maps to a constant <see cref="bool"/>
    /// </summary>
    Boolean = 3,

    /// <summary>
    /// Maps to a constant <see cref="long"/>
    /// </summary>
    Long = 4,

    /// <summary>
    /// Maps to a constant <see cref="float"/>
    /// </summary>
    Float = 5,

    /// <summary>
    /// Maps to a constant <see cref="double"/>
    /// </summary>
    Double = 6,

    /// <summary>
    /// Maps to a constant <see cref="char"/>
    /// </summary>
    Char = 7,

    /// <summary>
    /// Maps to a constant <see cref="byte"/>
    /// </summary>
    Byte = 8,

    /// <summary>
    /// Maps to a constant <see cref="short"/>
    /// </summary>
    Short = 9,

    /// <summary>
    /// Maps to the default value of a type.
    /// </summary>
    Ignore = 10
  }
}
