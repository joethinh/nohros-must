using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
    /// <summary>
    /// A basic implementation of the <see cref="IConfigurationNode"/> interface.
    /// </summary>
    public abstract class ConfigurationNode: IConfigurationNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        protected string name_;

        Dictionary<string, ConfigurationNode> child_nodes_;
        ConfigurationNode parent_node_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the ConfigurationNode class by using the specified XML node.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        public ConfigurationNode(string name) {
            name_ = name;
            parent_node_ = null;
            child_nodes_ = new Dictionary<string, ConfigurationNode>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Initializes a nes instance_ of the ConfigurationNode class by using the specified XML node.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="parent_node">The parent node.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent_node"/>is null.</exception>
        public ConfigurationNode(string name, ConfigurationNode parent_node): this(name) {
            if (parent_node == null)
                throw new ArgumentNullException("parent_node");

            parent_node_ = parent_node;
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
        protected bool GetAttributeValue(XmlNode node, string name, out string value) {
            if (node == null || name == null)
                throw new ArgumentNullException((node == null) ? "node" : "name");

            XmlAttribute att = node.Attributes[name];
            value = null;
            if (att != null)
                value = att.Value;
            return (value != null);
        }

        /// <summary>
        /// Parses the related XML node.
        /// </summary>
        public abstract void Parse(XmlNode node);

        /// <summary>
        /// Gets a node with the specified name that is a child of the current node.
        /// </summary>
        /// <param name="name">A string specifying the name  of the node. The name is case-insensitive.</param>
        /// <param name="node">When this method returns contains a node with the specified name or null if the node name
        /// was not found within the child nodes.</param>
        /// <returns>true if the a node with the specified name was found within the child nodes; otherwise false.</returns>
        internal bool GetChildNode(string name, out ConfigurationNode node) {
            node = this[name];
            return (node != null);
        }

        /// <summary>
        /// Gets a list of all child nodes of the node.
        /// </summary>
        internal List<ConfigurationNode> ChildNodes {
            get {
                List<ConfigurationNode> nodes = new List<ConfigurationNode>(child_nodes_.Count);
                foreach(KeyValuePair<string, ConfigurationNode> node in child_nodes_) {
                    nodes.Add(node.Value);
                }
                return nodes;
            }
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name {
            get { return name_; }
        }

        /// <summary>
        /// Gets the parent of this node(for nodes that can have parents).
        /// </summary>
        public ConfigurationNode ParentNode {
            get { return parent_node_; }
        }

        /// <summary>
        /// Gets a node with the specified name that is a child of the current node.
        /// </summary>
        /// <param name="name">A string specifying the name  of the node. The name is case-insensitive.</param>
        /// <returns>A child node with the specified name or null if the node could not be found.</returns>
        internal ConfigurationNode this[string name] {
            get {
                ConfigurationNode node = null;
                child_nodes_.TryGetValue(name, out node);
                return node;
            }
            set { child_nodes_[name] = value; }
        }
    }
}
