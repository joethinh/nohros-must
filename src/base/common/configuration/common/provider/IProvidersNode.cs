using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="IProvidersNode"/> is a collection of
  /// <see cref="IProvidersNode"/> objects.
  /// </summary>
  public interface IProvidersNode : IEnumerable<IProvidersNodeGroup>
  {
    /// <summary>
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="name"/> and is associated with the
    /// default group.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A simple provider whose name is <param name="name"> was
    /// not found.
    /// </param>
    /// </exception>
    IProvidersNodeGroup GetProvidersNodeGroup(string name);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose name is
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="node">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProvidersNodeGroup(string name, out IProvidersNodeGroup node);

    /// <summary>
    /// Adds an <see cref="IProvidersNodeGroup"/> element to the
    /// <see cref="IProvidersNode"/> collection.
    /// </summary>
    /// <param name="node">
    /// The <see cref="IProvidersNodeGroup"/> to add.
    /// </param>
    /// <remarks>
    /// The name of the node will be used as a node key and it should be
    /// the node's uniquely identifier.
    /// </remarks>
    void Add(IProvidersNodeGroup node);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="name"/> and is associated with the
    /// default group.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A simple provider whose name is <param name="name"> was
    /// not found.
    /// </param>
    /// </exception>
    /// <remarks>
    /// This method is a shortcut of the
    /// <see cref="GetProvidersNodeGroup(string)"/> method.
    /// </remarks>
    IProvidersNodeGroup this[string name] { get; }

    /// <summary>
    /// Gets the number of elements that the <see cref="IProvidersNode"/>
    /// contain.
    /// </summary>
    int Count { get; }
  }
}
