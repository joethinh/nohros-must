using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Data;
using Nohros.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// A basic implementation of the <see cref="IConfigurationNode"/> interface.
  /// </summary>
  public abstract class ConfigurationNode: Value, IConfigurationNode
  {
    /// <summary>
    /// The name of the node.
    /// </summary>
    protected string name_;

    Dictionary<string, ConfigurationNode> child_nodes_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the ConfigurationNode class by using
    /// the specified XML node.
    /// </summary>
    /// <param name="name">The name of the node.</param>
    public ConfigurationNode(string name)
      : base(Nohros.ValueType.Class) {
      name_ = name;
      child_nodes_ = new Dictionary<string, ConfigurationNode>(
        StringComparer.OrdinalIgnoreCase);
    }
    #endregion

    /// <summary>
    /// Gets the value of an attribute of a xml node.
    /// </summary>
    /// <param name="node">The node that contains the attribute.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="value">When this method returns contains the value of the
    /// attribute or null if the value could not be retrieved.</param>
    /// <returns>true if the atribbute retrieval operation is successful;
    /// otherwise false.</returns>
    public static bool GetAttributeValue(XmlNode node, string name,
      out string value) {
      if (node == null || name == null)
        throw new ArgumentNullException((node == null) ? "node" : "name");

      XmlAttribute att = node.Attributes[name];
      value = null;
      if (att != null)
        value = att.Value;
      return (value != null);
    }

    /// <summary>
    /// Gets the trimmed value of an attribute of a xml node.
    /// </summary>
    /// <param name="node">The node that contains the attribute.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="value">When this method returns contains the trimmed
    /// value of the attribute or null if the value could not be retrieved.
    /// </param>
    /// <returns>true if the atribbute retrieval operation is successful;
    /// otherwise false.</returns>
    public static bool GetTrimmedAttributeValue(XmlNode node, string name,
      out string value) {
      XmlAttribute att = node.Attributes[name];
      value = null;
      if (att != null) {
        value = att.Value.Trim();
        return true;
      }
      return false;
    }

    /// <summary>
    /// Parses a XML node that contains information about a configuration node.
    /// </summary>
    /// <param name="node">A XML node containing the data to parse.</param>
    /// <param name="config">The configuration object which this node belongs
    /// to.</param>
    /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
    /// valid representation of a configuration node.</exception>
    //public abstract void Parse(XmlNode node, DictionaryValue nodes);

    /// <summary>
    /// Gets a node with the specified name that is a child of the current node.
    /// </summary>
    /// <param name="name">A string specifying the name  of the node. The name
    /// is case-insensitive.
    /// </param>
    /// <param name="node">When this method returns contains a node with the
    /// specified name or null if the node name was not found within the child
    /// nodes.</param>
    /// <returns>true if the a node with the specified name was found within
    /// the child nodes; otherwise false.</returns>
    protected bool GetChildNode(string name, out ConfigurationNode node) {
      node = this[name];
      return (node != null);
    }

    /// <summary>
    /// Selects the first sibling <see cref="XmlNode"/> of the specified node
    /// that matches the specified name and, if a node is found, creates a new
    /// <see cref="DictionaryValue&lt;T&gt;"/> and adds it to the speciifed
    /// nodes collection.
    /// </summary>
    protected static bool GetNode<T>(string node_xpath, string nodes_tree,
      DictionaryValue nodes, XmlNode node, out XmlNode root_node,
        out DictionaryValue<T> nodes_dictionary) where T: ConfigurationNode {

#if DEBUG
      if (node_xpath == null)
        throw new ArgumentNullException("node_xpath");

      if (nodes_tree == null)
        throw new ArgumentNullException("nodes_tree");

      if (nodes == null)
        throw new ArgumentNullException("nodes");
#endif

      root_node = IConfiguration.SelectNode(node, node_xpath);
      if (root_node != null) {
        nodes_dictionary = new DictionaryValue<T>();
        nodes[nodes_tree] = (IValue)nodes_dictionary;
        return true;
      }
      nodes_dictionary = null;
      return false;
    }

    /// <summary>
    /// Gets a list of all child nodes of the node.
    /// </summary>
    protected List<ConfigurationNode> ChildNodes {
      get {
        List<ConfigurationNode> nodes = new List<ConfigurationNode>(child_nodes_.Count);
        foreach (KeyValuePair<string, ConfigurationNode> node in child_nodes_) {
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
      internal set { name_ = value; }
    }

    /// <summary>
    /// Gets a node with the specified name that is a child of the current node.
    /// </summary>
    /// <param name="name">A string specifying the name  of the node. The name is case-insensitive.</param>
    /// <returns>A child node with the specified name or null if the node could not be found.</returns>
    protected ConfigurationNode this[string name] {
      get {
        ConfigurationNode node = null;
        child_nodes_.TryGetValue(name, out node);
        return node;
      }
      set { child_nodes_[name] = value; }
    }
  }
}
