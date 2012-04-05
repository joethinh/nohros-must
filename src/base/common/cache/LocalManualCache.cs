using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A <see cref="ICache{T}"/> implementation that is populated manually by
  /// calling one of the Add(...) methods overloads.
  /// </summary>
  /// <typeparam name="T">The type of objects that the cache contains
  /// </typeparam>
  internal class LocalManualCache<T> : ICache<T>
  {
    readonly ICache<T> cache_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalManualCache{T}"/> by
    /// using the specified cache provider.
    /// </summary>
    /// <param name="cache">A <see cref="ICache"/> object that is used to
    /// store the cached items.</param>
    public LocalManualCache(ICache<T> cache) {
      cache_ = cache;
    }
    #endregion

    /// <inheritdoc/>
    public void Add(string key, T value) {
      cache_.Add(key, value);
    }

    /// <inheritdoc/>
    public T GetIfPresent(string key) {
      return cache_.GetIfPresent(key);
    }

    /// <inheritdoc/>
    public T Get(string key, CacheLoader<T> loader) {
      return cache_.Get(key, loader);
    }

    /// <inheritdoc/>
    public void Remove(string key) {
      cache_.Remove(key);
    }

    /// <inheritdoc/>
    public long Size {
      get { return cache_.Size; }
    }
  }
}
