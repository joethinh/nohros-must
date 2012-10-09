using System;
using System.IO;
using System.Reflection;
using Nohros.Caching.Providers;
using Nohros.Providers;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory used the created the main application object graph.
  /// </summary>
  internal class AppFactory
  {
    const string kRestQLSettingsFileName = "restql.config";
    const string kRestQLRootNodeName = "restql";

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AppFactory"/>.
    /// </summary>
    public AppFactory() {
    }
    #endregion

    public Settings CreateSettings() {
      string current_assembly_location =
        Assembly.GetExecutingAssembly().Location;
      string config_file_name = Path.Combine(current_assembly_location,
        kRestQLSettingsFileName);

      Settings settings = new Settings();
      settings.Load();
      return settings;
    }

    /// <summary>
    /// Creates an instance of the <see cref="QueryResolver"/> object using the
    /// specified cache provider, common data provider and query settings.
    /// </summary>
    /// <returns>
    /// The created <see cref="QueryResolver"/> object.
    /// </returns>
    public QueryResolver CreateQueryResolver(ICacheProvider cache_provider) {
      QueryResolver.QueryResolverCache query_resolver_cache =
        new QueryResolver.QueryResolverCache(cache_provider,
          settings.CommonDataProvider, settings);
      return new QueryResolver(GetQueryExecutors(settings), query_resolver_cache);
    }


    IQueryExecutor[] GetQueryExecutors(IQuerySettings settings) {
      IProviderNode[] providers = settings.Executors;
      int length = providers.Length;
      IQueryExecutor[] executors = new IQueryExecutor[length];
      for (int i = 0, j = length; i < j; i++) {
        IProviderNode provider = providers[i];
        executors[i] = ProviderFactory<IQueryExecutorFactory>
          .CreateProviderFactory(provider)
          .CreateQueryExecutor(provider.Options, settings);
      }
      return executors;
    }

    /// <summary>
    /// Creates an instance of the <see cref="IQueryProcessor"/> class using
    /// the specified <see cref="IQueryResolver"/> object.
    /// </summary>
    /// <param name="resolver">
    /// A <see cref="IQueryResolver"/> object.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IQueryProcessor"/> object.
    /// </returns>
    public QueryProcessor CreateQueryProcessor(IQueryResolver resolver) {
      return new QueryProcessor(resolver);
    }
  }
}
