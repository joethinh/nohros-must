using System;
using System.Data;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that maps a
  /// <see cref="Single"/> type to a json number token.
  /// </summary>
  public class JsonFloat : JsonNumber<float>, IJsonDataField
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFloat"/> class
    /// that uses the general number format("G") to convert this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonFloat(float value) : this(value, "G") {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFloat"/> class
    /// that uses <paramref name="format"/> to converts this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// The format to use when converting this instance to a string.
    /// </param>
    public JsonFloat(float value, string format)
      : base(value, format) {
    }
    #endregion

    #region IJsonDataField Members
    /// <summary>
    /// Gets a <see cref="JsonFloat"/> object that contains the float value
    /// readed from the <see cref="IDataReader"/> at
    /// <paramref name="position"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a float
    /// value at <paramref name="position"/>.
    /// </param>
    /// <param name="position">
    /// A integer that identifies the position to read the float value that
    /// will be associated with the json float token.
    /// </param>
    /// <returns>
    /// The newly created <see cref="JsonFloat"/> object.
    /// </returns>
    IJsonToken IJsonDataField.GetJsonToken(IDataReader reader, int position) {
      return new JsonFloat(reader.GetFloat(position));
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
    /// Explicit converts an <see cref="Single"/> object to a
    /// <see cref="JsonFloat"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Single"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonFloat"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonFloat(float value) {
      return new JsonFloat(value);
    }
  }
}
