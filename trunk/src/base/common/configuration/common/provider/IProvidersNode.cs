using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="IProvidersNode"/> is a collection of
  /// <see cref="IProvidersNode"/> objects.
  /// </summary>
  public interface IProvidersNode : IEnumerable<IProviderNode>
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
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="name"/> and is associated with the
    /// group <paramref name="group"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="group">
    /// The name of the group associated with the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A provider whose name is <param name="name"> and is
    /// associated with the group <paramref name="group"/> was not
    /// found.
    /// </param>
    /// </exception>
    IProviderNode GetProviderNode(string name, string group);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose name is
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProviderNode(string name, out IProviderNode simple_provider);

    /// <summary>
    /// Gets a <see cref="IProvidersNode"/> node whose name is
    /// <paramref name="name"/> and is associated with the
    /// group <paramref name="group"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="group">
    /// The name of the group associated with the provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProviderNode(string name,
      string group, out IProviderNode simple_provider);

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
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="name"/> and is associated with the
    /// group <paramref name="group"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="group">
    /// The name of the group associated with the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A provider whose name is <param name="name"> and is
    /// associated with the group <paramref name="group"/> was not
    /// found.
    /// </param>
    /// </exception>
    /// <remarks>
    /// This method is a shortcut for the
    /// <see cref="GetProviderNode(string, string)"/> method.
    /// </remarks>
    IProviderNode this[string name, string group] { get; }
  }
}
