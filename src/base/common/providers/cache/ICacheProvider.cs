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
  public interface ICacheProvider
  {
    /// <summary>
    /// Gets the value associated with the given key.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <returns>A value associated with the given key.</returns>
    V Get<V>(string key);

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
    /// <param name="expiry">The time at which the added object expires from
    /// now and is removed from the cache. If it's is zero, the item should
    /// never expires(although it may be deleted from the cache to make place
    /// or other items, depending on the cache provider implementation).
    /// </param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="expiry"/> to less than <c>TimeSpan.Zero</c> or to a
    /// value that exceeds the maximum system date time.
    /// </exception>
    void Set(string key, object value, TimeSpan expiry);

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
    /// parameter, use the <see cref="Set(K, V)"/> method.
    /// <para>
    /// The item added to the cache should never expires(although it may be
    /// deleted from the cache to make place or other items, depending on the
    /// cache provider implementation).
    /// </para>
    /// </remarks>
    /// <seealso cref="Set(K, V)"/>
    /// <seealso cref="Add(string key, object value)"/>
    void Add(string key, object value);

    /// <summary>
    /// Adds the specified item to the cache with expiration policy.
    /// </summary>
    /// <param name="key">The cache key used to reference the item.</param>
    /// <param name="value">The item to be added to the cache.</param>
    /// <param name="expiry">The time at which the added object expires from
    /// now and is removed from the cache. If it's is zero, the item should
    /// never expires(although it may be deleted from the cache to make place
    /// or other items, depending on the cache provider implementation).
    /// </param>
    /// <exception cref="ArgumentNullException">The <paramref name="key"/> or
    /// <paramref name="value"/> parameter is set to <c>null.</c></exception>
    /// <exception cref="InvalidOperationException">An item with the ke
    /// <paramref name="key"/> already exists in the cache.</exception>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="expiry"/> to less than <c>TimeSpan.Zero</c> or to a
    /// value that exceeds the maximum system date time.
    /// </exception>
    /// <remarks>
    /// Calls to this method should throws a exception if an item with the same
    /// <paramref name="key"/> parameter is already stored in the cache. To
    /// overwrite an existing cache item using the same <paramref name="key"/>
    /// parameter, use the <see cref="Set(K, V, TimeSpan)"/> method.
    /// </remarks>
    void Add(string key, object value, TimeSpan expiry);

    /// <summary>
    /// Discards any cached value for key <paramref name="key"/>, so that a
    /// future invocation of <see cref="Get(K)"/> will result in a cache miss
    /// and reload.
    /// </summary>
    void Remove(string key);

    /// <summary>
    /// Discards all entries in the cache.
    /// </summary>
    void Clear();
  }
}