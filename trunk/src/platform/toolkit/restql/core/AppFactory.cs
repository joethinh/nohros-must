using System;
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
    readonly ISettings settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AppFactory"/>.
    /// </summary>
    public AppFactory(ISettings settings) {
      settings_ = settings;
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
    /// <returns>
    /// An instance of a class that implements or derives from a class that
    /// implements the <see cref="ITokenPrincipalMapper"/> interface.
    /// </returns>
    /// <remarks></remarks>
    public ITokenPrincipalMapper CreateTokenPrincipalMapper(
      ITokenPrincipalMapperSettings settings, IProviderNode node) {
      return ProviderFactory<ITokenPrincipalMapperFactory>
        .CreateProviderFactory(node)
        .CreateTokenPrincipalMapper(node.Options, settings);
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
    public IQueryProcessor CreateQueryProcessor(IQueryResolver resolver) {
      return new QueryProcessor(resolver);
    }
  }
}
