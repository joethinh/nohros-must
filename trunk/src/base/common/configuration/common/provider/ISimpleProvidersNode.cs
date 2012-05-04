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
    ISimpleProvidersNode GetSimpleProvidersNode(
      string simple_provider_name);

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
    bool GetSimpleProvidersNode(string simple_provider_name,
      out ISimpleProvidersNode simple_provider);
  }
}
