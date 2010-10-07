using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Data;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class CommonNode : ConfigurationNode
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the CommonNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="config">The related <see cref="NohrosConfiguration"/> object.</param>
        public CommonNode() : base(NohrosConfiguration.kCommonNodeName) { }
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
            XmlNode data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kRepositoryNodeName);
            if (data_node != null) {
                DictionaryValue<RepositoryNode> repositories = new DictionaryValue<RepositoryNode>();
                config.Nodes[NohrosConfiguration.kRepositoryNodeTree] = repositories;

                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, "attribute name", NohrosConfiguration.kCommonNodeTree + "." + NohrosConfiguration.kRepositoryNodeName));

                        RepositoryNode repository = new RepositoryNode(name);
                        repository.Parse(n, config);
                        repositories[repository.Name] = repository;
                    }
                }
            }

            // parse the connection strings
            data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kConnectionStringsNodeName);
            if (data_node != null) {
                DictionaryValue<ConnectionStringNode> connection_strings = new DictionaryValue<ConnectionStringNode>();
                config.Nodes[NohrosConfiguration.kConnectionStringNodeTree] = connection_strings;

                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null;
                        if (!(GetAttributeValue(n, "name", out name)))
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kCommonNodeTree + "." + NohrosConfiguration.kConnectionStringsNodeName));

                        ConnectionStringNode conn_string_node = new ConnectionStringNode(name);
                        conn_string_node.Parse(n, config);
                        connection_strings[conn_string_node.Name] = conn_string_node;
                    }
                }
            }

            // parse the providers
            data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kProvidersNodeName);
            if (data_node != null) {

                ProviderType provider_type = default(ProviderType);
                DictionaryValue<DataProviderNode> data_providers = null;
                DictionaryValue<MessengerProviderNode> messenger_providers = null;

                foreach (XmlNode provider_node in data_node.ChildNodes) {
                    if (string.Compare(provider_node.Name, NohrosConfiguration.kDataProviderNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        // parse the data providers
                        data_providers = new DictionaryValue<DataProviderNode>();
                        config.Nodes[NohrosConfiguration.kDataProviderNodeTree] = data_providers;
                        provider_type = ProviderType.Data;
                    } else if (string.Compare(provider_node.Name, NohrosConfiguration.kMessengerProviderNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        messenger_providers = new DictionaryValue<MessengerProviderNode>();
                        config.Nodes[NohrosConfiguration.kMessengerProviderNodeTree] = messenger_providers;
                        provider_type = ProviderType.Messenger;
                    }

                    foreach (XmlNode n in provider_node.ChildNodes) {
                        if (string.Compare(n.Name, NohrosConfiguration.kProviderNodeName, StringComparison.OrdinalIgnoreCase) == 0) {

                            // the name and type property are mandatory for all providers.
                            string name = null, type = null;
                            if (!(GetAttributeValue(n, "name", out name) && GetAttributeValue(n, "type", out type)))
                                throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name or type", NohrosConfiguration.kProvidersNodeTree));

                            switch (provider_type) {
                                case ProviderType.Data:
                                    DataProviderNode provider = new DataProviderNode(name, type);
                                    provider.Parse(n, config);
                                    data_providers[provider.Name] = provider;
                                    break;

                                case ProviderType.Messenger:
                                    MessengerProviderNode messenger = new MessengerProviderNode(name, type);
                                    messenger.Parse(n, config);
                                    messenger_providers[messenger.Name] = messenger;
                                    break;
                            }
                        }
                    }
                }
            }

            // parse the login modules
            data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kLoginModulesNodeName);
            if (data_node != null) {
                DictionaryValue<LoginModuleNode> login_modules = new DictionaryValue<LoginModuleNode>();
                config.Nodes[NohrosConfiguration.kLoginModuleNodeTree] = login_modules;

                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, NohrosConfiguration.kModuleNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kLoginModuleNodeTree));

                        LoginModuleNode login_module = new LoginModuleNode(name);
                        login_module.Parse(n, config);
                        login_modules[login_module.Name] = login_module;
                    }
                }
            }

            // parse the chains
            data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kChainsNodeName);
            if(data_node != null) {
                DictionaryValue<ChainNode> chains = new DictionaryValue<ChainNode>();
                config.Nodes[NohrosConfiguration.kChainNodeTree] = chains;

                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, NohrosConfiguration.kChainNodeName, StringComparison.OrdinalIgnoreCase) == 0) {
                        string name = null;
                        if (!GetAttributeValue(n, "name", out name))
                            throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kChainNodeTree + "." + NohrosConfiguration.kChainNodeName));

                        ChainNode chain = new ChainNode(name);
                        chain.Parse(n, config);
                        chains[chain.Name] = chain;
                    }
                }
            }
        }
    }
}
