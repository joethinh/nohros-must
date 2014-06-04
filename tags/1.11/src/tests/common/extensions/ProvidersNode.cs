using System;
using NUnit.Framework;
using Nohros.Configuration;

namespace Nohros.Extensions
{
  public static class ProvidersNodes
  {
    /// <summary>
    /// Adds the given <see cref="IProvidersNode"/> object to a empty
    /// <see cref="IProvidersNodeGroup"/> inside the given
    /// <see cref="IProvidersNode"/> object.
    /// </summary>
    /// <param name="nodes">
    /// A <see cref="IProvidersNode"/> object to add the <paramref name="node"/>
    /// </param>
    /// <param name="node">
    /// The <see cref="IProvidersNode"/> object to be added to the
    /// <paramref name="nodes"/>.
    /// </param>
    public static void Add(this IProvidersNode nodes, IProviderNode node) {
      nodes.Add(node.Name, node);
    }

    /// <summary>
    /// Adds a <see cref="IProvidersNode"/> object to a empty
    /// <see cref="IProvidersNodeGroup"/> inside the given
    /// <see cref="IProvidersNode"/> object using the given provider name and
    /// type.
    /// </summary>
    /// <param name="nodes">
    /// The type of the provider to be added.
    /// </param>
    /// <param name="type">
    /// The <see cref="IProvidersNode"/> object to be added to the
    /// <paramref name="nodes"/>.
    /// </param>
    /// <param name="name">
    /// The name to be associated with the provider node.
    /// </param>
    /// <remarks>
    /// This method creates an instance of the <see cref="IProviderNode"/>
    /// representing the given type and added it to the <paramref name="nodes"/>
    /// collection.
    /// </remarks>
    public static void Add(this IProvidersNode nodes, string name, Type type) {
      nodes.Add(name, new ProviderNode.Builder(name, type).Build());
    }

    /// <summary>
    /// Adds the given <see cref="IProvidersNode"/> object to a empty
    /// <see cref="IProvidersNodeGroup"/> inside the given
    /// <see cref="IProvidersNode"/> object using the given provider name.
    /// </summary>
    /// <param name="nodes">
    /// A <see cref="IProvidersNode"/> object to add the <paramref name="node"/>
    /// </param>
    /// <param name="node">
    /// The <see cref="IProvidersNode"/> object to be added to the
    /// <paramref name="nodes"/>.
    /// </param>
    /// <param name="name">
    /// The name to be associated with the provider node.
    /// </param>
    public static void Add(this IProvidersNode nodes, string name,
      IProviderNode node) {
      IProvidersNodeGroup empty_provider_group;
      if (!nodes.GetProvidersNodeGroup(string.Empty, out empty_provider_group)) {
        empty_provider_group = new ProvidersNodeGroup();
        nodes.Add(empty_provider_group);
      }
      empty_provider_group.Add(name, node);
    }
  }
}
