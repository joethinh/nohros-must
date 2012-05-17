using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An abstract implementation of the <see cref="IJsonToken{T}"/> that serves
  /// as  a base class to types that maps to a json number token.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the underlying value.
  /// </typeparam>
  public abstract class JsonNumber<T> : JsonToken<T>
  {
    /// <summary>
    /// The format used to convert the underlying value to a string.
    /// </summary>
    protected readonly string format;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonNumber{T}"/> class using
    /// the specified number format.
    /// </summary>
    /// <param name="value">
    /// The value to be associated with this class.
    /// </param>
    /// <param name="format">
    /// A standard or custom number format.
    /// </param>
    protected JsonNumber(T value, string format) : base(value) {
      this.format = format;
    }
    #endregion
  }
}
