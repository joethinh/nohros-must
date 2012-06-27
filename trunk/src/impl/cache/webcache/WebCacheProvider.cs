using System;
using System.Web.Caching;

using Nohros.Caching.Providers;
using Nohros.Resources;

namespace Nohros.Caching
{
  /// <summary>
  /// A <see cref="ICacheProvider"/> implementation that uses the
  /// <see cref="System.Web.Caching.Cache"/> class as the underlying cache
  /// mechanism.
  /// </summary>
  public class WebCacheProvider : ICacheProvider
  {
    Cache cache_;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebCacheProvider"/>
    /// class by using the specified <see cref="Cache"/> object.
    /// </summary>
    /// <param name="cache">
    /// </param>
    public WebCacheProvider(Cache cache) {
      cache_ = cache;
    }

    /// <inheritdoc/>
    public T Get<T>(string key) {
      object local_item = cache_.Get(key);
      if (local_item != null && local_item is T) {
        return (T)local_item;
      }
      return default(T);
    }

    /// <inheritdoc/>
    public bool Get<T>(string key, out T item) {
      object local_item = cache_.Get(key);
      if (local_item != null && local_item is T) {
        item = (T) local_item;
        return true;
      }
      item = default(T);
      return false;
    }

    /// <inheritdoc/>
    public void Set(string key, object value) {
      cache_[key] = value;
    }

    /// <inheritdoc/>
    public void Set(string key, object value, long duration, TimeUnit unit) {
      cache_.Insert(key, value, null,
        DateTime.Now.AddMilliseconds(TimeUnitHelper.ToSeconds(duration, unit)),
        Cache.NoSlidingExpiration);
    }

    /// <inheritdoc/>
    public void Add(string key, object value) {
      object local_item = cache_.Get(key);
      if (local_item != null) {
        throw new ArgumentException(
          string.Format(StringResources.Argument_AddingDuplicateKey), key);
      }
      Set(key, value);
    }

    /// <inheritdoc/>
    public void Add(string key, object value, long duration, TimeUnit unit) {
      object local_item = cache_.Get(key);
      if (local_item != null) {
        throw new ArgumentException(
          string.Format(StringResources.Argument_AddingDuplicateKey), key);
      }
      Set(key, value, duration, unit);
    }

    /// <inheritdoc/>
    public void Remove(string key) {
      cache_.Remove(key);
    }

    /// <inheritdoc/>
    public void Clear() {
    }
  }
}
