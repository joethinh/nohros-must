using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Data;
using Nohros.Data.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class CommonNode : ConfigurationNode
    {
        string config_file_location_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the CommonNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="config_file_location">A string representing the path of the folder where the
        /// configuration file is stored.</param>
        public CommonNode(string config_file_location) : base(NohrosConfiguration.kCommonNodeName) {
            config_file_location_ = config_file_location;
        }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a common node.
        /// </summary>
        /// <param name="common_node">A XML node containing the data to parse.</param>
        /// <param name="nodes">A <see cref="DictionaryValue"/> containing the collection of
        /// configuration nodes.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The <paramref name="node"/>
        /// is not a valid representation of a common node.</exception>
        /// <remarks>
        /// The <paramref name="nodes"/> is used to store the nodes that is parsed by this class.
        /// </remarks>
        public void Parse(XmlNode common_node, DictionaryValue nodes) {
            XmlNode node = null;

            // parse each node group. We try to get a node group by usinf its name and
            // if it is found we start parsing it.
            DictionaryValue<RepositoryNode> repositories;
            if (GetNode<RepositoryNode>(NohrosConfiguration.kRepositoryNodeName,
                NohrosConfiguration.kRepositoryNodeTree, nodes, common_node, out node,
                out repositories)) {
                ParseRepository(node, repositories);
            }

            DictionaryValue<ConnectionStringNode> connection_strings;
            if (GetNode<ConnectionStringNode>(NohrosConfiguration.kConnectionStringsNodeName,
                NohrosConfiguration.kConnectionStringNodeTree, nodes, common_node, out node,
                out connection_strings)) {
                ParseConnectionStrings(node, connection_strings);
            }

            // parse the data providers, messenger providers and cache providers.
            ParseProviders(node, nodes, connection_strings);

            DictionaryValue<LoginModuleNode> login_modules;
            if (GetNode<LoginModuleNode>(NohrosConfiguration.kLoginModulesNodeName,
                NohrosConfiguration.kLoginModuleNodeTree, nodes, common_node, out node,
                out login_modules)) {
                ParseLoginModules(node, login_modules);
            }


            DictionaryValue<ChainNode> chains;
            if (GetNode<ChainNode>(NohrosConfiguration.kChainsNodeName,
                NohrosConfiguration.kChainNodeTree, nodes, common_node, out node, out chains)) {
                ParseChains(node, chains);
            }
        }

        /// <summary>
        /// Parse the repository group node. Each valid found repository is added to the specified
        /// repositories collection.
        /// </summary>
        /// <param name="root_repository_node">A Xml node representing the repository root node.</param>
        /// <param name="repositories">A dictionary where the parsed repositorie will be added.</param>
        /// <exception cref="ConfigurationErrorsException">An node that is child of the
        /// <paramref name="root_repository_node"/> does not represents a valid repository node.</exception>
        void ParseRepository(XmlNode root_repository_node, DictionaryValue<RepositoryNode> repositories) {
            foreach (XmlNode n in root_repository_node.ChildNodes) {
                if (string.Compare(n.Name, "add") == 0) {
                    string name = null;

                    // the "name" parameter is mandatory.
                    if (!GetAttributeValue(n, "name", out name))
                        throw new ConfigurationErrorsException(
                            string.Format(
                                StringResources.Config_ErrorAt,
                                "attribute name",
                                string.Concat(
                                    NohrosConfiguration.kCommonNodeTree,
                                    ".",
                                    NohrosConfiguration.kRepositoryNodeName
                                ) // string.concat
                            ) //string.Format
                    ); // throw

                    RepositoryNode repository = new RepositoryNode(name);
                    repositories[repository.Name] = repository;
                }
            }
        }

        /// <summary>
        /// Parses the connection string node and added the parsed connections string to the specified
        /// dictionary.
        /// </summary>
        /// <param name="conn_string_root_node">A XmlNode representing the connection string root node.</param>
        /// <param name="connection_strings">A dictionary where the parsed connection strings will be
        /// added.</param>
        /// <exception cref="ConfigurationErrorsException">An node that is child of the
        /// <paramref name="conn_string_root_node"/> node does not represents a valid connection string.</exception>
        void ParseConnectionStrings(XmlNode conn_string_root_node,
            DictionaryValue<ConnectionStringNode> connection_strings) {

            foreach (XmlNode n in conn_string_root_node.ChildNodes) {
                if (string.Compare(n.Name, "add") == 0) {
                    string name = null;

                    if (!(GetAttributeValue(n, "name", out name))) {
                        throw new ConfigurationErrorsException(
                            string.Format(StringResources.Config_MissingAt, "name",
                                string.Concat(
                                    NohrosConfiguration.kCommonNodeTree
                                    ,"."
                                    , NohrosConfiguration.kConnectionStringsNodeName)));
                    }

                    ConnectionStringNode conn_string_node = new ConnectionStringNode(name);
                    conn_string_node.Parse(n);
                    connection_strings[conn_string_node.Name] = conn_string_node;
                }
            }
        }

        void ParseProviders(XmlNode node, DictionaryValue nodes,
            DictionaryValue<ConnectionStringNode> connection_string_node) {

            XmlNode data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kProvidersNodeName);
            if (data_node != null) {

                ProviderType provider_type = default(ProviderType);
                DictionaryValue<DataProviderNode> data_providers = null;
                DictionaryValue<MessengerProviderNode> messenger_providers = null;
                DictionaryValue<CacheProviderNode> cache_providers = null;

                foreach (XmlNode provider_node in data_node.ChildNodes) {
                    if (string.Compare(provider_node.Name, NohrosConfiguration.kDataProviderNodeName,
                        StringComparison.OrdinalIgnoreCase) == 0) {

                        // parse the data providers
                        data_providers = new DictionaryValue<DataProviderNode>();
                        nodes[NohrosConfiguration.kDataProviderNodeTree] = data_providers;
                        provider_type = ProviderType.Data;
                    } else if (string.Compare(provider_node.Name,
                        NohrosConfiguration.kMessengerProviderNodeName,
                        StringComparison.OrdinalIgnoreCase) == 0) {

                        messenger_providers = new DictionaryValue<MessengerProviderNode>();
                        nodes[NohrosConfiguration.kMessengerProviderNodeTree] = messenger_providers;
                        provider_type = ProviderType.Messenger;
                    } else if (string.Compare(provider_node.Name,
                        NohrosConfiguration.kCacheProviderNodeName,
                        StringComparison.OrdinalIgnoreCase) == 0) {

                        cache_providers = new DictionaryValue<CacheProviderNode>();
                        nodes[NohrosConfiguration.kCacheProviderNodeTree] = cache_providers;
                        provider_type = ProviderType.Cache;
                    }

                    foreach (XmlNode n in provider_node.ChildNodes) {
                        if (string.Compare(n.Name, NohrosConfiguration.kProviderNodeName, StringComparison.OrdinalIgnoreCase) == 0) {

                            // the name and type property are mandatory for all providers.
                            string name = null, type = null;
                            if (!(GetAttributeValue(n, "name", out name) && GetAttributeValue(n, "type",
                                out type))) {
                                throw new ConfigurationErrorsException(
                                    string.Format(
                                        StringResources.Config_MissingAt,
                                        "name or type",
                                        NohrosConfiguration.kProvidersNodeTree));
                            }

                            switch (provider_type) {
                                case ProviderType.Data:
                                    DataProviderNode provider = new DataProviderNode(name, type);
                                    provider.Parse(n, config_file_location_);

                                    // resolve the connection strings references.
                                    provider.ResolveReferences(connection_string_node);
                                    data_providers[provider.Name] = provider;
                                    break;

                                case ProviderType.Messenger:
                                    MessengerProviderNode messenger = new MessengerProviderNode(name, type);
                                    messenger.Parse(n, config_file_location_);
                                    messenger_providers[messenger.Name] = messenger;
                                    break;

                                case ProviderType.Cache:
                                    CacheProviderNode cache_provider = new CacheProviderNode(name, type);
                                    cache_provider.Parse(n, config_file_location_);
                                    cache_providers[cache_provider.Name] = cache_provider;
                                    break;
                            } // switch
                        } // if
                    } // foreach
                } // foreach
            } // if
        }

        void ParseLoginModules(XmlNode login_modules_root_node, DictionaryValue<LoginModuleNode> login_modules) {
            foreach (XmlNode n in login_modules_root_node.ChildNodes) {
                if (string.Compare(n.Name, NohrosConfiguration.kModuleNodeName,
                    StringComparison.OrdinalIgnoreCase) == 0) {

                    string name = null;
                    if (!GetAttributeValue(n, "name", out name))
                        throw new ConfigurationErrorsException(
                            string.Format(
                                string.Concat(StringResources.Config_MissingAt,
                                "name",
                                NohrosConfiguration.kLoginModuleNodeTree)));

                    LoginModuleNode login_module = new LoginModuleNode(name);
                    login_module.Parse(n);
                    login_modules[login_module.Name] = login_module;
                }
            }
        }

        void ParseChains(XmlNode chains_root_node, DictionaryValue<ChainNode> chains) {
            foreach (XmlNode n in chains_root_node.ChildNodes) {
                if (string.Compare(n.Name, NohrosConfiguration.kChainNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                    string name = null;
                    if (!GetAttributeValue(n, "name", out name))
                        throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kChainNodeTree + "." + NohrosConfiguration.kChainNodeName));

                    ChainNode chain = new ChainNode(name);
                    chain.Parse(n);
                    chains[chain.Name] = chain;
                }
            }
        }
    }
}
