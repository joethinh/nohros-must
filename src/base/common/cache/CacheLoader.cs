using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// Computes an retrieve values, based on a key, for use in populating a
  /// <see cref="Cache{T}"/>
  /// </summary>
  /// <typeparam name="T">The type of the objects that is stored loaded.
  /// </typeparam>
  public abstract partial class CacheLoader<T>
  {
    /// <summary>
    /// Returns a <see cref="CacheLoader{V} "/> which creates values by applying
    /// a <see cref="CacheLoaderDelegate{V} "/> using a key.
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
  }
}
