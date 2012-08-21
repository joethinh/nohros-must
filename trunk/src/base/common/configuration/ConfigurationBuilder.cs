using System;
using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Configuration
{
  /// <summary>
  /// A class that can be used to incrementally build instances of the
  /// <see cref="Configuration"/> class.
  /// </summary>
  public class ConfigurationBuilder
  {
    /// <summary>
    /// A delegate that acts as a factory and us used to create an instance of
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="builder">
    /// A <see cref="ConfigurationBuilder"/> object that contains the
    /// already loaded configuration data.
    /// </param>
    /// <returns></returns>
    public delegate T LoaderDelegate<out T>(ConfigurationBuilder builder);

    LogLevel log_level_;
    LoginModulesNode login_modules_;
    DictionaryValue properties_;
    ProvidersNode providers_;
    RepositoriesNode repositories_;
    XmlElementsNode xml_elements_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ConfigurationBuilder"/> class.
    /// </summary>
    public ConfigurationBuilder() {
      properties_ = new DictionaryValue();
      repositories_ = new RepositoriesNode();
      providers_ = new ProvidersNode();
      login_modules_ = new LoginModulesNode();
      xml_elements_ = new XmlElementsNode();
    }
    #endregion

    /// <summary>
    /// Sets the <see cref="RepositoriesNode"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="repositories">The <see cref="RepositoriesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="repositories"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public ConfigurationBuilder SetRepositories(
      RepositoriesNode repositories) {
      repositories_ = repositories;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="ProvidersNode"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="providers">The <see cref="ProvidersNode"/>
    /// to set.</param>
    /// <returns>
    /// A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="providers"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.
    /// </returns>
    public ConfigurationBuilder SetProviders(ProvidersNode providers) {
      providers_ = providers;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="LoginModulesNode"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="login_modules">The <see cref="LoginModulesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="login_modules"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public ConfigurationBuilder SetLoginModules(
      LoginModulesNode login_modules) {
      login_modules_ = login_modules;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="XmlElementsNode"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="xml_elements">The <see cref="XmlElementsNode"/>
    /// to set.</param>
    /// <returns>A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="xml_elements"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public ConfigurationBuilder SetXmlElements(
      XmlElementsNode xml_elements) {
      xml_elements_ = xml_elements;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="LogLevel"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/>
    /// to set.</param>
    /// <returns>A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="level"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public ConfigurationBuilder SetLogLevel(LogLevel level) {
      log_level_ = level;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="DictionaryValue"/> object for
    /// the <see cref="ConfigurationBuilder"/>
    /// </summary>
    /// <param name="properties">The <see cref="DictionaryValue"/>
    /// to set.</param>
    /// <returns>A <see cref="ConfigurationBuilder"/> object that
    /// associates <paramref name="properties"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public ConfigurationBuilder SetProperties(
      DictionaryValue properties) {
      properties_ = properties;
      return this;
    }

    /// <summary>
    /// Copies the configured data from the specified
    /// <see cref="ConfigurationBuilder"/> object.
    /// </summary>
    /// <param name="builder">
    /// A <see cref="ConfigurationBuilder"/> object that contains the
    /// configuration data to be copied.
    /// </param>
    /// <remarks>
    /// A <see cref="ConfigurationBuilder"/> object configured with the
    /// data copied from <paramref name="builder"/>.
    /// </remarks>
    public ConfigurationBuilder CopyFrom(
      ConfigurationBuilder builder) {
      properties_ = builder.properties_;
      providers_ = builder.providers_;
      log_level_ = builder.log_level_;
      login_modules_ = builder.login_modules_;
      repositories_ = builder.repositories_;
      xml_elements_ = builder.xml_elements_;
      return this;
    }

    /// <summary>
    /// Creates an instance of the <see cref="Nohros.Configuration.IConfiguration"/> using the
    /// builder configured values.
    /// </summary>
    /// <remarks>
    /// This method implies that <typeparamref name="T"/> implements a
    /// constructor with the folowing signature:
    /// <code>
    /// .ctor(ConfigurationBuilder builder)
    /// </code>
    /// </remarks>
    /// <returns></returns>
    public T Build<T>() {
      return Factories<T>.CreateFactory(this);
    }

    /// <summary>
    /// Creates an instance of the <see cref="Nohros.Configuration.IConfiguration"/> using the
    /// <paramref name="loader"/> and the builder configured values.
    /// </summary>
    /// <returns></returns>
    public T Build<T>(LoaderDelegate<T> loader) {
      return loader(this);
    }

    /// <summary>
    /// Gets the configured login modules.
    /// </summary>
    /// <value>The configured login modules or <c>null</c> if no
    /// login modules are configured.</value>
    public LoginModulesNode LoginModules {
      get { return login_modules_; }
    }

    /// <summary>
    /// Gets the configured properties.
    /// </summary>
    /// <value>The configured properties or <c>null</c> if no
    /// properties are configured.</value>
    public DictionaryValue Properties {
      get { return properties_; }
    }

    /// <summary>
    /// Gets the configured providers.
    /// </summary>
    /// <value>The configured providers or <c>null</c> if no
    /// providers are configured.</value>
    public ProvidersNode Providers {
      get { return providers_; }
    }

    /// <summary>
    /// Gets the configured repositories.
    /// </summary>
    /// <value>The configured repositories or <c>null</c> if no
    /// repositories are configured.</value>
    public RepositoriesNode Repositories {
      get { return repositories_; }
    }

    /// <summary>
    /// Gets the configured xml elements.
    /// </summary>
    /// <value>The configured xml elements or <c>null</c> if no
    /// xml elements are configured.</value>
    public XmlElementsNode XmlElements {
      get { return xml_elements_; }
    }

    /// <summary>
    /// Gets the configured log level.
    /// </summary>
    public LogLevel LogLevel {
      get { return log_level_; }
    }
  }
}
