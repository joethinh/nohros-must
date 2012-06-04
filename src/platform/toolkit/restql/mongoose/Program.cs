using System;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Represents the RestQL application, that is the library entry-point.
  /// </summary>
  public sealed class Program
  {
    public static void Main(string[] args) {
      MongooseSettings settings = MongooseSettings.CreateSettings();
      IQueryResolver resolver = QueryResolver.CreateQueryResolver(settings);
      QueryProcessor processor = new QueryProcessor(resolver);

      Mongoose server = new Mongoose();
      server.set_option("ports")
    }
  }
}
