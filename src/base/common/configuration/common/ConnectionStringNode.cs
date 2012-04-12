using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  public class ConnectionStringNode: AbstractConfigurationNode
  {
    const string kDataBaseOwnerAttributeName = "database-owner";
    const string kConnectionStringAttributeName = "connection-string";

    string database_owner_;
    string connection_string_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ConnectionStringNode by using the connection string node name.
    /// </summary>
    public ConnectionStringNode(string name) : base(name) { }
    #endregion

    /// <summary>
    /// Parses a XML node that contains information about a connection string node.
    /// </summary>
    /// <param name="node">A XML node containing the data to parse.</param>
    /// <param name="config">The configuration object which this node belongs to.</param>
    /// <remarks>
    /// This method will try to extract the database owner and connection string from the specified
    /// XML node. If the data could not be retrieved attempt to get the value of the
    /// <see cref="DatabaseOwner"/> property or <see cref="ConnectionString"/> property will returns
    /// a empty string.
    /// </remarks>
    public void Parse(XmlNode node) {
      if (!GetAttributeValue(node, kDataBaseOwnerAttributeName, out database_owner_)) {
        database_owner_ = string.Empty;
      }

      if (!GetAttributeValue(node, kConnectionStringAttributeName, out connection_string_)) {
        connection_string_ = string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the database owner related with the connection string.
    /// </summary>
    public string DatabaseOwner {
      get { return database_owner_; }
    }

    /// <summary>
    /// Gets a string that can be used to open a database connection.
    /// </summary>
    public string ConnectionString {
      get { return connection_string_; }
    }
  }
}
