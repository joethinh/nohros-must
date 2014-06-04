using System;
using System.Collections.Generic;
using System.Linq;

namespace Nohros.Extensions
{
  public static class Sets
  {
    /// <summary>
    /// Removes all the ocurrences of the given type in the given
    /// <see cref="ISet{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of oject that are stored in the set.
    /// </typeparam>
    /// <param name="set">
    /// The set to remove objects from.
    /// </param>
    /// <param name="type">
    /// The type of the object that should be removed from the set.
    /// </param>
    public static void RemoveByType<T>(this ISet<T> set, Type type) {
      var list = set
        .Where(t => t.GetType() == type)
        .ToList();

      foreach (T t in list) {
        set.Remove(t);
      }
    }
  }
}
