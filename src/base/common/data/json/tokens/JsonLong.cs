using System;
using System.Data;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that maps a
  /// <see cref="Int64"/> type to a json number token.
  /// </summary>
  public class JsonLong : JsonNumber<long>, IJsonDataField
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonLong"/> class
    /// that uses the general number format("G") to convert this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonLong(long value) : this(value, "G") {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonLong"/> class
    /// that uses <paramref name="format"/> to converts this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// The format to use when converting this instance to a string.
    /// </param>
    public JsonLong(long value, string format) : base(value, format) {
    }
    #endregion

    #region IJsonDataField Members
    /// <summary>
    /// Gets a <see cref="JsonLong"/> object that contains the long value
    /// readed from the <see cref="IDataReader"/> at
    /// <paramref name="position"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a long
    /// value at <paramref name="position"/>.
    /// </param>
    /// <param name="position">
    /// A integer that identifies the position to read the long value that
    /// will be associated with the json long token.
    /// </param>
    /// <returns>
    /// The newly created <see cref="JsonLong"/> object.
    /// </returns>
    IJsonToken IJsonDataField.GetJsonToken(IDataReader reader, int position) {
      return new JsonLong(reader.GetInt64(position));
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
      return value.ToString(format);
    }

    /// <summary>
    /// Explicit converts an <see cref="Int64"/> object to a
    /// <see cref="JsonLong"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Int64"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonLong"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonLong(long value) {
      return new JsonLong(value);
    }
  }
}
