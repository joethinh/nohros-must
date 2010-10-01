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
    /// A basic implementation of the <see cref="IConfiguration"/> used to parse the nohros configuration file.
    /// </summary>
    public class NohrosConfiguration : IConfiguration
    {
        const string kNohrosNodeName = "nohros";
        const string kConfigurationFileKey = "NohrosConfigurationFile";

        protected static NohrosConfiguration default_process_config_;

        DictionaryValue properties_;
        DictionaryValue config_nodes_;

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
            config_nodes_ = new DictionaryValue();
        }

        /// <summary>
        /// Singleton initializer. Used to load the default configuration file.
        /// </summary>
        static NohrosConfiguration() {
            default_process_config_ = null;

            string config_file_path = ConfigurationManager.AppSettings[kConfigurationFileKey];
            if (config_file_path == null || Path.IsPathRooted(config_file_path))
                return;

            default_process_config_ = new NohrosConfiguration();
            default_process_config_.Load();
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
                CommonNode common = CommonNode.FromXmlNode(node, this);

                // parse the web node
                node = IConfiguration.SelectNode(root_node, WebNode.kWebNodeName);
                if (node != null) {
                    WebNode web = WebNode.FromXmlNode(node, this);
                    config_nodes_[WebNode.kNodeTree] = web;
                }

                config_nodes_[CommonNode.kNodeTree] = common;
            }
        }

        /// <summary>
        /// Gets the default application configuration file.
        /// </summary>
        public static NohrosConfiguration DefaultConfiguration {
            get { return default_process_config_; }
            protected set { default_process_config_ = value; }
        }

        /// <summary>
        /// Gets the configuration common node.
        /// </summary>
        public CommonNode CommonNode {
            get {
                return config_nodes_[CommonNode.kNodeTree] as CommonNode;
            }
        }

        /// <summary>
        /// Gets the configuration web node.
        /// </summary>
        public WebNode WebNode {
            get {
                return config_nodes_[WebNode.kNodeTree] as WebNode;
            }
        }

        /// <summary>
        /// Gets all the data providers configured for this application.
        /// </summary>
        /// <remarks>DataProviders will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no data providers.</remarks>
        public DictionaryValue DataProviders {
            get { return this[DataProviderNode.kNodeTree] as DictionaryValue; }
        }

        /// <summary>
        /// Gets a collection of all the login modules in the configuration.
        /// </summary>
        /// <remarks>LoginModules will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no login modules.</remarks>
        public DictionaryValue LoginModules {
            get { return GetNode(LoginModuleNode.kNodeTree) as DictionaryValue; }
        }

        public DictionaryValue Repositories {
            get { return GetNode(RepositoryNode.kNodeTree) as DictionaryValue; }
        }

        public DictionaryValue Chains {
            get { return GetNode(ChainNode.kNodeTree) as DictionaryValue; }
        }

        public DictionaryValue ContentGroups {
            get { return GetNode(ContentGroupNode.kNodeTree) as DictionaryValue; }
        }

        /// <summary>
        /// Gets a node in the configuration.
        /// </summary>
        /// <remarks>GetNode method will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no node with the specified <paramref name="path"/>.</remarks>
        internal DictionaryValue this[string path] {
            get {
                DictionaryValue node = config_nodes_[path] as DictionaryValue;
                if (node == null)
                    node = new DictionaryValue();
                return node;
            }
        }

        /// <summary>
        /// Gets a collection of all the child nodes in the configuration.
        /// </summary>
        /// <remarks>Nodes will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no child nodes.</remarks>
        internal DictionaryValue Nodes {
            get { return config_nodes_; }
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
        /// <seealso cref="this[string]"/>
        public IValue GetProperty(string key) {
            return properties_[key];
        }

        /// <summary>
        /// Sets the value associated with the specified key within the default namespace.
        /// </summary>
        /// <param name="key">The key whose value to set</param>
        /// <param name="value">An string associated with the specified key within the given namespace</param>
        public void SetProperty(string key, IValue value) {
            properties_[key] = value;
        }
        #endregion
    }
}