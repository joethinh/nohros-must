using System;

namespace Nohros.Security.Extensions
{
  /// <summary>
  /// Common security extension methods.
  /// </summary>
  public static class Security
  {
    /// <summary>
    /// Computes teh MD5 hash pf the given string.
    /// </summary>
    /// <param name="str">
    /// A string to compute the hash.
    /// </param>
    /// <returns>
    /// The hash of the string <paramref name="str"/>.
    /// </returns>
    public static string MD5Hash(this string str) {
      return NSecurity.MD5Hash(str);
    }
  }
}
