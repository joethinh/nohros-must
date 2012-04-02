using System;
using Nohros.Concurrent;
using Nohros.Logging;
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
    private const string kTypeForLogger = "[Nohros.Caching.AbstractCache.";

    /// <summary>
    /// How long after the last access to an entry in the cache will retain
    /// that entry
    /// </summary>
    protected readonly long expire_after_write_nanos_;

    /// <summary>
    /// How long after the last write to an entry in the cache will retain
    /// that entry.
    /// </summary>
    protected readonly long expire_after_access_nanos_;

    /// <summary>
    /// How long after the last write an entry becomes a candidate for refresh.
    /// </summary>
    protected readonly long refresh_nanos_;

    readonly object mutex_;

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
      refresh_nanos_ = builder.RefreshNanos;
      mutex_ = new object();
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
    /// <exception cref="ExecutionException">A failure occur while loading
    /// the item using the specified loader delegate.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> or
    /// <paramref name="loader"/> are <c>null</c>.</exception>
    public T Get(string key, CacheLoader<T> loader) {
      if (key == null || loader == null) {
        Thrower.ThrowArgumentNullException(
          key == null ? ExceptionArgument.key : ExceptionArgument.loader);
      }

      string cache_key = CacheKey(key);
      try {
        T value;
        CacheEntry<T> entry;

        // the cache provider should provide the thread safeness behavior for
        // the reading operation.
        bool ok = cache_provider_.Get(cache_key, out entry);
        if (!ok) {
          long now = Clock.NanoTime;
          if (GetLiveValue(entry, now, out value)) {
            RecordRead(entry, now);
            return ScheduleRefresh(entry, key, value, now, loader);
          }
          IValueReference<T> value_reference = entry.ValueReference;
          if (value_reference.IsLoading) {
            return WaitForLoadingValue(entry, key, value_reference);
          }
        }

        // at this point entry does not exists or is expired, so lets load
        // the value.
        if(!LockedGetOrLoad(key, loader, out value)) {
          // TODO(neylor.silva): continue from here.
        }
        return value;
      } catch(Exception e) {
        MustLogger.ForCurrentProcess.Error(kTypeForLogger + "Get]", e);
        throw new ExecutionException(e);
      }
    }

    bool LockedGetOrLoad(string key, CacheLoader<T> loader, out T value) {
      CacheEntry<T> entry;
      IValueReference<T> value_reference = null;
      LoadingValueReference<T> loading_value_reference = null;
      bool create_new_entry = true;

      lock(mutex_) {
        // re-read the time once inside the lock
        long now = Clock.NanoTime;
        if (cache_provider_.Get(key, out entry)) {
          value_reference = entry.ValueReference;
          if(value_reference.IsLoading) {
            create_new_entry = false;
          } else {
            value = value_reference.Value;
            if(IsExpired(entry, now)) {
              // TODO(neylor.silva) Notificate the caller about the
              // expiration(Reason: EXPIRED).
            } else {
              RecordLockedRead(entry, now);
              // TODO(neylor.sila): Record hits stats.
              return value;
            }

            // TODO(neylor.silva): update the cache count(size).
          }
        }

        // at this point an entry was not found or it is expired.
        if(create_new_entry) {
          loading_value_reference = new LoadingValueReference<T>();
          if(entry == null) {
            entry = new CacheEntry<T>(key);
            entry.ValueReference = loading_value_reference;
            cache_provider_.Set(key, entry);
          } else {
            // entry exists but is expired, lets update it with a new
            // loading value.
            entry.ValueReference = loading_value_reference;
          }
        }
      }

      // at this point an entry associated with the specified key exists
      // in cache, but it is a loading the value.
      if(create_new_entry) {
        try {
          // TODO (neylor.silva): Add a mechanism to detect recursive loads.
          value = LoadSync(key, loading_value_reference, loader);
          return true;
        } finally {
          // TODO (neylor.silva): Record the misses stats.
        }
      } else {
        // the entry already exists and the loading process is already
        // started. Wait for loading.
        value = WaitForLoadingValue(entry, key, value_reference);
        return true;
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

    /// <summary>
    /// Schedule a refresh for an entry.
    /// </summary>
    /// <param name="entry">The entry to be refreshed.</param>
    /// <param name="key">The key associated with the entry to be refreshed.
    /// </param>
    /// <param name="old_value">The old value of the entry.</param>
    /// <param name="now">The current time, it is used to check if the
    /// entry needs a refresh.</param>
    /// <param name="loader">A <see cref="CacheLoader{T}"/> object that is
    /// used to load the new value fro the entry.</param>
    /// <returns>The refreshed value if a refresh was performed; otherwise,
    /// the old value.</returns>
    T ScheduleRefresh(CacheEntry<T> entry, string key, T old_value, long now, CacheLoader<T> loader) {
      if(Refreshes && (now - entry.WriteTime > refresh_nanos_)) {
        T new_value;
        if(Refresh(key, loader, out new_value)) {
          return new_value;
        }
      }
      return old_value;
    }

    /// <summary>
    /// Refreshes the value associated with <paramref name="key"/>, unless
    /// another thread is already doing so.
    /// </summary>
    /// <param name="key">The key associated with the value to refresh.</param>
    /// <param name="loader">A <see cref="CacheLoader{T}"/> that is used to
    /// refresh the value.</param>
    /// <param name="value">The newly refreshed value associated with
    /// <paramref name="key"/> if the value was refreshed inline, or the
    /// default value of <typeparamref name="T"/> if another thread is
    /// performing the refresh, or if a error occurs during refresh.</param>
    /// <returns><c>true</c> if the value was refreshed inline, or <c>false</c>
    /// value for <typeparamref name="T"/> if another thread is performing
    /// the refresh, or if a error occurs during refresh.</returns>
    bool Refresh(string key, CacheLoader<T> loader , out T value) {
      LoadingValueReference<T> loading_value_reference =
        InsertLoadingValueReference(key);
      if(loading_value_reference == null) {
        value = default(T);
        return false;
      }

      IFuture<T> result = LoadAsync(key, loading_value_reference, loader);
      if(result.IsDone) {
        try {
          result.Get(out value);
          return true;
        } catch(Exception e) {
          // don't let refresh exceptions propagate; error was already logged.
        }
      }
      value = default(T);
      return false;
    }

    /// <summary>
    /// Creates a new <see cref="LoadingValueReference{T}"/> and inserts it on
    /// the cache by using the <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key that will be associated with the newly
    /// created <see cref="LoadingValueReference{T}"/>.</param>
    /// <returns>The newly inserted <see cref="LoadingValueReference{T}"/>, or
    /// null if the live value reference is already loading.</returns>
    LoadingValueReference<T> InsertLoadingValueReference(string key) {
      lock(mutex_) {
        long now = Clock.NanoTime;

        LoadingValueReference<T> loading_value_reference;

        // look for an existing entry
        CacheEntry<T> entry;
        if(cache_provider_.Get(key, out entry)) {
          IValueReference<T> value_reference = entry.ValueReference;
          if(value_reference.IsLoading) {
            // refresh is a no-op if loading is pending.
            return null;
          }

          // continue returning old value while loading
          loading_value_reference = new LoadingValueReference<T>(value_reference);
          entry.ValueReference = loading_value_reference;
          return loading_value_reference;
        }

        loading_value_reference = new LoadingValueReference<T>();
        entry = new CacheEntry<T>(key);
        entry.ValueReference = loading_value_reference;

        // send the entry to the cache provider.
        cache_provider_.Set(key, entry);
        return loading_value_reference;
      }
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

    // at most one of LoadSync/LoadAsync may be called for any given
    // LoadingValueReference.

    T LoadSync(string key, LoadingValueReference<T> loading_value_reference, CacheLoader<T> loader) {
      IFuture<T> loading_future = loading_value_reference.LoadFuture(key, loader);
      // TODO: record stats.

      T value;
      try {
        // TODO(neylor.silva) gets the future value uninterruptibly, makes
        // sense on c#.
        value = loading_future.Get();
      } finally {
        // TODO(neylor.silva):
        RemoveLoadingValue(key, loading_value_reference);
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
