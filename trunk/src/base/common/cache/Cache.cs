using System;

using Nohros.Resources;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// This class provides a skeletal implementation of the
  /// <see cref="ICache{T}"/> interface to minimize the effort required to
  /// implement this interface.
  /// </summary>
  internal class Cache<T>
  {
    readonly ICacheProvider cache_;
    readonly string cache_guid_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Cache{T}"/> class
    /// by using the specified cache provider and item loader.
    /// </summary>
    /// <param name="cache">A <see cref="ICacheProvider"/> object that is
    /// used to store(cache) the items.</param>
    public Cache(ICacheProvider cache) {
      cache_ = cache;

      // this value is used to distinghuish the items added through this
      // class from the others items in the cache. Since the cache could
      // be used for more than one application we cannot use the GetHashCode()
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
    DateTime GetExpiryDateTime(TimeSpan expiry) {
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
    /// Gets the value associated with the <paramref name="key"/> in cache, or
    /// the default value of type <typeparamref name="T"/> if there is no
    /// cached value for <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <returns>The value associated with the <paramref name="key"/> in cache,
    /// or the default value of type <typeparamref name="T"/> if there is no
    /// cached value for <paramref name="key"/>
    /// </returns>
    public T Get(string key) {
      return cache_.Get<T>(CacheKey(key));
    }

    /// <summary>
    /// Associates <paramref name="value"/> with <paramref name="key"/> in this
    /// cache. If the cache previously contained a value associated with
    /// <paramref name="key"/>, the old values is replaced by
    /// <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="value">The value to be associated with the
    /// <paramref name="key"/> in the cache.</param>
    public void Add(string key, T value) {
      Add(key, value, DateTime.MaxValue);
    }

    /// <summary>
    /// Associates <paramref name="value"/> with <paramref name="key"/> in this
    /// cache. If the cache previously contained a value associated with
    /// <paramref name="key"/>, the old values is replaced by
    /// <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="value">The value to be associated with the
    /// <paramref name="key"/> in the cache.</param>
    /// <param name="expiry">The date and time at which the added object
    /// expires and is removed from the cache.</param>
    public void Add(string key, T value, DateTime expiry) {
      Add(key, value, expiry.Subtract(DateTime.Now));
    }

    /// <summary>
    /// Associates <paramref name="value"/> with <paramref name="key"/> in this
    /// cache. If the cache previously contained a value associated with
    /// <paramref name="key"/>, the old values is replaced by
    /// <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="value">The value to be associated with the
    /// <paramref name="key"/> in the cache.</param>
    /// <param name="expiry">The time at which the object expires from now and
    /// is removed from the cache.
    /// </param>
    public void Add(string key, T value, TimeSpan expiry) {
      cache_.Set(key, value, expiry);
    }
    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// that value if necessary.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="loader">A <see cref="CacheLoader{T}"/> object that could
    /// be used to create the value if it is not present in the cache.</param>
    /// <remarks>
    /// No state associated with this cache is modified until loading is
    /// complete.
    /// </remarks>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is
    /// <c>null</c></exception>
    public T Get(string key, CacheLoader<T> loader) {
      if (key == null)
        Thrower.ThrowArgumentNullException(ExceptionArgument.key);

      return Get(key, loader, DateTime.MaxValue);
    }

    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// that value if necessary.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrive.</param>
    /// <param name="loader">A <see cref="CacheLoader{T}"/> object that could
    /// be used to create the value if it is not present in the cache.</param>
    /// <param name="expiry">The date and time at which the added object
    /// expires and is removed from the cache.</param>
    /// <returns></returns>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is
    /// <c>null</c></exception>
    public T Get(string key, CacheLoader<T> loader, DateTime expiry) {
      return Get(key, loader, expiry.Subtract(DateTime.Now));
    }

    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// taht value if necessary using the specified expiration police.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="loader">A <see cref="CacheLoader{T}"/> object that could
    /// be used to create the value if it is not present in the cache.</param>
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
    public T Get(string key, CacheLoader<T> loader, TimeSpan expiry) {
      if (key == null || loader == null) {
        Thrower.ThrowArgumentNullException(key == null
                                             ? ExceptionArgument.key
                                             : ExceptionArgument.loader);
      }

      string cache_key = CacheKey(key);
      T value = cache_.Get<T>(cache_key);

      // If the value is not present in cache, create, cache and return it.
      if (value == null) {
        try {
          value = loader.Load(key);
          cache_.Add(cache_key, value, expiry);
        } catch (Exception e) {
          throw new TypeLoadException(StringResources.Type_Load, e);
        }
      }

      return value;
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
