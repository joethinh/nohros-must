using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

using Nohros.Caching.Providers;

namespace Nohros.Net
{
  /// <summary>
  /// A implementation of the <see cref="ICacheProvider"/> that uses the
  /// <see cref="System.Web.Caching.Cache"/> as the underlying provider.
  /// </summary>
  public class HttpCacheProvider : CacheProvider
  {
    Cache cache_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpCacheProvider"/> by
    /// using the specified <see cref="Cache"/>
    /// </summary>
    public HttpCacheProvider(Cache cache) {
      cache_ = cache;
    }
    #endregion

    /// <inheritdoc/>
    public override void Clear() {
      List<string> keys = new List<string>(cache_.Count);

      // we cannot modify the collection while enumerating it. So, save
      // the keys and remove the objects in other loop.
      foreach(DictionaryEntry entry in cache_) {
        keys.Add(entry.Key as string);
      }

      foreach (string key in keys) {
        cache_.Remove(key as string);
      }
    }

    /// <inheritdoc/>
    public override void Remove(string key) {
      cache_.Remove(key);
    }

    /// <inheritdoc/>
    public override V Get<V>(string key) {
      object value = cache_.Get(key);
      return (value == null) ? default(V) : (V)value;
    }

    public override void Add(string key, object value) {
      Add(key, value, TimeSpan.Zero);
    }

    public override void Set(string key, object value) {
      Set(key, value, TimeSpan.Zero);
    }

    /// <inheritdoc/>
    public override void Set(string key, object value, TimeSpan expiry) {
      cache_.Insert(key, value, null, GetExpiryDateTime(expiry),
        Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
    }

    /// <inheritdoc/>
    public override void Add(string key, object value, TimeSpan expiry) {
      cache_.Add(key, value, null, GetExpiryDateTime(expiry),
        Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
    }
  }
}