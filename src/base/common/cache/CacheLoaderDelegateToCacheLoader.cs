using System;

namespace Nohros.Caching
{
  public abstract partial class CacheLoader<T>
  {
    class CacheLoaderDelegateToCacheLoader<T> : CacheLoader<T>
    {
      readonly CacheLoaderDelegate<T> loader_;

      /// <summary>
      /// Initializes a new instance of the
      /// <see cref="CacheLoader{T}.CacheLoaderDelegateToCacheLoader{T}"/>
      /// class by using the specified <see cref="CacheLoaderDelegate{T}"/>
      /// </summary>
      /// <param name="loader">The loader that is used to load instances of
      /// type <typeparamref name="T"/></param>
      public CacheLoaderDelegateToCacheLoader(
        CacheLoaderDelegate<T> loader) {
        loader_ = loader;
      }

      /// <inheritdoc/>
      public override T Load(string key) {
        return loader_(key);
      }
    }
  }
}
