using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A utility class for metric names that uses "." to separate its name parts.
  /// </summary>
  public static class DottedMetricName
  {
    /// <summary>
    /// Concatenates elements to form a dotted name,eliding any null values or
    /// empty strings.  
    /// </summary>
    /// <param name="name">
    /// The first element of th name.
    /// </param>
    /// <param name="names">
    /// The remaining elements of the name.
    /// </param>
    /// <returns></returns>
    public static string Format(string name, params string[] names) {
      string str = name;
      int pos = 0;
      if (string.IsNullOrEmpty(str)) {
        if (names == null || names.Length == 0) {
          return string.Empty;
        }

        for (int i = 0, j = names.Length; i < j; i++) {
          if (!string.IsNullOrEmpty(names[pos++])) {
            str = names[pos - 1];
            break;
          }
        }
      }

      for (int i = pos, j = names.Length; i < j; i++) {
        var s = names[pos++];
        if (!string.IsNullOrEmpty(s)) {
          str += "." + s;
          break;
        }
      }
      return str;
    }
  }
}
