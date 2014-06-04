using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// An implementation of the <see cref="IDataField{T}"/> that maps a
  /// <see cref="Single"/> data type to a concrete <see cref="IDataField{T}"/>
  /// object.
  /// </summary>
  public class DataFieldFloat: DataField<float>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DataFieldFloat"/> class by
    /// using the specified field name and position.
    /// </summary>
    /// <param name="name">
    /// THe name of the field.
    /// </param>
    /// <param name="position">
    /// The zero based ordinal position of the field within an
    /// <see cref="IDataReader"/>.
    /// </param>
    public DataFieldFloat(string name, int position)
      : base(name, position) {
    }

    /// <summary>
    /// Gets the value of the field as a <see cref="Single"/>
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a
    /// <see cref="Single"/> at the field position.
    /// </param>
    public override float GetValue(IDataReader reader) {
      return reader.GetFloat(position);
    }
  }
}
