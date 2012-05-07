using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="ProvidersNode"/> is a collection of
  /// <see cref="ProviderNode"/> objects.
  /// </summary>
  public partial class ProvidersNode: AbstractHierarchicalConfigurationNode, IProvidersNode
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidersNode"/> class.
    /// </summary>
    public ProvidersNode() : base(Strings.kProvidersNodeName) { }

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
    public IProviderNode GetProviderNode(string simple_provider_name) {
      return GetProviderNode(simple_provider_name, string.Empty);
    }

    /// <inheritdoc/>
    public IProviderNode GetProviderNode(
      string simple_provider_name, string simple_provider_group) {
      return
        base[ProviderKey(simple_provider_name, simple_provider_group)] as
          IProviderNode;
    }

    static string ProviderKey(string simple_provider_name,
      string simple_provider_group) {
      return "group:" + simple_provider_group + ",name:" + simple_provider_name;
    }
  }
}
