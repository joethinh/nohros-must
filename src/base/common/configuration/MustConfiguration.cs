using System;
using System.Reflection;
using System.Xml;
using System.IO;
using Nohros.Logging;
using Nohros.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// A basic implementation of the <see cref="IMustConfiguration"/> used to
  /// parse the configuration files that follows the nohros schema.
  /// </summary>
  /// <remarks>
  /// The nohros shcema is defined by the
  /// http://nohros.com/schemas/nohros/nohros.xsd file.
  /// </remarks>
  public partial class MustConfiguration : IMustConfiguration,
                                           ILoginConfiguration
  {
    LogLevel log_level_;
    ILoginModulesNode login_modules_;
    DictionaryValue properties_;
    ProvidersNode providers_;
    RepositoriesNode repositories_;
    XmlElementsNode xml_elements_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MustConfiguration"/> class.
    /// </summary>
    public MustConfiguration(MustConfigurationBuilder builder) {
      properties_ = builder.Properties;
      repositories_ = builder.Repositories;
      providers_ = builder.Providers;
      login_modules_ = builder.LoginModules;
      xml_elements_ = builder.XmlElements;
    }
    #endregion

    /// <summary>
    /// Gets the login modules that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no login modules configured, this property will
    /// returns a empty <see cref="LoginModulesNode"/>, that is a
    /// <see cref="LoginModulesNode"/> object that contains no login modules.
    /// </remarks>
    public ILoginModulesNode LoginModules {
      get { return login_modules_; }
    }

    /// <summary>
    /// Gets the repositories that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no repositories configured, this property will
    /// returns a empty <see cref="RepositoriesNode"/>, that is a
    /// <see cref="RepositoriesNode"/> object that has no repository.
    /// </remarks>
    public IRepositoriesNode Repositories {
      get { return repositories_; }
    }

    /// <summary>
    /// Gets the providers that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no providers configured, this property will
    /// returns a empty <see cref="ProvidersNode"/>, that is a
    /// <see cref="ProvidersNode"/> object that has no repository.
    /// </remarks>
    public IProvidersNode Providers {
      get { return providers_; }
    }

    /// <summary>
    /// Gets the xml elements that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no xml elements configured, this property will
    /// returns an empty <see cref="XmlElementsNode"/>, that is a
    /// <see cref="XmlElementsNode"/> object that contains no xml elements.
    /// </remarks>
    public IXmlElementsNode XmlElements {
      get { return xml_elements_; }
    }

    /// <summary>
    /// Gets the logging level that was configured for this application.
    /// </summary>
    public LogLevel LogLevel {
      get { return log_level_; }
    }

    /// <summary>
    /// Gets the the path to the configuration file named
    /// <paramref name="name"/> using the running assembly path.
    /// </summary>
    /// <param name="name">
    /// The name of the configuration file to get the path.
    /// </param>
    public static string GetLocalConfigurationFilePath(string name) {
      return
        Path.Combine(
          Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
    }

    /// <summary>
    /// Copies the configuration data from the specified
    /// <see cref="MustConfiguration"/> object.
    /// </summary>
    /// <param name="configuration">
    /// A <see cref="MustConfiguration"/> object that contains the
    /// configuration data to be copied.
    /// </param>
    public void CopyFrom(MustConfiguration configuration) {
      providers_ = configuration.providers_;
      repositories_ = configuration.repositories_;
      xml_elements_ = configuration.xml_elements_;
      log_level_ = configuration.log_level_;
      login_modules_ = configuration.login_modules_;
      properties_ = configuration.properties_;
    }

    /// <summary>
    /// Gets the value of a dynamic property by using the specified property namespace and name.
    /// </summary>
    /// <param name="ns">The namespace of the property</param>
    /// <param name="property">The name of the property</param>
    /// <returns>The value of the property within a given namespace or null if the property could not
    /// be found.</returns>
    protected string PropertyKey(string ns, string property) {
      return string.Concat(ns.ToLower(), "-", property.ToLower());
    }

    /// <summary>
    /// Recursively gets the dynamic properties.
    /// </summary>
    /// <param name="node">
    /// A XML node containing the dynamic properties.
    /// </param>
    /// <param name="path">
    /// The path to the node value.
    /// </param>
    /// <remarks>
    /// If the namespace of the property is not defined it will be assigned to
    /// the default namespace.
    /// </remarks>
    void GetProperties(XmlNode node, string path) {
      if (node != null && node.ChildNodes.Count > 0) {
        foreach (XmlNode inner_node in node.ChildNodes) {
          if (inner_node.NodeType == XmlNodeType.Element) {
            if (inner_node.ChildNodes.Count > 0) {
              GetProperties(inner_node, path + "." + inner_node.Name);
            } else {
              XmlAttributeCollection properties = inner_node.Attributes;
              if (properties.Count == 0)
                return;

              DictionaryValue keys = new DictionaryValue();
              foreach (XmlAttribute property in properties) {
                keys.SetString(property.Name, property.Value);
              }
              properties_[path] = keys;
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose value to get</param>
    /// <returns>An string associated with the specified key.</returns>
    /// <seealso cref="this(string)"/>
    public IValue GetProperty(string key) {
      return properties_[key];
    }

    /// <summary>
    /// Sets the value associated with the specified key within the default
    /// namespace.
    /// </summary>
    /// <param name="key">The key whose value to set</param>
    /// <param name="value">An string associated with the specified key within
    /// the given namespace</param>
    public void SetProperty(string key, IValue value) {
      properties_[key] = value;
    }
  }
}
