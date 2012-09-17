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
    public bool GetProvidersNodeGroup(string name, out IProvidersNodeGroup node) {
      return GetChildNode(name, out node);
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
  }
}
