using System;

using Nohros.Caching;
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
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AppFactory"/>.
    /// </summary>
    public AppFactory() {
    }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="ITokenPrincipalMapper"/> object
    /// using the information that is defined on the application settings.
    /// </summary>
    /// <param name="node">
    /// A <see cref="IProviderNode"/> containing information about the
    /// the token principal mapper that will be created.
    /// </param>
    /// <param name="settings">
    /// A <see cref="ITokenPrincipalMapperSettings"/> object containing
    /// configuration data that is associated with the token mapper.
    /// </param>
    /// <returns>
    /// An instance of a class that implements or derives from a class that
    /// implements the <see cref="ITokenPrincipalMapper"/> interface.
    /// </returns>
    /// <remarks></remarks>
    public ITokenPrincipalMapper CreateTokenPrincipalMapper(
      ITokenPrincipalMapperSettings settings, IProviderNode node) {
      ITokenPrincipalMapperFactory factory =
        ProviderFactory<ITokenPrincipalMapperFactory>.CreateProviderFactory(
          node);
      return factory.CreateTokenPrincipalMapper(node.Options, settings);
    }

    /// <summary>
    /// Creates an instance of the <see cref="IQueryExecutor"/> object using
    /// the information defined on the given provider and configuraton settings.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="IQuerySettings"/> containing the configuration settings
    /// for the query processor.
    /// </param>
    /// <param name="provider">
    /// A <see cref="IProviderNode"/> containing inforamtion about the provider
    /// to be created.
    /// </param>
    /// <returns>
    /// A new <see cref="IQueryExecutor"/> object.
    /// </returns>
    public IQueryExecutor CreateQueryProcessor(IQuerySettings settings,
      IProviderNode provider) {
      IQueryExecutorFactory factory =
        ProviderFactory<IQueryExecutorFactory>.CreateProviderFactory(provider);
      return factory.CreateQueryExecutor(provider.Options, settings);
    }

    /// <summary>
    /// Creates an instance of the <see cref="QueryResolver"/> object using the
    /// specified cache provider, common data provider and query settings.
    /// </summary>
    /// <returns>
    /// The created <see cref="QueryResolver"/> object.
    /// </returns>
    public QueryResolver CreateQueryResolver(ICacheProvider cache_provider,
      IQuerySettings settings, CommonDataProvider data_provider) {
      CacheBuilder<Query> cache = new CacheBuilder<Query>();
      cache.ExpireAfterAccess(settings.QueryCacheDuration, TimeUnit.Minutes);
      ILoadingCache<Query> loading_cache = cache.Build(cache_provider,
        CacheLoader<Query>.From(data_provider.GetQuery));
      return new QueryResolver(loading_cache);
    }
  }
}
