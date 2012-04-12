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
  public abstract class AbstractConfigurationNode : Value, IConfigurationNode
  {
    Dictionary<string, AbstractConfigurationNode> child_nodes_;

    /// <summary>
    /// The name of the configuration node.
    /// </summary>
    protected string name;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the AbstractConfigurationNode class by using
    /// the specified XML node.
    /// </summary>
    /// <param name="name">The name of the node.</param>
    protected AbstractConfigurationNode(string name)
      : base(Nohros.ValueType.Class) {
      this.name = name;
      child_nodes_ = new Dictionary<string, AbstractConfigurationNode>(
        StringComparer.OrdinalIgnoreCase);
    }
    #endregion

    #region IConfigurationNode Members
    /// <summary>
    /// Gets the name of the node.
    /// </summary>
    public string Name {
      get { return name; }
      internal set { name = value; }
    }
    #endregion

    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="node">
    /// The node that contains the attribute.
    /// </param>
    /// <param name="name">
    /// The name of the attribute.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the value of the attribute or
    /// <c>null</c> if the value could not be retrieved.</param>
    /// <returns>
    /// <c>true</c> if the atribbute retrieval operation is successful;
    /// otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method returns <c>false</c> when the type of the given xml
    /// node is not equals to <see cref="XmlNodeType.Element"/>.
    /// </remarks>
    public static bool GetAttributeValue(XmlNode node, string name,
      out string value) {
      return GetAttributeValue(node, name, false, out value);
    }

    /// <summary>
    /// Gets the trimmed value of an attribute of a xml node.
    /// </summary>
    /// <param name="node">
    /// The node that contains the attribute.
    /// </param>
    /// <param name="name">
    /// The name of the attribute.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the trimmed value of the attribute
    /// or <c>null</c> if the value could not be retrieved.
    /// </param>
    /// <returns>true if the atribbute retrieval operation is successful;
    /// otherwise false.</returns>
    public static bool GetTrimmedAttributeValue(XmlNode node, string name,
      out string value) {
      return GetAttributeValue(node, name, true, out value);
    }

    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="node">
    /// The node that contains the attribute.
    /// </param>
    /// <param name="name">
    /// The name of the attribute.
    /// </param>
    /// <param name="trim_value">
    /// A value indicating if the attribute value should be trimmed.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the trimmed value of the attribute
    /// or <c>null</c> if the value could not be retrieved.
    /// </param>
    /// <returns></returns>
    static bool GetAttributeValue(XmlNode node, string name, bool trim_value,
      out string value) {
      if (node == null || name == null) {
        throw new ArgumentNullException((node == null) ? "node" : "name");
      }

      // The attribute is defined only for xml element nodes.
      if (node.NodeType != XmlNodeType.Element) {
        value = string.Empty;
        return false;
      }

      // A Xml node of type "Element" always have its attributes property
      // defined, we do not need to check for null.
      XmlAttribute att = node.Attributes[name];
      value = null;
      if (att != null) {
        value = (trim_value) ? att.Value.Trim() : att.Value;
      }
      return (value != null);
    }

    /// <summary>
    /// Gets a node with the specified name that is a child of the current node.
    /// </summary>
    /// <param name="name">
    /// A string specifying the name  of the node. The name is case-insensitive.
    /// </param>
    /// <param name="node">
    /// When this method returns contains a node with the specified name or
    /// <c>null</c> if the node name was not found within the child nodes.
    /// </param>
    /// <returns>
    /// <c>true</c> if the a node with the specified name was found; otherwise,
    /// false.
    /// </returns>
    protected bool GetChildNode(string name, out AbstractConfigurationNode node) {
      node = this[name];
      return (node != null);
    }

    /// <summary>
    /// Selects the first sibling <see cref="XmlNode"/> from the specified
    /// parent node that matches the given name and, if a node is found,
    /// creates a new <see cref="DictionaryValue{T}"/> object and add it to the
    /// <paramref name="nodes"/> collection.
    /// </summary>
    protected static bool GetNode<T>(string node_xpath, string nodes_tree,
      DictionaryValue nodes, XmlNode node, out XmlNode root_node,
      out DictionaryValue<T> nodes_dictionary)
      where T : AbstractConfigurationNode {
      if (node_xpath == null || nodes_tree == null || nodes == null) {
        throw new ArgumentNullException(node_xpath == null
          ? "node_xpath"
          : nodes_tree == null ? "nodes_tree" : "nodes");
      }

      root_node = IConfiguration.SelectNode(node, node_xpath);
      if (root_node != null) {
        nodes_dictionary = new DictionaryValue<T>();
        nodes[nodes_tree] = (IValue) nodes_dictionary;
        return true;
      }
      nodes_dictionary = null;
      return false;
    }

    /// <summary>
    /// Gets a list of all child nodes of the node.
    /// </summary>
    protected List<AbstractConfigurationNode> ChildNodes {
      get {
        List<AbstractConfigurationNode> nodes =
          new List<AbstractConfigurationNode>(child_nodes_.Count);
        foreach (
          KeyValuePair<string, AbstractConfigurationNode> node in child_nodes_) {
          nodes.Add(node.Value);
        }
        return nodes;
      }
    }

    /// <summary>
    /// Gets a node with the specified name that is a child of the current node.
    /// </summary>
    /// <param name="name">
    /// A string specifying the name  of the node. The name is case-insensitive.
    /// </param>
    /// <returns>
    /// A child node with the specified name or <c>null</c> if the node is not
    /// found.
    /// </returns>
    protected AbstractConfigurationNode this[string name] {
      get {
        AbstractConfigurationNode node = null;
        child_nodes_.TryGetValue(name, out node);
        return node;
      }
      set { child_nodes_[name] = value; }
    }
  }
}
