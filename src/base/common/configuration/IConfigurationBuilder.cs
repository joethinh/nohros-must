using System;
using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Configuration
{
  /// <summary>
  /// A builder for the <see cref="IConfiguration"/> class.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IConfigurationBuilder<out T> where T : IConfiguration
  {
    /// <summary>
    /// Builds an instance of <typeparamref name="T"/> using the configured
    /// data.
    /// </summary>
    /// <returns>
    /// The newly created instance of <typeparamref name="T"/>.
    /// </returns>
    T Build();

    /// <summary>
    /// Sets the <see cref="RepositoriesNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="repositories">The <see cref="RepositoriesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="repositories"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    IConfigurationBuilder<T> SetRepositories(RepositoriesNode repositories);

    /// <summary>
    /// Sets the <see cref="ProvidersNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="providers">The <see cref="ProvidersNode"/>
    /// to set.</param>
    /// <returns>
    /// A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="providers"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.
    /// </returns>
    IConfigurationBuilder<T> SetProviders(ProvidersNode providers);

    /// <summary>
    /// Sets the <see cref="LoginModulesNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="login_modules">The <see cref="LoginModulesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="login_modules"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    IConfigurationBuilder<T> SetLoginModules(LoginModulesNode login_modules);

    /// <summary>
    /// Sets the <see cref="XmlElementsNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="xml_elements">The <see cref="XmlElementsNode"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="xml_elements"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    IConfigurationBuilder<T> SetXmlElements(XmlElementsNode xml_elements);

    /// <summary>
    /// Sets the <see cref="LogLevel"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="level"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    IConfigurationBuilder<T> SetLogLevel(LogLevel level);

    /// <summary>
    /// Sets the <see cref="DictionaryValue"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="properties">The <see cref="DictionaryValue"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="properties"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    IConfigurationBuilder<T> SetProperties(DictionaryValue properties);


    /// <summary>
    /// Gets the configured login modules.
    /// </summary>
    /// <value>The configured login modules or <c>null</c> if no
    /// login modules are configured.</value>
    LoginModulesNode LoginModules { get; }

    /// <summary>
    /// Gets the configured properties.
    /// </summary>
    /// <value>The configured properties or <c>null</c> if no
    /// properties are configured.</value>
    DictionaryValue Properties { get; }

    /// <summary>
    /// Gets the configured providers.
    /// </summary>
    /// <value>The configured providers or <c>null</c> if no
    /// providers are configured.</value>
    ProvidersNode Providers { get; }

    /// <summary>
    /// Gets the configured repositories.
    /// </summary>
    /// <value>The configured repositories or <c>null</c> if no
    /// repositories are configured.</value>
    RepositoriesNode Repositories { get; }

    /// <summary>
    /// Gets the configured xml elements.
    /// </summary>
    /// <value>The configured xml elements or <c>null</c> if no
    /// xml elements are configured.</value>
    XmlElementsNode XmlElements { get; }

    /// <summary>
    /// Gets the configured log level.
    /// </summary>
    LogLevel LogLevel { get; }
  }
}
