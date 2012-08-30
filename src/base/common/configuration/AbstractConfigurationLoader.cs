using System;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Configuration;
using Nohros.Resources;
using Nohros.Logging;

namespace Nohros.Configuration
{
  /// <summary>
  /// A class used to build a <see cref="IConfiguration"/> object from a XML
  /// file.
  /// </summary>
  public abstract class AbstractConfigurationLoader<T> where T : IConfiguration
  {
    /// <summary>
    /// Creates an instance of the <see cref="T"/> using the configuration
    /// information contained in <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">
    /// A <see cref="ConfigurationBuilder"/> class that contains the configured
    /// data loaded from a XML file.
    /// </param>
    /// <returns>
    /// The newly created object.
    /// </returns>
    public abstract T CreateConfiguration(IConfigurationBuilder<T> builder);

    /// <summary>
    /// Defines a method to handle the
    /// <see cref="AbstractConfigurationLoader{T}.LoadComplete"/> events.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> that
    /// has been loaded.</param>
    public delegate void LoadCompleteEventHandler(IConfiguration configuration);

    /// <summary>
    /// A delegate that acts as a factory and is used to create an instance of
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="loader">The <see cref="AbstractConfigurationLoader{T}"/>
    /// that is executing the delegate.</param>
    /// <param name="builder">
    /// A <see cref="ConfigurationBuilder"/> object that contains the
    /// already loaded configuration data.
    /// </param>
    /// <returns></returns>
    public delegate T LoaderFactoryDelegate(
      AbstractConfigurationLoader<T> loader, ConfigurationBuilder builder);

    internal const string kDefaultRootNodeName = "appconfig";
    const string kLogLevel = "log-level";
    const string kConfigurationFileKey = "NohrosConfigurationFile";

    /// <summary>
    /// The <see cref="IConfigurationBuilder{T}"/> object specified on
    /// constructor.
    /// </summary>
    protected readonly IConfigurationBuilder<T> builder;
    FileInfo config_file_;

    /// <summary>
    /// The raw XML element used to load the configuration.
    /// </summary>
    protected XmlElement element;

    string location_;
    bool remove_hyphen_from_attribute_names_;
    bool use_dynamic_property_assignment_;
    DateTime version_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AbstractConfigurationLoader{T}"/> class that load the values
    /// of a XML file into a instance of instances of <typeparamref name="T"/>
    /// created through <paramref name="builder"/>.
    /// </summary>
    protected AbstractConfigurationLoader(IConfigurationBuilder<T> builder)
    {
      element = null;
      location_ = AppDomain.CurrentDomain.BaseDirectory;
      version_ = DateTime.Now;
      use_dynamic_property_assignment_ = true;
      remove_hyphen_from_attribute_names_ = true;
      this.builder = builder;
    }
    #endregion

    /// <summary>
    /// Gets a <see cref="XmlElement"/> named <paramref name="element_name"/>
    /// from the loaded configuration file.
    /// </summary>
    /// <param name="element_name">
    /// The name of the xml element to get.
    /// </param>
    /// <returns>
    /// A <see cref="XmlElement"/> which name is
    /// <paramref name="element_name"/>.
    /// </returns>
    /// <exception cref="ConfigurationException">
    /// A <see cref="XmlElement"/> named <paramref name="element_name"/> does
    /// not exists in the configuration file.
    /// </exception>
    /// <remarks>
    /// This is a convenient wrapper for <see cref="SelectElement"/> which uses
    /// the the <see cref="element"/> as the node parameter.
    /// </remarks>
    XmlElement GetConfigurationElement(string element_name) {
      XmlElement local_element = SelectElement(element, element_name);
      if (local_element == null) {
        throw new ConfigurationException(
          string.Format(
            StringResources.Configuration_MissingNode, element_name));
      }
      return local_element;
    }

    /// <summary>
    /// Gets a <see cref="XmlElement"/> named <paramref name="element_name"/>
    /// from the loaded configuration file.
    /// </summary>
    /// <param name="element_name">
    /// The name of the xml element to get.
    /// </param>
    /// <param name="element">
    /// A <see cref="XmlElement"/> which name is
    /// <paramref name="element_name"/> if it is found; otwherwise, null.
    /// </param>
    /// <returns>
    /// <c>true</c> when a element whose name is
    /// <paramref name="element_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ConfigurationException">
    /// A <see cref="XmlElement"/> named <paramref name="element_name"/> does
    /// not exists in the configuration file.
    /// </exception>
    /// <remarks>
    /// This is a convenient wrapper for <see cref="SelectElement"/> which uses
    /// the the <see cref="element"/> as the node parameter.
    /// </remarks>
    internal bool GetConfigurationElement(string element_name,
      out XmlElement element) {
      XmlNode node;
      if (SelectNode(this.element, element_name, XmlNodeType.Element, out node)) {
        element = node as XmlElement;
        return true;
      }
      element = null;
      return false;
    }

    /// <summary>
    /// Selects the first sibling <see cref="XmlNode"/> of the specified node
    /// that matches the specified name.
    /// </summary>
    /// <param name="node">
    /// The parent node.
    /// </param>
    /// <param name="xpath">
    /// The xpath of the node to search for.
    /// </param>
    /// <returns>
    /// The first sibling <see cref="XmlNode"/> of the <paramref name="node"/>
    /// whose name matches the specified name or null if no matching node is
    /// found.
    /// </returns>
    /// <remarks>
    /// Exists a bug into the .NET framework that causes the
    /// <see cref="XmlNode.SelectSingleNode(string)"/> to work incorrectly.
    /// When a XML document is explicit bounded to a schema(xmlns="shcema"),
    /// the following code will return null:
    /// <para>
    ///   <code>
    ///     doc.SelectSingleNode("/sibling-node");
    ///   </code>
    /// </para>
    /// This bug could be worked around by using a
    /// <see cref="XmlNamespaceManager"/> and the
    /// <see cref="XmlNode.SelectSingleNode(string, XmlNamespaceManager)"/>
    /// method overload.
    /// <para>
    ///   <code>
    ///     nsmgr = new XmlNamespaceManager(doc.NameTable);
    ///     nsmgr.AddNamespace("tns", xpath);
    ///     doc.SelectSingleNode("/tns:xpath", nsmgr);
    ///   </code>
    /// </para>
    /// <para>
    /// However, we need to add the prefix "tns" before every element in the
    /// XPath string,
    /// <example>/tns:xpath</example>. This is not good, and cause a lot of
    /// problems.
    /// </para>
    /// <para>
    /// The <see cref="SelectNode"/> method works like the
    /// <see cref="XmlNode.SelectSingleNode(string)"/> except for the above.
    /// <list type="bullet">
    /// <term>
    /// The namespaces are ignored while selecting a node
    /// </term>
    /// <term>
    /// The "/" is the only recognized token. Any other
    /// token(., .., //, @, etc) will be considered as part of the node name.
    /// </term>
    /// </list>
    /// </para>
    /// </remarks>
    public static XmlNode SelectNode(XmlNode node, string xpath) {
      if (node == null || xpath == null)
        throw new ArgumentNullException((node == null) ? "node" : "xpath");

      XmlNode found_node = null;
      SelectNode(node, xpath, XmlNodeType.None, out found_node);
      return found_node;
    }

    /// <summary>
    /// Selects the first sibling <see cref="XmlNode"/> of the specified
    /// node that matches the specified name and is a xml element.
    /// </summary>
    /// <param name="node">
    /// The parent xml node.
    /// </param>
    /// <param name="xpath">
    /// The xpath of the xml element to search for.
    /// </param>
    /// <returns>
    /// The first sibling <see cref="XmlNode"/> of the
    /// <paramref name="node"/> whose name matches the specified name or null
    /// if no matching node is found or if a match is found but the found node
    /// does not represents an xml element.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="node"/> or <paramref name="xpath"/> is a null
    /// reference.
    /// </exception>
    /// <remarks>
    /// Exists a bug into the .NET framework that causes the
    /// <see cref="XmlNode.SelectSingleNode(string)"/> to work incorrectly.
    /// When a XML document is explicit bounded to a schema(xmlns="shcema"),
    /// the following code will return null:
    /// <para>
    ///   <code>
    ///     doc.SelectSingleNode("/sibling-node");
    ///   </code>
    /// </para>
    /// This bug could be worked around by using a
    /// <see cref="XmlNamespaceManager"/> and the
    /// <see cref="XmlNode.SelectSingleNode(string, XmlNamespaceManager)"/>
    /// method overload.
    /// <para>
    ///   <code>
    ///     nsmgr = new XmlNamespaceManager(doc.NameTable);
    ///     nsmgr.AddNamespace("tns", xpath);
    ///     doc.SelectSingleNode("/tns:xpath", nsmgr);
    ///   </code>
    /// </para>
    /// <para>
    /// However, we need to add the prefix "tns" before every element in the
    /// XPath string,
    /// <example>/tns:xpath</example>. This is not good, and cause a lot of
    /// problems.
    /// </para>
    /// <para>
    /// The <see cref="SelectElement"/> method works like the
    /// <see cref="XmlNode.SelectSingleNode(string)"/> except for the above.
    /// <list type="bullet">
    /// <term>
    /// The namespaces are ignored while selecting a node
    /// </term>
    /// <term>
    /// The "/" is the only recognized token. Any other
    /// token(., .., //, @, etc) will be considered as part of the node name.
    /// </term>
    /// <term>
    /// Only xml nodes whose node type is XmlElement will be returned.
    /// </term>
    /// </list>
    /// </para>
    /// <para>
    /// If a xml node (that is not a xml element) and xml element with equals
    /// names exists in the specified xpath, this method ignores the xml node
    /// and returns the xml element.
    /// </para>
    /// </remarks>
    /// <see cref="SelectNode"/>
    public static XmlElement SelectElement(XmlNode node, string xpath) {
      if (node == null || xpath == null)
        throw new ArgumentNullException((node == null) ? "node" : "xpath");

      XmlNode found_node = null;
      SelectNode(node, xpath, XmlNodeType.Element, out found_node);
      return found_node as XmlElement;
    }

    static bool SelectNode(XmlNode node, string xpath, XmlNodeType node_type,
      out XmlNode found_node) {
      int len = xpath.Length;
      int first_character_position = 0;
      if (len > 0 && xpath[0] == '/') {
        if (len > 1 && xpath[1] == '/') {
          first_character_position = 2;
        } else {
          first_character_position = 1;
        }
      }

      string name = (first_character_position != 0)
        ? xpath.Substring(first_character_position)
        : xpath;

      return SelectNodeRecursive(node, name, XmlNodeType.None, out found_node);
    }

    static bool SelectNodeRecursive(XmlNode node, string xpath,
      XmlNodeType node_type, out XmlNode found_node) {
      string name = xpath;
      int delimiter_position = xpath.IndexOf('/');
      if (delimiter_position != -1) {
        name = xpath.Substring(0, delimiter_position);
      }

      foreach (XmlNode n in node.ChildNodes) {
        if (string.Compare(
          n.Name, name, StringComparison.OrdinalIgnoreCase) == 0) {
          if (delimiter_position == -1) {
            if (node_type == XmlNodeType.None) {
              found_node = n;
              return true;
            }
            // If a xml node whose name is "name" was found but it is not a xml
            // element we need to keep searching.
            SelectNodeRecursive(n, name, node_type, out found_node);
          }
          return SelectNodeRecursive(n,
            xpath.Substring(delimiter_position + 1), node_type, out found_node);
        }
      }
      found_node = null;
      return false;
    }

    /// <summary>
    /// Monitor the configuration file for changes and reload the
    /// configuration values if a change is detected.
    /// </summary>
    /// <param name="config_file_info">
    /// The XML configuration file to watch.
    /// </param>
    /// <remarks>
    /// The config file will be monitored using a
    /// <see cref="FileSystemWatcher"/> and is dependant on the behavior of
    /// that class.
    /// </remarks>
    void Watch(FileInfo config_file_info) {
      config_file_ = config_file_info;

      // create a new FileSystemWatcher and set its properties_.
      FileSystemWatcher watcher = new FileSystemWatcher();

      watcher.Path = config_file_info.DirectoryName;
      watcher.Filter = config_file_info.Name;

      // set the notification filters
      watcher.NotifyFilter = NotifyFilters.CreationTime |
        NotifyFilters.LastWrite | NotifyFilters.FileName;

      // add event handler
      watcher.Changed += new FileSystemEventHandler(Watcher_OnChanged);
      watcher.Created += new FileSystemEventHandler(Watcher_OnChanged);
      watcher.Deleted += new FileSystemEventHandler(Watcher_OnChanged);
      watcher.Renamed += new RenamedEventHandler(Watcher_OnRenamed);

      // begin watching
      watcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Event handler used by <see cref="Watch(FileInfo)"/>
    /// </summary>
    /// <param name="source">
    /// The <see cref="FileSystemWatcher"/> firing the event.
    /// </param>
    /// <param name="e">
    /// The argument indicates the file that caused the event to be fired.
    /// </param>
    /// <remarks>
    /// This handler reloads the configuration from the file when the event
    /// is fired.
    /// </remarks>
    void Watcher_OnChanged(object source, FileSystemEventArgs e) {
      // sanity check the root XML element and configuration file for null
      if (config_file_ != null && element != null)
        Load(config_file_, element.Name);
      version_ = DateTime.Now;
    }

    /// <summary>
    /// Event handler used by <see cref="Watch(FileInfo)"/>
    /// </summary>
    /// <param name="source">
    /// The <see cref="FileSystemWatcher"/> firing the event.
    /// </param>
    /// <param name="e">
    /// The argument indicates the file that caused the event to be fired.
    /// </param>
    /// <remarks>
    /// This handler reloads the configuration from the file when the event is
    /// fired.
    /// </remarks>
    void Watcher_OnRenamed(object source, RenamedEventArgs e) {
      // sanity check the root XML element for null
      if (element != null) {
        config_file_ = new FileInfo(e.FullPath);
        Load(config_file_, element.Name);
      }
      version_ = DateTime.Now;
    }

    /// <summary>
    /// Occurs when the configuration load finish.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This event is raised only if the configuration file is loaded
    /// succesfully.
    /// </para>
    /// <para>
    /// Note that if a error occurs while the configuration file is parsed,
    /// this event will not be raised.
    /// </para>
    /// </remarks>
    public event LoadCompleteEventHandler LoadComplete;

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
    public T LoadAndWatch() {
      return Load("nohros", true);
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
    public T LoadAndWatch(string root_node_name) {
      return Load(root_node_name, true);
    }

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
    public T Load() {
      return Load("nohros", false);
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
    public T Load(string root_node_name) {
      object element = ConfigurationManager.GetSection(root_node_name);
      if (element == null) {
        throw new ConfigurationException("The " + root_node_name +
          " node was not defined.");
      }
      return Load((XmlElement) element);
    }

    T Load(string root_node_name, bool watch) {
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
      if (!watch) {
        return Load(config_file_info, root_node_name);
      } else {
        return LoadAndWatch(config_file_info, root_node_name);
      }
    }

    /// <summary>
    /// Gets the configuration root node that is the first node whose name is
    /// "nohros" and is child of the <paramref name="element"/> node.
    /// </summary>
    XmlElement GetRootNode(XmlElement element) {
      XmlElement root_node = null;
      if (Strings.AreEquals(element.Name, Strings.kNohrosNodeName)) {
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
    T Parse(XmlElement element) {
      XmlElement root_node = GetRootNode(element);

      // the logger is used by some methods above and the level threshold of it
      // could be overloaded by a configuration key. So, we need to do the
      // first logger instantiation here and adjust the threshold level if
      // needed.
      builder.SetLogLevel(GetLogLevel(root_node));

      // parse any internal property
      if (use_dynamic_property_assignment_) {
        ParseProperties(root_node, this);
      }

      // parse the know configuration nodes.
      foreach (XmlNode node in root_node.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element) {
          string name = node.Name;
          if (Strings.AreEquals(name, Strings.kRepositoriesNodeName)) {
            builder.SetRepositories(RepositoriesNode.Parse((XmlElement) node,
              location_));
          } else if (Strings.AreEquals(name, Strings.kProvidersNodeName)) {
            builder.SetProviders(ProvidersNode.Parse((XmlElement) node,
              location_));
          } else if (Strings.AreEquals(name, Strings.kLoginModulesNodeName)) {
            builder.SetLoginModules(LoginModulesNode.Parse((XmlElement) node,
              location_));
          } else if (Strings.AreEquals(name, Strings.kXmlElementsNodeName)) {
            XmlElementsNode xml_elements =
              XmlElementsNode.Parse((XmlElement) node);

            // Add the element that was used to configure this class to the
            // collection of xml elements nodes.
            xml_elements[Strings.kRootXmlElementName] = element;
            builder.SetXmlElements(xml_elements);
          }
        }
      }
      T configuration = builder.Build();

      OnLoadComplete(configuration);

      return configuration;
    }

    /// <inheritdoc/>
    public T Load(string config_file_name,
      string root_node_name) {
      string app_base_directory = AppDomain.CurrentDomain.BaseDirectory;
      string config_file_path = Path.Combine(app_base_directory,
        config_file_name);

      FileInfo config_file_info = new FileInfo(config_file_path);

      return Load(config_file_info, root_node_name);
    }

    /// <inheritdoc/>
    public T Load(FileInfo config_file_info,
      string root_node_name) {
      if (config_file_info == null) {
        throw new ArgumentNullException("config_file_info");
      }

      // the root node name could be null, but not a empty string.
      if (root_node_name != null && root_node_name.Trim() == string.Empty) {
        throw new ArgumentException(
          string.Format(
            StringResources.Arg_KeyNotFound, root_node_name));
      }

      // have to use File.Exists() rather than config_file_info.Exists()
      // because config_file_info.Exists() caches the value, not what we want.
      if (File.Exists(config_file_info.FullName)) {
        using (FileStream fs = config_file_info.OpenRead()) {
          // loads the configuration file in memory.
          XmlDocument doc = new XmlDocument();
          doc.Load(fs);

          // retrieve the configuration file location
          location_ = Path.GetDirectoryName(config_file_info.FullName);

          // for backward compatibility this method allows the root_node_name
          // to be null and when its happen we need to select the first valid
          // XML element.
          XmlNode node = null;
          if (root_node_name != null) {
            // If a the root_node is explicit bounded to a schema
            // the XmlDocument.SelectSingleNode(string) will not return the
            // node correctly. To avoid this we will traverse down the XML
            // document trying to match a node with name equals to the
            // specified |root_node_name|, when the SelectSingleNode
            // node return null.
            node = doc.SelectSingleNode(root_node_name) ??
              SelectNode(doc.FirstChild, root_node_name);

            // if the |root_node_name| was specified and the node was not found
            // we need to throw an exception. The first valid Xml element
            // must be used only when |root_node_name| is a null reference.
            if (node == null) {
              throw new ArgumentException(
                string.Format(
                  StringResources.Arg_KeyNotFound, root_node_name));
            }
          } else {
            // If |root_node_name| is  null reference we need to use the
            // first valid Xml element of the document.
            foreach (XmlNode n in doc.ChildNodes) {
              if (n.NodeType == XmlNodeType.Element) {
                node = n;
                break;
              }
            }

            // sanity check the node again. Its unusual but a Xml could have no
            // valid elements. Ex. a xml that contains only the line above, does
            // not have a valid xml element:
            //   <?xml version="1.0" encoding="utf-8"?>
            if (node == null) {
              throw new ConfigurationException(
                string.Format(StringResources.Configuration_InvalidFormat,
                  config_file_info.FullName));
            }
          }

          return Load((XmlElement) node);
        }
      }
      throw new FileNotFoundException(config_file_info.FullName);
    }

    /// <inheritdoc/>
    public T Load(XmlElement element) {
      // This is the main "load" method. This method is called by all the
      // others "load" methods overloads. The null check is done only by this
      // method.
      if (element == null)
        throw new ArgumentNullException("element");

      // store the raw xml element into memory.
      this.element = element;

      return Parse(element);
    }

    /// <inheritdoc/>
    public T LoadAndWatch(string config_file_name,
      string root_node_name) {
      FileInfo config_file_info =
        new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
          config_file_name));
      return Load(config_file_info, root_node_name);
    }

    /// <inheritdoc/>
    public T LoadAndWatch(FileInfo config_file_info,
      string root_node_name) {
      // load the configuration file
      T config = Load(config_file_info, root_node_name);

      // monitor the file and reload the configuration values
      // whenever the config file is modified.
      Watch(config_file_info);

      return config;
    }

    /// <summary>
    /// Gets the configured log level.
    /// </summary>
    LogLevel GetLogLevel(XmlElement element) {
      LogLevel log_level = LogLevel.Info;
      XmlAttribute attribute = element.Attributes[kLogLevel];

      if (attribute != null) {
        switch (attribute.Value.ToLower()) {
          case "all":
            log_level = LogLevel.All;
            break;

          case "debug":
            log_level = LogLevel.Debug;
            break;

          case "info":
            log_level = LogLevel.Info;
            break;

          case "warn":
            log_level = LogLevel.Warn;
            break;

          case "error":
            log_level = LogLevel.Error;
            break;

          case "fatal":
            log_level = LogLevel.Fatal;
            break;

          case "off":
            log_level = LogLevel.Off;
            break;
        }
      }
      return log_level;
    }

    /// <summary>
    /// Raises the <see cref="LoadComplete"/> event.
    /// </summary>
    protected virtual void OnLoadComplete(T configuration) {
      Listeners.SafeInvoke(LoadComplete,
        delegate(LoadCompleteEventHandler runnable) { runnable(configuration); });
    }

    /// <summary>
    /// Load the configuration values by parsing a DOM tree of XML elements.
    /// </summary>
    /// <param name="element">
    /// The Xml element to parse.
    /// </param>
    /// <param name="configuration">
    /// The object to set the properties.
    /// </param>
    /// <remarks>
    /// This method is used to assign values from the root attributes
    /// to the configuration properties dynamically.
    /// <para>
    /// If <paramref name="configuration"/> contains a property whose name are
    /// equals to the name of an XML attribute of the <paramref name="element"/>
    /// node and if the property is writtable and it type is a ValueType or a
    /// String, we will try to set the value of this property to the value of
    /// the XML attribute. If the value of the XML attribute could not be
    /// converted to the ValueType of the property the property value will not
    /// be set.
    /// </para>
    /// <para>
    /// By default the character "-" will be removed from the attribute name
    /// before property search operation. This behavior could be changed by
    /// set the value of the <see cref="RemoveHyphenFromAttributeNames"/> to
    /// false.
    /// </para>
    /// <para>
    /// The dynamic property assignment behavior could be changed by set the
    /// value of the property <see cref="UseDynamicPropertyAssignment"/> to
    /// false.
    /// </para>
    /// </remarks>
    public void ParseProperties(XmlElement element, T configuration) {
      if (element == null) {
        throw new ArgumentNullException("element");
      }

      if (use_dynamic_property_assignment_) {
        ParseProperties(element, (object) configuration);
      }
    }

    /// <summary>
    /// Load the configuration values by parsing a DOM tree of XML elements.
    /// </summary>
    /// <param name="element">
    /// The Xml element to parse.
    /// </param>
    /// <remarks>
    /// This method is used to assign values from the root attributes
    /// to the configuration properties dynamically.
    /// <para>
    /// If a derived class contains a property whose name are equals to the
    /// name of an XML attribute of the <paramref name="element"/> node and if
    /// the property is writtable and it type is a ValueType or a String, we
    /// will try to set the value of this property to the value of the XML
    /// attribute. If the value of the XML attribute could not be converted to
    /// the ValueType of the property the property value will not be set.
    /// </para>
    /// <para>
    /// We do not want to throw an exception inside a protected method. So, the
    /// caller must ensure that the elelement is a valid XML element. If the
    /// specified XML element is a null refrence this method returns silently.
    /// </para>
    /// <para>
    /// By default the character "-" will be removed from the attribute name
    /// before property search operation. This behavior could be changed by
    /// set the value of the <see cref="RemoveHyphenFromAttributeNames"/> to
    /// false.
    /// </para>
    /// <para>
    /// The dynamic property assignment behavior could be changed by set the
    /// value of the property <see cref="UseDynamicPropertyAssignment"/> to
    /// false.
    /// </para>
    /// </remarks>
    protected void ParseProperties(XmlElement element) {
      if (element == null) {
        throw new ArgumentNullException("element");
      }

      if (use_dynamic_property_assignment_) {
        ParseProperties(element, this);
      }
    }

    void ParseProperties(XmlElement element, object obj) {
      XmlAttributeCollection attributes = element.Attributes;
      Type type = obj.GetType();
      PropertyInfo[] properties = type.GetProperties();

      // loop for each attribute defined in the given element and attempt to
      // set the value of a property whose name is equals to the element name.
      foreach (XmlAttribute att in attributes) {
        string property_name = att.Name;
        string property_value = att.Value;

        // If desired remove the "-" character from the attribute name.
        if (remove_hyphen_from_attribute_names_)
          property_name = property_name.Replace("-", "");

        PropertyInfo property;
        if (TryGetProperty(properties, property_name, out property)
          && property.CanWrite) {
          Type property_type = property.PropertyType;
          if (property_type.Name == "String") {
            property.SetValue(obj, property_value, null);
          } else if (property_type.IsValueType) {
            // try to convert the attribute value to the type of the property
            System.ValueType value;
            if (ValueTypes.TryParse(property_type, property_value, out value)) {
              property.SetValue(obj, value, null);
            }
          }
        }
      }
    }

    /// <summary>
    /// Try to find a property which name is equals to the specified property
    /// name.
    /// </summary>
    /// <param name="properties">The properties array to search for.</param>
    /// <param name="property_name">The name of the property to find.</param>
    /// <param name="property">When this method returns contains a property
    /// whose name is <paramref name="property_name"/>. If a property with
    /// name <see cref="property_name"/> could not be found, the
    /// <paramref name="property_name"/> will contains null.</param>
    /// <returns>true if a property with name <paramref name="property_name"/>
    /// is found; otherwise, false.</returns>
    bool TryGetProperty(PropertyInfo[] properties, string property_name,
      out PropertyInfo property) {
      property = null;
      for (int i = 0, j = properties.Length; i < j; i++) {
        property = properties[i];
        if (string.Compare(property.Name, property_name,
          StringComparison.OrdinalIgnoreCase) == 0) {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Gets the directory path where the configuration file is stored.
    /// </summary>
    /// <returns>
    /// An string that represents the location of the configuration file or the
    /// application base directory if the location could not be retrieved.
    /// </returns>
    public string Location {
      get { return location_; }
    }

    /// <summary>
    /// Gets the date and time when one of the Load methods was last called.
    /// </summary>
    public DateTime Version {
      get { return version_; }
    }

    /// <summary>
    /// Gets a value indicating if the dynamic property assignment feature must
    /// be used or not.
    /// </summary>
    /// <value>true to use the dynamic property assignment feature; otherwise,
    /// false.</value>
    public bool UseDynamicPropertyAssignment {
      get { return use_dynamic_property_assignment_; }
      set { use_dynamic_property_assignment_ = value; }
    }

    /// <summary>
    /// Gets a value indicating if the hyphen sign("_") should be removed from
    /// the attribute name before find for a property with equals name.
    /// </summary>
    /// <value>true to remove the hyphen; otherwise, false.</value>
    public bool RemoveHyphenFromAttributeNames {
      get { return remove_hyphen_from_attribute_names_; }
      set { remove_hyphen_from_attribute_names_ = value; }
    }
  }
}
