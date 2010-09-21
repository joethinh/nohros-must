using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Data;

namespace Nohros.Configuration
{
    public class ProviderNode : ConfigurationNode
    {
        internal const string kNodeTree = CommonNode.kNodeTree + CommonNode.kProvidersNodeName + ".";

        const string kDataBaseOwnerKey = "database-owner";
        const string kConnectionStringKey = "connection-string";
        const string kIsEncryptedKey = "encrypted";
        const string kAssemblyLocationKey = "assembly-location";
        const string kDataSourceTypeKey = "data-source-type";
        const string kTypeKey = "type";

        string type_;
        string database_owner_;
        string connection_string_;
        string assembly_location_;

        DataSourceType data_source_;
        NameValueCollection attributes_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ProviderNode.
        /// </summary>
        /// <param name="common_node">A CommonNode object which this provider belongs.</param>
        /// <remarks>
        /// The connection string and database owner of a provider is declared as a reference to a connection string
        /// declared on the connection-strings node of the configuration file. These references will be resolved
        /// through the specified common_node. If a reference could not be resolved we assume the reference as
        /// the final data.
        /// </remarks>
        public ProviderNode(string name, CommonNode parent_node): base(name, parent_node) {
            attributes_ = new NameValueCollection();
            type_ = null;
            database_owner_ = "dbo";
            connection_string_ = null;
            assembly_location_ = null;
        }
        #endregion

        public override void Parse(XmlNode node) {
            bool connstring_is_encrypted = false;

            XmlAttributeCollection attributes = node.Attributes;
            for (int i = 0, j = attributes.Count; i < j; i++) {
                XmlAttribute attribute = attributes[i];
                switch (attribute.Name) {
                    case kTypeKey:
                        type_ = attribute.Value;
                        break;

                    case kConnectionStringKey:
                        connection_string_ = attribute.Value;
                        break;

                    case kDataBaseOwnerKey:
                        database_owner_ = attribute.Value;
                        break;

                    case kIsEncryptedKey:
                        connstring_is_encrypted = (string.Compare("true", attribute.Value, StringComparison.OrdinalIgnoreCase) == 0) ? true : false;
                        break;

                    case kDataSourceTypeKey:
                        data_source_ = DataHelper.ParseStringEnum<DataSourceType>(attribute.Value, DataSourceType.Unknown);
                        break;

                    case kAssemblyLocationKey:
                        // if the provider assembly location property is a relative path we need to resolve it
                        // using the configuration file location.
                        string location = attribute.Value;
                        if (location != null && !Path.IsPathRooted(location)) {
                            location = Path.Combine((ParentNode as CommonNode).Configuration.Location, location);
                        }
                        assembly_location_ = location;
                        break;

                    default:
                        attributes_.Add(attribute.Name, attribute.Value);
                        break;
                }
            }

            // the name, type and connection string parameters are mandatory.
            if (name_ == null || type_ == null || connection_string_ == null)
                Thrower.ThrowProviderException((connection_string_ == null) ? ExceptionResource.DataProvider_ConnectionString : ExceptionResource.DataProvider_Provider_Attributes);

            // if the connection string node is a reference to a global value, we need to resolve it.
            ConnectionStringNode dbstring_node = (ParentNode as CommonNode).GetConnectionString(connection_string_);
            if (dbstring_node != null) {
                database_owner_ = dbstring_node.DatabaseOwner;
                connection_string_ = (connstring_is_encrypted) ? NSecurity.BasicDeCryptoString(dbstring_node.ConnectionString) : dbstring_node.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the attributes of the Provider
        /// </summary>
        public NameValueCollection Attributes {
            get { return attributes_; }
            set { attributes_ = value; }
        }

        /// <summary>
        /// Gets the type of the Provider.
        /// </summary>
        public string Type {
            get { return type_; }
            set { type_ = value; }
        }

        /// <summary>
        /// Gets the type of the data source that will be used by the provider.
        /// </summary>
        public DataSourceType DataSourceType {
            get { return data_source_; }
            set { data_source_ = value; }
        }

        /// <summary>
        /// Gets the name of the owner of the database
        /// </summary>
        public string DatabaseOwner {
            get { return database_owner_; }
            set { database_owner_ = value; }
        }

        /// <summary>
        /// Gets a connection string taht can be used to open the database.
        /// </summary>
        public string ConnectionString {
            get { return connection_string_; }
            set { connection_string_ = value; }
        }

        /// <summary>
        /// Gets a string representing the fully qualified path to the directory where
        /// the assembly related with the provider is located.
        /// </summary>
        /// <remarks>
        /// This must be an absolute path or a path relative to the configuration file.
        /// </remarks>
        public string AssemblyLocation {
            get { return assembly_location_; }
            set { assembly_location_ = value; }
        }
    }
}
