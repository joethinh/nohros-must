using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching.Providers
{
  /// <summary>
  /// The purpose of the <see cref="ICacheProvider"/> is to allow users to
  /// choose from one to multiple implementations of a cache engine in the
  /// application configuration, making the cache pluggable.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Implementations of this interface are expected to the thread-safe, and
  /// can be safely accessed by multiple concurrent threads.
  /// </para>
  /// </remarks>
  public interface ICacheProvider
  {
    /// <summary>
    /// Gets the value associated with the given key.
    /// </summary>
    /// <param name="key">The identifier for the item to retrieve from cache.
    /// </param>
    /// <returns>A value associated with the given key, ou the default value
    /// for <typeparamref name="T"/> if the key was not found.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Gets the value associated with the given key.
    /// </summary>
    /// <param name="key">The identifier for the item to retrieve from cache.
    /// </param>
    /// <param name="item">If a item associated with <paramref name="key"/>
    /// exists in the cache, the associated item; otherwise, the default
    /// value for <typeparamref name="T"/></param>
    /// <returns><c>true</c> if the key is found; otherwise, false.</returns>
    bool Get<T>(string key, out T item);

    /// <summary>
    /// Adds or replace the specified item to the cache.
    /// </summary>
    /// <param name="key">The cache key used to reference the item.</param>
    /// <param name="value">The item to be added to the cache.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <remarks>
    /// The item added to the cache should never expires(although it may be
    /// deleted from the cache to make place or other items, depending on the
    /// cache provider implementation).
    /// </remarks>
    void Set(string key, object value);

    /// <summary>
    /// Adds or replace the specified item to the cache with expiration policy.
    /// </summary>
    /// <param name="key">The cache key used to reference the item.</param>
    /// <param name="value">The item to be added to the cache.</param>
    /// <param name="duration">The length of time time at which the added object
    /// expires from a point in time(after access or after write, depending on
    /// the cache provider configuration and implementation) and is
    /// removed from the cache. If it's is zero, the item should
    /// never expires(although it may be deleted from the cache to make place
    /// or other items, depending on the cache provider implementation).
    /// </param>
    /// <param name="unit">The unit that <paramref name="duration"/> is
    /// expressed in.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="duration"/> to a value that is less than zero.
    /// </exception>
    void Set(string key, object value, long duration, TimeUnit unit);

    /// <summary>
    /// Adds the specified item to the cache.
    /// </summary>
    /// <param name="key">The cache key used to reference the item.</param>
    /// <param name="value">The item to be added to the cache.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <exception cref="InvalidOperationException">An item with the ke
    /// <paramref name="key"/> already exists in the cache.</exception>
    /// <remarks>
    /// Calls to this method should throws a exception if an item with the same
    /// <paramref name="key"/> parameter is already stored in the cache. To
    /// overwrite an existing cache item using the same <paramref name="key"/>
    /// parameter, use the <see cref="Set(string, object)"/> method.
    /// <para>
    /// The item added to the cache should never expires(although it may be
    /// deleted from the cache to make place or other items, depending on the
    /// cache provider implementation).
    /// </para>
    /// </remarks>
    /// <seealso cref="Set(string, object)"/>
    /// <seealso cref="Add(string, object)"/>
    void Add(string key, object value);

    /// <summary>
    /// Adds the specified item to the cache with expiration policy.
    /// </summary>
    /// <param name="key">The cache key used to reference the item.</param>
    /// <param name="value">The item to be added to the cache.</param>
    /// <param name="duration">The length of time time at which the added object
    /// expires from a point in time(after access or after write, depending on
    /// the cache provider configuration and implementation) and is
    /// removed from the cache. If it's is zero, the item should
    /// never expires(although it may be deleted from the cache to make place
    /// or other items, depending on the cache provider implementation).
    /// </param>
    /// <param name="unit">The unit that <paramref name="duration"/> is
    /// expressed in.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <exception cref="InvalidOperationException">An item with the ke
    /// <paramref name="key"/> already exists in the cache.</exception>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="duration"/> to a value that is less than zero.
    /// </exception>
    /// <remarks>
    /// Calls to this method should throws a exception if an item with the same
    /// <paramref name="key"/> parameter is already stored in the cache. To
    /// overwrite an existing cache item using the same <paramref name="key"/>
    /// parameter, use the <see cref="Set(string, object, long, TimeUnit)"/>
    /// method.
    /// </remarks>
    void Add(string key, object value, long duration, TimeUnit unit);

    /// <summary>
    /// Discards any cached value for key <paramref name="key"/>, so that a
    /// future invocation of <see cref="Get(T)"/> will result in a cache miss
    /// and reload.
    /// </summary>
    void Remove(string key);

    /// <summary>
    /// Discards all entries in the cache.
    /// </summary>
    void Clear();
  }
}