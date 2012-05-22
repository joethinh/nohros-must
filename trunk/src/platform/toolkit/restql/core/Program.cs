using System;
using Nohros.Caching.Providers;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Represents the RestQL application, that is the library entry-point.
  /// </summary>
  public sealed class Program
  {
    public static void Main(string[] args) {
      Settings settings = Settings.CreateSettings();

      AppFactory factory = new AppFactory(settings);
      ICacheProvider cache_provider = factory.CreateCacheProvider();
      ICommonDataProvider common_data_provider =
        factory.CreateCommonDataProvider();
      IQueryResolver resolver = QueryResolver.CreateQueryResolver(
        settings.QuerySettings, common_data_provider, cache_provider);
      IQueryProcessor processor = factory.CreateQueryProcessor(resolver);
    }
  }
}
