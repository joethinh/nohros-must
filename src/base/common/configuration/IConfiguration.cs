using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Web;
using System.Reflection;

using Nohros.Data;

namespace Nohros.Configuration
{
    public abstract class IConfiguration
    {
        #region Member variables
        
        /// <summary>
        /// key: propertie name, value: propertie value
        /// </summary>
        protected Hashtable properties = new Hashtable();

        /// <summary>
        /// Hold the XmlElement used to load the configuration values
        /// </summary>
        protected XmlElement _element;

        /// <summary>
        /// Hold informations about the cofiguration file.
        /// </summary>
        private FileInfo _configFile;

        /// <summary>
        /// An generic static object
        /// </summary>
        protected static object _lock = new object();

        #endregion

        /// <summary>
        /// Load the configuration values based on the application's
        /// configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same
        /// name as the application whith ' .config ' appended.
        /// This file is XML and calling this function prompts the
        /// loader to look in that file for a section called
        /// <c>appconfig</c> that contains the configuration data.
        /// </remarks>
        public virtual void Load()
        {
            Load((XmlElement)System.Configuration.ConfigurationManager.GetSection("appconfig"));
        }

        /// <summary>
        /// Load the configuration values using a <c>appconfig</c> element
        /// </summary>
        /// <remarks>
        /// Load the configuration from the XML element
        /// supplied as <paramref name="element"/>
        /// <para>
        /// All the Load(...) overloads will call this method at the end.
        /// </para>
        /// </remarks>
        /// <param name="element">The element to parse</param>
        public virtual void Load(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            _element = element;
            Parse(element);
        }

        /// <summary>
        /// Load the configuration values using the config file specified.
        /// </summary>
        /// <param name="configFile">the XML config file to load
        /// the configuration from</param>
        /// <remarks>
        /// The configuration file must be valid XML. It must contain
        /// at least one element called <c>appconfig</c> that holds
        /// the configuration data.
        /// <para>
        /// The config file could be specified in the applications
        /// configuration file (either <c>MyAppNAme.exe.config</c> for a
        /// normal application on <c>Web.config</c> for an ASP.NET application.
        /// To load the cofigurationuse code like:
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
        /// Load the configuation values using the config file specified.
        /// </summary>
        /// <param name="configFile">the XML config file to load
        /// the configuration from</param>
        /// <param name="element">the name of the element whose properties to get</param>
        /// <remarks>
        /// <para>
        /// The config file could be specified in the applications
        /// configuration file (either <c>MyAppName.exe.config</c> for a
        /// normal application on <c>Web.config</c> for an ASP.NET application
        /// To load the configuration use code like:
        /// </para>
        /// <code>
        /// using Nohros.Configuration;
        /// using System.IO;
        /// using System.Configuration;
        /// 
        /// ...
        /// 
        /// base.Load(new FileInfo(ConfigurationSettings.AppSettings["nhs-config-file"]));
        /// </code>
        /// <para> In your <c>.config</c> file you must specify the config file to
        /// use like this:
        /// </para>
        /// <code>
        /// &lt;configuration&gt;
        ///		&lt;appSettings&gt;
        ///			&lt;add key="nhs-config-file" value="MyCustom.config"/&gt;
        ///		&lt;/appSettings&gt;
        ///	&lt;/configuration&gt;
        /// </code>
        /// <para>
        /// If you need to monitor this file for changes and reload the
        /// configuration when the config file's contents changes then
        /// you should use the <see cref="LoadAndWatch"/>method instead.
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
                        // Load the config file into a DOM
                        XmlDocument doc = new XmlDocument();
                        doc.Load(fs);

                        // load using the 'element' element
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
        /// <param name="configFile">the XML config file to load
        /// the configuration from.</param>
        /// <remarks>
        /// the configuration file must be a valid XML. It must contain
        /// at least one element called <c>appconfig</c> that hold
        /// the configuration data.
        /// <para>
        /// the config file will be monitored using a <see cref="FileSystemWatcher"/>
        /// and is dependant on the behavior of that class.
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
        /// <param name="configFile">The XML config file to load
        /// the configuration from.</param>
        /// <remarks>
        /// The configuration file must be valid XML. It must contain
        /// at least one element called <paramref name="node"/> that holds
        /// the configuration data.
        /// <para>
        /// The config file will be monitored using a <see cref="FileSystemWatcher"/>
        /// and is dependant on the behavior of that class.
        /// </para>
        /// <para>
        /// The <see cref="Load(FileInfo, String)"/> method will be called to reload
        /// the cofiguration values.
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
            _configFile = configFile;

            // create a new FileSystemWatcher and set its properties.
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
            Load(_configFile, _element.Name);
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
            _configFile = new FileInfo(e.FullPath);
            Load(_configFile, _element.Name);
        }
        #endregion

        /// <summary>
        /// Used internally to load the configuration values by parsing a DOM tree of XML elements.
        /// </summary>
        /// <param name="element">the root element to parse</param>
        protected void Parse(XmlElement element)
        {
            if(element == null)
            {
                return;
            }
            
            /* attributes of the parent element node will be
             * set to the properties defined on the derived class
             */
            XmlAttributeCollection attributes = element.Attributes;

            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (XmlAttribute att in attributes)
            {
                string propertyName = att.Name;
                string propertyValue = att.Value;

                PropertyInfo property = GetProperty(properties, propertyName);
                Type propertyType = property.PropertyType;

                if (property != null && property.CanWrite)
                {
                    if (propertyType.Name == "String")
                    {
                        property.SetValue(this, propertyValue, null);
                    }
                    else if(propertyType.IsValueType)
                    {
                        // try to convert the attribute value to the
                        // type of the property
                        ValueType value;
                        if (DataHelper.TryParse(propertyType, propertyValue, out value))
                        {
                            property.SetValue(this, value, null);
                        }
                    }
                }
            }

            // Load the custom properties
            foreach (XmlElement child in element.ChildNodes)
            {
                if (child.Name == "nhs-properties")
                    GetProperties(child);
            }
        }

        #region Derived properties

        private PropertyInfo GetProperty(PropertyInfo[] properties, string propertyName)
        {
            PropertyInfo property = null;

            // case-insensitive lookup
            propertyName = propertyName.ToLower();

            for (int i = 0, j = properties.Length; i < j; i++)
            {
                property = properties[i];
                if (property.Name.ToLower() == propertyName)
                {
                    break;
                }
            }
            return property;
        }

        #endregion

        #region Dynamic Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected string PropertyKey(string ns, string property) {
            return string.Concat(ns.ToLower(), "-", property.ToLower());
        }

        /// <summary>
        /// Get properties from a XML node.
        /// </summary>
        /// <param name="node"></param>
        protected void GetProperties(XmlNode node)
        {
            XmlAttribute att = node.Attributes["ns"];
            string ns = (att != null) ? att.Value : "nhs-default";
            foreach (XmlNode property in node.ChildNodes)
            {
                XmlAttributeCollection atts = property.Attributes;
                
                string name, value, type;
                
                // the name of the propertie
                att = atts["name"];
                if (att == null)
                    throw new ArgumentNullException("name");
                name = atts["name"].Value;

                // tha value of the propertie
                att = atts["value"];
                if (atts == null)
                    throw new ArgumentNullException("value");
                value = atts["value"].Value;

                // the .NET type of the propertie value.
                att = atts["type"];
                type = (att == null) ? "string" : att.Value;

                switch (type)
                {
                    case "array":

                        string subtype, delimiter;

                        // the type of the elements of the array
                        att = atts["subtype"];
                        subtype = (att == null) ? "string" : att.Value;

                        att = atts["operator"];
                        delimiter = (att == null) ? ";" : att.Value;

                        properties[PropertyKey(ns, name)] = DataHelper.StringToArray(value, subtype, delimiter[0]);
                        break;

                    default:
                        properties[PropertyKey(ns, name)] = value;
                        break;
                }
            }
            properties[PropertyKey(ns, "NAMESPACE")] = ns;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key and ns
        /// </summary>
        /// <param name="key">The key whose value to get or set</param>
        /// <param name="ns">The name of the ns</param>
        public object this[string key, string ns]
        {
            get {
                return properties[PropertyKey(ns, key)];
            }
            set {
                properties[PropertyKey(ns, key)] = value;
            }
        }
        #endregion
    }
}