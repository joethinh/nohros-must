using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A simple implementation of the <see cref="IProvidersNodeGroup"/> class.
  /// </summary>
  public class ProvidersNodeGroup : AbstractHierarchicalConfigurationNode,
                                    IProvidersNodeGroup
  {
    readonly string group_;

    /// <summary>
    /// Initializes a new instance of the <see cref="IProvidersNodeGroup"/>
    /// that is not associated with any group.
    /// </summary>
    public ProvidersNodeGroup()
      : this(string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IProvidersNodeGroup"/>
    /// class that contains providers associated with a group whose name is
    /// <paramref name="group"/>.
    /// </summary>
    public ProvidersNodeGroup(string group) : base(group) {
      group_ = group;
    }

    /// <inheritdoc/>
    public void Add(IProviderNode node) {
      AddChildNode(node);
    }

    /// <inheritdoc/>
    public void Add(string name, IProviderNode node) {
      AddChildNode(name, node);
    }

    /// <inheritdoc/>
    public IProviderNode GetProviderNode(string name) {
      return GetChildNode<IProviderNode>(name);
    }

    /// <inheritdoc/>
    public bool GetProviderNode(string name, out IProviderNode provider) {
      return GetChildNode(name, out provider);
    }

    /// <inheritdoc/>
    public IProviderNode[] GetProvidersNode(string name) {
      List<IProviderNode> nodes = new List<IProviderNode>(ChildNodes.Count);
      foreach (IProviderNode node in ChildNodes) {
        nodes.Add(node);
      }
      return nodes.ToArray();
    }

    /// <inheritdoc/>
    IProviderNode IProvidersNodeGroup.this[string name] {
      get { return GetProviderNode(name); }
    }

    /// <inheritdoc/>
    public IEnumerator<IProviderNode> GetEnumerator() {
      foreach (IConfigurationNode node in ChildNodes) {
        yield return (IProviderNode) node;
      }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public void AddRange(IEnumerable<IProviderNode> nodes) {
      foreach (var node in nodes) {
        Add(node);
      }
    }
  }
}
