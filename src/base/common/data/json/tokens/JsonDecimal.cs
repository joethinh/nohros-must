using System;
using System.Data;
using System.Globalization;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that maps a
  /// <see cref="Decimal"/> type to a json number token.
  /// </summary>
  public class JsonDecimal : JsonNumber<decimal>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDecimal"/> class
    /// that uses the general number format("G") to convert this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonDecimal(decimal value)
      : this(value, "G") {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDecimal"/> class
    /// that uses <paramref name="format"/> to converts this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// The format to use when converting this instance to a string.
    /// </param>
    public JsonDecimal(decimal value, string format)
      : base(value, format) {
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
    /// Explicit converts an <see cref="Decimal"/> object to a
    /// <see cref="JsonDecimal"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Decimal"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonDecimal"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonDecimal(decimal value) {
      return new JsonDecimal(value);
    }
  }
}
