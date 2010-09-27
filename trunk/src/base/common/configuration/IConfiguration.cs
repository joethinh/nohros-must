using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Web;
using System.Reflection;
using System.Configuration;

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

        string location_;
        DateTime version_;

        #region .ctor
        /// <summary>
        /// Initializezs a new instance_ of the IConfiguration class.
        /// </summary>
        protected IConfiguration()
        {
            element_ = null;
            Location = null;
            version_ = DateTime.Now;
        }
        #endregion

        #region Load(...)overloads
        /// <summary>
        /// Loads the configuration values based on the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same name as the application
        /// whith ' .config ' appended. This file is XML and calling this function prompts the
        /// loader to look in that file for a section called <c>appconfig</c> that contains the
        /// configuration data.
        /// </remarks>
        public virtual void Load()
        {
            Load("appconfig");
        }

        /// <summary>
        /// Loads the configuration values based on the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same name as the application
        /// whith ' .config ' appended. This file is XML and calling this function prompts the
        /// loader to look in that file for a section called<paramref name="root_node_name"/>root_node_name that
        /// contains the configuration data.
        /// </remarks>
        public virtual void Load(string root_node_name) {
            Load((XmlElement)System.Configuration.ConfigurationManager.GetSection(root_node_name));
        }

        /// <summary>
        /// Load the configuration values using the specified XML element.
        /// </summary>
        /// <remarks>
        /// Load the configuration from the XML element supplied as <paramref name="element"/>
        /// <para>
        /// This is the main Load(...) overload. This method is called by all the others Load(...)
        /// methods overloads.
        /// </para>
        /// </remarks>
        /// <param name="element">The XmlElement containing the configuration values to parse.</param>
        public virtual void Load(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            // the location could not be set here
            // if it is not set yet this value will be null
            // location.

            element_ = element;

            Parse(element);
        }

        /// <summary>
        /// Load the configuration values using the specified configuration file.
        /// </summary>
        /// <param name="config_file_name">The name of the configuration file.</param>
        /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
        /// <remarks>
        /// If the <paramref name="root_node_name"/> is null, the first XML element will be used as a root node.
        /// <para>
        /// This method assumes that the specified configuration file is located in the application
        /// base directory.
        /// </para>
        /// <para>
        /// The configuration file must be valid XML. It must contain at least one element called
        /// <paramref name="root_node_name"/> that contains the configuration data.
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">The configuration file does not exists or is not located in
        /// the application base directory.</exception>
        public virtual void Load(string config_file_name, string root_node_name)
        {
            FileInfo config_file_info = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config_file_name));
            Load(config_file_info, root_node_name);
        }

        /// <summary>
        /// Load the configuation values using the specified configuration file and node name.
        /// </summary>
        /// <param name="config_file_info">the XML configuration file to load the configuration from.</param>
        /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
        /// <remarks>
        /// <para>
        /// The config file could be specified in the applications configuration file (either 
        /// <c>MyAppName.exe.config</c> for a normal application on <c>Web.config</c> for an
        /// ASP.NET application. To load the configuration use code like:
        /// </para>
        /// <code>
        /// using Nohros.Configuration;
        /// using System.IO;
        /// using System.Configuration;
        /// 
        /// ...
        /// 
        /// base.Load(new FileInfo(ConfigurationSettings.AppSettings["my-custom-config-file-path"]), "customConfigNode");
        /// </code>
        /// <para> In your application configuration file you must specify the configuration file to use like this:
        /// </para>
        /// <code>
        /// &lt;configuration&gt;
        ///		&lt;appSettings&gt;
        ///			&lt;add key="my-custom-config-file-path" value="MyCustom.config"/&gt;
        ///		&lt;/appSettings&gt;
        ///	&lt;/configuration&gt;
        /// </code>
        /// In that case your configuration file must have a node named "customConfigNode".
        /// <para>
        /// If you need to monitor this file for changes and reload the configuration when the config
        /// file's contents changes then you should use the <see cref="LoadAndWatch(FileInfo, string)"/>method instead.
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">If the config file does not exists</exception>
        public virtual void Load(FileInfo config_file_info, string root_node_name)
        {
            if (config_file_info == null)
                throw new ArgumentNullException("config_file_info");

            // Have to use File.Exists() rather than config_file_info.Exists()
            // because config_file_info.Exists() caches the value, not what we want.
            if (File.Exists(config_file_info.FullName))
            {
                // Open the file for reading
                FileStream fs = config_file_info.OpenRead();
                try
                {
                    // Loads the configuration file to memory.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fs);

                    // retrieve the configuration file location
                    Location = Path.GetDirectoryName(config_file_info.FullName);

                    XmlNode node = (root_node_name == null) ? null : doc.SelectSingleNode(root_node_name); // for backward compatibility
                    if (node == null) {
                        foreach (XmlNode n in doc.ChildNodes) {
                            if (n.NodeType == XmlNodeType.Element) {
                                node = n;
                                break;
                            }
                        }

                        if (root_node_name != null)
                            node = SelectNode(node, root_node_name);
                    }

                    Load((XmlElement)node);
                }
                finally
                {
                    // Force the file closed whatever happens
                    fs.Close();
                }
            }
            else
            {
                // TODO: Log the exception
                throw new System.IO.FileNotFoundException(string.Format(StringResources.Config_FileNotFound_Path, config_file_info.FullName));
            }
        }

        /// <summary>
        /// Selects the first child <see cref="XmlNode"/> of the specified node that matches the specified name.
        /// </summary>
        /// <param name="node">The parent node.</param>
        /// <param name="name">The name of the node.</param>
        /// <returns>The first child <see cref="XmlNode"/> of the <paramref name="node"/> that matches the XPath
        /// query or null if no matching node is found.</returns>
        internal static XmlNode SelectNode(XmlNode node, string name) {
            if (node == null || name == null)
                throw new ArgumentNullException((node == null) ? "name" : "xpath");

            foreach (XmlNode n in node.ChildNodes) {
                if (string.Compare(n.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    return n;
            }
            return null;
        }

        /// <summary>
        /// Load the configuration values using the specified configuration file, monitor the file for changes
        /// and reload the configuration if a change is detected.
        /// </summary>
        /// <param name="config_file_name">The name of the configuration file.</param>
        /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
        /// <remarks>
        /// If the <paramref name="root_node_name"/> is null, the first XML element will be used as a root node.
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
        public void LoadAndWatch(FileInfo config_file_info, string root_node_name)
        {
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
        /// <param name="config_file_info">the XML configuration file to watch</param>
        /// <remarks>
        /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
        /// on the behavior of that class.
        /// </remarks>
        void Watch(FileInfo config_file_info)
        {
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
        private void Watcher_OnChanged(object source, FileSystemEventArgs e)
        {
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
        private void Watcher_OnRenamed(object source, RenamedEventArgs e)
        {
            // sanity check the root XML element for null
            if (element_ != null != null) {
                config_file_ = new FileInfo(e.FullPath);
                Load(config_file_, element_.Name);
            }
            version_ = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// Used internally to load the configuration values by parsing a DOM tree of XML elements.
        /// </summary>
        /// <param name="element">the root element to parse</param>
        /// <remarks>
        /// If a derived class contains a property whose name are equals to the name of an
        /// XML attribute of the <paramref name="element"/> node and if the property is writtable
        /// and it type is a ValueType or a String, we will try to set the value of this property to
        /// the value of the XML attribute. If the value of the XML attribute could not be converted to
        /// the Type of the property the property value will not be set.
        /// <para>
        /// We do not want to throw an exception inside a protected method. So, the caller must
        /// ensure that the elelement is a valid XML element. If the specified XML element is a null
        /// refrence this method returns silently.
        /// </para>
        /// </remarks>
        internal virtual void Parse(XmlElement element)
        {
            if(element == null) {
                return;
            }

            XmlAttributeCollection attributes = element.Attributes;

            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (XmlAttribute att in attributes) {
                string propertyName = att.Name;
                string propertyValue = att.Value;

                PropertyInfo property = GetProperty(properties, propertyName);

                if (property != null && property.CanWrite) {
                    Type propertyType = property.PropertyType;
                    if (propertyType.Name == "String") {
                        property.SetValue(this, propertyValue, null);
                    }
                    else if(propertyType.IsValueType) {
                        // try to convert the attribute value to the type of the property
                        ValueType value;
                        if (DataHelper.TryParse(propertyType, propertyValue, out value)) {
                            property.SetValue(this, value, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private PropertyInfo GetProperty(PropertyInfo[] properties, string propertyName)
        {
            PropertyInfo property;

            for (int i = 0, j = properties.Length; i < j; i++) {
                property = properties[i];
                if (string.Compare(property.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0) {
                    return property;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the date and time when the <see cref="Load()"/> method was last called.
        /// </summary>
        public DateTime Version {
            get { return version_; }
        }

        /// <summary>
        /// Gets the directory path where the configuration file is stored.
        /// </summary>
        /// <returns>An string that represents the location of the configuration file or null if the location
        /// could not be retrieved.</returns>
        public string Location {
            get { return location_; }
            protected set {
                if (value == null)
                    location_ = AppDomain.CurrentDomain.BaseDirectory;
                else {
                    location_ = value;
                }
            }
        }
    }
}