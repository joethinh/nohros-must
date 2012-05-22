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
  public abstract class AbstractConfiguration : IConfiguration
  {
    internal const string kDefaultRootNodeName = "appconfig";

    FileInfo config_file_;

    /// <summary>
    /// The configuration root node.
    /// </summary>
    protected XmlElement element;

    /// <summary>
    /// The location of the configuration file or application base directory if the location
    /// could not be found.
    /// </summary>
    protected string location;

    protected bool remove_hyphen_from_attribute_names;

    /// <summary>
    /// The collection of all defined events for this class.
    /// </summary>
    protected bool use_dynamic_property_assignment;

    DateTime version_;

    #region .ctor
    /// <summary>
    /// Initializezs a new instance of the <see cref="AbstractConfiguration"/>
    /// class.
    /// </summary>
    protected AbstractConfiguration() {
      element = null;
      location = AppDomain.CurrentDomain.BaseDirectory;
      version_ = DateTime.Now;
      use_dynamic_property_assignment = true;
      remove_hyphen_from_attribute_names = true;
    }
    #endregion

    #region IConfiguration Members
    /// <inheritdoc/>
    public virtual void Load() {
      Load(kDefaultRootNodeName);
    }

    /// <inheritdoc/>
    public virtual void Load(string root_node_name) {
      object element = ConfigurationManager.GetSection(root_node_name);
      if (element == null) {
        throw new ConfigurationException("The appconfig node was not defined.");
      }
      Load((XmlElement) element);
    }

    /// <inheritdoc/>
    public virtual void Load(string config_file_name, string root_node_name) {
      string app_base_directory = AppDomain.CurrentDomain.BaseDirectory;
      string config_file_path = Path.Combine(app_base_directory,
        config_file_name);

      FileInfo config_file_info = new FileInfo(config_file_path);

      Load(config_file_info, root_node_name);
    }

    /// <inheritdoc/>
    public virtual void Load(FileInfo config_file_info, string root_node_name) {
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
          location = Path.GetDirectoryName(config_file_info.FullName);

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
            node = doc.SelectSingleNode(root_node_name);
            if (node == null) {
              node = SelectNode(doc.FirstChild, root_node_name);
            }

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

          Load((XmlElement) node);
        }
      } else {
        throw new FileNotFoundException(config_file_info.FullName);
      }
    }

    /// <inheritdoc/>
    public virtual void Load(XmlElement element) {
      OnPreLoad();

      // This is the main "load" method. This method is called by all the
      // others "load" methods overloads. The null check is done only by this
      // method.
      if (element == null)
        throw new ArgumentNullException("element");

      // store the raw xml element into memory.
      this.element = element;

      Parse(element);

      OnLoadComplete();
    }

    /// <inheritdoc/>
    public virtual void LoadAndWatch(string config_file_name,
      string root_node_name) {
      FileInfo config_file_info =
        new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
          config_file_name));
      Load(config_file_info, root_node_name);
    }

    /// <inheritdoc/>
    public void LoadAndWatch(FileInfo config_file_info, string root_node_name) {
      // load the configuration file
      Load(config_file_info, root_node_name);

      // monitor the file and reload the configuration values
      // whenever the config file is modified.
      Watch(config_file_info);
    }

    /// <summary>
    /// Gets the directory path where the configuration file is stored.
    /// </summary>
    /// <returns>
    /// An string that represents the location of the configuration file or the
    /// application base directory if the location could not be retrieved.
    /// </returns>
    public string Location {
      get { return location; }
    }
    #endregion

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
      return SelectNode(node, xpath, XmlNodeType.None);
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
      return SelectNode(node, xpath, XmlNodeType.Element) as XmlElement;
    }

    static XmlNode SelectNode(XmlNode node, string xpath, XmlNodeType node_type) {
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

      return SelectNodeRecursive(node, name, XmlNodeType.None);
    }

    static XmlNode SelectNodeRecursive(XmlNode node, string xpath,
      XmlNodeType node_type) {
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
              return n;
            }
            // If a xml node whose name is "name" was found but it is not a xml
            // element we need to keep searching.
            SelectNodeRecursive(n, name, node_type);
          }
          return SelectNodeRecursive(
            n, xpath.Substring(delimiter_position + 1), node_type);
        }
      }
      return null;
    }

    /// <summary>
    /// Compares two strings for equality.
    /// </summary>
    /// <param name="str_a">
    /// The first string to compare.
    /// </param>
    /// <param name="str_b">
    /// The second string to compare.
    /// </param>
    /// <returns>
    /// <c>true</c> if the two specified strings are equals; otherwise,
    /// <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs a ordinal case-insensitive comparison.
    /// </remarks>
    internal static bool StringsAreEquals(string str_a, string str_b) {
      return string.Compare(str_a, str_b, StringComparison.Ordinal) == 0;
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
    /// Occurs when the configuration file load beguns.
    /// </summary>
    public event RunnableDelegate PreLoad;

    /// <summary>
    /// Raises the <see cref="PreLoad"/> event.
    /// </summary>
    protected virtual void OnPreLoad() {
      Listeners.SafeInvoke(PreLoad,
        delegate(RunnableDelegate runnable) { runnable(); });
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
    public event RunnableDelegate LoadComplete;

    /// <summary>
    /// Raises the <see cref="LoadComplete"/> event.
    /// </summary>
    protected virtual void OnLoadComplete() {
      Listeners.SafeInvoke(LoadComplete,
        delegate(RunnableDelegate runnable) { runnable(); });
    }

    /// <summary>
    /// Parses a <see cref="XmlElement"/> object into a
    /// <see cref="AbstractConfiguration"/> object.
    /// </summary>
    /// <param name="element">
    /// A <see cref="XmlElement"/> that containing the configuration data.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This mehtod is called by the one of the Loads method to parses the
    /// <paramref name="element"/>. This method only calls the
    /// <see cref="ParseProperties"/> method, you should override this method
    /// if you want to parse more information than the element properties.
    /// </remarks>
    /// <seealso cref="ParseProperties"/>
    protected virtual void Parse(XmlElement element) {
      if (element == null) {
        throw new ArgumentNullException("element");
      }
      ParseProperties(element);
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
#if DEBUG
      if (element == null)
        throw new ArgumentNullException(
          "[Nohros.Configuration.AbstractConfiguration   Parse] element is null");
#endif

      if (!use_dynamic_property_assignment)
        return;

      XmlAttributeCollection attributes = element.Attributes;

      Type type = GetType();
      PropertyInfo[] properties = type.GetProperties();

      // loop for each attribute defined in the given element and attempt to
      // set the value of a property whose name is equals to the element name.
      foreach (XmlAttribute att in attributes) {
        string property_name = att.Name;
        string property_value = att.Value;

        // If desired remove the "-" character from the attribute name.
        if (remove_hyphen_from_attribute_names)
          property_name = property_name.Replace("-", "");

        PropertyInfo property;
        if (TryGetProperty(properties, property_name, out property)
          && property.CanWrite) {
          Type property_type = property.PropertyType;
          if (property_type.Name == "String") {
            property.SetValue(this, property_value, null);
          } else if (property_type.IsValueType) {
            // try to convert the attribute value to the type of the property
            System.ValueType value;
            if (ValueTypes.TryParse(property_type, property_value, out value)) {
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
      get { return use_dynamic_property_assignment; }
      set { use_dynamic_property_assignment = value; }
    }

    /// <summary>
    /// Gets a value indicating if the hyphen sign("_") should be removed from
    /// the attribute name before find for a property with equals name.
    /// </summary>
    /// <value>true to remove the hyphen; otherwise, false.</value>
    public bool RemoveHyphenFromAttributeNames {
      get { return remove_hyphen_from_attribute_names; }
      set { remove_hyphen_from_attribute_names = value; }
    }
  }
}
