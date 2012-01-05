using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A semi-persistent mapping from keys to values that uses the pluggable
  /// cache mechanism provided by the <see cref="ICacheProvider"/> interface.
  /// </summary>
  /// <remarks>
  /// Values are automatically loaded by the cache, and are stored in the
  /// cache until either evicted or manually invalidated.
  /// </remarks>
  public class Cache<V> where V : class
  {
    ICacheProvider cache_;
    CacheLoader<V> loader_;

    string cache_guid_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Cache&lt;V&gt;"/> class
    /// by using the specified cache provider and item loader.
    /// </summary>
    /// <param name="cache">A <see cref="ICacheProvider"/> object that is
    /// used to store(cache) the items.</param>
    /// <param name="loader">The cache item loader used to obtain new values.
    /// </param>
    public Cache(ICacheProvider cache, CacheLoader<V> loader) {
      cache_ = cache;
      loader_ = loader;

      // this value is used to distinghuish the items added through this
      // class from the others items in the cache. Since the cache could
      // be used for more than one application we canno use the GetHashCode()
      // method as key, because it is unique only within the running
      // application.
      cache_guid_ = Guid.NewGuid().ToString("N");
    }
    #endregion

    /// <summary>
    /// Generates a string that uniquely identifies the key.
    /// </summary>
    /// <param name="key">The key that should be unyfied.</param>
    /// <returns>A string that is globally unique.</returns>
    string CacheKey(string key) {
      return cache_guid_ + ":" + key;
    }

    /// <summary>
    /// Gets the absolute entry expiration date and time by adding the
    /// specified <paramref name="expiry"/> parameter to the current date and
    /// time.
    /// </summary>
    /// <param name="expiry">The interval between now and the date that the
    /// entry should expire.</param>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="expiry"/> to less than <c>TimeSpan.Zero</c> or to a
    /// value that exceeds the maximum system date time.</exception>
    /// <returns></returns>
    protected DateTime GetExpiryDateTime(TimeSpan expiry) {
      // store the current date time as soon as possible.
      DateTime now = DateTime.Now;

      // the expiration interval could not be negative.
      if (expiry < TimeSpan.Zero) {
        throw new ArgumentOutOfRangeException("expiry");
      }

      // the specified interval could not overlap the system maximum value for
      // date and time.
      TimeSpan max_time_span = DateTime.MaxValue.Subtract(now);
      if (expiry > max_time_span) {
        throw new ArgumentOutOfRangeException("expiry");
      }

      return now.Add(expiry);
    }

    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// that value if necessary.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrive.</param>
    /// <param name="expiry">The date and time at which the added object
    /// expires and is removed from the cache.</param>
    /// <returns></returns>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is
    /// <c>null</c></exception>
    protected V Get(string key, DateTime expiry) {
      string cache_key = CacheKey(key);
      V value = cache_.Get<V>(cache_key);

      if (value == null) {
        try {
          value = loader_.Load(key);
        } catch (Exception e) {
          throw new TypeLoadException(StringResources.Type_Load, e);
        }
      }

      return value;
    }

    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// that value if necessary.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <remarks>
    /// No state associated with this cache is modified until loading is
    /// complete.
    /// </remarks>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is
    /// <c>null</c></exception>
    public V Get(string key) {
      if (key == null)
        throw new ArgumentNullException("key");

      return Get(key, DateTime.MaxValue);
    }

    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// taht value if necessary using the specified expiration police.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="expiry">The time at which the object expires from now and
    /// is removed from the cache.
    /// </param>
    /// <returns>The value associated with the given key.</returns>
    /// <exception cref="ArgumentException"><paramref name="key"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="expiry"/> to less than <c>TimeSpan.Zero</c> or to a
    /// value that exceeds the maximum system date time.
    /// </exception>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    public V Get(string key, TimeSpan expiry) {
      if (key == null) {
        throw new ArgumentNullException("key");
      }

      return Get(key, GetExpiryDateTime(expiry));
    }

    /// <summary>
    /// Discards any cached value for the key <paramref name="key"/>, so that
    /// future invocation of <c>Get(...)</c> will result in a cache miss and
    /// reload.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    public void Remove(string key) {
      cache_.Remove(key);
    }
  }
}
