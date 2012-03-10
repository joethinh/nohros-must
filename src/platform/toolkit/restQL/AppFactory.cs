using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Providers;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory used the created the main application object graph.
  /// </summary>
  internal class AppFactory
  {
    ITokenPrincipalMapperFactory settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AppFactory"/>.
    /// </summary>
    public AppFactory() { }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="ITokenPrincipalMapper"/> object
    /// using the information that is defined on the application settings.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>An instance of a class that implements or derives from a
    /// class that implements the <see cref="ITokenPrincipalMapper"/>
    /// interface.
    /// </returns>
    /// <remarks></remarks>
    public ITokenPrincipalMapper CreateTokenPrincipalMapper(
      SimpleProviderNode node) {
      ITokenPrincipalMapperFactory factory =
        ProviderFactory<ITokenPrincipalMapperFactory>.CreateProviderFactory(
        node);
      return factory.CreateTokenPrincipalMapper(node.Options);
    }
  }
}
