using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Concurrent;

namespace Nohros.Caching
{
  /// <summary>
  /// Computes an retrieve values, based on a key, for use in populating a
  /// <see cref="ICache{T}"/>
  /// </summary>
  /// <typeparam name="T">The type of the objects that is loaded.
  /// </typeparam>
  public abstract partial class CacheLoader<T>
  {
    /// <summary>
    /// Returns a <see cref="CacheLoader{V} "/> which creates values by
    /// applying a <see cref="CacheLoaderDelegate{V} "/> using a key.
    /// </summary>
    /// <param name="loader">A <see cref="CacheLoaderDelegate{V} "/> that
    /// is used to compute the value from the key.</param>
    /// <returns>A <see cref="CacheLoader{V} "/> wich creates values by
    /// applying the <paramref name="loader"/> to the key.</returns>
    public static CacheLoader<T> From(CacheLoaderDelegate<T> loader) {
      return new CacheLoaderDelegateToCacheLoader<T>(loader);
    }

    /// <summary>
    /// Computes or retrieves the value correspnoding to
    /// <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose value should be loaded; will never
    /// be null.</param>
    /// <returns>The value associated with <paramref name="key"/>; may not be
    /// null.</returns>
    public abstract T Load(string key);

    /// <summary>
    /// Computes or retrieves a replacement value corresponding to an
    /// already-cached <paramref name="key"/>.
    /// </summary>
    /// <param name="key">
    /// The non-null key whose value should be loaded.
    /// </param>
    /// <param name="old_value">
    /// The non-null old value corresponding to <see cref="key"/>.
    /// </param>
    /// <returns>The future new value associated with <see cref="key"/>;
    /// must not be or return null(for reference types).</returns>
    /// <remarks>
    /// This method is called when an existing cache entry is refreshed by
    /// <see cref="CacheBuilder{T}.RefreshAfterWrite"/>, or through a call to
    /// cache refresh method.
    /// <para>
    /// This implementation synchronously delegates to <see cref="Load"/>. It
    /// is recommended that it be overridden with an asynchronous
    /// implementation when using
    /// <see cref="CacheBuilder{T}.RefreshAfterWrite"/>.
    /// </para>
    /// </remarks>
    public virtual IFuture<T> Reload(string key, T old_value) {
      return Futures.ImmediateFuture(Load(key));
    }
  }
}
