using System;
using Nohros.Configuration;

namespace Nohros.Extensions
{
  public static class Providers
  {
    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose anme is <paramref name="name"/>
    /// and belongs to the unamed providers group.
    /// </summary>
    /// <param name="providers">
    /// A <see cref="IProvidersNode"/> taht contains a group whose name is
    /// <see cref="string.Empty"/>.
    /// </param>
    /// <param name="name">
    /// The name of the provider to get.
    /// </param>
    /// <returns></returns>
    public static IProviderNode GetProviderNode(this IProvidersNode providers,
      string name) {
      return providers.GetProvidersNodeGroup(string.Empty).GetProviderNode(name);
    }

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose anme is <paramref name="name"/>
    /// and belongs to the unamed providers group.
    /// </summary>
    /// <param name="providers">
    /// A <see cref="IProvidersNode"/> taht contains a group whose name is
    /// <see cref="string.Empty"/>.
    /// </param>
    /// <param name="name">
    /// The name of the provider to get.
    /// </param>
    /// <returns></returns>
    public static bool GetProviderNode(this IProvidersNode providers,
      string name, out IProviderNode provider) {
      IProvidersNodeGroup group;
      if (providers.GetProvidersNodeGroup(string.Empty, out group)) {
        return group.GetProviderNode(name, out provider);
      }
      provider = default(IProviderNode);
      return false;
    }
  }
}
