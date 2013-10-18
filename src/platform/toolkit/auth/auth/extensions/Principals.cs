using System;

namespace Nohros.Security.Auth.Extensions
{
  /// <summary>
  /// Extensions methods for the <see cref="IPrincipal"/> interface.
  /// </summary>
  public static class Principals
  {
    /// <summary>
    /// Creates a <see cref="Principal{T}"/> object that uses the given
    /// <paramref name="guid"/> as its identifier.
    /// </summary>
    /// <param name="guid">
    /// A <see cref="Guid"/> that is used as principal identifier.
    /// </param>
    /// <returns>
    /// A <see cref="Principal{T}"/> object that uses the given
    /// <paramref name="guid"/> as its identifier.
    /// </returns>
    public static Principal<Guid> AsPrincipal(this Guid guid) {
      return new Principal<Guid>(guid);
    }

    /// <summary>
    /// Creates a <see cref="Principal{T}"/> object that uses the given
    /// long id as its identifier.
    /// </summary>
    /// <param name="id">
    /// A <see cref="long"/> that is used as principal identifier.
    /// </param>
    /// <returns>
    /// A <see cref="Principal{T}"/> object that uses the given
    /// long <paramref name="id"/> as its identifier.
    /// </returns>
    public static Principal<long> AsPrincipal(this long id) {
      return new Principal<long>(id);
    }

    /// <summary>
    /// Creates a <see cref="Principal{T}"/> object that uses the given
    /// integer id as its identifier.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> that is used as principal identifier.
    /// </param>
    /// <returns>
    /// A <see cref="Principal{T}"/> object that uses the given
    /// integer <paramref name="id"/> as its identifier.
    /// </returns>
    public static Principal<int> AsPrincipal(this int id) {
      return new Principal<int>(id);
    }

    /// <summary>
    /// Creates a <see cref="Principal{T}"/> object that uses the given
    /// string id as its identifier.
    /// </summary>
    /// <param name="id">
    /// A <see cref="string"/> that is used as principal identifier.
    /// </param>
    /// <returns>
    /// A <see cref="Principal{T}"/> object that uses the given
    /// string <paramref name="id"/> as its identifier.
    /// </returns>
    public static Principal<string> AsPrincipal(this string id) {
      return new Principal<string>(id);
    }
  }
}
