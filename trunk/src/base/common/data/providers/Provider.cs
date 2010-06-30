using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace Nohros.Data
{
    public enum ConfigurationRepository
    {
        WindowsRegistry = 0,
        ConfigurationFile = 1,
        Unknown = 2
    }

    public enum DataSourceType
    {
        MsSql = 0,
        OleDb = 1,
        Odbc = 2,
        Unknown = 100
    }

    public class Provider
    {
        string name_;
        string type_;
        ConfigurationRepository config_repository_;
        DataSourceType data_source_;
        NameValueCollection attributes_;
        bool is_encrypted_;

        #region .ctor

        /// <summary>
        /// Default constructor.
        /// </summary>
        Provider()
        {
            attributes_ = new NameValueCollection();
            is_encrypted_ = false;

            // default for compatibility with legacy applications
            config_repository_ = ConfigurationRepository.ConfigurationFile;
            data_source_ = DataSourceType.Unknown;
        }

        /// <summary>
        /// Initializes a new instanceof the <see cref="Provider"/> class by using
        /// the specified attributes
        /// </summary>
        /// <param name="attributes">A <see cref="XmlAttributeCollection"/> containing
        /// the attributes of the provider</param>
        /// <remarks>If the <paramref name="attributes"/> argumentdoes not have
        /// a definition for the configReposirory attribute, this constructor will
        /// set the <see cref="Provider.ConfigurationRepository"/> property to
        /// <see cref="ConfigurationRepository.ConfigurationFile"/></remarks>
        public Provider(XmlAttributeCollection attributes):this()
        {
            // Set the name and type of the provider
            name_ = attributes["name"].Value;
            type_ = attributes["type"].Value;

            // Set the configuration repository of the provider
            XmlAttribute attribute = attributes["configRepository"];
            if (attribute != null) {
                try {
                    config_repository_ = (ConfigurationRepository)Enum.Parse(typeof(ConfigurationRepository), attribute.Value, true);
                }
                catch {
                    config_repository_ = ConfigurationRepository.Unknown;
                }
            }

            attribute = attributes["dataSourceType"];
            if (attribute != null)
            {
                switch (attribute.Value.ToLower())
                {
                    case "mssql":
                        data_source_ = DataSourceType.MsSql;
                        break;
                    case "oledb":
                        data_source_ = DataSourceType.OleDb;
                        break;
                    case "odbc":
                        data_source_ = DataSourceType.Odbc;
                        break;
                    default:
                        break;
                }
            }

            // store all the attributes in the attributes bucket
            for (int i = 0, j = attributes.Count; i < j; i++) {
                attribute = attributes[i];
                if ((attribute.Name != "name") && (attribute.Name != "type")) {
                    attributes_.Add(attribute.Name, attribute.Value);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Provider"/> class by using
        /// the specified <see cref="RegistryKey"/> like a attributes repository.
        /// </summary>
        /// <param name="attributes">A key-level node in the windows registry containing
        /// the attributes for the provider.</param>
        /// <remarks>If the <paramref name="attributes"/> argument does not have
        /// a definition for the configReposirory attribute, this constructor will
        /// set the <see cref="Provider.ConfigurationRepository"/> property to
        /// <see cref="ConfigurationRepository.ConfigurationFile"/></remarks>
        public Provider(RegistryKey attributes):this()
        {
            string[] atts = attributes.GetValueNames();

            if (atts == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_Provider_Attributes);

            // Set the name of the provider
            name_ = (string)attributes.GetValue("name", null);
            type_ = (string)attributes.GetValue("type", null);
            if (name_ == null || type_ == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_Provider_Attributes);

            // set the configuration repository of the provider
            string attribute = (string)attributes.GetValue("configRepository", null);
            if (attribute != null) {
                try {
                    config_repository_ = (ConfigurationRepository)Enum.Parse(typeof(ConfigurationRepository), attribute, true);
                }
                catch {
                    config_repository_ = ConfigurationRepository.Unknown;
                }
            }

            // store all the attributes in the attributes bucket
            for (int i = 0, j = atts.Length; i < j; i++) {
                attribute = atts[i].ToLower();
                if ((attribute != "name") && (attribute != "type")) {
                    attributes_.Add(attribute, (string)attributes.GetValue(attribute, string.Empty));
                }
            }
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
        public ConfigurationRepository ConfigurationRepository
        {
            get { return config_repository_; }
        }

        /// <summary>
        /// Gets the type of the data source that will be used by the provider.
        /// </summary>
        public DataSourceType DataSourceType
        {
            get { return data_source_; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a connection string is encrypted or not.
        /// </summary>
        /// <remarks>The connection string must be encripted with the <see cref="BasicCryptoString(string)"/>
        /// method of the <see cref="Nohros.NSecurity"/> class.</remarks>
        public bool IsEncrypted {
            get { return is_encrypted_; }
            set { is_encrypted_ = value; }
        }
    }
}