using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// An implementation of the <see cref="IDataField{T}"/> that maps a
  /// <see cref="Boolean"/> data type to a concrete <see cref="IDataField{T}"/>
  /// object.
  /// </summary>
  public class DataFieldBoolean : DataField<bool>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DataFieldBoolean"/> class by
    /// using the specified field name and position.
    /// </summary>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero based ordinal position of the field within an
    /// <see cref="IDataReader"/>.
    /// </param>
    public DataFieldBoolean(string name, int position)
      : base(name, position) {
    }
    #endregion

    /// <summary>
    /// Gets the value of the field as a <see cref="Boolean"/>
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a
    /// <see cref="Boolean"/> at the field position.
    /// </param>
    public override bool GetValue(IDataReader reader) {
      return reader.GetBoolean(position);
    }
  }
}
