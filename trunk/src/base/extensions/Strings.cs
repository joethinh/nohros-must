using System;

namespace Nohros.Extensions
{
  public static class StringExtensions
  {
    public static bool CompareOrdinal(this String str, string comparand) {
      return string.Compare(str, comparand, StringComparison.Ordinal) == 0;
    }

    public static bool CompareOrdinalIgnoreCase(this String str,
      string comparand) {
      return
        string.Compare(str, comparand, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool CompareCurrentCultureIgnoreCase(this String str,
      string comparand) {
      return string.
        Compare(str, comparand, StringComparison.CurrentCultureIgnoreCase) == 0;
    }
  }
}
