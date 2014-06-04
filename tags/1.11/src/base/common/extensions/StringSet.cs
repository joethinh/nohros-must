using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Extensions
{
  public static class StringSet
  {
    /// <summary>
    /// Computes the hash code of the string consisting of the joined string
    /// of the <paramref name="strs"/> collection.
    /// </summary>
    /// <returns>
    /// The hash code of the string consisting of the joined string
    /// of the <paramref name="strs"/> collection.
    /// </returns>
    public static int Hash(this IEnumerable<string> strs) {
      var str = new StringBuilder();
      strs.Join(":", str);
      return str.ToString().GetHashCode();
    }

    /// <summary>
    /// Concatenates all the elements of <paramref name="strs"/>, using the
    /// specified separator between each element.
    /// </summary>
    /// <returns>
    /// A string that consists of the elements in the <paramref name="strs"/>
    /// delimited by the <paramref name="separator"/> string. If
    /// <paramref name="strs"/> does not contain any element, the method returns
    /// <see cref="string.Empty"/>.
    /// </returns>
    public static string Join(this IEnumerable<string> strs, string separator) {
      var str = new StringBuilder();
      strs.Join(separator, str);
      return str.ToString();
    }

    /// <summary>
    /// Concatenates all the elements of <paramref name="strs"/>, using the
    /// specified separator between each element.
    /// </summary>
    /// <remarks>
    /// A string that consists of the elements in the <paramref name="strs"/>
    /// delimited by the <paramref name="separator"/> will be appended to
    /// the given <see cref="StringBuilder"/> object.
    /// </remarks>
    public static void Join(this IEnumerable<string> strs, string separator,
      StringBuilder str) {
      foreach (string field in strs) {
        str
          .Append(field)
          .Append(separator);
      }
      if (str.Length != 0) {
        str.Remove(str.Length - separator.Length, separator.Length);
      }
    }
  }
}
