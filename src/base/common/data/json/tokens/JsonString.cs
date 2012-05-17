using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
  /// json string token.
  /// </summary>
  public class JsonString : JsonToken<string>
  {
    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="JsonString"/> class using
    /// the string <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonString(string value) : base(value) {
    }
    #endregion

    /// <summary>
    /// Returns the string <paramref name="str"/> suitably quoted to be used
    /// as a json string token.
    /// </summary>
    /// <param name="str">
    /// The string to be quoted.
    /// </param>
    /// <returns>
    /// The string <paramref name="str"/> suitably quoted to be used as a
    /// json string token.
    /// </returns>
    public static string QuoteStringToken(string str) {
      return "\"" + str + "\"";
    }

    /// <summary>
    /// Returns the string <paramref name="str"/> suitably quoted to be used
    /// as a json string token or the json null token if <paramref name="str"/>
    /// is <c>null</c>.
    /// </summary>
    /// <param name="str">
    /// The string to be quoted.
    /// </param>
    /// <returns>
    /// The string <paramref name="str"/> suitably quoted to be used as a
    /// json string token.
    /// </returns>
    public static string QuoteStringOrNullToken(string str) {
      return str == null ? "null" : "\"" + str + "\"";
    }

    /// <summary>
    /// Gets the json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </summary>
    /// <returns>
    /// The json string representation of the <see cref="IJsonToken{T}"/>
    /// class. The returned string is enclosed in double qutoes.
    /// </returns>
    public override string AsJson() {
      return QuoteStringOrNullToken(value);
    }

    /// <summary>
    /// Explicit converts an <see cref="String"/> object to a
    /// <see cref="JsonString"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="String"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonString"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonString(string value) {
      return new JsonString(value);
    }
  }
}
