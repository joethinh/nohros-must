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
    /// 
    /// </summary>
    public abstract class NohrosConfiguration : IConfiguration
    {
        const string kRepositoryNodeName = "x:repository";
        const string kCommonNodeName = "x:common";
        const string kWebNodeName = "x:web";
        const string kContentGroupsNodeName = "x:content-groups";
        const string kProvidersNodeName = "x:providers";
        const string kConnectionStringNodeName = "x:connection-string";

        const string kFileNameAttributeName = "file-name";
        const string kNameAttributeName = "name";
        const string kBuildAttributeName = "build";
        const string kMimeTypeAttributeName = "mime-type";
        const string kPathRefAttributeName = "path-ref";
        const string kDataBaseOwnerAttributeName = "dbowner";
        const string kConnectionStringAttributeName = "dbstring";

        const string kPathPrefix = "path:";
        const string kContentGroupPrefix = "ctgrp:";
        const string kConnectionStringPrefix = "connstr:";
        const string kDataBaseOwnerPrefix = "dbo:";

        const string kDataSourceTypeKey = "data-source-type";
        const string kNameKey = "name";
        const string kTypeKey = "type";
        const string kDataBaseOwnerKey = "database-owner";
        const string kConnectionStringKey = "connection-string";
        const string kIsEncryptedKey = "encrypted";
        const string kAssemblyLocationKey = "assembly-location";

        const string kConfigurationFileNodeName = "NohrosConfigurationFile";

        DictionaryValue properties_;
        Dictionary<string, Value> name_value_pairs_;

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
            name_value_pairs_ = new Dictionary<string, Value>();
        }
        #endregion

        /// <summary>
        /// Loads the configuration values based on the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Each application has a configuration file. This has the same name as the application
        /// whith ' .config ' appended. This file is XML and calling this function prompts the
        /// loader to look in that file for a key named NohrosConfigurationFile that contains the
        /// relative path for the configuration file.
        /// </remarks>
        public override void Load() {
            string config_file_path = ConfigurationManager.AppSettings[kConfigurationFileNodeName];
            if (config_file_path == null)
                throw new ConfigurationErrorsException(string.Format(StringResources.Config_KeyNotFound, kConfigurationFileNodeName));

            if (!Path.IsPathRooted(config_file_path))
                config_file_path = Path.Combine(Location, config_file_path);

            Load(new FileInfo(config_file_path), null);
        }

        #region Parsers
        internal override void Parse(XmlElement element) {
            base.Parse(element);

            XmlNodeList nodes = element_.GetElementsByTagName("nohros");
            if (nodes == null || nodes.Count == 0)
                Thrower.ThrowConfigurationException();

            // If the nohros root node has a namespace we need to
            // set it on the namespace manager in order to retrieve nodes.
            XmlNode root_node = nodes[0];
            XmlNamespaceManager namespace_manager = new XmlNamespaceManager(element.OwnerDocument.NameTable);
            namespace_manager.AddNamespace("x", root_node.NamespaceURI);

            // parses the common node
            XmlNode node = root_node.SelectSingleNode(kCommonNodeName, namespace_manager);
            if(node != null)
                ParseCommonNode(node, namespace_manager);

            // parses the web node
            node = root_node.SelectSingleNode(kWebNodeName, namespace_manager);
            if (node != null)
                ParseWebNode(node, namespace_manager);
        }

        /// <summary>
        /// Parses the providers node of the nohros common node.
        /// </summary>
        /// <param name="provider_node">A node representing the provider to parse.</param>
        void ParseProvider(XmlNode provider_node) {
            string name = null;
            if (!GetAttributeValue(provider_node, kNameAttributeName, out name) || name.Trim().Length == 0)
                Thrower.ThrowConfigurationException();

            Provider provider = new Provider();
            bool connstring_is_encrypted = false;

            XmlAttributeCollection attributes = provider_node.Attributes;
            for (int i = 0, j = attributes.Count; i < j; i++) {
                XmlAttribute attribute = attributes[i];
                switch(attribute.Name) {
                    case kNameKey:
                        provider.Name = attribute.Value;
                        break;

                    case kTypeKey:
                        provider.Type = attribute.Value;
                        break;

                    case kConnectionStringKey:
                        provider.ConnectionString = attribute.Value;
                        break;

                    case kDataBaseOwnerKey:
                        provider.DatabaseOwner = attribute.Value;
                        break;

                    case kIsEncryptedKey:
                        connstring_is_encrypted = (string.Compare("true", attribute.Value, StringComparison.OrdinalIgnoreCase) == 0) ? true : false;
                        break;

                    case kDataSourceTypeKey:
                        provider.DataSourceType = DataHelper.ParseStringEnum<DataSourceType>(attribute.Value, DataSourceType.Unknown);
                        break;

                    case kAssemblyLocationKey:
                        // if the provider assembly location property is a relative path we need to resolve it
                        // using the configuration file location.
                        string location = attribute.Value;
                        if (location != null && !Path.IsPathRooted(location)) {
                            location = Path.Combine(Location, location);
                        }
                        provider.AssemblyLocation = location;
                        break;

                    default:
                        provider.Attributes.Add(attribute.Name, attribute.Value);
                        break;
                }
            }

            // the name, type and connection string parameters are mandatory.
            if (provider.Name == null || provider.Type == null || provider.ConnectionString == null)
                Thrower.ThrowProviderException((provider.ConnectionString== null) ? ExceptionResource.DataProvider_ConnectionString : ExceptionResource.DataProvider_Provider_Attributes);

            // if the connection string and/or data base owner is a reference to a
            // global value, we need to resolve it.
            Value value = null;
            if (name_value_pairs_.TryGetValue(DatabaseOwnerKey(provider.DatabaseOwner), out value))
                provider.DatabaseOwner = value.GetAsString();

            if (name_value_pairs_.TryGetValue(ConnectionStringKey(provider.ConnectionString), out value))
                provider.ConnectionString = value.GetAsString();

            if (connstring_is_encrypted)
                provider.ConnectionString = NSecurity.BasicDeCryptoString(provider.ConnectionString);

            providers_.Add(provider.Name, provider);
        }

        /// <summary>
        /// Parses the web node.
        /// </summary>
        /// <param name="web_node">An XML node representing the web node.</param>
        void ParseWebNode(XmlNode web_node, XmlNamespaceManager namespace_manager) {
            XmlNode node = web_node.SelectSingleNode(kContentGroupsNodeName, namespace_manager);
            if (node == null) {
                Thrower.ThrowConfigurationException();
            }

            foreach (XmlNode n in node.ChildNodes) {
                if (string.Compare(n.Name, "group", StringComparison.OrdinalIgnoreCase) == 0) {
                    ParseContentGroup(n);
                }
            }
        }

        /// <summary>
        /// Parses the group node of the web node.
        /// </summary>
        /// <param name="group_node">An XML node representing a group within a web node.</param>
        void ParseContentGroup(XmlNode group_node) {
            string name = null, build = null, mime_type = null, path_ref = null;
            if (!(GetAttributeValue(group_node, kNameAttributeName, out name) &&
                    GetAttributeValue(group_node, kBuildAttributeName, out build) &&
                    GetAttributeValue(group_node, kMimeTypeAttributeName, out mime_type) &&
                    GetAttributeValue(group_node, kPathRefAttributeName, out path_ref)
                )) {
                // TODO: log the exception.
                Thrower.ThrowConfigurationException();
            }

            // sanity check the build type
            if (build != "release" && build != "debug")
                // TODO: log the exception.
                // TODO: explain the exception.
                Thrower.ThrowConfigurationException();  

            // resolve the base path
            if (!Path.IsPathRooted(path_ref)) {
                Value value = null;
                if (name_value_pairs_.TryGetValue(PathKey(path_ref), out value)) {
                    path_ref = value.GetAsString();
                } else {
                    // TODO: explain the exception.
                    Thrower.ThrowConfigurationException();
                }
            }

            List<string> files = new List<string>();
            ContentGroup content_group = new ContentGroup(name, (build == "release" ? BuildType.Release : BuildType.Debug), mime_type, path_ref, files);

            // store the content group into the cache
            name_value_pairs_[ContentGroupKey(name, build, mime_type)] = Value.CreateGenericValue<ContentGroup>(content_group);
            
            string file_name = null;
            foreach (XmlNode file_node in group_node.ChildNodes) {
                if (string.Compare(file_node.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                    if (!GetAttributeValue(file_node, kFileNameAttributeName, out file_name)) {
                        // TODO: log the exception.
                        Thrower.ThrowConfigurationException();
                    }
                    files.Add(file_name);
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets the value of an attribute of a xml node.
        /// </summary>
        /// <param name="node">The node that contains the attribute.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">When this method returns contains the value of the attribute or null
        /// if the value could not be retrieved.</param>
        /// <returns>true if the atribbute retrieval operation is successful; otherwise false.</returns>
        bool GetAttributeValue(XmlNode node, string name, out string value) {
            XmlAttribute att = node.Attributes[name];
            value = null;
            if(att != null)
                value = att.Value;
            return (value != null);
        }

        /// <summary>
        /// Cleans the configuration values for reload.
        /// </summary>
        void CleanUp() {
            properties_.Clear();
            providers_.Clear();
        }

        #region Keys
        static string PathKey(string path) {
            return string.Concat(kPathPrefix, path);
        }

        static string ContentGroupKey(string group_name, string build, string mime_type) {
            return string.Concat(kContentGroupPrefix, group_name + build + mime_type);
        }

        static string ConnectionStringKey(string connection_string) {
            return string.Concat(kConnectionStringPrefix, connection_string);
        }

        static string DatabaseOwnerKey(string database_owner) {
            return string.Concat(kDataBaseOwnerPrefix, database_owner);
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
        public Provider GetProvider(string provider_name) {
            Provider provider = null;
            providers_.TryGetValue(provider_name, out provider);
            return provider;
        }

        /// <summary>
        /// Gets an string that represents the name of a owner of a database.
        /// </summary>
        /// <param name="name">An string that identifies the database owner.</param>
        /// <returns>An string that can be used to identify the ownere of the database related with
        /// the specified <paramref name="name"/> or null if the <paramref name="name"/> could not be found.</returns>
        public string GetDatabaseOwner(string name) {
            return GetValueAsString(DatabaseOwnerKey(name));
        }

        /// <summary>
        /// Gets an string that can be used to connects to a database.
        /// </summary>
        /// <param name="name">A string that identifies the connection string</param>
        /// <returns>An string that can be used to connects to a database or null if the <paramref name="name"/>
        ///could not be found.</returns>
        public string GetConnectionString(string name) {
            return GetValueAsString(ConnectionStringKey(name));
        }

        string GetValueAsString(string key) {
            Value value = null;
            if (name_value_pairs_.TryGetValue(key, out value))
                return value.GetAsString();
            return null;
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
        public ContentGroup GetContentGroup(string name, string build, string mime_type) {
            if (name == null || build == null || mime_type == null)
                throw new ArgumentException((name == null) ? (build == null) ? "mime_type" : "build" : "name");

            Value content_group;
            if (name_value_pairs_.TryGetValue(ContentGroupKey(name, build, mime_type), out content_group))
                return (content_group as GenericValue<ContentGroup>).TValue;
            return null;
        }
        #endregion
    }
}