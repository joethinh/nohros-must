using System;

namespace Nohros.Data
{
  /// <summary>
  /// Provides an abstract interface to the database where information about
  /// the HiLo ids is stored.
  /// </summary>
  public interface IHiLoDao
  {
    /// <summary>
    /// Get the next High value for the given <see cref="key"/>.
    /// </summary>
    /// <param name="key">
    /// The key to get the next High value.
    /// </param>
    /// <returns>
    /// The next High value for the given key.
    /// </returns>
    IHiLoRange GetNextHi(string key);
  }
}
