using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="IProvidersNode"/> is a collection of
  /// <see cref="IProvidersNode"/> objects.
  /// </summary>
  public interface IProvidersNode
  {
    /// <summary>
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="provider_name"/> and is associated with the
    /// default group.
    /// </summary>
    /// <param name="provider_name">
    /// The name of the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A simple provider whose name is <param name="provider_name"> was
    /// not found.
    /// </param>
    /// </exception>
    IProviderNode GetProviderNode(string provider_name);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> node whose name is
    /// <paramref name="provider_name"/> and is associated with the
    /// group <paramref name="provider_group"/>.
    /// </summary>
    /// <param name="provider_name">
    /// The name of the provider.
    /// </param>
    /// <param name="provider_group">
    /// The name of the group associated with the provider.
    /// </param>
    /// <returns>
    /// A <see cref="IProviderNode"/> object whose name is
    /// <paramref name="provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A provider whose name is <param name="provider_name"> and is
    /// associated with the group <paramref name="provider_group"/> was not
    /// found.
    /// </param>
    /// </exception>
    IProviderNode GetProviderNode(string provider_name, string provider_group);

    /// <summary>
    /// Gets a <see cref="IProviderNode"/> whose name is
    /// <paramref name="provider_name"/>.
    /// </summary>
    /// <param name="provider_name">
    /// The name of the provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProviderNode(string provider_name, out IProviderNode simple_provider);

    /// <summary>
    /// Gets a <see cref="IProvidersNode"/> node whose name is
    /// <paramref name="provider_name"/> and is associated with the
    /// group <paramref name="provider_group"/>.
    /// </summary>
    /// <param name="provider_name">
    /// The name of the provider.
    /// </param>
    /// <param name="provider_group">
    /// The name of the group associated with the provider.
    /// </param>
    /// <param name="simple_provider">
    /// When this method returns contains a <see cref="IProviderNode"/>
    /// object whose name is <paramref name="provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetProviderNode(string provider_name,
      string provider_group, out IProviderNode simple_provider);
  }
}
