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

    #region IProvidersNode Members
    /// <inheritdoc/>
    public bool GetProviderNode(string simple_provider_name,
      out IProviderNode simple_provider) {
      return GetProviderNode(simple_provider_name, string.Empty,
        out simple_provider);
    }

    /// <inheritdoc/>
    public bool GetProviderNode(string simple_provider_name,
      string simple_provider_group, out IProviderNode simple_provider) {
      return
        GetChildNode(
          ProviderKey(simple_provider_name, simple_provider_group),
          out simple_provider);
    }

    /// <inheritdoc/>
    public IProviderNode GetProviderNode(string name) {
      return GetProviderNode(name, string.Empty);
    }

    /// <inheritdoc/>
    public IProviderNode GetProviderNode(string name, string group) {
      return
        GetChildNode<IProviderNode>(ProviderKey(name, group));
    }

    /// <inheritdoc/>
    IProviderNode IProvidersNode.this[string name] {
      get { return GetProviderNode(name); }
    }

    /// <inheritdoc/>
    public IProviderNode[] GetProvidersNode(string group) {
      List<IProviderNode> providers = new List<IProviderNode>(ChildNodes.Count);
      foreach (IConfigurationNode node in ChildNodes) {
        IProviderNode provider = node as IProviderNode;
        if (StringsAreEquals(provider.Group, group)) {
          providers.Add(provider);
        }
      }
      return providers.ToArray();
    }

    /// <inheritdoc/>
    IProviderNode IProvidersNode.this[
      string provider_name, string provider_group] {
      get { return GetProviderNode(provider_name, provider_group); }
    }

    public IEnumerator<IProviderNode> GetEnumerator() {
      foreach (IConfigurationNode node in ChildNodes) {
        yield return (IProviderNode) node;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
    #endregion

    static string ProviderKey(string name, string group) {
      return "group:" + group + ",name:" + name;
    }
  }
}
