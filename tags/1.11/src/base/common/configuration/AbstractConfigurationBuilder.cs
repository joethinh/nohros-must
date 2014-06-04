using System;
using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Configuration.Builders
{
  /// <summary>
  /// A class that can be used to incrementally build instances of the
  /// <see cref="Configuration"/> class.
  /// </summary>
  public abstract class AbstractConfigurationBuilder<T> :
    IConfigurationBuilder<T> where T : IConfiguration
  {
    LogLevel log_level_;
    DictionaryValue properties_;
    ProvidersNode providers_;
    RepositoriesNode repositories_;
    XmlElementsNode xml_elements_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AbstractConfigurationBuilder{T}"/> class.
    /// </summary>
    protected AbstractConfigurationBuilder() {
      properties_ = new DictionaryValue();
      repositories_ = new RepositoriesNode();
      providers_ = new ProvidersNode();
      xml_elements_ = new XmlElementsNode();
    }
    #endregion

    /// <summary>
    /// Builds an instance of <typeparamref name="T"/> using the configured
    /// data.
    /// </summary>
    /// <returns>
    /// The newly created instance of <typeparamref name="T"/>.
    /// </returns>
    public abstract T Build();

    /// <summary>
    /// Sets the <see cref="RepositoriesNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="repositories">The <see cref="RepositoriesNode"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="repositories"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public IConfigurationBuilder<T> SetRepositories(
      RepositoriesNode repositories) {
      repositories_ = repositories;
      return this;
    }

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
    public IConfigurationBuilder<T> SetProviders(ProvidersNode providers) {
      providers_ = providers;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="XmlElementsNode"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="xml_elements">The <see cref="XmlElementsNode"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="xml_elements"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public IConfigurationBuilder<T> SetXmlElements(
      XmlElementsNode xml_elements) {
      xml_elements_ = xml_elements;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="LogLevel"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="level"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public IConfigurationBuilder<T> SetLogLevel(LogLevel level) {
      log_level_ = level;
      return this;
    }

    /// <summary>
    /// Sets the <see cref="DictionaryValue"/> object for
    /// the <see cref="AbstractConfigurationBuilder{T}"/>
    /// </summary>
    /// <param name="properties">The <see cref="DictionaryValue"/>
    /// to set.</param>
    /// <returns>A <see cref="AbstractConfigurationBuilder{T}"/> object that
    /// associates <paramref name="properties"/> with the builded
    /// <see cref="Nohros.Configuration.IConfiguration"/>.</returns>
    public IConfigurationBuilder<T> SetProperties(
      DictionaryValue properties) {
      properties_ = properties;
      return this;
    }

    /// <summary>
    /// Copies the configured data from the specified
    /// <see cref="AbstractConfigurationBuilder{T}"/> object.
    /// </summary>
    /// <param name="builder">
    /// A <see cref="AbstractConfigurationBuilder{T}"/> object that contains the
    /// configuration data to be copied.
    /// </param>
    /// <remarks>
    /// A <see cref="AbstractConfigurationBuilder{T}"/> object configured with the
    /// data copied from <paramref name="builder"/>.
    /// </remarks>
    public IConfigurationBuilder<T> CopyFrom(
      AbstractConfigurationBuilder<T> builder) {
      properties_ = builder.properties_;
      providers_ = builder.providers_;
      log_level_ = builder.log_level_;
      repositories_ = builder.repositories_;
      xml_elements_ = builder.xml_elements_;
      return this;
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
