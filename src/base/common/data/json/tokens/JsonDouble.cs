﻿using System;
using System.Data;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that maps a
  /// <see cref="Double"/> type to a json number token.
  /// </summary>
  public class JsonDouble : JsonNumber<double>, IJsonDataField
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDouble"/> class
    /// that uses the general number format("G") to convert this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    public JsonDouble(double value) : this(value, "G") {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDouble"/> class
    /// that uses <paramref name="format"/> to converts this instance to
    /// a string.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// The format to use when converting this instance to a string.
    /// </param>
    public JsonDouble(double value, string format) : base(value, format) {
    }
    #endregion

    #region IJsonDataField Members
    /// <summary>
    /// Gets a <see cref="JsonDouble"/> object that contains the double value
    /// readed from the <see cref="IDataReader"/> at
    /// <paramref name="position"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> that can be used to extract a double
    /// value at <paramref name="position"/>.
    /// </param>
    /// <param name="position">
    /// A integer that identifies the position to read the double value that
    /// will be associated with the json double token.
    /// </param>
    /// <returns>
    /// The newly created <see cref="JsonDouble"/> object.
    /// </returns>
    IJsonToken IJsonDataField.GetJsonToken(IDataReader reader, int position) {
      return new JsonDouble(reader.GetDouble(position));
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
    /// Explicit converts an <see cref="Double"/> object to a
    /// <see cref="JsonDouble"/> object.
    /// </summary>
    /// <param name="value">
    /// The <see cref="Double"/> object to be converted.
    /// </param>
    /// <returns>
    /// A <see cref="JsonDouble"/> that represents the value
    /// <paramref name="value"/>.
    /// </returns>
    public static explicit operator JsonDouble(double value) {
      return new JsonDouble(value);
    }
  }
}