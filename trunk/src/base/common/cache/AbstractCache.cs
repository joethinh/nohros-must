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
  internal abstract partial class AbstractCache<T> : ICache<T>
  {   
    /// <summary>
    /// How long after the last access to an entry in the cache will retain
    /// that entry
    /// </summary>
    protected readonly long expire_after_write_nanos_;

    /// <summary>
    /// Hoe long after the last write to an entry in the cache will retain
    /// that entry.
    /// </summary>
    protected readonly long expire_after_access_nanos_;

    readonly ICacheProvider cache_provider_;

    // this value is used to distinghuish the items added through this
    // class from the others items in the cache provider. Since the cache
    // provider could be used for more than one application we cannot use
    // the object.GetHashCode() method as key, because it is unique only
    // within the running application.
    readonly string cache_guid_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractCache{T}"/> class
    /// by using the specified cache provider and item loader.
    /// </summary>
    /// <param name="cache_provider">A <see cref="ICacheProvider"/> object that is
    /// used to store(cache) the items.</param>
    /// <param name="builder">A <see cref="CacheBuilder{T}"/> object that
    /// contains run-time configuration such as expiration polices.</param>
    protected AbstractCache(ICacheProvider cache_provider, CacheBuilder<T> builder) {
      cache_provider_ = cache_provider;
      cache_guid_ = Guid.NewGuid().ToString("N");
      expire_after_access_nanos_ = builder.ExpiryAfterAccessNanos;
      expire_after_write_nanos_ = builder.ExpiryAfterWriteNanos;
    }
    #endregion

    /// <summary>
    /// Generates a string that uniquely identifies the key within a cache
    /// provider.
    /// </summary>
    /// <param name="key">The key that should be unyfied.</param>
    /// <returns>A string that is globally unique.</returns>
    string CacheKey(string key) {
      return cache_guid_ + ":" + key;
    }

    /// <inheritdoc/>
    public T GetIfPresent(string key) {
      return cache_provider_.Get<T>(CacheKey(key));
    }

    /// <inheritdoc/>
    public void Add(string key, T value) {
      cache_provider_.Add(key, value);
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
    /// <exception cref="ArgumentNullException"><paramref name="key"/> or
    /// <paramref name="loader"/> are <c>null</c>.</exception>
    public T Get(string key, CacheLoader<T> loader) {
      if (key == null || loader == null) {
        Thrower.ThrowArgumentNullException(
          key == null ? ExceptionArgument.key : ExceptionArgument.loader);
      }

      string cache_key = CacheKey(key);
      CacheEntry<T> entry;

      // the cache provider should provide the thread safeness behavior.
      bool ok = cache_provider_.Get(cache_key, out entry);
      if (!ok) {
        long now = Clock.NanoTime;
        T value;
        if(GetLiveValue(entry, now, out value)) {
          RecordRead(entry);
          return ScheduleRefresh(entry, key, value, now, loader);
        }

        if(entry.IsLoading) {
          return WaitForLoadingValue(entry, key);
        }
      }
    }

    /// <summary>
    /// Gets the value from an entry.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="now"></param>
    /// <param name="value"></param>
    /// <returns><c>false</c> if entry is invalid, loading, or expired.
    /// </returns>
    bool GetLiveValue(CacheEntry<T> entry, long now, out T value) {
      IValueReference<T> value_reference = entry.ValueReference;
      if (value_reference == Unset) {
        value = default(T);
        return false;
      }
      value = value_reference.Value;
      return IsExpired(entry, now);
    }

    /// <summary>
    /// Records the time the entry was accessed.
    /// </summary>
    /// <param name="entry">The accessed entry.</param>
    /// <param name="now">The time taht <paramref name="entry"/> was
    /// last accessed.</param>
    void RecordRead(CacheEntry<T> entry, long now) {
      if(RecordAccess) {
        entry.AccessTime = now;
      }
    }

    T ScheduleRefresh(CacheEntry<T> entry, string key, T old_value, long now, CacheLoader<T> loader) {
      if(Refreshes && (now - entry.WriteTime > RefreshNanos)) {
        if(Refresh(key, loader, out new_value)) {
          return new_value;
        }
      }
      return old_value;
    }

    T WaitForLoadingValue(CacheEntry<T> entry, string key, IValueReference<T> value_reference) {
      try {
        T value = value_reference.WaitForValue();
        // re-read time now that loading has completed.
        long now = Clock.NanoTime;
        RecordRead(entry, now);
        return value;
      } catch {
        // TODO: We should log it.
        throw;
      }
    }

    /// <summary>
    /// Gets a value indicating if a entry is expired or not.
    /// </summary>
    /// <returns><c>true</c> if the entry is expired; otherwise, false.
    /// </returns>
    bool IsExpired(CacheEntry<T> entry, long now) {
      if (ExpireAfterAccess && now - entry.AccessTime > expire_after_access_nanos_) {
        return true;
      }

      if(ExpireAfterWrite && now - entry.WriteTime > expire_after_write_nanos_) {
        return true;
      }
      return false;
    }

    bool Refreshes {
      get { return refresh_nanos_ > 0; }
    }

    bool ExpireAfterAccess {
      get { return expire_after_access_nanos_ > 0; }
    }

    bool ExpireAfterWrite {
      get { return expire_after_write_nanos_ > 0; }
    }

    bool RecordAccess {
      get { return ExpireAfterAccess; }
    }

    public static IValueReference<T>  Unset {
      get { return (IValueReference<T>) UnsetValueReference.UNSET; }
    }

    /// <summary>
    /// Discards any cached value for the key <paramref name="key"/>, so that
    /// future invocation of <c>Get(...)</c> will result in a cache miss and
    /// reload.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    public void Remove(string key) {
      cache_provider_.Remove(key);
    }
  }
}
