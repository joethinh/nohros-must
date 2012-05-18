using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// A <see cref="IDataField"/> defines a field associated with a
  /// <see cref="IDataReader"/>.
  /// </summary>
  public interface IDataField
  {
    /// <summary>
    /// Gets zero-based column ordinal of the field.
    /// </summary>
    int Position { get; }

    /// <summary>
    /// Gets the name of the field.
    /// </summary>
    string Name { get; }
  }
}
