using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="SimpleProvidersNode"/> is a collection of
  /// <see cref="SimpleProviderNode"/>.
  /// </summary>
  public partial class SimpleProvidersNode :
    AbstractHierarchicalConfigurationNode, ISimpleProvidersNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleProvidersNode"/>
    /// class.
    /// </summary>
    public SimpleProvidersNode() : base(Strings.kSimpleProvidersNodeName) {
    }
    #endregion

    /// <inheritdoc/>
    public bool GetSimpleProviderNode(string simple_provider_name,
      out ISimpleProviderNode simple_provider) {
      return GetSimpleProviderNode(simple_provider_name, string.Empty,
        out simple_provider);
    }

    /// <inheritdoc/>
    public bool GetSimpleProviderNode(string simple_provider_name,
      string simple_provider_group, out ISimpleProviderNode simple_provider) {
      return
        GetChildNode(
          SimpleProviderKey(simple_provider_name, simple_provider_group),
          out simple_provider);
    }

    /// <inheritdoc/>
    public ISimpleProviderNode GetSimpleProviderNode(string simple_provider_name) {
      return GetSimpleProviderNode(simple_provider_name, string.Empty);
    }

    /// <inheritdoc/>
    public ISimpleProviderNode GetSimpleProviderNode(
      string simple_provider_name, string simple_provider_group) {
      return
        base[SimpleProviderKey(simple_provider_name, simple_provider_group)] as
          ISimpleProviderNode;
    }

    static string SimpleProviderKey(string simple_provider_name,
      string simple_provider_group) {
      return "group:" + simple_provider_group + ",name:" + simple_provider_name;
    }
  }
}
