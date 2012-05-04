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
    /// Gets the configured data providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a
    /// empty <see cref="IDataProvidersNode"/> object that is a
    /// <see cref="IDataProvidersNode"/> that contains no providers.
    /// </remarks>
    IDataProvidersNode DataProviders { get; }

    /// <summary>
    /// Gets all the configured cache providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a
    /// empty <see cref="ICacheProvidersNode"/> object that is a
    /// <see cref="ICacheProvidersNode"/> that contains no providers.
    /// </remarks>
    ICacheProvidersNode CacheProviders { get; }

    /// <summary>
    /// Gets all the configured simple providers.
    /// </summary>
    /// <remarks>
    /// If no providers was configured, this property will returns a empty
    /// <see cref="ISimpleProvidersNode"/> object that is a
    /// <see cref="ISimpleProvidersNode"/> that contains no providers.
    /// </remarks>
    ISimpleProvidersNode SimpleProviders { get; }
  }
}
