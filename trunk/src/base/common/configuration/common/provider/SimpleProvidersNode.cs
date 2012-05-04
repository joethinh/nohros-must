using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="SimpleProvidersNode"/> is a collection of
  /// <see cref="SimpleProviderNode"/>.
  /// </summary>
  public partial class SimpleProvidersNode: AbstractHierarchicalConfigurationNode, ISimpleProvidersNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleProvidersNode"/>
    /// class.
    /// </summary>
    public SimpleProvidersNode() : base(Strings.kSimpleProvidersNodeName) { }
    #endregion

    /// <summary>
    /// Gets a <see cref="SimpleProvidersNode"/> node whose name is
    /// <paramref name="simple_provider_name"/>.
    /// </summary>
    /// <param name="simple_provider_name">
    /// The name of the simple provider.
    /// </param>
    /// <returns>
    /// A <see cref="SimpleProvidersNode"/> object whose name is
    /// <paramref name="simple_provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A simple provider whose name is <param name="simple_provider_name"> was
    /// not found.
    /// </param>
    /// </exception>
    public ISimpleProvidersNode GetSimpleProvidersNode(string simple_provider_name) {
      return base[simple_provider_name] as ISimpleProvidersNode;
    }

    /// <summary>
    /// Gets a <see cref="SimpleProvidersNode"/> whose name is
    /// <paramref name="simple_provider_name"/>.
    /// </summary>
    /// <param name="simple_provider_name">
    /// The name of the simple provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="SimpleProvidersNode"/>
    /// object whose name is <paramref name="simple_provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a simple provider whose name is
    /// <paramref name="simple_provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    public bool GetSimpleProvidersNode(string simple_provider_name,
      out ISimpleProvidersNode simple_provider) {
      return GetChildNode(simple_provider_name, out simple_provider);
    }
  }
}
