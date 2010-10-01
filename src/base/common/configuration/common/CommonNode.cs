using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Data;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class CommonNode : ConfigurationNode
    {
        internal const string kCommonNodeName = "common";

        internal const string kNodeTree = kCommonNodeName;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the CommonNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="config">The related <see cref="NohrosConfiguration"/> object.</param>
        public CommonNode() : base(kCommonNodeName) { }
        #endregion

        public static CommonNode FromXmlNode(XmlNode node, NohrosConfiguration config) {
            CommonNode common_node = new CommonNode();
            common_node.Parse(node, config);
            return common_node;
        }

        /// <summary>
        /// Parses a XML node that contains information about a common node.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <param name="config">The configuration object which this node belongs to.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a common node.</exception>
        public override void Parse(XmlNode node, NohrosConfiguration config) {
            // parse the repository node.
            XmlNode data_node = IConfiguration.SelectNode(node, RepositoryNode.kRepositoryNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ErrorAt, "attribute name", kNodeTree + "." + RepositoryNode.kRepositoryNodeName));

                        RepositoryNode repository = new RepositoryNode(name);
                        repository.Parse(n, config);
                        config.Repositories[RepositoryKey(repository.Name)] = repository;
                    }
                }
            }

            // parse the connection strings
            data_node = IConfiguration.SelectNode(node, kConnectionStringsNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null;
                        if (!(GetAttributeValue(n, "name", out name)))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + "." + kConnectionStringsNodeName));

                        ConnectionStringNode conn_string_node = new ConnectionStringNode(name);
                        conn_string_node.Parse(n, config);
                        this[ConnectionStringKey(conn_string_node.Name)] = conn_string_node;
                    }
                }
            }

            // parse the providers
            data_node = IConfiguration.SelectNode(node, kProvidersNodeName);
            if (data_node != null) {
                foreach (XmlNode provider_node in data_node.ChildNodes) {
                    foreach (XmlNode n in provider_node.ChildNodes) {
                        if (string.Compare(n.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                            string name = null, type = null;
                            if (!(GetAttributeValue(n, "name", out name) && GetAttributeValue(n, "type", out type)))
                                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name or type", kNodeTree + "." + kProvidersNodeName));

                            DataProviderNode provider = new DataProviderNode(name, type);
                            provider.Parse(n, config);
                            config.Nodes[DataProviderKey(provider.Name)] = provider;
                        }
                    }
                }
            }

            // parse the login modules
            data_node = IConfiguration.SelectNode(node, kLoginModulesNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, kModuleNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + "." + kLoginModulesNodeName));

                        LoginModuleNode login_module = new LoginModuleNode(name);
                        login_module.Parse(n, config);
                        config.Nodes[LoginModuleKey(login_module.Name)] = login_module;
                    }
                }
            }

            // parse the chains
            data_node = IConfiguration.SelectNode(node, kChainsNodeName);
            if(data_node != null) {   
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, kChainNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + "." + kChainsNodeName + "." + kChainNodeName));

                        ChainNode chain = new ChainNode(name);
                        chain.Parse(n, config);
                        config.Nodes[ChainNodeKey(chain.Name)] = chain;
                    }
                }
            }
        }

        #region Dictionary Keys
        /// <summary>
        /// Gets a string that uniquely identifies a connection string within the common node.
        /// </summary>
        /// <param name="name">The name of the connection string.</param>
        /// <returns>A string that uniquely identifies a connection string within a common node.</returns>
        string ConnectionStringKey(string name) {
            return string.Concat(CommonNode.kNodeTree, kConnectionStringsNodeName, ".", name);
        }

        /// <summary>
        /// Gets a string that uniquely identifies a provider within the common node.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <returns>A string that uniquely identifies a provider within a common node.</returns>
        string DataProviderKey(string name) {
            return string.Concat(CommonNode.kNodeTree, kProvidersNodeName, kDataProviderNodeName, ".", name);
        }

        /// <summary>
        /// Gets a string that uniquely identifies a login module within the common node.
        /// </summary>
        /// <param name="name">The name of the login module.</param>
        /// <returns>A string that uniquely identifies a login module within a common node.</returns>
        string LoginModuleKey(string name) {
            return string.Concat(CommonNode.kNodeTree, kLoginModulesNodeName, ".", name);
        }

        /// <summary>
        /// Gets a string that uniquely identifies a chain within the common node.
        /// </summary>
        /// <param name="name">The anme of the chain.</param>
        /// <returns>A string that uniquely identifies a chain within a common node.</returns>
        string ChainNodeKey(string name) {
            return string.Concat(CommonNode.kNodeTree, kChainsNodeName, ".", name);
        }

        string RepositoryKey(string name) {
            return string.Concat(CommonNode.kNodeTree, kRepositoryNodeName, ".", name);
        }
        #endregion

        /// <summary>
        /// Gets a ConnectionStringNode with the specified name.
        /// </summary>
        /// <param name="name">The name of the node to get.</param>
        /// <returns>A ConnectionStringNode with the specified name or null if the <paramref name="name"/> was not
        /// found within the connection strings list.</returns>
        public ConnectionStringNode GetConnectionString(string name) {
            return this[ConnectionStringKey(name)] as ConnectionStringNode;
        }

        /// <summary>
        /// Gets a ConnectionStringNode with the specified name.
        /// </summary>
        /// <param name="name">The name of the node to get.</param>
        /// <returns>A ConnectionStringNode with the specified name or null if the <paramref name="name"/> was not
        /// found within the connection strings list.</returns>
        public bool GetConnectionString(string name, ConnectionStringNode node) {
            node = GetConnectionString(name);
            return (node != null);
        }
    }
}
