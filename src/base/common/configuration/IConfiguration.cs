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

        DictionaryValue properties_;

        /// <summary>
        /// A collection of the parsed data providers.
        /// </summary>
        protected Dictionary<string, Provider> providers_;

        /// <summary>
        /// The configuration root node.
        /// </summary>
        protected XmlElement element_;

        /// <summary>
        /// Default namespace name.
        /// </summary>
        protected const string kDefaultNamespace = "nhs-nsdefault";

        string providers_node_name_;
        string properties_node_name_;

        #region .ctor
        /// <summary>
        /// Protected member initializer.
        /// </summary>
        protected IConfiguration(): this("nhs-properties", "nhs_providers") {
        }

        /// <summary>
        /// Initializes a new instance of the IConfiguration class by using th specified
        /// properties and providers node name.
        /// </summary>
        protected IConfiguration(string providers_node_name, string properties_node_name) {
            properties_ = new DictionaryValue();
            providers_ = new Dictionary<string, Provider>(StringComparer.OrdinalIgnoreCase);
            element_ = null;
            properties_node_name_ = properties_node_name;
            providers_node_name_ = providers_node_name;
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
            Load((XmlElement)System.Configuration.ConfigurationManager.GetSection("appconfig"));
        }

        /// <summary>
        /// Load the configuration values using the specified XML element.
        /// </summary>
        /// <remarks>
        /// Load the configuration from the XML element supplied as <paramref name="element"/>
        /// <para>
        /// This is the main Load(...) overload. This method is called by all the others Load(...)
        /// method overloads.
        /// </para>
        /// </remarks>
        /// <param name="element">The XmlElement containing the configuration values to parse.</param>
        public virtual void Load(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element_ = element;
            Parse(element);
        }

        /// <summary>
        /// Load the configuration values using the specified configuration file.
        /// </summary>
        /// <param name="config_file_name">The name of the configuration file.</param>
        /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
        /// <remarks>
        /// This method assumes that the specified configuration file is located in the application
        /// base directory.
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
            if (config_file_info != null)
            {
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

                        // searching for the configuration element.
                        Load((XmlElement)doc.SelectSingleNode(root_node_name));
                    }
                    finally
                    {
                        // Force the file closed whatever happens
                        fs.Close();
                    }
                }
                else
                {
                    throw new System.IO.FileNotFoundException(string.Format(StringResources.Config_FileNotFound_Path, config_file_info.FullName));
                }
            }
        }

        /// <summary>
        /// Load the configuration values using the specified configuration file, monitor the file for changes
        /// and reload the configuration if a change is detected.
        /// </summary>
        /// <param name="config_file_name">The name of the configuration file.</param>
        /// <param name="root_node_name">the xpath of the node that contains the configuration data.</param>
        /// <remarks>
        /// This method assumes that the specified configuration file is located in the application
        /// base directory.
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
        /// <remarks>
        /// The configuration file must be valid XML. It must contain at least one element called
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
            if (config_file_info != null)
            {
                // load the configuration file
                Load(config_file_info, root_node_name);

                // monitor the file and reload the configuration values
                // whenever the config file is modified.
                Watch(config_file_info);
            }
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
            Load(config_file_, element_.Name);
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
            config_file_ = new FileInfo(e.FullPath);
            Load(config_file_, element_.Name);
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
        protected void Parse(XmlElement element)
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
                Type propertyType = property.PropertyType;

                if (property != null && property.CanWrite) {
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

            foreach (XmlElement child in element.ChildNodes) {
                // load the dynamic properties
                if (string.Compare(child.Name, properties_node_name_, StringComparison.OrdinalIgnoreCase) == 0)
                    GetProperties(child, string.Empty);

                // load the data providers
                if (child.Name == providers_node_name_)
                    GetProviders(child);
            }
        }

        #region Derived properties

        private PropertyInfo GetProperty(PropertyInfo[] properties, string propertyName)
        {
            PropertyInfo property = null;

            for (int i = 0, j = properties.Length; i < j; i++) {
                property = properties[i];
                if (string.Compare(property.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0) {
                    break;
                }
            }
            return property;
        }

        #endregion

        #region Dynamic Properties
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
        /// <param name="node">A XML node containing the dynamic properties.</param>
        /// <param name="path">The path to the node value.</param>
        /// <remarks>
        /// If the namespace of the property is not defined it will be assigned
        /// to the default namespace.
        /// </remarks>
        void GetProperties(XmlNode node, string path)
        {
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
                            properties_.Set(path, keys);
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
        /// <seealso cref="this[string]"/>
        public Value Get(string key)
        {
            return properties_.Get(key);
        }
        
        /// <summary>
        /// Sets the value associated with the specified key within the default namespace.
        /// </summary>
        /// <param name="key">The key whose value to set</param>
        /// <param name="value">An string associated with the specified key within the given namespace</param>
        public void Set(string key, Value value)
        {
            properties_.Set(key, value);
        }

        /// <summary>
        /// A convenient form of <code>Get(string, string) and Set(string, string, Value)</code>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        public string this[string key]
        {
            get {
                Value value;
                if (properties_.Get(key, out value))
                    return value.GetAsString();
                return null;
            }
            set {
                Set(key, Value.CreateStringValue(value));
            }
        }
        #endregion

        #region Data Providers
        /// <summary>
        /// Extract the data providers information from specified Xml Node.
        /// </summary>
        /// <param name="node">A XML node that contains information about the data providers.</param>
        /// <remarks>
        /// The XML node must contain an add node for each provider and, each node must contains at minimum
        /// the name and the .NET class type of the provider implementor.
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <nhs-providers>
        ///   <add name="MyCustomDataProviderName"
        ///         type=MyCustomDataProviderType, MyCustomDataProviderAssembly
        ///         configRepository="ConfigurationFile"
        ///         dataSourceType="MSSQL"/>
        /// </nhs-providers>
        /// ]]>
        /// </code>
        /// </example>
        /// </remarks>
        void GetProviders(XmlNode node)
        {
            foreach (XmlNode provider in node.ChildNodes)
            {
                XmlAttributeCollection attributes = provider.Attributes;
                switch(provider.Name) {
                    case "add":
                        XmlAttribute name = attributes["name"];
                        if(name == null || name.Value == null || name.Value.Length == 0)
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, providers_node_name_), provider);

                        providers_.Add(name.Value, new Provider(provider));
                        break;

                    default:
                        throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, providers_node_name_), provider);
                }
            }
        }

        /// <summary>
        /// Gets informations about a data provider by using the specified provider name.
        /// </summary>
        /// <param name="provider_name">The name of the data provider to get informations from</param>
        /// <returns>A <see cref="Provider"/> object that contains information about the provider
        /// associated with the specified <paramref name="provider_name"/> or null if the <paramref name="provider_name"/>
        /// could not be found.
        /// </returns>
        public Provider GetProvider(string provider_name)
        {
            Provider provider = null;
            providers_.TryGetValue(provider_name, out provider);
            return provider;
        }
        #endregion
    }
}