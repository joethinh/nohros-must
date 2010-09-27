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
        internal const string kNodeTree = kCommonNodeName + ".";
        internal const string kRepositoryNodeName = "repository";
        internal const string kProvidersNodeName = "providers";
        internal const string kConnectionStringsNodeName = "connection-strings";
        internal const string kLoginModulesNodeName = "login-modules";

        StringMap paths_;
        NohrosConfiguration configuration_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the CommonNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        public CommonNode(NohrosConfiguration configuration):base(kCommonNodeName) {
            configuration_ = configuration;
            paths_ = new StringMap();
        }
        #endregion

        public static CommonNode FromXmlNode(XmlNode node, NohrosConfiguration configuration) {
            CommonNode common_node = new CommonNode(configuration);
            common_node.Parse(node);
            return common_node;
        }

        /// <summary>
        /// Parses a XML node that contains information about a nohros common module.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a nohros common node.</exception>
        public override void Parse(XmlNode node) {
            // parse the repository node.
            XmlNode data_node = IConfiguration.SelectNode(node, kRepositoryNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null, relative_path = null;
                        if (!(GetAttributeValue(n, "name", out name) && GetAttributeValue(n, "relative-path", out relative_path)))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ErrorAt, "attribute name or relative-path", kNodeTree + kRepositoryNodeName));

                        // resolve the relative path
                        if (Path.IsPathRooted(relative_path))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_PathIsRooted, relative_path));

                        paths_[name] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relative_path);
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
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + kConnectionStringsNodeName));

                        ConnectionStringNode conn_string_node = new ConnectionStringNode(name, this);
                        conn_string_node.Parse(n);
                        this[ConnectionStringKey(conn_string_node.Name)] = conn_string_node;
                    }
                }
            }

            // parse the providers
            data_node = IConfiguration.SelectNode(node, kProvidersNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    if (string.Compare(n.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                        string name = null;
                        if (!(GetAttributeValue(n, "name", out name)))
                            Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + kProvidersNodeName));

                        ProviderNode provider = new ProviderNode(name, this);
                        provider.Parse(n);
                        this[ProviderNodeKey(provider.Name)] = provider;
                    }
                }
            }

            // parse the login modules
            data_node = IConfiguration.SelectNode(node, kLoginModulesNodeName);
            if (data_node != null) {
                foreach (XmlNode n in data_node.ChildNodes) {
                    LoginModuleNode login_module = new LoginModuleNode(n.Name, this);
                    this[LoginModuleKey(login_module.Name)] = login_module;
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
            return string.Concat(kConnectionStringsNodeName, ".", name);
        }

        /// <summary>
        /// Gets a string that uniquely identifies a provider within the common node.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <returns>A string that uniquely identifies a provider within a common node.</returns>
        string ProviderNodeKey(string name) {
            return string.Concat(kProvidersNodeName, ".", name);
        }

        /// <summary>
        /// Gets a string that uniquely identifies a login module within the common node.
        /// </summary>
        /// <param name="name">The name of the login module.</param>
        /// <returns>A string that uniquely identifies a login module within a common node.</returns>
        string LoginModuleKey(string name) {
            return string.Concat(kLoginModulesNodeName, ".", name);
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

        /// <summary>
        /// Gets a fully qualified path for a repository with the specified name.
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        /// <returns>The fully qualified path for the repository with the specified name or null if the <paramref name="name"/>
        /// was not found within the repositories list.</returns>
        public string GetRepository(string name) {
            return paths_[name];
        }

        /// <summary>
        /// Gets a fully qualified path for a repository with the specified name.
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        /// <param name="repository">The repository fully qualified path.</param>
        /// <returns>true if the <paramref name="name"/> was found within the list of repositories; otherwise null.</returns>
        public bool GetRepository(string name, out string repository) {
            return paths_.TryGetValue(name, out repository);
        }

        /// <summary>
        /// Gets informations about a data provider by using the specified provider name.
        /// </summary>
        /// <param name="provider_name">The name of the data provider to get informations from</param>
        /// <returns>A <see cref="Provider"/> object that contains information about the provider
        /// associated with the specified <paramref name="provider_name"/> or null if the <paramref name="provider_name"/>
        /// could not be found.
        /// </returns>
        public ProviderNode GetProvider(string name) {
            return this[ProviderNodeKey(name)] as ProviderNode;
        }

        /// <summary>
        /// Gets informations about a data provider by using the specified provider name.
        /// </summary>
        /// <param name="provider_name">The name of the data provider to get informations from</param>
        /// <returns>true if the specified <paramref name="provider_name"/> is found;otherwise false.</returns>
        public bool GetProvider(string name, out ProviderNode provider) {
            provider = GetProvider(name);
            return (provider != null);
        }

        /// <summary>
        /// Gets a login module configured for the application by using the specified login module name.
        /// </summary>
        /// <param name="name">The name of the login module.</param>
        /// <returns>A login module with the specified name, or null if the name was not found.</returns>
        public LoginModuleNode GetLoginModule(string name) {
            return this[LoginModuleKey(name)] as LoginModuleNode;
        }

        /// <summary>
        /// Gets a login module configured for the application by using the specified login module name.
        /// </summary>
        /// <param name="name">The name of the login module.</param>
        /// <param name="login_module">When this method return contains a login module with the specified name
        /// or null if the name was not found.</param>
        /// <returns>true if a login module with the specified name was found; otherwise false.</returns>
        public bool GetLoginModule(string name, out LoginModuleNode login_module) {
            login_module = GetLoginModule(name);
            return (login_module != null);
        }

        /// <summary>
        /// Gets the underlying NohrosConfiguration object.
        /// </summary>
        public NohrosConfiguration Configuration {
            get { return configuration_; }
        }
    }
}
