using System;
using System.Data;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the 
  /// </summary>
  public class JsonShort : JsonNumber<short>, IJsonDataField
  {
    readonly short value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonShort"/> class
    /// that uses the general number format("G") to convert this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonShort(short value)
      : this(value, "G") {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonShort"/> class
    /// that uses <paramref name="format"/> to converts this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// The format to use when converting this instance to a string.
    /// </param>
    public JsonShort(short value, string format)
      : base(value, format) {
      value_ = value;
    }
    #endregion

    #region IJsonDataField Members
    /// <summary>
    /// Gets a <see cref="JsonShort"/> object that contains the short value
    /// readed from the <see cref="IDataReader"/> at
    /// <paramref name="position"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a short
    /// value at <paramref name="position"/>.
    /// </param>
    /// <param name="position">
    /// A integer that identifies the position to read the short value that
    /// will be associated with the json short token.
    /// </param>
    /// <returns>
    /// The newly created <see cref="JsonShort"/> object.
    /// </returns>
    IJsonToken IJsonDataField.GetJsonToken(IDataReader reader, int position) {
      return new JsonShort(reader.GetInt16(position));
    }
    #endregion

    /// <summary>
    /// Gets the json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </summary>
    /// <returns>
    /// The json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </returns>
    public override string AsJson() {
      return value_.ToString(format);
    }

    /// <summary>
    /// Explicit converts an <see cref="Int16"/> object to a
    /// <see cref="JsonShort"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Int16"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonShort"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonShort(short value) {
      return new JsonShort(value);
    }
  }
}
