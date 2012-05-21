using System;
using System.Data;

namespace Nohros.Data.Json
{
  public class JsonDataFieldShort: DataFieldShort, IJsonDataField<short>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDataFieldShort"/>
    /// class using the specified field <paramref name="name"/> and
    /// ordianl <paramref name="position"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero-based positon of the field within a <see cref="IDataReader"/>.
    /// </param>
    public JsonDataFieldShort(string name, int position)
      : base(name, position) {
    }
    #endregion

    #region IJsonDataField<short> Members
    /// <summary>
    /// Gets the a <see cref="IJsonToken{T}"/> object that represents the
    /// the field's value.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a
    /// <see cref="Int16"/> value at the field position.
    /// </param>
    public IJsonToken<short> GetValue(IDataReader reader) {
      return new JsonShort(base.GetValue(reader));
    }
    #endregion
  }
}
