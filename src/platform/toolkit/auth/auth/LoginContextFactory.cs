using System;
using System.Linq;
using Nohros.Configuration;
using LoginModuleFactoryTuple =
  System.Collections.Generic.KeyValuePair
    <Nohros.Security.Auth.ILoginModuleFactory,
      System.Collections.Generic.IDictionary<string, string>>;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A factory for the <see cref="LoginContext"/> class.
  /// </summary>
  public class LoginContextFactory
  {
    /// <summary>
    /// The name of the group that should be associated with the login
    /// module providers on the configuration file.
    /// </summary>
    public const string kDefaultLoginModuleFactoryGroupName = "loginModules";

    /// <summary>
    /// Creates a new instance of the <see cref="LoginContext"/> class by using
    /// the given application settings and the default name for the login
    /// module group.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="IConfiguration"/> object containing the user defined
    /// application settings.
    /// </param>
    /// <returns>
    /// The newly create <see cref="LoginContext"/> instance.
    /// </returns>
    public LoginContext CreateLoginContext(IConfiguration settings) {
      return CreateLoginContext(settings, kDefaultLoginModuleFactoryGroupName);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LoginContext"/> class by using
    /// the given application settings and the provider group name.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="IConfiguration"/> object containing the user defined
    /// application settings.
    /// </param>
    /// <param name="provider_node_group">
    /// The name of the group to search for the providers of the type
    /// <see cref="ILoginModuleFactory"/> in the settings object.
    /// </param>
    /// <returns>
    /// The newly create <see cref="LoginContext"/> instance.
    /// </returns>
    public LoginContext CreateLoginContext(IConfiguration settings,
      string provider_node_group) {
      IProvidersNodeGroup providers = settings.Providers[provider_node_group];
      var modules = from provider in providers
                    select CreateLoginModule(settings, provider);
      return new LoginContext(modules);
    }

    ILoginModule CreateLoginModule(IConfiguration settings,
      IProviderNode provider) {
      return RuntimeTypeFactory<ILoginModuleFactory>
        .CreateInstanceFallback(provider, settings)
        .CreateLoginModule(provider.Options.ToDictionary());
    }
  }
}
