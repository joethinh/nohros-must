using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// Represents an json token.
  /// </summary>
  public interface IJsonToken
  {
    /// <summary>
    /// Gets the json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </summary>
    /// <returns>
    /// The json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </returns>
    string AsJson();
  }
}
