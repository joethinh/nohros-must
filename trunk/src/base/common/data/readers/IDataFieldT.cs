using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// A <see cref="IDataField{T}"/> is an enhanced version of the
  /// <see cref="IDataReader"/> interface that allows callers to retrieve
  /// the native value associated with the data field.
  /// </summary>
  /// <seealso cref="IDataReader"/>
  public interface IDataField<out T>: IDataField
  {
    /// <summary>
    /// Gets the value of the field in its native format.
    /// </summary>
    T GetValue(IDataReader reader);
  }
}
