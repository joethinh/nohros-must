using System;
using Nohros.Logging;
using Nohros.Concurrent;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  /// <summary>
  /// A generic implementation of the <see cref="ILoadingCache{T}"/> interface
  /// that uses the pluggable cache mechanins provided by the
  /// <see cref="ICacheProvider"/> interface.
  /// </summary>
  /// <seealso cref="ICacheProvider"/>
  public partial class LoadingCache<T> : ILoadingCache<T>
  {
    const string kTypeForLogger = "[Nohros.Caching.AbstractCache.";
    static readonly IExecutor same_thread_executor_thread;

    readonly string cache_guid_;
    readonly ICacheProvider cache_provider_;
    readonly CacheLoader<T> default_cache_loader_;

    // How long after the last write to an entry in the cache will retain
    // that entry.
    readonly long expire_after_access_nanos_;

    // How long after the last access to an entry in the cache will retain
    // that entry.
    readonly long expire_after_write_nanos_;

    // synchronization object for thread-safeness.
    readonly object mutex_;

    // How long after the last write an entry becomes a candidate for refresh.
    readonly long refresh_nanos_;

    readonly StrengthType strength_type_;

    // this member is used to check at construction time if the T parameter
    // is a value type. It exists to avoid checking the type of the parameter
    // each time a delegate returns something.
    readonly bool t_is_value_type_;

    // the executor used to execute refreshes.

    // The default cache loader to use on loading operations.

    #region .ctor
    /// <summary>
    /// Initializes the static members.
    /// </summary>
    static LoadingCache() {
      same_thread_executor_thread = Executors.SameThreadExecutor();
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingCache{T}"/> class
    /// by using the specified cache provider and builder.
    /// </summary>
    /// <param name="provider">
    /// A <see cref="ICacheProvider"/> that is used to cache object.
    /// </param>
    /// <param name="builder">
    /// A <see cref="CacheBuilder{T}"/> object that contains information about
    /// the cache configuration.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="provider"/> or <paramref name="builder"/> are
    /// <c>null</c>.
    /// </exception>
    public LoadingCache(ICacheProvider provider, CacheBuilder<T> builder) {
      if (provider == null || builder == null) {
        Thrower.ThrowArgumentNullException(provider == null
          ? ExceptionArgument.provider
          : ExceptionArgument.builder);
      }

      cache_provider_ = provider;
      cache_guid_ = Guid.NewGuid().ToString("N");
      expire_after_access_nanos_ = builder.ExpiryAfterAccessNanos;
      expire_after_write_nanos_ = builder.ExpiryAfterWriteNanos;
      refresh_nanos_ = builder.RefreshNanos;
      strength_type_ = builder.ValueStrength;
      mutex_ = new object();
      t_is_value_type_ = typeof (T).IsValueType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingCache{T}"/> using the
    /// specified cache provider, builder and automatic loader.
    /// </summary>
    /// <param name="provider">
    /// A <see cref="ICacheProvider"/> that is used to cache object.
    /// </param>
    /// <param name="builder">
    /// A <see cref="CacheBuilder{T}"/> object that contains information about
    /// the cache configuration.
    /// </param>
    /// <param name="loader">
    /// An <see cref="CacheLoader{T}"/> that is used to automatically load new
    /// items in cache when an item for a given key does not exists or is
    /// expired.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="provider"/>, <paramref name="builder"/> or
    /// <paramref name="loader"/> are <c>null</c>.
    /// </exception>
    public LoadingCache(ICacheProvider provider, CacheBuilder<T> builder,
      CacheLoader<T> loader) : this(provider, builder) {
      if (loader == null) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.loader);
      }
      default_cache_loader_ = loader;
    }
    #endregion

    #region ILoadingCache<T> Members
    /// <inheritdoc/>
    public T GetIfPresent(string key) {
      T value;
      GetIfPresent(key, out value);
      return value;
    }

    /// <inheritdoc/>
    public bool GetIfPresent(string key, out T value) {
      if (key == null) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.key);
      }

      // TODO(neylor.silva) check if the cache is empty.
      long now = Clock.NanoTime;
      CacheEntry<T> entry = GetLiveEntry(key, now);
      if (entry != null) {
        value = entry.ValueReference.Value;
        if (!IsNull(value)) {
          // TODO(neylor.silva): record the cache hit.
          RecordRead(entry, now);
          value = ScheduleRefresh(entry, key, value, now, default_cache_loader_);
          return true;
        }
      } else {
        // TODO(neylor.silva): record the cache misses
      }
      value = default(T);
      return false;
    }

    /// <inheritdoc/>
    public void Put(string key, T value) {
      if (key == null || IsNull(value)) {
        Thrower.ThrowArgumentNullException(key == null
          ? ExceptionArgument.key
          : ExceptionArgument.value);
      }
      lock (mutex_) {
        long now = Clock.NanoTime;
        CacheEntry<T> entry = cache_provider_.Get<CacheEntry<T>>(key);
        if (entry != null) {
          // TODO(neylor.silva): notify the caller about the item removal
          // REPLACED.
        } else {
          // create a new entry
          entry = new CacheEntry<T>(key);
        }

        // add the clobber/new entry entry to cache.
        SetValue(entry, key, value, now);
      }
    }

    /// <inheritdoc/>
    public T Get(string key) {
      return Get(key, default_cache_loader_);
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
        if (ok) {
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
        return LockedGetOrLoad(key, loader);
      } catch (Exception e) {
        MustLogger.ForCurrentProcess.Error(kTypeForLogger + "Get]", e);
        throw new ExecutionException(e);
      }
    }

    /// <summary>
    /// Discards any cached value for the key <paramref name="key"/>, so that
    /// future invocation of <c>Get(...)</c> will result in a cache miss and
    /// reload.
    /// </summary>
    /// <param name="key">The identifier for the cache item to retrieve.
    /// </param>
    public void Remove(string key) {
      long now = Clock.NanoTime;
      CacheEntry<T> entry = cache_provider_.Get<CacheEntry<T>>(key);
      if (entry != null) {
        // Notify the caller about the item removal(EXPLICIT).
      }
      cache_provider_.Remove(key);
    }

    public long Size {
      get {
        // TODO(neylor.silva): Implement this.
        throw new NotImplementedException();
      }
    }

    /// <inheritdoc/>
    public void Refresh(string key) {
      T value;
      Refresh(key, default_cache_loader_, out value);
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

    /// <summary>
    /// Atomically get or loads and get the value for the specified key.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <param name="loader">
    /// A <see cref="CacheLoader{T}"/> that could be used to load the value
    /// for the key <paramref name="key"/>.
    /// </param>
    /// <returns>
    /// A object of type <typeparamref name="T"/> that is associated with the
    /// key <paramref name="key"/>.
    /// </returns>
    T LockedGetOrLoad(string key, CacheLoader<T> loader) {
      CacheEntry<T> entry;
      IValueReference<T> value_reference = null;
      LoadingValueReference<T> loading_value_reference = null;
      bool create_new_entry = true;

      lock (mutex_) {
        // re-read the time once inside the lock
        long now = Clock.NanoTime;
        if (cache_provider_.Get(key, out entry)) {
          value_reference = entry.ValueReference;
          if (value_reference.IsLoading) {
            create_new_entry = false;
          } else {
            T value = value_reference.Value;
            if (IsExpired(entry, now)) {
              // TODO(neylor.silva) Notificate the caller about the
              // expiration(Reason: EXPIRED).
            } else {
              RecordRead(entry, now);
              // TODO(neylor.sila): Record hits stats.
              return value;
            }

            // TODO(neylor.silva): Update the cache count(size).
          }
        }

        // at this point an entry was not found or it is expired.
        if (create_new_entry) {
          loading_value_reference = new LoadingValueReference<T>();
          if (entry == null) {
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
      if (create_new_entry) {
        try {
          // TODO (neylor.silva): Add a mechanism to detect recursive loads.
          return LoadSync(key, loading_value_reference, loader);
        } finally {
          // TODO (neylor.silva): Record the misses stats.
        }
      } else {
        // the entry already exists and the loading process is already
        // started. Wait for loading.
        return WaitForLoadingValue(entry, key, value_reference);
      }
    }

    /// <summary>
    /// Gets the value associated with the given entry.
    /// </summary>
    /// <param name="entry">
    /// The entry to get the value from.
    /// </param>
    /// <param name="now">
    /// The date and time to use as reference while checking for expiration.
    /// </param>
    /// <param name="value">
    /// The value associated with the specified entry if it is valid(not
    /// expired or loading).
    /// </param>
    /// <returns>
    /// <c>false</c> if entry is invalid, loading, or expired; otherwise,
    /// <c>true</c>.
    /// </returns>
    bool GetLiveValue(CacheEntry<T> entry, long now, out T value) {
      value = entry.ValueReference.Value;
      if (IsNull(value)) {
        value = default(T);
        return false;
      }
      return IsExpired(entry, now);
    }

    /// <summary>
    /// Gets the entry associated with the given key.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <param name="now">
    /// The time used as reference for expiration check.
    /// </param>
    /// <returns>
    /// A <see cref="IValueReference{T}"/> associated with the key
    /// <paramref name="key"/> if it is not expired.
    /// </returns>
    CacheEntry<T> GetLiveEntry(string key, long now) {
      CacheEntry<T> entry = cache_provider_.Get<CacheEntry<T>>(key);
      if (entry == null) {
        return null;
      } else if (IsExpired(entry, now)) {
        return null;
      }
      return entry;
    }

    /// <summary>
    /// Records the time the entry was written.
    /// </summary>
    /// <param name="entry">
    /// The entry to update the write time.
    /// </param>
    /// <param name="now">
    /// The time that the entry was written.
    /// </param>
    void RecordWrite(CacheEntry<T> entry, long now) {
      if (RecordsWrite) {
        entry.WriteTime = now;
      }
    }

    /// <summary>
    /// Records the time the entry was accessed.
    /// </summary>
    /// <param name="entry">The accessed entry.</param>
    /// <param name="now">The time taht <paramref name="entry"/> was
    /// last accessed.</param>
    void RecordRead(CacheEntry<T> entry, long now) {
      if (RecordsAccess) {
        entry.AccessTime = now;
      }
    }

    /// <summary>
    /// Schedule a refresh for an entry.
    /// </summary>
    /// <param name="entry">
    /// The entry to be refreshed.
    /// </param>
    /// <param name="key">
    /// The key associated with the entry to be refreshed.
    /// </param>
    /// <param name="old_value">
    /// The old value of the entry.
    /// </param>
    /// <param name="now">
    /// The current time, it is used to check if the entry needs a refresh.
    /// </param>
    /// <param name="loader">
    /// A <see cref="CacheLoader{T}"/> object that is used to load the new
    /// value for the entry.
    /// </param>
    /// <returns>
    /// The refreshed value if a refresh was performed; otherwise, the old
    /// value.
    /// </returns>
    T ScheduleRefresh(CacheEntry<T> entry, string key, T old_value, long now,
      CacheLoader<T> loader) {
      if (Refreshes && (now - entry.WriteTime > refresh_nanos_)) {
        T new_value;
        if (Refresh(key, loader, out new_value)) {
          return new_value;
        }
      }
      return old_value;
    }

    /// <summary>
    /// Refreshes the value associated with <paramref name="key"/>, unless
    /// another thread is already doing so.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to refresh.
    /// </param>
    /// <param name="loader">
    /// A <see cref="CacheLoader{T}"/> that is used to refresh the value.
    /// </param>
    /// <param name="value">
    /// The newly refreshed value associated with <paramref name="key"/> if
    /// the value was refreshed inline, or the default value of
    /// <typeparamref name="T"/> if another thread is performing the refresh or
    /// if a error occurs during refresh.
    /// </param>
    /// <returns>
    /// <c>true</c> if the value was refreshed and <c>false</c> if another
    /// thread is performing the refresh or if a error has been occured during
    /// the refresh.
    /// </returns>
    bool Refresh(string key, CacheLoader<T> loader, out T value) {
      LoadingValueReference<T> loading_value_reference =
        InsertLoadingValueReference(key);
      value = default(T);
      if (loading_value_reference == null) {
        return false;
      }

      IFuture<T> result = LoadAsync(key, loading_value_reference, loader);
      if (result.IsCompleted) {
        try {
          value = result.Get();
        } catch {
          // don't let refresh exceptions propagate; error was already logged.
        }
      }
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
      lock (mutex_) {
        long now = Clock.NanoTime;

        LoadingValueReference<T> loading_value_reference;

        // look for an existing entry
        CacheEntry<T> entry;
        if (cache_provider_.Get(key, out entry)) {
          IValueReference<T> value_reference = entry.ValueReference;
          if (value_reference.IsLoading) {
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

    /// <summary>
    /// Blocks while waiting for the value to load.
    /// </summary>
    /// <param name="entry">
    /// The entry that references the loading value reference.</param>
    /// <param name="key">
    /// The key associated with the cache entry.
    /// </param>
    /// <param name="value_reference">
    /// 
    /// </param>
    /// <returns>
    /// The loaded value.
    /// </returns>
    T WaitForLoadingValue(CacheEntry<T> entry, string key,
      IValueReference<T> value_reference) {
      // TODO (neylor.silva): Add a mechanism to detect recursive load.
      try {
        T value = value_reference.WaitForValue();
        if (!t_is_value_type_ && (object) value == null) {
          throw new InvalidCacheLoadException(
            "CacheLoader returned null for key " + key + ".");
        }

        // re-read time now that loading has completed.
        long now = Clock.NanoTime;
        RecordRead(entry, now);
        return value;
      } finally {
        // TODO(neylor.silva): Record the cache misses statistics.
      }
    }

    // At most one of LoadSync/LoadAsync may be called for any given
    // LoadingValueReference.

    T LoadSync(string key, LoadingValueReference<T> loading_value_reference,
      CacheLoader<T> loader) {
      IFuture<T> loading_future = loading_value_reference.LoadFuture(key, loader);
      return GetUninterruptibly(key, loading_value_reference, loading_future);
    }

    IFuture<T> LoadAsync(string key,
      LoadingValueReference<T> loading_value_reference, CacheLoader<T> loader) {
      IFuture<T> loading_future = loading_value_reference.LoadFuture(key, loader);
      loading_future.AddListener(delegate()
      {
        try {
          T new_value = GetUninterruptibly(key, loading_value_reference,
            loading_future);
          // update loading future for the sake of other pending requests.
          loading_value_reference.Set(new_value);
        } catch (Exception exception) {
          MustLogger.ForCurrentProcess.Warn("Exception thrown during refresh",
            exception);
        }
      }, same_thread_executor_thread);
      return loading_future;
    }

    /// <summary>
    /// Waits uninterruptibly for <paramref name="new_value"/> to be loaded.
    /// </summary>
    /// <param name="key">The key associated with the laoding value.</param>
    /// <param name="loading_value_reference"></param>
    /// <param name="new_value"></param>
    /// <returns></returns>
    T GetUninterruptibly(string key,
      LoadingValueReference<T> loading_value_reference, IFuture<T> new_value) {
      T value = default(T);
      try {
        value = Uninterruptibles.GetUninterruptibly(new_value);

        // Cache loader should never returns null for reference types.
        if (IsNull(value)) {
          throw new InvalidCacheLoadException(
            "CacheLoader returned a null for key " + key + ".");
        }
        // TODO(neylor.silva): Record load success stats.
        StoreLoadedValue(key, loading_value_reference, value);
        return value;
      } finally {
        if (IsNull(value)) {
          // TODO(neylor.silva): Record load exception stats.
          RemoveLoadingValue(key, loading_value_reference);
        }
      }
    }

    /// <summary>
    /// Store the loaded value in cache.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value.
    /// </param>
    /// <param name="old_value_reference">
    /// A <see cref="LoadingValueReference{T}"/> that was used to load the
    /// value.
    /// </param>
    /// <param name="new_value">
    /// The value to be stored in cache.
    /// </param>
    /// <returns>
    /// <c>true</c> if the value was successfully stored in cache; otherwise,
    /// false.
    /// </returns>
    /// <remarks>
    /// This method is called to store the result of the loading computation.
    /// If the value was sucessfully loaded it will be stored in cache using
    /// the given key.
    /// <para>
    /// This method fails to store the value if a value for the given key was
    /// already replaced by another item, while it is loading.
    /// </para>
    /// </remarks>
    bool StoreLoadedValue(string key,
      LoadingValueReference<T> old_value_reference, T new_value) {
      lock (mutex_) {
        long now = Clock.NanoTime;
        CacheEntry<T> entry;
        if (cache_provider_.Get(key, out entry)) {
          IValueReference<T> value_reference = entry.ValueReference;
          if (old_value_reference == value_reference) {
            // If the old value is still active notify the caller that we
            // are replacing it.
            if (old_value_reference.IsActive) {
              // TODO(neylor.silva): Notify the caller about the removal cause
              // of the item from the cache(REPLACED).
            }
            SetValue(entry, key, new_value, now);
            return true;
          }

          // the loaded value was already clobbered.
          // TODO(neylor.silva): Notify the caller about the removal cause
          // of the item from the cache(REPLACED).
          return false;
        }

        // an entry for the given key does not exist yet, create a new one.
        entry = new CacheEntry<T>(key);
        SetValue(entry, key, new_value, now);
        return true;
      }
    }

    /// <summary>
    /// Sets a new value of an entry and add that entry in cache.
    /// </summary>
    /// <param name="entry">
    /// The entry to set the new value.
    /// </param>
    /// <param name="key">
    /// The key for the entry.
    /// </param>
    /// <param name="now">
    /// The date and time where the value was created.
    /// </param>
    /// <param name="value">
    /// The value set.
    /// </param>
    void SetValue(CacheEntry<T> entry, string key, T value, long now) {
      // TODO(neylor.silva): Implement the use of soft and weak references?
      entry.ValueReference = Strength.ReferenceValue(entry, value,
        strength_type_);
      RecordWrite(entry, now);
      cache_provider_.Set(key, entry);
    }

    /// <summary>
    /// Checks if the given value is represents a <c>null</c> reference.
    /// </summary>
    /// <param name="value">
    /// The value to check for nullability.
    /// </param>
    /// <returns>
    /// <c>true</c> if the the given value is a reference type and is null;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method always returns true for value types, even if it is equals
    /// to the default value for <typeparamref name="T"/>.
    /// </remarks>
    bool IsNull(T value) {
      return (!t_is_value_type_ && (object) value == null);
    }

    /// <summary>
    /// Remove the loading value reference from the cache.
    /// </summary>
    /// <param name="key">The key associated with the loading value.</param>
    /// <param name="value_reference">The value reference to be removed from
    /// the cache.</param>
    /// <returns>
    /// <c>true</c> if the value is succesfully removed from the cache;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// If a loading value reference has an active value means that the loading
    /// process has failed and the old valud is still valid, in that case this
    /// method will replace the loading value reference with the old value.
    /// </remarks>
    bool RemoveLoadingValue(string key, LoadingValueReference<T> value_reference) {
      lock (mutex_) {
        CacheEntry<T> entry;
        if (cache_provider_.Get(key, out entry)) {
          IValueReference<T> value = entry.ValueReference;
          if (value == value_reference) {
            // If a loading value is active means that the new value has failed
            // to load and the old value is still valid.
            if (value_reference.IsActive) {
              entry.ValueReference = value_reference.OldValue;
            } else {
              cache_provider_.Remove(key);
            }
            return true;
          }
        }
        return false;
      }
    }

    /// <summary>
    /// Gets a value indicating if a entry is expired or not.
    /// </summary>
    /// <returns><c>true</c> if the entry is expired; otherwise, false.
    /// </returns>
    bool IsExpired(CacheEntry<T> entry, long now) {
      if (ExpireAfterAccess &&
        now - entry.AccessTime > expire_after_access_nanos_) {
        return true;
      }

      if (ExpireAfterWrite && now - entry.WriteTime > expire_after_write_nanos_) {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Gets a value indicating if the cache allows items to be refreshed.
    /// </summary>
    bool Refreshes {
      get { return refresh_nanos_ > 0; }
    }

    /// <summary>
    /// Gets a value indicating if the expiry after access expiration police is
    /// enabled.
    /// </summary>
    bool ExpireAfterAccess {
      get { return expire_after_access_nanos_ > 0; }
    }

    /// <summary>
    /// Gets a value indicating if the expiry after write expiration police is
    /// enabled.
    /// </summary>
    bool ExpireAfterWrite {
      get { return expire_after_write_nanos_ > 0; }
    }

    /// <summary>
    /// Gets a value indicating if the entry's access operation should be
    /// recorded.
    /// </summary>
    bool RecordsAccess {
      get { return ExpireAfterAccess; }
    }

    /// <summary>
    /// Gets a value indicating if the entry's write operation should be
    /// recorded.
    /// </summary>
    bool RecordsWrite {
      get { return ExpireAfterWrite || Refreshes; }
    }
  }
}
