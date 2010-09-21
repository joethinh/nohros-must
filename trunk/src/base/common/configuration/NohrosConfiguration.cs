using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Data;
using Nohros.Resources;

namespace Nohros.Configuration
{
    /// <summary>
    /// A abstract implementation of the <see cref="IConfiguration"/> used to parse the nohros configuration file.
    /// </summary>
    public abstract class NohrosConfiguration : IConfiguration
    {
        const string kNohrosNodeName = "nohros";
        const string kConfigurationFileKey = "NohrosConfigurationFile";

        DictionaryValue properties_;

        CommonNode common_node_;
        WebNode web_node_;

        /// <summary>
        /// A collection of the parsed data providers.
        /// </summary>
        protected Dictionary<string, Provider> providers_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Configuration class.
        /// </summary>
        public NohrosConfiguration(): base()
        {
            providers_ = new Dictionary<string, Provider>();
            properties_ = new DictionaryValue();
            common_node_ = null;
            web_node_ = null;
        }
        #endregion

        /// <summary>
        /// Loads the configuration values based on the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same name as the application
        /// whith ' .config ' appended.
        /// <para>This file is XML and calling this function prompts the loader to look in that file
        /// for a key named [NohrosConfigurationFile] that contains the path for the configuration file.
        /// </para>
        /// <para>
        /// The value of the [NohrosConfigurationFile] must be absolute or relative to the application
        /// base directory.
        /// </para>
        /// </remarks>
        public override void Load() {
            Load((string)null);
        }

        /// <summary>
        /// Loads the configuration values based on the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same name as the application
        /// whith ' .config ' appended. This file is XML and calling this function prompts the loader to
        /// look in that file for a key named [NohrosConfigurationFile] that contains the path for the
        /// configuration file.
        /// <para>
        /// The value of the [NohrosConfigurationFile] must be absolute or relative to the application
        /// base directory.
        /// </para>
        /// <para>
        /// The configuration file must be valid XML. It must contain at least one element called
        /// <paramref name="root_node_name"/> that contains the configuration data.
        /// </para>
        /// </remarks>
        public override void Load(string root_node_name) {
            string config_file_path = ConfigurationManager.AppSettings[kConfigurationFileKey];
            if (config_file_path == null)
                throw new ConfigurationErrorsException(string.Format(StringResources.Config_KeyNotFound, kConfigurationFileKey));

            if (Path.IsPathRooted(config_file_path))
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_PathIsRooted, config_file_path));

            config_file_path = Path.Combine(Location, config_file_path);

            Load(new FileInfo(config_file_path), root_node_name);
        }

        internal override void Parse(XmlElement element) {
            base.Parse(element);

            // attempt to get the "nohros" node.
            XmlNode root_node = null;
            if (string.Compare(element.Name, kNohrosNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                root_node = element as XmlNode;
            } else {
                root_node = element.SelectSingleNode(kNohrosNodeName); // for backward compatibility
                if (root_node == null) {
                    XmlNodeList nodes = element.GetElementsByTagName(kNohrosNodeName);
                    if (nodes.Count > 0)
                        root_node = nodes[0];
                }
            }

            if (root_node == null)
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_KeyNotFound, kNohrosNodeName));

            // parse the common node
            XmlNode node = IConfiguration.SelectNode(root_node, CommonNode.kCommonNodeName);
            if (node != null) {
                common_node_ = CommonNode.FromXmlNode(node, this);

                // parse the web node
                node = IConfiguration.SelectNode(root_node, WebNode.kWebNodeName);
                if (node != null)
                    web_node_ = WebNode.FromXmlNode(node, common_node_);
            }
        }

        /// <summary>
        /// Gets the configuration common node.
        /// </summary>
        public CommonNode CommonNode {
            get { return common_node_; }
        }

        /// <summary>
        /// Gets the configuration web node.
        /// </summary>
        public WebNode WebNode {
            get { return web_node_; }
        }

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
        void GetProperties(XmlNode node, string path) {
            if (node != null && node.ChildNodes.Count > 0) {
                foreach (XmlNode inner_node in node.ChildNodes) {
                    if (inner_node.NodeType == XmlNodeType.Element) {
                        if (inner_node.ChildNodes.Count > 0) {
                            GetProperties(inner_node, path + "." + inner_node.Name);
                        }
                        else {
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
        public Value Get(string key) {
            return properties_.Get(key);
        }

        /// <summary>
        /// Sets the value associated with the specified key within the default namespace.
        /// </summary>
        /// <param name="key">The key whose value to set</param>
        /// <param name="value">An string associated with the specified key within the given namespace</param>
        public void Set(string key, Value value) {
            properties_.Set(key, value);
        }

        /// <summary>
        /// A convenient form of <code>Get(string, string) and Set(string, string, Value)</code>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        public string this[string key] {
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

        #region Common
        /// <summary>
        /// Gets informations about a data provider by using the specified provider name.
        /// </summary>
        /// <param name="provider_name">The name of the data provider to get informations from</param>
        /// <returns>A <see cref="Provider"/> object that contains information about the provider
        /// associated with the specified <paramref name="provider_name"/> or null if the <paramref name="provider_name"/>
        /// could not be found.
        /// </returns>
        public ProviderNode GetProvider(string provider_name) {
            return (common_node_ != null) ? common_node_.GetProvider(provider_name) : null;
        }

        /// <summary>
        /// Gets a <see cref="ConnectionStringNode"/> object containing information that can be used
        /// to connects and queries a database.
        /// </summary>
        /// <param name="name">A string that identifies the connection node</param>
        /// <returns>a <see cref="ConnectionStringNode"/> object containing information that can be used
        /// to connects and queries a database.- or null if the <paramref name="name"/>
        /// could not be found.</returns>
        public ConnectionStringNode GetConnectionString(string name) {
            return (common_node_ != null) ? common_node_.GetConnectionString(name) : null;
        }
        #endregion

        #region Web
        /// <summary>
        /// Gets a list of files related with the specified content group, build version and mime type.
        /// </summary>
        /// <param name="name">The name of the group to get.</param>
        /// <param name="build">The build version</param>
        /// <param name="mime_type">The mime type of the group.</param>
        /// <returns></returns>
        public ContentGroupNode GetContentGroup(string name, string build, string mime_type) {
            return (web_node_ != null) ? web_node_.GetContentGroup(name, build, mime_type) : null;
        }
        #endregion
    }
}