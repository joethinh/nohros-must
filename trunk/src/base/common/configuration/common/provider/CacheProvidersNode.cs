using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="CacheProvidersNode"/> is a collection of
  /// <see cref="CacheProviderNode"/> objects.
  /// </summary>
  public partial class CacheProvidersNode :
    AbstractHierarchicalConfigurationNode, ICacheProvidersNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvidersNode"/> class.
    /// </summary>
    public CacheProvidersNode() : base(Strings.kCacheProvidersNodeName) {
    }
    #endregion
  }
}
