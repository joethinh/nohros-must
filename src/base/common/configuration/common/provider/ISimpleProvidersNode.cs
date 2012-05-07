using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="ISimpleProvidersNode"/> is a collection of
  /// <see cref="ISimpleProvidersNode"/>.
  /// </summary>
  public interface ISimpleProvidersNode : IConfigurationNode
  {
    /// <summary>
    /// Gets a <see cref="SimpleProvidersNode"/> node whose name is
    /// <paramref name="simple_provider_name"/> and is associated with the
    /// default group.
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
    ISimpleProviderNode GetSimpleProviderNode(string simple_provider_name);

    /// <summary>
    /// Gets a <see cref="SimpleProvidersNode"/> node whose name is
    /// <paramref name="simple_provider_name"/> and is associated with the
    /// group <paramref name="simple_provider_group"/>.
    /// </summary>
    /// <param name="simple_provider_name">
    /// The name of the simple provider.
    /// </param>
    /// <param name="simple_provider_group">
    /// The name of the group associated with the simple provider.
    /// </param>
    /// <returns>
    /// A <see cref="SimpleProviderNode"/> object whose name is
    /// <paramref name="simple_provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A simple provider whose name is <param name="simple_provider_name">
    /// and is associated with the group
    /// <paramref name="simple_provider_group"/> was not found.
    /// </param>
    /// </exception>
    ISimpleProviderNode GetSimpleProviderNode(string simple_provider_name,
      string simple_provider_group);

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
    bool GetSimpleProviderNode(string simple_provider_name,
      out ISimpleProviderNode simple_provider);

    /// <summary>
    /// Gets a <see cref="SimpleProvidersNode"/> node whose name is
    /// <paramref name="simple_provider_name"/> and is associated with the
    /// group <paramref name="simple_provider_group"/>.
    /// </summary>
    /// <param name="simple_provider_name">
    /// The name of the simple provider.
    /// </param>
    /// <param name="simple_provider_group">
    /// The name of the group associated with the simple provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="SimpleProvidersNode"/>
    /// object whose name is <paramref name="simple_provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a simple provider whose name is
    /// <paramref name="simple_provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetSimpleProviderNode(string simple_provider_name,
      string simple_provider_group, out ISimpleProviderNode simple_provider);
  }
}
