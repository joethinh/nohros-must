using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
    public class ConnectionStringNode : ConfigurationNode
    {
        string database_owner_;
        string connection_string_;

        const string kDataBaseOwnerAttributeName = "dbowner";
        const string kConnectionStringAttributeName = "dbstring";

        internal const string kNodeTree = CommonNode.kNodeTree + "connection-strings.";

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ConnectionStringNode.
        /// </summary>
        public ConnectionStringNode(string name, ConfigurationNode parent_node) : base(name, parent_node) { }
        #endregion

        /// <summary>
        /// Parse the XML node.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <remarks>
        /// This method will try to extract the database owner and connection string from the specified XML node.
        /// If the data could not be retrieved attempt to get the database owner or connection string returns a null
        /// reference.
        /// </remarks>
        public override void Parse(XmlNode node) {
            GetAttributeValue(node, kDataBaseOwnerAttributeName, out database_owner_);
            GetAttributeValue(node, kConnectionStringAttributeName, out connection_string_);
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
