using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32;

namespace Nohros.Data
{
    #region enumerations

    /// <summary>
    /// Indicates where the connection strings is located.
    /// </summary>
    public enum ConnectionStringsRepository
    {
        /// <summary>
        /// The connection strings are located  in the windows registry.
        /// </summary>
        WindowsRegistry = 0,

        /// <summary>
        /// The connection strings are located on the configuration file.
        /// </summary>
        ConfigurationFile = 1,
    }

    public enum DataSourceType
    {
        MsSql = 0,
        OleDb = 1,
        Odbc = 2,
        Unknown = 100
    }
    #endregion

    /// <summary>
    /// Stores all the data needed to create instances of the IDataProvider provider's implementation
    /// of the data source classes.
    /// </summary>
    public class Provider
    {
        const string kRepositoryKey = "repository";
        const string kDataSourceTypeKey = "dataSourceType";
        const string kNameKey = "name";
        const string kTypeKey = "type";
        const string kDataBaseOwnerKey = "databaseOwner";
        const string kConnectionStringKey = "connectionString";
        const string kIsEncryptedKey = "encrypted";

        string name_;
        string type_;
        string database_owner_;
        string connection_string_;
        bool is_encrypted_;

        ConnectionStringsRepository repository_;
        DataSourceType data_source_;
        NameValueCollection attributes_;
        

        #region .ctor

        /// <summary>
        /// Default constructor.
        /// </summary>
        Provider()
        {
            attributes_ = new NameValueCollection();
            is_encrypted_ = false;

            // default for compatibility with legacy applications
            repository_ = ConnectionStringsRepository.ConfigurationFile;
            data_source_ = DataSourceType.Unknown;
            database_owner_ = "dbo";
            connection_string_ = null;
        }

        /// <summary>
        /// Initializes a new instanceof the <see cref="Provider"/> class by using
        /// the specified attributes
        /// </summary>
        /// <param name="node">A <see cref="XmlNode"/> that contains informations about the provider.</param>
        /// <remarks>If the <paramref name="attributes"/> argumentdoes not have
        /// a definition for the configReposirory attribute, this constructor will
        /// set the <see cref="Provider.ConnectionStringsRepository"/> property to
        /// <see cref="Nohros.Data.ConnectionStringsRepository.ConfigurationFile"/></remarks>
        public Provider(XmlNode node):this()
        {
            if (node == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.any);

            XmlAttributeCollection attributes = node.Attributes;
            for (int i = 0, j = attributes.Count; i < j; i++) {
                XmlAttribute attribute = attributes[i];
                switch(attribute.Name) {
                    case kNameKey:
                        name_ = attribute.Value;
                        break;

                    case kTypeKey:
                        type_ = attribute.Value;
                        break;

                    case kConnectionStringKey:
                        connection_string_ = attribute.Value;
                        break;

                    case kDataBaseOwnerKey:
                        database_owner_ = attribute.Value;
                        break;

                    case kRepositoryKey:
                        repository_ = DataHelper.ParseStringEnum<ConnectionStringsRepository>(attribute.Value, ConnectionStringsRepository.ConfigurationFile);
                        break;

                    case kIsEncryptedKey:
                        is_encrypted_ = (string.Compare("true", attribute.Value, StringComparison.OrdinalIgnoreCase) ==0) ? true : false;
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
                Thrower.ThrowProviderException((connection_string_== null) ? ExceptionResource.DataProvider_ConnectionString : ExceptionResource.DataProvider_Provider_Attributes);

            if(repository_ == ConnectionStringsRepository.ConfigurationFile) {
                try {
                    XmlNode xml_node = node.OwnerDocument.FirstChild.SelectSingleNode(database_owner_);
                    if (xml_node != null)
                        database_owner_ = xml_node.Value;
                } catch (XPathException) {
                    database_owner_ = "dbo";
                }

                try {
                    XmlNode xml_node = node.OwnerDocument.FirstChild.SelectSingleNode(connection_string_);
                    if (xml_node != null)
                        connection_string_ = xml_node.Value;
                }
                catch (XPathException) {
                    // connection string are mandatory.
                    Thrower.ThrowProviderException(ExceptionResource.DataProvider_ConnectionString);
                }
            }

            if (is_encrypted_)
                connection_string_ = NSecurity.BasicDeCryptoString(connection_string_);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Provider"/> class by using the specified
        /// registry key repository.
        /// </summary>
        /// <param name="attributes">A key-level node in the windows registry containing
        /// the attributes for the provider.</param>
        /// <remarks>If the <paramref name="attributes"/> argument does not have
        /// a definition for the configReposirory attribute, this constructor will
        /// set the <see cref="Provider.ConnectionStringsRepository"/> property to
        /// <see cref="Nohros.Data.ConnectionStringsRepository.ConfigurationFile"/></remarks>
        public Provider(RegistryKey attributes):this()
        {
            string[] keys = attributes.GetValueNames();

            if (attributes != null) {
                for (int i = 0, j = keys.Length; i < j; i++) {
                    switch (keys[i]) {
                        case kNameKey:
                            name_ = (string)attributes.GetValue(kNameKey, null);
                            break;

                        case kTypeKey:
                            type_ = (string)attributes.GetValue(kTypeKey, null);
                            break;

                        case kConnectionStringKey:
                            connection_string_ = (string)attributes.GetValue(kConnectionStringKey, null);
                            break;

                        case kDataBaseOwnerKey:
                            database_owner_ = attributes.GetValue(kDataBaseOwnerKey, null) as string;
                            break;

                        case kRepositoryKey:
                            repository_ = DataHelper.ParseStringEnum<ConnectionStringsRepository>(
                                (string)attributes.GetValue(kRepositoryKey, null),
                                ConnectionStringsRepository.ConfigurationFile);
                            break;

                        case kIsEncryptedKey:
                            is_encrypted_ = (string.Compare("true",
                                (string)attributes.GetValue(kIsEncryptedKey, null),
                                StringComparison.OrdinalIgnoreCase) == 0);
                            break;

                        default:
                            attributes_.Add(keys[i], (string)attributes.GetValue(keys[i], null));
                            break;
                    }
                }
            }

            // the name, type and connection string parameters are mandatory.
            if (name_ == null || type_ == null || connection_string_ == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_Provider_Attributes);
        }
        #endregion

        /// <summary>
        /// Gets the attributes of the Provider
        /// </summary>
        public NameValueCollection Attributes
        {
            get { return attributes_; }
        }

        /// <summary>
        /// Gets the name of the Provider.
        /// </summary>
        public string Name
        {
            get { return name_; }
        }

        /// <summary>
        /// Gets the type of the Provider.
        /// </summary>
        public string Type
        {
            get { return type_; }
        }

        /// <summary>
        /// Gets the repository used to store the configuration values.
        /// </summary>
        public ConnectionStringsRepository ConnectionStringsRepository
        {
            get { return repository_; }
        }

        /// <summary>
        /// Gets the type of the data source that will be used by the provider.
        /// </summary>
        public DataSourceType DataSourceType
        {
            get { return data_source_; }
        }

        /// <summary>
        /// Gets the name of the owner of the database
        /// </summary>
        public string DatabaseOwner {
            get { return database_owner_; }
        }

        /// <summary>
        /// Gets a connection string taht can be used to open the database.
        /// </summary>
        public string ConnectionString {
            get { return connection_string_; }
        }
    }
}