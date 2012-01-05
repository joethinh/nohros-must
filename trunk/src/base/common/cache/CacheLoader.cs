using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  public abstract class CacheLoader<V>
  {
    #region CacheLoaderDelegateToCacheLoader
    class CacheLoaderDelegateToCacheLoader<V>: CacheLoader<V>
    {
      CacheLoaderDelegate<V> loader_;

      /// <summary>
      /// Initializes a new instance of the
      /// <see cref="CacheLoaderDelegateToCacheLoader"/> class by using the
      /// specified <see cref="CacheLoaderDelegate&gt;K, V&lt;"/>
      /// </summary>
      /// <param name="loader"></param>
      public CacheLoaderDelegateToCacheLoader(
        CacheLoaderDelegate<V> loader) {
        loader_ = loader;
      }

      /// <inheritdoc/>
      public override V Load(string key) {
        return loader_(key);
      }
    }
    #endregion

    /// <summary>
    /// Returns a <see cref="CacheLoader"/> which creates values by applying
    /// a <see cref="CacheLoaderDelegate"/> using a key.
    /// </summary>
    /// <param name="loader">A <see cref="CacheLoaderDelegate"/> that
    /// is used to compute the value from the key.</param>
    /// <returns>A <see cref="CacheLoader"/> wich creates values by applying
    /// the <paramref name="loader"/> to the key.</returns>
    public static CacheLoader<V> From(CacheLoaderDelegate<V> loader) {
      return new CacheLoaderDelegateToCacheLoader<V>(loader);
    }

    /// <summary>
    /// Computes or retrieves the value correspnoding to <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose value should be loaded; will never
    /// be null.</param>
    /// <returns>The value associated with <paramref name="key"/>; may not be
    /// null.</returns>
    public abstract V Load(string key);
  }
}
