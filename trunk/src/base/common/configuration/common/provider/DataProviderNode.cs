using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Resources;
using Nohros.Data;

namespace Nohros.Configuration
{
    public class DataProviderNode : ProviderNode
    {
        const string kDataBaseOwnerKey = "database-owner";
        const string kConnectionStringKey = "connection-string";
        const string kIsEncryptedKey = "encrypted";
        const string kAssemblyLocationKey = "assembly-location";
        const string kDataSourceTypeKey = "data-source-type";
        const string kTypeKey = "type";

        string database_owner_;
        string connection_string_;

        DataSourceType data_source_;
        NameValueCollection attributes_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the DataProviderNode.
        /// </summary>
        /// <param name="name">The name of the data provider.</param>
        /// <param name="type">The data type of the provider.</param>
        /// <remarks>
        /// The connection string and database owner of a provider is declared as a reference to a connection string
        /// declared on the connection-strings node of the configuration file. These references will be resolved
        /// through the specified common_node. If a reference could not be resolved we assume the reference as
        /// the final data.
        /// </remarks>
        public DataProviderNode(string name, string type): base(name, type) {
            attributes_ = new NameValueCollection();
            database_owner_ = "dbo";
            connection_string_ = null;
        }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a data provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The <paramref name="node"/> is not a
        /// valid representation of a data provider.</exception>
        public override void Parse(XmlNode node, NohrosConfiguration config) {
            InternalParse(node, config);

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

                    default:
                        attributes_.Add(attribute.Name, attribute.Value);
                        break;
                }
            }

            // the name, type and connection string parameters are mandatory.
            if (name_ == null || type_ == null || connection_string_ == null)
                Thrower.ThrowProviderException((connection_string_ == null) ? ExceptionResource.DataProvider_ConnectionString : ExceptionResource.DataProvider_Provider_Attributes);

            // if the connection string node is a reference to a global value, we need to resolve it.
            ConnectionStringNode dbstring_node = config.ConnectionStrings[connection_string_];
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
    }
}
