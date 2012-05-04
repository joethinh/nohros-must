using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="ProvidersNode"/> is a collection of
  /// <see cref="ProviderNode"/> objects.
  /// </summary>
  public partial class ProvidersNode : AbstractConfigurationNode, IProvidersNode
  {
    IDataProvidersNode data_provider_node_;
    ICacheProvidersNode cache_providers_node_;
    ISimpleProvidersNode simple_providers_node_;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidersNode"/> class.
    /// </summary>
    public ProvidersNode() : base(Strings.kProvidersNodeName) {
      data_provider_node_ = new DataProvidersNode();
      cache_providers_node_ = new CacheProvidersNode();
      simple_providers_node_ = new SimpleProvidersNode();
    }

    /// <summary>
    /// Gets the configured data providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a
    /// empty <see cref="DataProvidersNode"/> object that is a
    /// <see cref="DataProvidersNode"/> that contains no providers.
    /// </remarks>
    public IDataProvidersNode DataProviders {
      get { return data_provider_node_; }
    }

    /// <summary>
    /// Gets all the configured cache providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a
    /// empty <see cref="CacheProvidersNode"/> object that is a
    /// <see cref="CacheProvidersNode"/> that contains no providers.
    /// </remarks>
    public ICacheProvidersNode CacheProviders {
      get { return cache_providers_node_; }
    }

    /// <summary>
    /// Gets all the configured simple providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a empty
    /// <see cref="SimpleProvidersNode"/> object that is a
    /// <see cref="SimpleProvidersNode"/> that contains no providers.
    /// </remarks>
    public ISimpleProvidersNode SimpleProviders {
      get { return simple_providers_node_; }
    }
  }
}
