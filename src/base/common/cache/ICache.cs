using System;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A semi-persistent mapping from keys to values that uses the pluggable
  /// cache mechanism provided by the <see cref="ICacheProvider"/> interface.
  /// </summary>
  /// <remarks>
  /// Cache entries are manually added using
  /// <see cref="Get(string, CacheLoader{T}"/> or
  /// <see cref="Put"/> and are stored in the cache until either
  /// evicted or manually invalidated.
  /// <para>
  /// Implementations of this interface are expected to the thread-safe, and
  /// can be safely accessed by multiple concurrent threads.
  /// </para>
  /// <typeparam name="T">
  /// The type of the object that will be stored in cache.
  /// </typeparam>
  /// </remarks>
  public interface ICache<T>
  {
    /// <summary>
    /// Gets the value associated with the given key, creating or retrieving
    /// that value if necessary.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="loader">
    /// A <see cref="CacheLoader{T}"/> object that could be used to create the
    /// value if it is not present in the cache.</param>
    /// <exception cref="ExecutionException">
    /// A failure occur while loading the item using the specified loader
    /// delegate.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// No observable state associated with the cache is modified until loading
    /// is complete. This method provides a simple substitute for the
    /// conventional "if cached, return; otherwise create, cache and return"
    /// pattern.
    /// <para>
    /// Warning: <paramref name="loader"/> must not return null; it will
    /// may either return a non-null value or throw an exception.
    /// </para>
    /// </remarks>
    T Get(string key, CacheLoader<T> loader);

    /// <summary>
    /// Gets the value associated with the <paramref name="key"/> in cache, or
    /// the default value of type <typeparamref name="T"/> if there is no
    /// cached value for <paramref name="key"/>.
    /// </summary>
    /// <param name="key">
    /// The identifier for the cache item to retrieve.
    /// </param>
    /// <returns>
    /// The value associated with the <paramref name="key"/> in cache,
    /// or the default value of type <typeparamref name="T"/> if there is no
    /// cached value for <paramref name="key"/>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    T GetIfPresent(string key);

    /// <summary>
    /// Gets the value associated with the <paramref name="key"/> in cache if
    /// the given key exists in cache.
    /// </summary>
    /// <param name="key">
    /// The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the value associated with the key
    /// <paramref name="key"/> if taht key is found; otherwise, the default
    /// value for <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the given key exists in cache and the value associated
    /// with it is successfully retrieved, otherwise, false.
    /// </returns>
    bool GetIfPresent(string key, out T value);

    /// <summary>
    /// Associates <paramref name="value"/> with <paramref name="key"/> in this
    /// cache. If the cache previously contained a value associated with
    /// <paramref name="key"/>, the old values is replaced by
    /// <paramref name="value"/>.
    /// </summary>
    /// <param name="key">
    /// The identifier for the cache item to retrieve.
    /// </param>
    /// <param name="value">
    /// The value to be associated with the <paramref name="key"/> in the
    /// cache.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> or <paramref name="value"/> is <c>null</c> (
    /// for reference exceptions).
    /// </exception>
    void Put(string key, T value);

    /// <summary>
    /// Discards any cached value for the key <paramref name="key"/>, so that
    /// future invocation of <c>Get(...)</c> will result in a cache miss and
    /// reload.
    /// </summary>
    /// <param name="key">
    /// The identifier for the cache item to retrieve.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// If a value with the given key does not exists in cache this method
    /// fails silently.
    /// </remarks>
    void Remove(string key);

    /// <summary>
    /// Gets the approximate number of entries in this cache.
    /// </summary>
    long Size { get; }
  }
}