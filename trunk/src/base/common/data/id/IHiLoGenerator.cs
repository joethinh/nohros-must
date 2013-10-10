using System;

namespace Nohros.Data
{
  /// <summary>
  /// A class that is used to generate integer identity values using the Hi/Lo
  /// algorithm.
  /// </summary>
  public interface IHiLoGenerator
  {
    /// <summary>
    /// Generate the next global available identity.
    /// </summary>
    /// <returns>
    /// The next available global identity value.
    /// </returns>
    /// <remarks>
    /// A global identity value is the identity that is associated with a
    /// <see cref="string.Empty"/> key.
    /// </remarks>
    long Generate();

    /// <summary>
    /// Generate the next available identity.
    /// </summary>
    /// <param name="key">
    /// A string that identifies the object to get the next high value(ex. The
    /// name of a table in a RDBMS).
    /// </param>
    /// <returns>
    /// The next available identity value.
    /// </returns>
    long Generate(string key);
  }
}
