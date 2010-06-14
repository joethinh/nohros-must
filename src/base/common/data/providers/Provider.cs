using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace Nohros.Data
{
    public enum ExtensionType
    {
        Security,
        Encription,
        Unknown
    }

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
        string _name;
        string _type;
        ExtensionType _extensionType;
        ConfigurationRepository _configRepository;
        DataSourceType _dataSource;
        NameValueCollection _attributes;
        bool _isEncrypted = false;

        #region .ctor

        /// <summary>
        /// Default constructor.
        /// </summary>
        Provider()
        {
            _attributes = new NameValueCollection();
            _extensionType = ExtensionType.Unknown;

            // default for compatibility with legacy applications
            _configRepository = ConfigurationRepository.ConfigurationFile;
            _dataSource = DataSourceType.Unknown;
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
            _name = attributes["name"].Value;
            _type = attributes["type"].Value;

            // Set the extensionType of the provider
            XmlAttribute attribute = attributes["extensionType"];
            if (attribute != null)
            {
                try
                {
                    _extensionType = (ExtensionType)Enum.Parse(typeof(ExtensionType), attribute.Value, true);
                }
                catch
                {
                    _extensionType = ExtensionType.Unknown;
                }
            }

            // Set the configuration repository of the provider
            attribute = attributes["configRepository"];
            if (attribute != null)
            {
                try
                {
                    _configRepository = (ConfigurationRepository)Enum.Parse(typeof(ConfigurationRepository), attribute.Value, true);
                }
                catch
                {
                    _configRepository = ConfigurationRepository.Unknown;
                }
            }

            attribute = attributes["dataSourceType"];
            if (attribute != null)
            {
                switch (attribute.Value.ToLower())
                {
                    case "mssql":
                        _dataSource = DataSourceType.MsSql;
                        break;
                    case "oledb":
                        _dataSource = DataSourceType.OleDb;
                        break;
                    case "odbc":
                        _dataSource = DataSourceType.Odbc;
                        break;
                    default:
                        break;
                }
            }

            // Store all the attributes in the attributes bucket
            for (int i = 0, j = attributes.Count; i < j; i++)
            {
                attribute = attributes[i];
                if ((attribute.Name != "name") && (attribute.Name != "type"))
                {
                    _attributes.Add(attribute.Name, attribute.Value);
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

            // if the key does not have values the provider configuration is invalid
            // we can't throw an exception here because this class could be hosted
            // by a windows service.
            if (atts == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_Provider_Attributes);

            // Set the name of the provider
            _name = (string)attributes.GetValue("Name", null);
            _type = (string)attributes.GetValue("Type", null);
            if (_name == null || _type == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_Provider_Attributes);

            // Set the extensionType of the provider
            string attribute = (string)attributes.GetValue("ExtensionType", null);
            if (attribute != null)
            {
                try
                {
                    _extensionType = (ExtensionType)Enum.Parse(typeof(ExtensionType), attribute, true);
                }
                catch
                {
                    _extensionType = ExtensionType.Unknown;
                }
            }

            // Set the configuration repository of the provider
            attribute = (string)attributes.GetValue("configRepository", null);
            if (attribute != null)
            {
                try
                {
                    _configRepository = (ConfigurationRepository)Enum.Parse(typeof(ConfigurationRepository), attribute, true);
                }
                catch
                {
                    _configRepository = ConfigurationRepository.Unknown;
                }
            }

            // Store all the attributes in the attributes bucket
            for (int i = 0, j = atts.Length; i < j; i++)
            {
                attribute = atts[i];
                if ((attribute != "Name") && (attribute != "Type"))
                {
                    _attributes.Add(attribute, (string)attributes.GetValue(attribute, string.Empty));
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets the attributes of the Provider
        /// </summary>
        public NameValueCollection Attributes
        {
            get { return _attributes; }
        }

        public ExtensionType ExtensionType
        {
            get { return _extensionType; }
        }

        /// <summary>
        /// Gets a value indicating where the provider is valid or not.
        /// </summary>
        [Obsolete("This class will throw an exception when it is invalid. This property will be removed on future versions.")]
        public bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the name of the Provider
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the type of the Provider
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the repository used to store the configuration values.
        /// </summary>
        public ConfigurationRepository ConfigurationRepository
        {
            get { return _configRepository; }
        }

        /// <summary>
        /// Gets the type of the data source that will be used by the provider.
        /// </summary>
        public DataSourceType DataSourceType
        {
            get { return _dataSource; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a connection string is encrypted
        /// </summary>
        public bool IsEncrypted {
            get { return _isEncrypted; }
            set { _isEncrypted = value; }
        }
    }
}