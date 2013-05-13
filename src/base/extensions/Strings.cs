using System;
using System.Globalization;
using System.Text;

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

    public static string Fmt(this string str, object arg0) {
      return string.Format(str, arg0);
    }

    public static string Fmt(this string str, object arg0, object arg1) {
      return string.Format(str, arg0, arg1);
    }

    public static string Fmt(this string str, object arg0, object arg1,
      object arg2) {
      return string.Format(str, arg0, arg1, arg2);
    }

    public static string Fmt(this string str, params object[] args) {
      return string.Format(str, args);
    }

    public static string RemoveDiacritics(this string str) {
      string normalized_string = str.Normalize(NormalizationForm.FormD);
      var builder = new StringBuilder(str.Length);
      for (int i = 0, j = normalized_string.Length; i < j; i++) {
        UnicodeCategory category =
          CharUnicodeInfo.GetUnicodeCategory(normalized_string[i]);
        if (category != UnicodeCategory.NonSpacingMark) {
          builder.Append(normalized_string[i]);
        }
      }
      return builder.ToString();
    }
  }
}
