using System;
using System.Collections.Generic;
using System.Web;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A factory used to create instances of the <see cref="WebCacheProvider"/>
  /// class.
  /// </summary>
  public class WebCacheProviderFactory : ICacheProviderFactory
  {
    #region ICacheProviderFactory Members
    /// <inheritdoc/>
    public ICacheProvider CreateCacheProvider(
      IDictionary<string, string> options) {
      return new WebCacheProvider(HttpRuntime.Cache);
    }
    #endregion
  }
}
