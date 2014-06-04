using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A set of <see cref="IProviderNode"/> that belongs to the same group
  /// </summary>
  public interface IProvidersNodeGroup : IConfigurationNode,
                                         IEnumerable<IProviderNode>
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
    IProviderNode GetProviderNode(string name);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose name is
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="provider">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProviderNode(string name, out IProviderNode provider);

    /// <summary>
    /// Gets an array of <see cref="IProviderNode"/> containing all the
    /// providers that belongs to the grou <paramref name="group"/>
    /// </summary>
    /// <param name="group">
    /// The name of the group associated with the providers.
    /// </param>
    /// <returns>
    /// An array of <see cref="IProviderNode"/> containing all the providers
    /// associated with the group <paramref name="group"/>.
    /// </returns>
    /// <remarks>
    /// If there are no providers associated with the group
    /// <paramref name="group"/> this method will returns an empty array.
    /// </remarks>
    IProviderNode[] GetProvidersNode(string group);

    /// <summary>
    /// Adds an <see cref="IProviderNode"/> to the
    /// <see cref="IProvidersNodeGroup"/> collection.
    /// </summary>
    /// <param name="node">
    /// The <see cref="IProviderNode"/> to add.
    /// </param>
    /// <remarks>
    /// The name of the node will be used as a node key and it should be
    /// the node's uniquely identifier.
    /// </remarks>
    void Add(IProviderNode node);

    /// <summary>
    /// Adds an <see cref="IProviderNode"/> to the
    /// <see cref="IProvidersNodeGroup"/> collection using the given provider
    /// name.
    /// </summary>
    /// <param name="name">
    /// The name to use to identify the provider node.
    /// </param>
    /// <param name="node">
    /// The <see cref="IProviderNode"/> to add.
    /// </param>
    /// <remarks>
    /// The name of the node will be used as a node key and it should be
    /// the node's uniquely identifier.
    /// </remarks>
    void Add(string name, IProviderNode node);

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
    /// This method is a shortcut for the <see cref="GetProviderNode(string)"/>
    /// method.
    /// </remarks>
    IProviderNode this[string name] { get; }

    /// <summary>
    /// Gets the number of elements that the <see cref="IProvidersNodeGroup"/>
    /// contain.
    /// </summary>
    int Count { get; }
  }
}
