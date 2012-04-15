using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  internal class AbstractCacheMock<T> : AbstractCache<T>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractCache{T}"/> class
    /// by using the specified cache provider and item loader.
    /// </summary>
    /// <param name="cache_provider">
    /// A <see cref="ICacheProvider"/> object that is used to store(cache) the
    /// items.
    /// </param>
    /// <param name="builder">
    /// A <see cref="CacheBuilder{T}"/> object that contains run-time
    /// configuration such as expiration polices.
    /// </param>
    public AbstractCacheMock(CacheBuilder<T> builder) : base(new CacheProviderMock(), builder, null) {
    }
  }
}
