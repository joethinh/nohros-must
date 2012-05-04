using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Nohros.Configuration
{
  /// <summary>
  /// A implementation of the <see cref="IConfigurationNode"/> interface that
  /// can contains child nodes.
  /// </summary>
  public abstract class AbstractHierarchicalConfigurationNode :
    AbstractConfigurationNode
  {
    /// <summary>
    /// A dictionary containing the child nodes.
    /// </summary>
    readonly IDictionary<string, IConfigurationNode> child_nodes_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AbstractHierarchicalConfigurationNode"/> class that use a
    /// <see cref="Dictionary{TKey,TValue}"/> to store the child nodes and could
    /// be identified by using the string <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the node, within a single
    /// configuration object.
    /// </param>
    protected AbstractHierarchicalConfigurationNode(string name)
      : this(name, new Dictionary<string, IConfigurationNode>(
        StringComparer.OrdinalIgnoreCase)) {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AbstractHierarchicalConfigurationNode"/> class that use a
    /// specified <see cref="Dictionary{TKey,TValue}"/> to store the child
    /// nodes and could be identified through the string
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the node, within a single
    /// configuration object.
    /// </param>
    /// <param name="child_nodes_container">
    /// A <see cref="IDictionary{TKey,TValue}"/> that is used to store the
    /// child nodes.
    /// </param>
    protected AbstractHierarchicalConfigurationNode(string name,
      IDictionary<string, IConfigurationNode> child_nodes_container)
      : base(name) {
      child_nodes_ = child_nodes_container;
    }
    #endregion

    /// <summary>
    /// Adds the specified node to the internal collection of
    /// <see cref="AbstractHierarchicalConfigurationNode"/>.
    /// </summary>
    /// <param name="node">
    /// An <see cref="AbstractHierarchicalConfigurationNode"/> to be added to
    /// the child nodes collection.
    /// </param>
    protected virtual void AddChildNode(IConfigurationNode node) {
      child_nodes_.Add(node.Name, node);
    }

    /// <summary>
    /// Gets a node with the specified name that is a child of the current node.
    /// </summary>
    /// <param name="child_node_name">
    /// A string specifying the name of the child node.
    /// </param>
    /// <param name="node">
    /// When this method returns contains a node with the specified name or
    /// <c>null</c> if the node name was not found within the child nodes.
    /// </param>
    /// <returns>
    /// <c>true</c> if the a node with the specified name was found; otherwise,
    /// <c>false</c>.
    /// </returns>
    protected virtual bool GetChildNode<T>(string child_node_name,
      out T node) where T : class, IConfigurationNode {
      node = this[child_node_name] as T;
      return (node != null);
    }

    /// <summary>
    /// Gets a child node whose name is <paramref name="child_node_name"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The node's type.
    /// </typeparam>
    /// <param name="child_node_name">
    /// The name of the node to get.
    /// </param>
    /// <returns>
    /// A node whose type is <typeparamref name="T"/> and name is
    /// <paramref name="child_node_name"/>.
    /// </returns>
    protected virtual T GetChildNode<T>(string child_node_name)
      where T : class, IConfigurationNode {
      T node = this[child_node_name] as T;
      if (node == null) {
        throw new KeyNotFoundException(child_node_name);
      }
      return node;
    }

    /// <summary>
    /// Gets a list of all child nodes of the node.
    /// </summary>
    protected virtual List<IConfigurationNode> ChildNodes {
      get {
        List<IConfigurationNode> nodes =
          new List<IConfigurationNode>(child_nodes_.Count);
        foreach (
          KeyValuePair<string, IConfigurationNode> node in
            child_nodes_) {
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
    protected virtual IConfigurationNode this[string name] {
      get {
        IConfigurationNode node = null;
        child_nodes_.TryGetValue(name, out node);
        return node;
      }
      set { child_nodes_[name] = value; }
    }
  }
}
