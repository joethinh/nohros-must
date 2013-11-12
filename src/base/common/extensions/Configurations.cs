using System;
using System.Collections.Generic;
using Nohros.Configuration;
using Nohros.Logging;
using Nohros.Providers;
using Nohros.Resources;

namespace Nohros.Extensions
{
  /// <summary>
  /// Extension methods for <see cref="IConfiguration"/> classes.
  /// </summary>
  public static class Configurations
  {
    const string kClassName = "Nohros.Extensions.Configurations";

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

    /// <summary>
    /// Creates an instance of all configured providers that implements the
    /// the <see cref="IProviderFactory"/> intrface and has a constructor
    /// that accepts no parameters or a single parameter of the type
    /// <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="IConfiguration"/> containing the configured providers.
    /// </param>
    /// <param name="callback">
    /// A <see cref="Action{T}"/> method that will be called for every
    /// provider that is sucessfully created.
    /// </param>
    public static void CreateProviders(this IConfiguration settings,
      Action<object> callback) {
      var providers = settings.CreateProviders();
      foreach (var provider in providers) {
        callback(provider);
      }
    }

    /// <summary>
    /// Creates an instance of all configured providers that has a factory
    /// that implements the <see cref="IProviderFactory"/> intrface and has a
    /// constructor that accepts no parameters or a single parameter of the type
    /// <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="IConfiguration"/> containing the configured providers.
    /// </param>
    /// <returns>
    /// A array containing all providers that was created using the configured
    /// factories that implements the <see cref="IProviderFactory"/> interface.
    /// </returns>
    public static object[] CreateProviders(this IConfiguration settings) {
      var providers = settings.Providers;
      var list = new List<object>();
      foreach (IProvidersNodeGroup group in providers) {
        foreach (IProviderNode node in group) {
          var factory = RuntimeTypeFactory<IProviderFactory>
            .CreateInstance(node, true, false, settings);
          if (factory != null) {
            try {
              list.Add(factory.CreateProvider(node.Options.ToDictionary()));
            } catch (Exception e) {
              MustLogger.ForCurrentProcess.Error(
                StringResources.Log_MethodThrowsException.Fmt(
                  "CreateProviders", kClassName), e);
            }
          }
        }
      }
      return list.ToArray();
    }
  }
}
