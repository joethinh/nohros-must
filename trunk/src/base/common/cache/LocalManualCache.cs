using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A <see cref="ICache{T}"/> implementation that is populated manually by
  /// calling one of the Put(...) methods overloads.
  /// </summary>
  /// <typeparam name="T">The type of objects that the cache contains
  /// </typeparam>
  internal class LocalManualCache<T> : LoadingCache<T>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalManualCache{T}"/> by
    /// using the specified cache provider and builder.
    /// </summary>
    /// <param name="provider">
    /// A <see cref="ICacheProvider"/> object that is used to store the cached
    /// items.
    /// </param>
    /// <param name="builder">
    /// A <see cref="CacheBuilder{T}"/> containing the configured options for
    /// this cache.
    /// </param>
    internal LocalManualCache(ICacheProvider provider, CacheBuilder<T> builder)
      : base(provider, builder, null) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalManualCache{T}"/> by
    /// using the specified cache provider, builder and automatic loader.
    /// </summary>
    /// <param name="provider">
    /// A <see cref="ICacheProvider"/> object that is used to store the cached
    /// items.
    /// </param>
    /// <param name="builder">
    /// A <see cref="CacheBuilder{T}"/> containing the configured options for
    /// this cache.
    /// </param>
    /// <param name="loader">
    /// A <see cref="loader"/> that is used to automatically load values.
    /// </param>
    protected LocalManualCache(ICacheProvider provider, CacheBuilder<T> builder,
      CacheLoader<T> loader) : base(provider, builder, loader) { }
    #endregion
  }
}
