using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// Represents an json token.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the json token.
  /// </typeparam>
  public interface IJsonToken<out T> : IJsonToken
  {
    /// <summary>
    /// Gets the underlying value.
    /// </summary>
    T Value { get; }
  }
}
