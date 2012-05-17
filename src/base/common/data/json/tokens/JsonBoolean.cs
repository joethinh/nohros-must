using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
  /// json bool token.
  /// </summary>
  public class JsonBoolean: JsonToken<bool>
  {
    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="JsonBoolean"/> class using
    /// the bool <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonBoolean(bool value)
      : base(value) {
    }
    #endregion

    /// <summary>
    /// Gets the json literal (true or false) representation of the
    /// <see cref="IJsonToken{T}"/> class.
    /// </summary>
    /// <returns>
    /// The json literal (true or false) representation of the
    /// <see cref="IJsonToken{T}"/> class. The returned bool is enclosed in
    /// double qutoes.
    /// </returns>
    public override string AsJson() {
      return value ? "true" : "false";
    }

    /// <summary>
    /// Explicit converts an <see cref="Boolean"/> object to a
    /// <see cref="JsonBoolean"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Boolean"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonBoolean"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonBoolean(bool value) {
      return new JsonBoolean(value);
    }
  }
}
