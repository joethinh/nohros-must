using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="DataProvidersNode"/> is a collection of
  /// <see cref="DataProviderNode"/>.
  /// </summary>
  public partial class DataProvidersNode : AbstractHierarchicalConfigurationNode, IDataProvidersNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DataProvidersNode"/> class.
    /// </summary>
    public DataProvidersNode() : base(Strings.kDataProvidersNodeName) { }
    #endregion

    /// <summary>
    /// Gets a <see cref="DataProviderNode"/> node whose name is
    /// <paramref name="data_provider_name"/>.
    /// </summary>
    /// <param name="data_provider_name">
    /// The name of the data provider.
    /// </param>
    /// <returns>
    /// A <see cref="DataProviderNode"/> object whose name is
    /// <paramref name="data_provider_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A data provider whose name is <param name="data_provider_name"> was
    /// not found.
    /// </param>
    /// </exception>
    public IDataProviderNode GetDataProviderNode(string data_provider_name) {
      return base[data_provider_name] as DataProviderNode;
    }

    /// <summary>
    /// Gets a <see cref="DataProviderNode"/> whose name is
    /// <paramref name="data_provider_name"/>.
    /// </summary>
    /// <param name="data_provider_name">
    /// The name of the data provider.
    /// </param>
    /// <param name="data_provider">
    /// When this method returns contains a <see cref="DataProviderNode"/>
    /// object whose name is <paramref name="data_provider_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a data provider whose name is
    /// <paramref name="data_provider_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    public bool GetDataProviderNode(string data_provider_name,
      out IDataProviderNode data_provider) {
      return GetChildNode(data_provider_name, out data_provider);
    }
  }
}
