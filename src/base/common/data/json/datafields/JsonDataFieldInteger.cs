using System;
using System.Data;

namespace Nohros.Data.Json
{
  public class JsonDataFieldInteger : DataFieldInteger, IJsonDataField
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDataFieldInteger"/>
    /// class using the specified field <paramref name="name"/> and
    /// ordianl <paramref name="position"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero-based positon of the field within a <see cref="IDataReader"/>.
    /// </param>
    public JsonDataFieldInteger(string name, int position)
      : base(name, position) {
    }
    #endregion

    #region IJsonDataField Members
    /// <summary>
    /// Gets the a <see cref="IJsonToken{T}"/> object that represents the
    /// the field's value.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a
    /// <see cref="Int32"/> at the field position.
    /// </param>
    public IJsonToken GetValue(IDataReader reader) {
      return new JsonInteger(base.GetValue(reader));
    }
    #endregion
  }
}
