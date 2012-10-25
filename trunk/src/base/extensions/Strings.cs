using System;

namespace Nohros.Extensions
{
  public static class StringExtensions
  {
    public static bool CompareOrdinal(this string str, string comparand) {
      return string.Compare(str, comparand, StringComparison.Ordinal) == 0;
    }

    public static bool CompareOrdinalIgnoreCase(this String str,
      string comparand) {
      return
        string.Compare(str, comparand, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool CompareCurrentCultureIgnoreCase(this string str,
      string comparand) {
      return string.
        Compare(str, comparand, StringComparison.CurrentCultureIgnoreCase) == 0;
    }

    public static string Format(this string str, object arg0) {
      return string.Format(str, arg0);
    }

    public static string Format(this string str, object arg0, object arg1) {
      return string.Format(str, arg0, arg1);
    }

    public static string Format(this string str, object arg0, object arg1,
      object arg2) {
      return string.Format(str, arg0, arg1, arg2);
    }

    public static string Format(this string str, params object[] args) {
      return string.Format(str, args);
    }
  }
}
