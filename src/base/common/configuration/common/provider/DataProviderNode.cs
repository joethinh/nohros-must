using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Resources;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// Contains configuration informations for data providers.
  /// </summary>
  public class DataProviderNode: ProviderNode
  {
    const string kDataBaseOwnerKey = "database-owner";
    const string kConnectionStringKey = "connection-string";
    const string kIsEncryptedKey = "encrypted";
    const string kAssemblyLocationKey = "assembly-location";
    const string kDataSourceTypeKey = "data-source-type";
    const string kTypeKey = "type";

    string database_owner_;
    string connection_string_;
    bool connstring_is_encrypted_;

    DataSourceType data_source_;
    NameValueCollection attributes_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the DataProviderNode.
    /// </summary>
    /// <param name="name">The name of the data provider.</param>
    /// <param name="type">The assembly-qualified type of the provider.</param>
    public DataProviderNode(string name, string type)
      : base(name, type) {
      database_owner_ = "dbo";
      connection_string_ = null;
      connstring_is_encrypted_ = false;
    }
    #endregion

    /// <summary>
    /// Parses a XML node that contains information about a data provider.
    /// </summary>
    /// <param name="node">The XML node to parse.</param>
    /// <param name="location_base_path">A string repreenting the base path of the location of the
    /// provider.
    /// </param>
    /// <param name="nodes">A <see cref="DictionaryValue&lt;IConfigurationNode&gt;"/> that can be
    /// used to store the parsed connection string could be stored or resolve referenced connection
    /// strings.</param>
    /// <exception cref="System.Configuration.ConfigurationErrorsException">The
    /// <paramref name="node"/> is not a valid representation of a data provider.</exception>
    /// <exception cref="ArgumentException">The <see cref="base_path"/> is not rooted.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="Node"/> or
    /// <paramref name="base_path"/> is a null reference.</exception>
    /// The location attribute of a provider could be absolute or relative. When it is relative we
    /// it will be resolved by using the specified <paramref name="location_base_path"/> path. An empty
    /// location will be resolved to the specified <paramref name="location_base_path"/> path.
    /// </remarks>
    public override void Parse(XmlNode node, string location_base_path) {

      if (node == null || location_base_path == null)
        throw new ArgumentNullException((node == null) ? "node" : "location_base_path");

      InternalParse(node, location_base_path);

      string attribute;

      // "connection-string" attribute is mandatory.
      if (!GetAttributeValue(node, kConnectionStringKey, out connection_string_))
        throw new ConfigurationErrorsException(StringResources.DataProvider_Provider_Attributes);

      // get the "database-owner" attribute
      if (!GetAttributeValue(node, kDataBaseOwnerKey, out database_owner_))
        database_owner_ = "dbo";

      // get the "encrypted" attribute
      GetAttributeValue(node, kIsEncryptedKey, out attribute);
      connstring_is_encrypted_ = (string.Compare("true", attribute, StringComparison.OrdinalIgnoreCase) == 0) ? true : false;

      // get the "data-source-type" attribute
      GetAttributeValue(node, kDataSourceTypeKey, out attribute);
      DataHelper.ParseStringEnum<DataSourceType>(attribute, DataSourceType.Unknown);
    }

    /// <summary>
    /// Resolve references to connection strings.
    /// </summary>
    /// <param name="connection_string_node">A <see cref="ConnectionStringNode"/> containing
    /// all the configured connection strings.</param>
    /// <remarks>
    /// To avoid write a connection string more than once on the configuration file, data provider
    /// connections strings and database owners could be specified as a reference to a global defined
    /// connection string.
    /// <para>
    /// This method must be called immediatelly after the Parse method is executed.
    /// </para>
    /// </remarks>
    public void ResolveReferences(DictionaryValue<ConnectionStringNode> connection_string_nodes) {
      // if the connection string node is a reference to a global value, we need to resolve it.
      ConnectionStringNode dbstring_node =
          connection_string_nodes[connection_string_];

      if (dbstring_node != null) {
        database_owner_ = dbstring_node.DatabaseOwner;
        connection_string_ = (connstring_is_encrypted_) ?
            NSecurity.BasicDeCryptoString(
                dbstring_node.ConnectionString) : dbstring_node.ConnectionString;
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