using System;
using System.Runtime.Caching;
using System.Collections.Generic;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  public class MemoryCacheProviderFactory : ICacheProviderFactory
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MemoryCacheProviderFactory"/> class.
    /// </summary>
    public MemoryCacheProviderFactory() {
    }
    #endregion

    #region ICacheProviderFactory Members
    public ICacheProvider CreateCacheProvider(
      IDictionary<string, string> options) {
      return new MemoryCacheProvider(System.Runtime.Caching.MemoryCache.Default);
    }
    #endregion
  }
}
