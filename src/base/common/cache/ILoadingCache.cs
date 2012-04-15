using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A semi-persistent mapping from keys to values that uses the pluggable
  /// cache mechanism provided by the <see cref="ICacheProvider"/> interface.
  /// </summary>
  /// <remarks>
  /// Cache entries are automatically loaded by the cache, and are stored in
  /// the cache until either evicted or manually invalidated.
  /// <para>Implementations of this interface are expected to the
  /// thread-safe, and can be safely accessed by multiple concurrent threads.
  /// </para>
  /// </remarks>
  /// <typeparam name="T">The type of objects that the cache can store.
  /// </typeparam>
  public interface ILoadingCache<T> : ICache<T>
  {
    /// <summary>
    /// Gets the value associated with <paramref name="key"/> in this cache,
    /// first loading that value if necessary. No observable state associated
    /// with this cache is modified until loading completes.
    /// </summary>
    /// <param name="key">The key that will be associated with the cached
    /// value.</param>
    /// <returns>The value associated with <paramref name="key"/>.</returns>
    /// <exception cref="TypeLoadException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <remarks>If another call to <see cref="Get(string)"/> is currently
    /// loading the value for <paramref name="key"/>, simple waits for that
    /// thread to finish and returns its loaded value. Note that multiple
    /// threads can concurrently load values for distinct key.
    /// <para>Caches loaded by a <see cref="CacheLoader{T}"/> will call
    /// <see cref="CacheLoader{T}.Load"/> to load new values into cache. Newly
    /// loaded values are added to the cache only if <paramref name="key"/>
    /// is not already associated with a value.</para>
    /// </remarks>
    T Get(string key);

    /// <summary>
    /// Loads a new value for key <see cref="key"/>, possibly asynchronously.
    /// </summary>
    /// <param name="key">
    /// The value associated with the value to get.
    /// </param>
    /// <remarks>
    /// While the new value is loading the previous value(if any) will continue
    /// to be returned by <see cref="Get(string)"/> unless it is evicted. If
    /// the new value is loaded successfully it will replace the previous
    /// value in the cache; if an exception is thrown while refreshing the
    /// previous value will remain, and the exception will be logged (using
    /// <see cref="MustLogger"/>) and swallowed.
    /// </remarks>
    void Refresh(string key);
  }
}
