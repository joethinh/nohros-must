using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Web;
using System.Reflection;
using System.Configuration;

using Nohros.Logging;
using Nohros.Resources;
using Nohros.Data;

namespace Nohros.Configuration
{
  /// <summary>
  /// A class to be used like a base class for configuration.
  /// </summary>
  public abstract class IConfiguration
  {
    FileInfo config_file_;

    /// <summary>
    /// The configuration root node.
    /// </summary>
    protected XmlElement element_;

    /// <summary>
    /// The location of the configuration file or application base directory if the location
    /// could not be found.
    /// </summary>
    protected string location_;

    internal const string kDefaultRootNodeName = "appconfig";

    DateTime version_;

    /// <summary>
    /// The collection of all defined events for this class.
    /// </summary>
    protected System.ComponentModel.EventHandlerList events_;
    protected bool use_dynamic_property_assignment_;
    protected bool remove_hyphen_from_attribute_names_;

    #region events keys
    /// <summary>
    /// Key for the PreLoad event
    /// </summary>
    protected static readonly object EventPreLoad;

    /// <summary>
    /// Key for the LoadComplete object.
    /// </summary>
    protected static readonly object EventLoadComplete;
    #endregion

    #region .ctor
    /// <summary>
    /// Initialize the static event keys.
    /// </summary>
    static IConfiguration() {
      EventPreLoad = new object();
      EventLoadComplete = new object();
    }

    /// <summary>
    /// Initializezs a new instance of the <see cref="IConfiguration"/> class.
    /// </summary>
    protected IConfiguration() {
      element_ = null;
      location_ = AppDomain.CurrentDomain.BaseDirectory;
      version_ = DateTime.Now;
      events_ = new System.ComponentModel.EventHandlerList();
    }
    #endregion

    #region Load(...) overloads
    /// <summary>
    /// Loads the configuration values by parsing the application's configuration settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as the application
    /// whith ' .config ' appended. This file is XML and calling this function prompts the
    /// loader to look in that file for a section called <c>appconfig</c> that contains the
    /// configuration data.
    /// </remarks>
    /// <exception cref="ArgumentNullException">A element with name "appconfig" could not be found
    /// into the application configuration file.</exception>
    /// <seealso cref="Load(string)"/>
    /// <seealso cref="Load(XmlElement)"/>
    public virtual void Load() {
      Load(kDefaultRootNodeName);
    }

    /// <summary>
    /// Loads the configuration by parsing the application's configuration settings.
    /// </summary>
    /// <param name="root_node_name">The xpath of the node that contains the configuration data.</param>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as the application
    /// whith ' .config ' appended. This file is XML and calling this function prompts the
    /// loader to look in that file for a section called <paramref name="root_node_name"/> that
    /// contains the configuration data.
    /// </remarks>
    /// <exception cref="ArgumentNullException">A element whose xpath is <paramref name="root_node_name"/>
    /// could not be found into the loaded configuration file.</exception>
    /// <seealso cref="Load(XmlElement)"/>
    public virtual void Load(string root_node_name) {
      Load((XmlElement)System.Configuration.ConfigurationManager.GetSection(root_node_name));
    }

    /// <summary>
    /// Load the configuration by parsing using the specified configuration file.
    /// </summary>
    /// <param name="config_file_name">The name of the configuration file.</param>
    /// <param name="root_node_name">The xpath of the node that contains the configuration data.</param>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first XML element will be used as 
    /// root node.
    /// <para>
    /// This method assumes that the specified configuration file is located in the application
    /// base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one element called
    /// <paramref name="root_node_name"/>.
    /// </para>
    /// <para>
    /// For backward compatibility this method allows the <see cref="root_node_name"/> to be a null
    /// reference and when is the case the first valid Xml element will be used like the
    /// configuration root node.
    /// </para>
    /// <para>
    /// If you need to monitor this file for changes and reload the configuration when the config
    /// file's contents changes then you should use the <see cref="LoadAndWatch(string, string)"/>
    /// method instead.
    /// </para>
    /// </remarks>
    /// <exception cref="FileNotFoundException">The configuration file does not exists or is not
    /// located into the application base directory.</exception>
    /// <exception cref="ArgumentNullException">A element with name <paramref name="root_node_name"/>
    /// could not be found into the loaded configuration file.</exception>
    /// <seealso cref="Load(string, string)"/>
    /// <seealso cref="Load(FileInfo, string)"/>
    public virtual void Load(string config_file_name, string root_node_name) {
      string app_base_directory = AppDomain.CurrentDomain.BaseDirectory;
      string config_file_path = Path.Combine(app_base_directory, config_file_name);

      FileInfo config_file_info = new FileInfo(config_file_path);

      Load(config_file_info, root_node_name);
    }

    /// <summary>
    /// Load the configuation values using the specified configuration file and node name.
    /// </summary>
    /// <param name="config_file_info">The XML configuration file to load the configuration from.</param>
    /// <param name="root_node_name">The xpath of the node that contains the configuration data.</param>
    /// <exception cref="FileNotFoundException">If the config file does not exists.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="config_file_info"/> is a null
    /// reference</exception>
    /// <exception cref="ArgumentException">A element with name <paramref name="root_node_name"/>
    /// could not be found into the configuration file.</exception>
    /// <remarks>
    /// <para>
    /// The config file could be specified into the main application configuration file through a
    /// custom configuration key. To load the configuration use code like above:
    /// </para>
    /// <para>
    /// For backward compatibility this method allows the <paramref name="root_node_name"/> to be a
    /// null reference and when is the case the first Xml element will be used like the
    /// configuration root node.
    /// </para>
    /// <code>
    /// using Nohros.Configuration;
    /// using System.IO;
    /// using System.Configuration;
    /// 
    /// ...
    /// string app_base_directory =  AppDomain.CurrentDomain.BaseDirectory;
    /// string config_file_name = ConfigurationSettings.AppSettings["my-custom-config-file-path"];
    /// string config_file_path = Path.Combine(app_base_directory, config_file_name);
    /// 
    /// FileInfo config_file = new FileInfo(config_file_path);
    /// IConfiguration config_object = MyFactory.GetConfigObject();
    /// config_object.Load(config_file, "my-config-root-node");
    /// </code>
    /// <para>
    /// In your application configuration file you must specify the configuration file to use like
    /// this:
    /// </para>
    /// <code>
    /// &lt;configuration&gt;
    ///		&lt;appSettings&gt;
    ///			&lt;add key="my-custom-config-file-path" value="MyCustom.config"/&gt;
    ///		&lt;/appSettings&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// In that case your configuration file must have a node named "my-config-root-node".
    /// <para>
    /// If you need to monitor this file for changes and reload the configuration when the config
    /// file's contents changes then you should use the <see cref="LoadAndWatch(FileInfo, string)"/>
    /// method instead.
    /// </para>
    /// </remarks>
    public virtual void Load(FileInfo config_file_info, string root_node_name) {
      if (config_file_info == null)
        throw new ArgumentNullException("config_file_info");

      root_node_name = root_node_name.Trim();
      if (root_node_name == string.Empty)
        throw new ArgumentException(
            string.Format(
                StringResources.Config_KeyNotFound, root_node_name));

      // have to use File.Exists() rather than config_file_info.Exists()
      // because config_file_info.Exists() caches the value, not what we want.
      if (File.Exists(config_file_info.FullName)) {
        using (FileStream fs = config_file_info.OpenRead()) {
          // loads the configuration file to memory.
          XmlDocument doc = new XmlDocument();
          doc.Load(fs);

          // retrieve the configuration file location
          location_ = Path.GetDirectoryName(config_file_info.FullName);

          // for backward compatibility this method allows the root_node_name
          // to be null and when its happen we need to select the first
          // valid XML element. If a the root_node is explicit bounded to a schema
          // the XmlDocument.SelectSIngleNode(string) will not return the node correctly.
          // If this happen we will traverse down the XML trying to match a node name
          // with the specified root_node_name.
          XmlNode node = (root_node_name == null) ? null : doc.SelectSingleNode(root_node_name);
          if (node == null) {
            if (root_node_name != null) {
              // if the root_node_name was specified and the node was not found
              // we need to throw an exception. The first valid Xml element must be
              // used only when root_node_name is a null reference.
              node = SelectNode(node, root_node_name);
              if (node == null)
                throw new ArgumentException(string.Format(StringResources.Config_KeyNotFound, root_node_name));
            } else {
              // If root_node_name is  null reference we need to used the first valid
              // Xml element.
              foreach (XmlNode n in doc.ChildNodes) {
                if (n.NodeType == XmlNodeType.Element) {
                  node = n;
                  break;
                }
              }

              // sanity check the node again. Its unusual but a Xml could have no
              // valid elements. Ex. a xml that contains only the line above:
              //      <?xml version="1.0" encoding="utf-8"?>
              if (node == null)
                throw new ArgumentException(string.Format(StringResources.Config_KeyNotFound, root_node_name));
            }
          }

          Load((XmlElement)node);
        }
      } else {
        throw new FileNotFoundException(string.Format(StringResources.Config_FileNotFound_Path, config_file_info.FullName));
      }
    }

    /// <summary>
    /// Load the configuration values parsing the specified XML element.
    /// </summary>
    /// <remarks>
    /// Load the configuration from the XML element supplied as <paramref name="element"/>
    /// </remarks>
    /// <param name="element">The <see cref="XmlElement"/> containing the configuration values to
    /// parse.</param>
    /// <exception cref="ArgumentNullException">element is a null reference.</exception>
    public virtual void Load(XmlElement element) {
      OnPreLoad(EventArgs.Empty);

      // This is the main Load() method. This method is called by all the others Load() methods
      // overloads. The null check is done only by this method.
      if (element == null)
        throw new ArgumentNullException("element");

      // store the raw xml element into memory.
      element_ = element;

      Parse(element);

      OnLoadComplete(EventArgs.Empty);
    }

    /// <summary>
    /// Selects the first sibling <see cref="XmlNode"/> of the specified node that matches the
    /// specified name.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <param name="xpath">The xpath of the node to search for.</param>
    /// <returns>The first sibling <see cref="XmlNode"/> of the <paramref name="node"/> whose name
    /// matches the specified name or null if no matching node is found.</returns>
    /// <remarks>
    /// Exists a bug into the .NET framework that causes the
    /// <see cref="XmlNode.SelectSingleNode(string)"/> to work incorrectly. When a XML document is
    /// explicit bounded to a schema(xmlns="shcema"), the following code will return null:
    /// <para>
    ///     <code>
    ///         doc.SelectSingleNode("/smsproject");
    ///     </code>
    /// </para>
    /// This bug could be worked around by using a <see cref="XmlNamespaceManager"/> and the
    /// <see cref="XmlNode.SelectSingleNode(string, XmlNamespaceManager)"/> method overload.
    /// <para>
    ///     <code>
    ///         nsmgr = new XmlNamespaceManager(doc.NameTable);
    ///         nsmgr.AddNamespace("tns", xpath);
    ///         doc.SelectSingleNode("/tns:xpath", nsmgr);
    ///     </code>
    /// </para>
    /// <para>
    /// However, we need to add the prefix "tns" before every element in the XPath string,
    /// <example>/tns:xpath</example>. This is not good, and cause a lot of problems.
    /// </para>
    /// <para>
    /// The SelectNode method works like the <see cref="XmlNode.SelectSingleNode(string)"/> except
    /// for the above.
    ///     . The namespaces are ignored while selecting a node
    ///     . The "/" is the only recognized token. Any other token(., .., //, @, etc) will be considered
    ///       as part of the node name.
    /// </para>
    /// </remarks>
    public static XmlNode SelectNode(XmlNode node, string xpath) {
      if (node == null || xpath == null)
        throw new ArgumentNullException((node == null) ? "node" : "xpath");

      string name = xpath;
      int delimiter_position = xpath.IndexOf('/', 0);
      if (delimiter_position != -1)
        name = xpath.Substring(0, delimiter_position);

      foreach (XmlNode n in node.ChildNodes) {
        if (string.Compare(n.Name, name, StringComparison.OrdinalIgnoreCase) == 0) {
          if (delimiter_position == -1)
            return n;
          return SelectNode(n, xpath.Substring(delimiter_position + 1));
        }
      }
      return null;
    }

    /// <summary>
    /// Load the configuration values using the specified configuration file, monitor the file for
    /// changes and reload the configuration if a change is detected.
    /// </summary>
    /// <param name="config_file_name">The name of the configuration file.</param>
    /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first XML element will be used as a
    /// root node.
    /// <para>
    /// This method assumes that the specified configuration file is located in the application
    /// base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one element called
    /// <paramref name="root_node_name"/> that contains the configuration data.
    /// </para>
    /// <para>
    /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
    /// on the behavior of that class.
    /// </para>
    /// <para>
    /// The <see cref="Load(FileInfo, String)"/> method will be called to reload the cofiguration
    /// values.
    /// </para>
    /// </remarks>
    /// <exception cref="FileNotFoundException">The configuration file does not exists or is not located in
    /// the application base directory.</exception>
    public virtual void LoadAndWatch(string config_file_name, string root_node_name) {
      FileInfo config_file_info = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config_file_name));
      Load(config_file_info, root_node_name);
    }

    /// <summary>
    /// Load the configuration values using the file specified, monitor the file for changes
    /// and reload the configuration if a change is detected.
    /// </summary>
    /// <param name="config_file_info">The XML config file to load the configuration from.</param>
    /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
    /// <exception cref="ArgumentNullException"><paramref name="config_file_info"/> is null.</exception>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first XML element will be used as a root node.
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one element called
    /// </para>>
    /// <paramref name="root_node_name"/> that contains the configuration data.
    /// <para>
    /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
    /// on the behavior of that class.
    /// </para>
    /// <para>
    /// The <see cref="Load(FileInfo, String)"/> method will be called to reload the cofiguration
    /// values.
    /// </para>
    /// </remarks>
    public void LoadAndWatch(FileInfo config_file_info, string root_node_name) {
      // load the configuration file
      Load(config_file_info, root_node_name);

      // monitor the file and reload the configuration values
      // whenever the config file is modified.
      Watch(config_file_info);
    }
    #endregion

    #region Watch handler
    /// <summary>
    /// Monitor the configuration file for changes and reload the configuration values
    /// if a change is detected.
    /// </summary>
    /// <param name="config_file_info">The XML configuration file to watch.</param>
    /// <remarks>
    /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
    /// on the behavior of that class.
    /// </remarks>
    void Watch(FileInfo config_file_info) {
      config_file_ = config_file_info;

      // create a new FileSystemWatcher and set its properties_.
      FileSystemWatcher watcher = new FileSystemWatcher();

      watcher.Path = config_file_info.DirectoryName;
      watcher.Filter = config_file_info.Name;

      // set the notification filters
      watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;

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
    /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event</param>
    /// <param name="e">The argument indicates the file that caused the event to be fired</param>
    /// <remarks>
    /// This handler reloads the configuration from the file when the event is fired.
    /// </remarks>
    private void Watcher_OnChanged(object source, FileSystemEventArgs e) {
      // sanity check the root XML element and configuration file for null
      if (config_file_ != null && element_ != null)
        Load(config_file_, element_.Name);
      version_ = DateTime.Now;
    }

    /// <summary>
    /// Event handler used by <see cref="Watch(FileInfo)"/>
    /// </summary>
    /// <param name="source">the <see cref="FileSystemWatcher"/> firing the event</param>
    /// <param name="e">the argument indicates the file that caused the event to be fired</param>
    /// <remarks>
    /// This handler reloads the configuration from the file when the event is fired.
    /// </remarks>
    private void Watcher_OnRenamed(object source, RenamedEventArgs e) {
      // sanity check the root XML element for null
      if (element_ != null) {
        config_file_ = new FileInfo(e.FullPath);
        Load(config_file_, element_.Name);
      }
      version_ = DateTime.Now;
    }
    #endregion

    #region Events
    /// <summary>
    /// Occurs when the configuration file load beguns.
    /// </summary>
    public event EventHandler PreLoad {
      add {
        events_.AddHandler(EventPreLoad, value);
      }
      remove {
        events_.RemoveHandler(EventPreLoad, value);
      }
    }

    /// <summary>
    /// Raises the <see cref="PreLoad"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnPreLoad(EventArgs e) {
      EventHandler handler = events_[EventPreLoad] as EventHandler;
      if (handler != null) {
        handler(this, e);
      }
    }

    /// <summary>
    /// Occurs when the configuration load finish.
    /// </summary>
    /// <remarks>
    /// <para></para>
    /// This event is raised only if the configuration file is loaded succesfully.
    /// </para>
    /// <para>
    /// Note that if a error occurs while the configuration file is parsed, this event will not be
    /// raised.
    /// </para>
    /// </remarks>
    public event EventHandler LoadComplete {
      add {
        events_.AddHandler(EventLoadComplete, value);
      }
      remove {
        events_.RemoveHandler(EventLoadComplete, value);
      }
    }

    /// <summary>
    /// Raises the <see cref="LoadComplete()"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnLoadComplete(EventArgs e) {
      EventHandler handler = events_[EventLoadComplete] as EventHandler;
      if (handler != null) {
        handler(this, e);
      }
    }
    #endregion

    /// <summary>
    /// Used internally to load the configuration values by parsing a DOM tree
    /// of XML elements.
    /// </summary>
    /// <param name="element">The root element to parse.</param>
    /// <remarks>
    /// This method is used only to assign values from the root attributes
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
    internal virtual void Parse(XmlElement element) {
#if DEBUG
      if (element == null)
        throw new ArgumentNullException("[Nohros.Configuration.IConfiguration   Parse] element is null");
#endif

      if (!use_dynamic_property_assignment_)
        return;

      XmlAttributeCollection attributes = element.Attributes;

      Type type = this.GetType();
      PropertyInfo[] properties = type.GetProperties();

      // loop for each attribute defined in the given element and attempt to
      // set the value of a property whose name is equals to the element name.
      // If a match could ot be found try to remove the "-" character from the
      // attribute name and do the match again.
      foreach (XmlAttribute att in attributes) {
        string property_name = att.Name;
        string property_value = att.Value;

        PropertyInfo property;
        if (TryGetProperty(properties, property_name, out property)
          && property.CanWrite) {
          Type property_type = property.PropertyType;
          if (property_type.Name == "String") {
            property.SetValue(this, property_value, null);
          } else if (property_type.IsValueType) {
            // try to convert the attribute value to the type of the property
            System.ValueType value;
            if (DataHelper.TryParse(property_type, property_value, out value)) {
              property.SetValue(this, value, null);
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

    /// <summary>
    /// Gets the directory path where the configuration file is stored.
    /// </summary>
    /// <returns>An string that represents the location of the configuration file or the application
    /// base directory if the location could not be retrieved.</returns>
    public string Location {
      get { return location_; }
      set { location_ = value; }
    }
  }
}