using System;
using System.Linq;
using Nohros.Caching.Providers;
using Nohros.Extensions;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory used the created the main application object graph.
  /// </summary>
  internal class AppFactory
  {
    readonly IQuerySettings settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AppFactory"/>.
    /// </summary>
    public AppFactory(IQuerySettings settings) {
      settings_ = settings;
    }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="QueryResolver"/> object using the
    /// specified cache provider, common data provider and query settings.
    /// </summary>
    /// <returns>
    /// The created <see cref="QueryResolver"/> object.
    /// </returns>
    public QueryResolver CreateQueryResolver() {
      IProviderNode provider = settings_
        .Providers
        .GetProviderNode(Strings.kCacheProviderName);
      ICacheProvider cache_provider =
        RuntimeTypeFactory<ICacheProviderFactory>
          .CreateInstanceFallback(provider, settings_)
          .CreateCacheProvider(provider.Options.ToDictionary());
      return CreateQueryResolver(cache_provider);
    }

    /// <summary>
    /// Creates an instance of the <see cref="QueryResolver"/> object using the
    /// specified cache provider, common data provider and query settings.
    /// </summary>
    /// <returns>
    /// The created <see cref="QueryResolver"/> object.
    /// </returns>
    public QueryResolver CreateQueryResolver(ICacheProvider provider) {
      IQueryDataProvider query_data_provider = GetQueryDataProvider();
      IQueryExecutor[] executors = GetQueryExecutors();
      return new QueryResolver(executors, query_data_provider);
    }

    IQueryExecutor[] GetQueryExecutors() {
      IProvidersNodeGroup executors_providers;
      if (settings_.Providers.GetProvidersNodeGroup(
        Strings.kQueryExecutorsGroup, out executors_providers)) {
        return executors_providers
          .Select(
            provider => RuntimeTypeFactory<IQueryExecutorFactory>
              .CreateInstanceFallback(provider, settings_)
              .CreateQueryExecutor(provider.Options.ToDictionary()))
          .ToArray();
      }
      return new IQueryExecutor[0];
    }

    IQueryDataProvider GetQueryDataProvider() {
      IProviderNode provider = settings_
        .Providers
        .GetProviderNode(Strings.kQueryDataProviderName);
      return RuntimeTypeFactory<IQueryDataProviderFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateCommonDataProvider(provider.Options.ToDictionary());
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
