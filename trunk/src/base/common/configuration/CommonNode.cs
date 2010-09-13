using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.configuration
{
    internal class CommonNode : IConfigurationNode
    {
        #region .ctor
        public CommonNode() {
        }
        #endregion

        /// <summary>
        /// Parses the nohros common node.
        /// </summary>
        void ParseCommonNode(XmlNode common_node, XmlNamespaceManager namespace_manager) {

            // parse the repository
            XmlNode node = common_node.SelectSingleNode(kRepositoryNodeName, namespace_manager);
            if (node != null) {
                foreach (XmlNode n in node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null, relative_path = null;
                        if (!(GetAttributeValue(n, "name", out name) && GetAttributeValue(n, "relative-path", out relative_path)))
                            Thrower.ThrowConfigurationException();

                        // resolve the relative path
                        if (Path.IsPathRooted(relative_path))
                            throw new ConfigurationErrorsException(StringResources.Config_PathIsRooted);

                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relative_path);
                        name_value_pairs_[PathKey(name)] = Value.CreateStringValue(path);
                    }
                }
            }

            // parse the connection strings
            node = common_node.SelectSingleNode(kConnectionStringNodeName, namespace_manager);
            if (node != null) {
                foreach (XmlNode n in node.ChildNodes) {
                    if (string.Compare(n.Name, "add") == 0) {
                        string name = null;
                        if (!(GetAttributeValue(n, "name", out name)))
                            Thrower.ThrowConfigurationException();

                        string database_owner = null;
                        if (GetAttributeValue(n, kDataBaseOwnerAttributeName, out database_owner))
                            name_value_pairs_[DatabaseOwnerKey(name)] = Value.CreateStringValue(database_owner);

                        string connection_string = null;
                        if (GetAttributeValue(n, kConnectionStringAttributeName, out connection_string))
                            name_value_pairs_[ConnectionStringKey(name)] = Value.CreateStringValue(connection_string);
                    }
                }
            }

            // parse the providers
            node = common_node.SelectSingleNode(kProvidersNodeName, namespace_manager);
            if (node != null) {
                foreach (XmlNode n in node.ChildNodes) {
                    if (string.Compare(n.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                        ParseProvider(n);
                    }
                }
            }

            // parse the login-modules
            node = common_node.SelectSingleNode(kProvidersNodeName, namespace_manager);
            if (node != null) {
                foreach (XmlNode n in node.ChildNodes) {
                    if (string.Compare(n.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                        ParseProvider(n);
                    }
                }
            }
        }
    }
}
