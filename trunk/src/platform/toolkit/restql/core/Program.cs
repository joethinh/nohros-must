using System;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Represents the RestQL application, that is the library entry-point.
  /// </summary>
  public sealed class Program
  {
    public static void Main(string[] args) {
      Settings settings = Settings.CreateSettings();
      IQueryResolver resolver = QueryResolver.CreateQueryResolver(settings);
    }
  }
}
