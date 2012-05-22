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
      IQuerySettings query_settings = settings.CreateQuerySettings();
      ITokenPrincipalMapperSettings token_principal_mapper_settings =
        settings.CreateTokenPrincipalMapperSettings();

      IQueryResolver resolver = QueryResolver.CreateQueryResolver(query_settings);
    }
  }
}
