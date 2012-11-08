using System;
using System.Data;
using System.Globalization;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that maps a
  /// <see cref="Int64"/> type to a json number token.
  /// </summary>
  public class JsonLong : JsonNumber<long>
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

    /// <summary>
    /// Gets the json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </summary>
    /// <returns>
    /// The json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </returns>
    public override string AsJson() {
      return value.ToString(format, NumberFormatInfo.InvariantInfo);
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
