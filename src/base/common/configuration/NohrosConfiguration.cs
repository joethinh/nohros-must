using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Logging;
using Nohros.Data;
using Nohros.Data.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    /// <summary>
    /// A basic implementation of the <see cref="IConfiguration"/> used to parse the configuration
    /// files that follows the nohros schema.
    /// </summary>
    /// <remarks>
    /// The nohros shcema is defined into http://nohros.com/schemas/nohros/nohros.xsd.
    /// </remarks>
    public class NohrosConfiguration : IConfiguration
    {
        #region configuration consts
        // configuration nodes names
        internal const string kNohrosNodeName = "nohros";
        internal const string kCommonNodeName = "common";
        internal const string kRepositoryNodeName = "repository";
        internal const string kConnectionStringsNodeName = "connection-strings";
        internal const string kProvidersNodeName = "providers";
        internal const string kDataProviderNodeName = "data";
        internal const string kMessengerProviderNodeName = "messenger";
        internal const string kCacheProviderNodeName = "cache";
        internal const string kProviderNodeName = "provider";
        internal const string kLoginModulesNodeName = "login-modules";
        internal const string kModuleNodeName = "module";
        internal const string kChainsNodeName = "chains";
        internal const string kChainNodeName = "chain";
        internal const string kWebNodeName = "web";
        internal const string kContentGroupsNodeName = "content-groups";

        // general purpose nodes
        internal const string kCommonNodeTree = kCommonNodeName;
        internal const string kSelfCommonNodeTree = kCommonNodeTree + "." + kCommonNodeName;
        internal const string kRepositoryNodeTree = kCommonNodeTree + "." + kRepositoryNodeName;
        internal const string kConnectionStringNodeTree = kCommonNodeTree + "." + kConnectionStringsNodeName;
        internal const string kLoginModuleNodeTree = kCommonNodeTree + "." + kLoginModulesNodeName;
        internal const string kChainNodeTree = kCommonNodeTree + "." + kChainsNodeName;

        // providers node trees
        internal const string kProvidersNodeTree = kCommonNodeTree + "." + kProvidersNodeName;
        internal const string kDataProviderNodeTree = kCommonNodeTree + "." + kProvidersNodeName + "." + kDataProviderNodeName;
        internal const string kMessengerProviderNodeTree = kCommonNodeTree + "." + kProvidersNodeName + "." + kMessengerProviderNodeName;
        internal const string kCacheProviderNodeTree = kCommonNodeTree + "." + kProvidersNodeName + "." + kCacheProviderNodeName;

        // web related nodes
        internal const string kWebNodeTree = kWebNodeName;
        internal const string kSelfWebNodeTree = kWebNodeName + "." + kWebNodeName;
        internal const string kContentGroupNodeTree = kWebNodeTree + "." + kContentGroupsNodeName;
        #endregion

        const string kConfigurationFileKey = "NohrosConfigurationFile";
        const string kLog4NetThreshold = "log4net-threshold";

        static NohrosConfiguration default_process_config_;

        DictionaryValue properties_;
        DictionaryValue config_nodes_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="NohrosConfiguration"/> class.
        /// </summary>
        public NohrosConfiguration(): base() {
            properties_ = new DictionaryValue();
            config_nodes_ = new DictionaryValue();
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
                throw new ConfigurationErrorsException(string.Format(StringResources.Config_PathIsRooted, config_file_path));

            config_file_path = Path.Combine(Location, config_file_path);

            Load(new FileInfo(config_file_path), root_node_name);
        }

        /// <summary>
        /// Parses the configuration node using the nohros schema.
        /// </summary>
        /// <param name="element">A Xml element representing the configuration root node.</param>
        /// <remarks>
        /// The <paramref name="element"/> does not need to be the nohros configuration node, but a node
        /// with name "nohros" must exists on the node hierarchy.
        /// </remarks>
        internal override void Parse(XmlElement element) {
            base.Parse(element);

            // attempt to get the "nohros" node.
            XmlNode root_node = null;
            if (string.Compare(element.Name, NohrosConfiguration.kNohrosNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                root_node = element as XmlNode;
            } else {
                root_node = element.SelectSingleNode(NohrosConfiguration.kNohrosNodeName); // for backward compatibility
                if (root_node == null) {
                    XmlNodeList nodes = element.GetElementsByTagName(NohrosConfiguration.kNohrosNodeName);
                    if (nodes.Count > 0)
                        root_node = nodes[0];
                }
            }

            if (root_node == null)
                throw new ConfigurationErrorsException(string.Format(StringResources.Config_KeyNotFound,
                    NohrosConfiguration.kNohrosNodeName));

            // the logger is used by some methods above and the level threshold of it
            // could be overloaded by a configuration key. So, we need to do the first logger
            // instantiation here and adjust the threshold level if needed.
            log4net.Core.Level level = log4net.Core.Level.Info;
            XmlAttribute attribute = root_node.Attributes[kLog4NetThreshold];
            if (attribute != null) {
                switch (attribute.Value.ToLower()) {
                    case "all":
                        level = log4net.Core.Level.All;
                        break;

                    case "debug":
                        level = log4net.Core.Level.Debug;
                        break;

                    case "info":
                        level = log4net.Core.Level.Info;
                        break;

                    case "warn":
                        level = log4net.Core.Level.Warn;
                        break;

                    case "error":
                        level = log4net.Core.Level.Error;
                        break;

                    case "fatal":
                        level = log4net.Core.Level.Fatal;
                        break;

                    case "off":
                        level = log4net.Core.Level.Off;
                        break;
                }
                FileLogger.ForCurrentProcess.Threshold = level;
                ConsoleLogger.ForCurrentProcess.Threshold = level;
            }


            // parse the common node
            XmlNode node = IConfiguration.SelectNode(root_node, kCommonNodeName);
            if (node != null) {
                CommonNode common_node = new CommonNode(Location);
                common_node.Parse(node, Nodes);

                // parse the web node
                node = IConfiguration.SelectNode(root_node, kWebNodeName);
                if (node != null) {
                    WebNode web_node = new WebNode();
                    web_node.Parse(node, Nodes);

                    // store the common node into the nodes collection
                    config_nodes_[kSelfWebNodeTree] = web_node;
                }

                // store the web node into the nodes collection
                config_nodes_[kSelfCommonNodeTree] = common_node;
            }
        }

        /// <summary>
        /// Gets the configuration common node.
        /// </summary>
        public CommonNode CommonNode {
            get {
                return config_nodes_[kSelfCommonNodeTree] as CommonNode;
            }
        }

        /// <summary>
        /// Gets the configuration web node.
        /// </summary>
        public WebNode WebNode {
            get {
                return config_nodes_[kSelfWebNodeTree] as WebNode;
            }
        }

        #region Nodes dictionaries
        /// <summary>
        /// Gets all the data providers configured for this application.
        /// </summary>
        /// <remarks>DataProviders will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no data providers.</remarks>
        public DictionaryValue<DataProviderNode> DataProviders {
            get { return GetDictionary<DataProviderNode>(kDataProviderNodeTree); }
        }

        /// <summary>
        /// Gets all the messenger providers configured for this application.
        /// </summary>
        /// <remarks>MessengerProviders will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no messenger providers.</remarks>
        public DictionaryValue<MessengerProviderNode> MessengerProviders {
            get { return GetDictionary<MessengerProviderNode>(kMessengerProviderNodeTree); }
        }

        /// <summary>
        /// Gets all the connection strings nodes in configuration.
        /// </summary>
        /// <remarks>ConnectionStrings will never return a null reference; however, the returned
        /// <see cref="DictionaryValue&lt;ConnectionStringNode&gt;"/> will contain zero elements if configuration
        /// contains no connections string nodes.</remarks>
        public DictionaryValue<ConnectionStringNode> ConnectionStrings {
            get { return GetDictionary<ConnectionStringNode>(kConnectionStringNodeTree); }
        }

        /// <summary>
        /// Gets a collection of all the login modules in configuration.
        /// </summary>
        /// <remarks>LoginModules will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no login modules.</remarks>
        public DictionaryValue<LoginModuleNode> LoginModules {
            get { return GetDictionary<LoginModuleNode>(kLoginModuleNodeTree); }
        }

        /// <summary>
        /// Gets a collection of all the repositories in configuration.
        /// </summary>
        /// <remarks>
        /// Repositories will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no repositories.</remarks>
        public DictionaryValue<RepositoryNode> Repositories {
            get { return GetDictionary<RepositoryNode>(kRepositoryNodeTree); }
        }

        /// <summary>
        /// Gets a collection of all the chains in configuration.
        /// </summary>
        /// </remarks>Chains will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no chains.</remarks>
        public DictionaryValue<ChainNode> Chains {
            get { return GetDictionary<ChainNode>(kChainNodeTree); }
        }

        /// <summary>
        /// Gets a collection of all the chains in configuration.
        /// </summary>
        /// </remarks>ContentGroups will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no content groups.</remarks>
        public DictionaryValue<ContentGroupNode> ContentGroups {
            get { return GetDictionary<ContentGroupNode>(kContentGroupNodeTree); }
        }

        /// <summary>
        /// Gets a collection of all the child nodes in the configuration.
        /// </summary>
        /// <remarks>Nodes will never return a null reference; however, the returned <see cref="DictionaryValue"/>
        /// will contain zero elements if configuration contains no child nodes.</remarks>
        internal DictionaryValue Nodes {
            get { return config_nodes_; }
        }

        /// <summary>
        /// Gets a node in the configuration.
        /// </summary>
        /// <remarks>This method will never return a null reference; however, the returned <see cref="DictionaryValue&lt;T&gt;"/>
        /// will contain zero elements if configuration contains no node with the specified <paramref name="path"/>.</remarks>
        DictionaryValue<T> GetDictionary<T>(string path) where T : class, IValue {
            DictionaryValue<T> node = config_nodes_[path] as DictionaryValue<T>;
            if (node == null)
                node = new DictionaryValue<T>();
            return node;
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
        /// If the namespace of the property is not defined it will be assigned to the default namespace.
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
        /// <seealso cref="Item(string)"/>
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