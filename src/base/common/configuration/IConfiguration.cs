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

        protected Dictionary<string, Provider> providers_;
        protected Dictionary<string, Value> properties_;
        protected XmlElement element_;
        protected const string kDefaultNamespace = "nhs-nsdefault";
        protected const string kProvidersNodeName = "nhs-providers";

        #region .ctor
        static IConfiguration()
        {
        }

        /// <summary>
        /// protected member initializer.
        /// </summary>
        protected IConfiguration()
        {
            properties_ = new Dictionary<string, Value>(StringComparer.OrdinalIgnoreCase);
            providers_ = new Dictionary<string, Provider>(StringComparer.OrdinalIgnoreCase);
            element_ = null;
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
        /// <param name="configFile">the XML config file used to load the configuration from</param>
        /// <remarks>
        /// The configuration file must be valid XML. It must contain at least one element
        /// called <c>appconfig</c> that holds the configuration data.
        /// <para>
        /// The config file could be specified in the applications configuration file (either
        /// <c>MyAppNAme.exe.config</c> for a normal application on <c>Web.config</c> for an
        /// ASP.NET application. To load the cofiguration use code like:
        /// </para>
        /// <code>
        /// using Nohros.Configuration;
        /// using System.IO;
        /// using System.Configuration;
        /// 
        /// ...
        /// 
        /// Load(new FileInfo(ConfigurationSettings.AppSettings["app-config-file"]));
        /// </code>
        /// <para> In your <c>.config</c> file you must specify the config file to
        /// use like this:
        /// </para>
        /// <code>
        /// &lt;configuration&gt;
        ///		&lt;appSettings&gt;
        ///			&lt;add key="app-config-file" value="MyCustom.config"/&gt;
        ///		&lt;/appSettings&gt;
        ///	&lt;/configuration&gt;
        /// </code>
        /// </remarks>
        /// <exception cref="FileNotFoundException">If the config file does not exists</exception>
        /// <seealso cref="Load(FileInfo, String)"/>
        public virtual void Load(FileInfo configFile)
        {
            Load(configFile, "appconfig");
        }

        /// <summary>
        /// Load the configuation values using the specified configuration file and node name.
        /// </summary>
        /// <param name="configFile">the XML configuration file to load the configuration from</param>
        /// <param name="element">the name of the XmlNode that contains the configuration data</param>
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
        /// This configuration file must have a node named "customConfigNode".
        /// <para>
        /// If you need to monitor this file for changes and reload the configuration when the config
        /// file's contents changes then you should use the <see cref="LoadAndWatch"/>method instead.
        /// </para>
        /// </remarks>
        /// <exception cref="FileNotFoundException">If the config file does not exists</exception>
        /// <seealso cref="LoadAndWatch"/>
        public virtual void Load(FileInfo configFile, string element)
        {
            if (configFile != null)
            {
                // Have to use File.Exists() rather than configFile.Exists()
                // because configFile.Exists() caches the value, not what we want.
                if (File.Exists(configFile.FullName))
                {
                    // Open the file for reading
                    FileStream fs = configFile.OpenRead();
                    try
                    {
                        // Loads the configuration file to memory.
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fs);

                        // searching for the configuration element.
                        Load((XmlElement)doc.SelectSingleNode(element));
                    }
                    finally
                    {
                        // Force the file closed whatever happens
                        fs.Close();
                    }
                }
                else
                {
                    throw new System.IO.FileNotFoundException(configFile.FullName);
                }
            }
        }

        /// <summary>
        /// Load the configuration values using the file specified, monitor the file for changes
        /// and reload the configuration if a change is detected.
        /// </summary>
        /// <param name="configFile">the XML config file to load the configuration from.</param>
        /// <remarks>
        /// The configuration file must be a valid XML. It must contain at least one element called
        /// <c>appconfig</c> that contains the configuration data.
        /// <para>
        /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
        /// on the behavior of that class.
        /// </para>
        /// </remarks>
        /// <seealso cref="Load(FileInfo)"/>
        /// <seealso cref="LoadAndWatch(FileInfo, String)"/>
        public void LoadAndWatch(FileInfo configFile)
        {
            LoadAndWatch(configFile, "appconfig");
        }

        /// <summary>
        /// Load the configuration values using the file specified, monitor the file for changes
        /// and reload the configuration if a change is detected.
        /// </summary>
        /// <param name="configFile">The XML config file to load the configuration from.</param>
        /// <remarks>
        /// The configuration file must be valid XML. It must contain at least one element called
        /// <paramref name="node"/> that contains the configuration data.
        /// <para>
        /// The config file will be monitored using a <see cref="FileSystemWatcher"/> and is dependant
        /// on the behavior of that class.
        /// </para>
        /// <para>
        /// The <see cref="Load(FileInfo, String)"/> method will be called to reload the cofiguration
        /// values.
        /// </para>
        /// </remarks>
        /// <seealso cref="Load(FileInfo)"/>
        /// <seealso cref="Load(FileInfo, String)"/>
        public void LoadAndWatch(FileInfo configFile, string node)
        {
            if(configFile != null)
            {
                // load the configuration file
                Load(configFile, node);

                // monitor the file and reload the configuration values
                // whenever the config file is modified.
                Watch(configFile);
            }
        }
        #endregion

        #region Watch handler
        /// <summary>
        /// Monitor the configuration file for changes and reload the configuration values
        /// if a change is detected.
        /// </summary>
        /// <param name="configFile">the XML config file to watch</param>
        /// <remarks>
        /// The configuration file must be valid XML. It must contain
        /// at least one element called <paramref name="node"/> that holds
        /// the configuration data.
        /// <para>
        /// The config file will be monitored useing a <see cref="FIleSystemWatcher"/>
        /// and is dependant on the behavior of that class.
        /// </para>
        /// </remarks>
        protected void Watch(FileInfo configFile)
        {
            config_file_ = configFile;

            // create a new FileSystemWatcher and set its properties_.
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = configFile.DirectoryName;
            watcher.Filter = configFile.Name;

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
        /// XML attribute of the root node and if the property is writtable and it type is a
        /// ValueType or a String, we will try to set the value of this property to the value
        /// of the XML attribute. If the value of the XML attribute could not be converted to
        /// the Type of the property it will not be setted.
        /// <para>
        /// We do not want to throw an exception inside a protected method. So, the caller must
        /// ensure that the elelement is a valid XML element. If the specified XML element is a null
        /// refrence this method will return silent.
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
                if (string.Compare(child.Name, "nhs-properties", StringComparison.OrdinalIgnoreCase) ==0)
                    GetProperties(child);

                // load the data providers
                if (child.Name == kProvidersNodeName)
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
        /// Gets the dynamic properties_.
        /// </summary>
        /// <param name="node">A XML node containing the dynamic properties_.</param>
        /// <remarks>
        /// If the namespace of a property is not defined then that property will be assigned
        /// to the default namespace.
        /// </remarks>
        /// <exception cref=""></exception>
        protected void GetProperties(XmlNode node)
        {
            XmlAttribute att = node.Attributes["ns"];
            string ns = (att != null) ? att.Value : kDefaultNamespace;
            foreach (XmlNode property in node.ChildNodes) {
                XmlAttributeCollection atts = property.Attributes;
                
                string name, value, type;
                
                att = atts["name"];
                name = (att == null) ? null : atts["name"].Value;

                att = atts["value"];
                value = (att == null) ? null : atts["value"].Value;

                // the value and name are mandatory
                if (name == null || value == null)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, (name == null) ? "value" : "name"));

                // the .NET type of the property value. System.String is the default type.
                att = atts["type"];
                type = (att == null) ? "string" : att.Value;

                switch (type)
                {
                    case "array":
                        break;

                    default:
                        this[name, ns] = value;
                        break;
                }
            }
            this["NAMESPACE", ns] = ns;
        }

        /// <summary>
        /// Gets the value associated with the specified key within the default namespace.
        /// </summary>
        /// <param name="key">The key whose value to get</param>
        /// <returns>An string associated with the specified key within the given namespace.</returns>
        public Value Get(string key)
        {
            return Get(key, kDefaultNamespace);
        }

        /// <summary>
        /// Gets the value associated with the specified key within a given namespace.
        /// </summary>
        /// <param name="key">The key whose value to get</param>
        /// <returns>An string associated with the specified key within the given namespace.</returns>
        /// <seealso cref="IConfiguration[string, string]"/>
        public Value Get(string key, string ns)
        {
            Value value;
            properties_.TryGetValue(PropertyKey(kDefaultNamespace, key), out value);
            return value;
        }
        
        /// <summary>
        /// Sets the value associated with the specified key within the default namespace.
        /// </summary>
        /// <param name="key">The key whose value to set</param>
        /// <param name="value">An string associated with the specified key within the given namespace</param>
        public void Set(string key, Value value)
        {
            Set(key, kDefaultNamespace, value);
        }

        /// <summary>
        /// Sets the value associated with the specified key within a given namespace.
        /// </summary>
        /// <param name="key">The key whose value to set</param>
        /// <param name="value">An string associated with the specified key within the given namespace</param>
        public void Set(string key, string ns, Value value)
        {
            properties_[PropertyKey(ns, key)] = value;
        }

        /// <summary>
        /// A convenient form of <code>Get(string) ans Set(string, Value)</code>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        public string this[string key]
        {
            get {
                return this[key, kDefaultNamespace];
            }
            set {
                this[key, kDefaultNamespace] = value;
            }
        }

        /// <summary>
        /// A convenient form of <code>Get(string, string) and Set(string, string, Value)</code>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <param name="ns">The name of the namespace.</param>
        public string this[string key, string ns]
        {
            get {
                Value value;
                if (properties_.TryGetValue(PropertyKey(ns, key), out value))
                    return value.GetAsString();
                return null;
            }
            set {
                properties_[PropertyKey(ns, key)] = Value.CreateStringValue(value);
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
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, kProvidersNodeName), provider);

                        providers_.Add(name.Value, new Provider(attributes));
                        break;

                    default:
                        throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, kProvidersNodeName), provider);
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