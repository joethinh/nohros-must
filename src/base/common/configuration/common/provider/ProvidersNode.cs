using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="ProvidersNode"/> is a collection of
  /// <see cref="ProviderNode"/> objects.
  /// </summary>
  public partial class ProvidersNode : AbstractHierarchicalConfigurationNode,
                                       IProvidersNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidersNode"/> class.
    /// </summary>
    public ProvidersNode() : base(Strings.kProvidersNodeName) {
    }
    #endregion

    /// <inheritdoc/>
    public void Add(IProvidersNodeGroup node) {
      AddChildNode(node);
    }

    /// <inheritdoc/>
    public bool GetProvidersNodeGroup(string name, out IProvidersNodeGroup node) {
      if (!GetChildNode(name, out node)) {
        node = new ProvidersNodeGroup();
        return false;
      }
      return true;
    }

    /// <inheritdoc/>
    public IProvidersNodeGroup GetProvidersNodeGroup(string name) {
      return
        GetChildNode<IProvidersNodeGroup>(name);
    }

    /// <inheritdoc/>
    IProvidersNodeGroup IProvidersNode.this[string name] {
      get { return GetProvidersNodeGroup(name); }
    }

    public IEnumerator<IProvidersNodeGroup> GetEnumerator() {
      foreach (IConfigurationNode node in ChildNodes) {
        yield return (IProvidersNodeGroup) node;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/> and is not associated with any group.
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="IProviderNode"/> to retrieve.
    /// </param>
    /// <remarks>
    /// This methos is a shortcut of
    /// the most common variation of the
    /// <see cref="IProvidersNodeGroup.GetProviderNode(string)"/> method.
    /// </remarks>
    public IProviderNode GetProviderNode(string name) {
      return
        GetProvidersNodeGroup(string.Empty)
          .GetProviderNode(name);
    }

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/> and is not associated with any group.
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="IProviderNode"/> to retrieve.
    /// </param>
    /// <remarks>
    /// This methos is a shortcut of
    /// the most common variation of the
    /// <see cref="IProvidersNodeGroup.GetProviderNode(string)"/> method.
    /// </remarks>
    public bool GetProviderNode(string name, out IProviderNode node) {
      IProvidersNodeGroup group;
      if (GetProvidersNodeGroup(string.Empty, out group)) {
        return group.GetProviderNode(name, out node);
      }
      node = default(IProviderNode);
      return false;
    }
  }
}
