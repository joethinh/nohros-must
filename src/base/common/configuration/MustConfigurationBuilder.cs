using System;
using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Configuration
{
  /// <summary>
  /// A class that can be used to incrementally build instances of the
  /// <see cref="MustConfiguration"/> class.
  /// </summary>
  public class MustConfigurationBuilder
  {
    LogLevel log_level_;
    LoginModulesNode login_modules_;
    DictionaryValue properties_;
    ProvidersNode providers_;
    RepositoriesNode repositories_;
    XmlElementsNode xml_elements_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MustConfigurationBuilder"/> class.
    /// </summary>
    public MustConfigurationBuilder() {
      properties_ = new DictionaryValue();
      repositories_ = new RepositoriesNode();
      providers_ = new ProvidersNode();
      login_modules_ = new LoginModulesNode();
      xml_elements_ = new XmlElementsNode();
    }
    #endregion

    /// <summary>
    /// Sets the <see cref="RepositoriesNode"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="repositories">The <see cref="RepositoriesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="repositories"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder SetRepositories(
      RepositoriesNode repositories) {
      repositories_ = repositories;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="ProvidersNode"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="providers">The <see cref="ProvidersNode"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="providers"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder SetProviders(ProvidersNode providers) {
      providers_ = providers;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="LoginModulesNode"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="login_modules">The <see cref="LoginModulesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="login_modules"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder SetLoginModules(
      LoginModulesNode login_modules) {
      login_modules_ = login_modules;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="XmlElementsNode"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="xml_elements">The <see cref="XmlElementsNode"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="xml_elements"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder SetXmlElements(
      XmlElementsNode xml_elements) {
      xml_elements_ = xml_elements;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="LogLevel"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="level"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder SetLogLevel(LogLevel level) {
      log_level_ = level;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="DictionaryValue"/> object for
    /// the <see cref="MustConfigurationBuilder"/>
    /// </summary>
    /// <param name="properties">The <see cref="DictionaryValue"/>
    /// to set.</param>
    /// <returns>A <see cref="MustConfigurationBuilder"/> object that
    /// associates <paramref name="properties"/> with the builded
    /// <see cref="IMustConfiguration"/>.</returns>
    public MustConfigurationBuilder Setproperties(DictionaryValue properties) {
      properties_ = properties;
      return this;
    }

    /// <summary>
    /// Creates an instance of the <see cref="IMustConfiguration"/> using the
    /// values
    /// </summary>
    /// <typeparam name="TFactory"></typeparam>
    /// <typeparam name="TClass"></typeparam>
    /// <returns></returns>
    public virtual TClass Build<TFactory, TClass>()
      where TClass : IMustConfiguration
      where TFactory : IMustConfigurationFactory<TClass>, new() {
      return new TFactory().CreateMustConfiguration(this);
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
