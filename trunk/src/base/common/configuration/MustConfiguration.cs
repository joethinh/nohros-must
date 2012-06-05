using System;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Configuration;
using Nohros.Logging;
using Nohros.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
  /// <summary>
  /// A basic implementation of the <see cref="AbstractConfiguration"/> used to
  /// parse the configuration files that follows the nohros schema.
  /// </summary>
  /// <remarks>
  /// The nohros shcema is defined by the
  /// http://nohros.com/schemas/nohros/nohros.xsd file.
  /// </remarks>
  public class MustConfiguration : AbstractConfiguration, IMustConfiguration,
                                   ILoginConfiguration
  {
    const string kLogLevel = "log-level";

    /// <summary>
    /// The name of the key that must exists in the main application
    /// configuration file and should contains the relative path to the
    /// nohros configuration file.
    /// </summary>
    const string kConfigurationFileKey = "NohrosConfigurationFile";

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
    public MustConfiguration() {
      properties_ = new DictionaryValue();
      repositories_ = new RepositoriesNode();
      providers_ = new ProvidersNode();
      login_modules_ = new LoginModulesNode();
      xml_elements_ = new XmlElementsNode();
    }
    #endregion

    #region ILoginConfiguration Members
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
    #endregion

    #region IMustConfiguration Members
    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended.
    /// <para>This file is XML and calling this function prompts the loader to
    /// look in that file for a key named [NohrosConfigurationFile] that
    /// contains the path for the configuration file.
    /// </para>
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The xml document must constains at leats one node with name euqlas to
    /// "nohros" that is descendant of the root node.
    /// </para>
    /// </remarks>
    public override void Load() {
      InternalLoad("nohros", false);
    }

    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file is XML and
    /// calling this function prompts the loader to look in that file for a key
    /// named [NohrosConfigurationFile] that contains the path for the
    /// configuration file.
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data. Note that a element with name "nohros" must
    /// exists some place on the root nodes tree.
    /// </para>
    /// </remarks>
    public override void Load(string root_node_name) {
      InternalLoad(root_node_name, false);
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
    #endregion

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
    /// Loads the configuration values based on the application's configuration
    /// settings and watch it for modifications.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended.
    /// <para>This file is XML and calling this function prompts the loader to
    /// look in that file for a key named [NohrosConfigurationFile] that
    /// contains the path for the configuration file.
    /// </para>
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The xml document must constains at leats one node with name euqlas to
    /// "nohros" that is descendant of the root node.
    /// </para>
    /// </remarks>
    /// <para>
    /// This methods watches the nohros configuration file for modifications
    /// and when it is modified the configuration values is reloaded.
    /// </para>
    public void LoadAndWatch() {
      InternalLoad("nohros", true);
    }

    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings and watch it for modifications.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file is XML and
    /// calling this function prompts the loader to look in that file for a key
    /// named [NohrosConfigurationFile] that contains the path for the
    /// configuration file.
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data. Note that a element with name "nohros" must
    /// exists some place on the root nodes tree.
    /// </para>
    /// <para>
    /// This methods watches the nohros configuration file for modifications
    /// and when it is modified the configuration values is reloaded.
    /// </para>
    /// </remarks>
    public void LoadAndWatch(string root_node_name) {
      InternalLoad(root_node_name, true);
    }

    void InternalLoad(string root_node_name, bool watch) {
      string config_file_path =
        ConfigurationManager.AppSettings[kConfigurationFileKey];

      if (config_file_path == null) {
        throw new ConfigurationException(string.Format(
          StringResources.Arg_KeyNotFound, kConfigurationFileKey));
      }

      if (Path.IsPathRooted(config_file_path)) {
        throw new ConfigurationException(string.Format(
          StringResources.Arg_PathRooted, config_file_path));
      }

      config_file_path = Path.Combine(Location, config_file_path);

      FileInfo config_file_info = new FileInfo(config_file_path);
      if (watch) {
        Load(config_file_info, root_node_name);
      } else {
        LoadAndWatch(config_file_info, root_node_name);
      }
    }

    /// <summary>
    /// Parses the configuration node using the nohros schema.
    /// </summary>
    /// <param name="element">
    /// A Xml element representing the configuration root node.
    /// </param>
    /// <remarks>
    /// The <paramref name="element"/> does not need to be the nohros
    /// configuration node, but a node with name "nohros" must exists on the
    /// node hierarchy.
    /// </remarks>
    protected override void Parse(XmlElement element) {
      base.Parse(element);

      XmlElement root_node = GetRootNode(element);

      // the logger is used by some methods above and the level threshold of it
      // could be overloaded by a configuration key. So, we need to do the
      // first logger instantiation here and adjust the threshold level if
      // needed.
      log_level_ = GetLogLevel(root_node);

      // parse the know configuration nodes.
      foreach (XmlNode node in root_node.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element) {
          string name = node.Name;
          if (StringsAreEquals(name, Strings.kRepositoriesNodeName)) {
            repositories_ = RepositoriesNode.Parse((XmlElement) node, location);
          } else if (StringsAreEquals(name, Strings.kProvidersNodeName)) {
            providers_ = ProvidersNode.Parse((XmlElement) node, location);
          } else if (StringsAreEquals(name, Strings.kLoginModulesNodeName)) {
            login_modules_ = LoginModulesNode.Parse((XmlElement) node, location);
          } else if (StringsAreEquals(name, Strings.kXmlElementsNodeName)) {
            xml_elements_ = XmlElementsNode.Parse((XmlElement) node);

            // Add the element that was used to configure this class to the
            // collection of xml elements nodes.
            xml_elements_[Strings.kRootXmlElementName] = element;
          }
        }
      }
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
    }

    /// <summary>
    /// Gets the configured log level.
    /// </summary>
    LogLevel GetLogLevel(XmlElement element) {
      log_level_ = LogLevel.Info;
      XmlAttribute attribute = element.Attributes[kLogLevel];

      if (attribute != null) {
        switch (attribute.Value.ToLower()) {
          case "all":
            log_level_ = LogLevel.All;
            break;

          case "debug":
            log_level_ = LogLevel.Debug;
            break;

          case "info":
            log_level_ = LogLevel.Info;
            break;

          case "warn":
            log_level_ = LogLevel.Warn;
            break;

          case "error":
            log_level_ = LogLevel.Error;
            break;

          case "fatal":
            log_level_ = LogLevel.Fatal;
            break;

          case "off":
            log_level_ = LogLevel.Off;
            break;
        }
      }
      return log_level_;
    }

    /// <summary>
    /// Gets the configuration root node that is the first node whose name is
    /// "nohros" and is child of the <paramref name="element"/> node.
    /// </summary>
    XmlElement GetRootNode(XmlElement element) {
      XmlElement root_node = null;
      if (StringsAreEquals(element.Name, Strings.kNohrosNodeName)) {
        root_node = element;
      } else {
        // the given node is not the "nohros" node, search for the that node
        // in the xml hierarchy.
        XmlNode node = element.SelectSingleNode(Strings.kNohrosNodeName);
        if (node == null || node.NodeType != XmlNodeType.Element) {
          XmlNodeList nodes =
            element.GetElementsByTagName(Strings.kNohrosNodeName);
          foreach (XmlNode n in nodes) {
            if (n.NodeType == XmlNodeType.Element) {
              root_node = (XmlElement) n;
              break;
            }
          }
        } else {
          root_node = (XmlElement) node;
        }
      }

      if (root_node == null) {
        throw new ConfigurationErrorsException(string.Format(
          StringResources.Arg_KeyNotFound, Strings.kNohrosNodeName));
      }
      return root_node;
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
