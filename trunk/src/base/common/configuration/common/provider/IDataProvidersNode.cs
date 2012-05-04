using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="DataProvidersNode"/> is a collection of
  /// <see cref="DataProviderNode"/>.
  /// </summary>
  public interface IDataProvidersNode : IConfigurationNode
  {
    /// <summary>
    /// Gets a <see cref="IDataProviderNode"/> node whose name is
    /// <paramref name="data_provider_name"/>.
    /// </summary>
    /// <param name="data_provider_name">
    /// The name of the data provider.
    /// </param>
    /// <returns>
    /// A <see cref="IDataProviderNode"/> object whose name is
    /// <paramref name="data_provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A data provider whose name is <param name="data_provider_name"> was
    /// not found.
    /// </param>
    /// </exception>
    IDataProviderNode GetDataProviderNode(string data_provider_name);

    /// <summary>
    /// Gets a <see cref="IDataProviderNode"/> whose name is
    /// <paramref name="data_provider_name"/>.
    /// </summary>
    /// <param name="data_provider_name">
    /// The name of the data provider.
    /// </param>
    /// <param name="data_provider">
    /// When this method returns contains a <see cref="IDataProviderNode"/>
    /// object whose name is <paramref name="data_provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a data provider whose name is
    /// <paramref name="data_provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetDataProviderNode(string data_provider_name,
      out IDataProviderNode data_provider);
  }
}
