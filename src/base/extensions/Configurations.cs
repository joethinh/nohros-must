using System;
using System.Collections.Generic;
using Nohros.Configuration;

namespace Nohros.Extensions
{
  /// <summary>
  /// Extension methods for <see cref="IConfiguration"/> classes.
  /// </summary>
  public static class Configurations
  {
    /// <summary>
    /// Creates an instance of the <see cref="TResult"/> class using the
    /// information contained in the provider node named
    /// <see cref="node_name"/>.
    /// </summary>
    /// <typeparam name="TFactory">
    /// The type of the factory class that is used to create instances of the
    /// <see cref="TResult"/> class.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the class that should be created.
    /// </typeparam>
    /// <param name="settings">
    /// A <see cref="IConfiguration"/> class contained the list of configured
    /// providers.
    /// </param>
    /// <param name="node_name">
    /// The name of the provider node that contains the information about the
    /// provider to be created.
    /// </param>
    /// <param name="instantiator">
    /// A <see cref="Func{T1, T2, TResult}"/> delegate that can be
    /// used to create an instance of the <see cref="TResult"/> class.
    /// </param>
    /// <returns>
    /// The <see cref="TFactory"/> class is created by either, using a
    /// constructor that accepts a <see cref="IConfiguration"/> object or a
    /// constructor that receives no parameters.
    /// </returns>
    public static TResult CreateProvider<TFactory, TResult>(
      this IConfiguration settings, string node_name,
      Func<TFactory, IDictionary<string, string>, TResult> instantiator)
      where TFactory : class {
      var node = settings
        .Providers
        .GetProviderNode(node_name);
      TFactory factory = RuntimeTypeFactory<TFactory>
        .CreateInstanceFallback(node, settings);
      return instantiator(factory, node.Options.ToDictionary());
    }
  }
}
