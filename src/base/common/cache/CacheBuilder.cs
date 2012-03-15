using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// A builder of <see cref="ICache"/> and ILoadingCache
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class CacheBuilder<T>
  {
    long expiry_after_access_nanos_;
    long expiry_after_write_nanos_;
    long refresh_nanos_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheBuilder{T}"/> using.
    /// </summary>
    public CacheBuilder() { }
    #endregion

    /// <summary>
    /// Specifies that each entry should be automatically removed from the
    /// cache once a fixed duration has elapsed after entry's creation, the
    /// most recent replacement of its value, or its last access.
    /// </summary>
    /// <param name="duration">The length of time after an entry is last
    /// accessed that it should be automatically removed.</param>
    /// <param name="unit">The unit taht <paramref name="duration"/> is
    /// expressed in.</param>
    /// <returns>This cache builder instance with the expire after nanos
    /// set to the specified values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="duration"/> to a value that is less than zero.
    /// </exception>
    /// <remarks>Expired entries may be counted in <see cref="ICache{T}.Size"/>,
    /// but should never be visible to read or write operations.
    /// </remarks>
    public CacheBuilder<T> ExpireAfterAccessNanos(long duration, TimeUnit unit) {
      if (duration < 0) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.duration);
      }
      expiry_after_access_nanos_ = TimeUnitHelper.ToNanos(duration, unit);
      return this;
    }

    /// <summary>
    /// Specifies that each entry should be automatically removed from the
    /// cache once a fixed duration has elapsed after the entry's creation,
    /// or the most recent replacement of its value.
    /// </summary>
    /// <param name="duration">The length of time after an entry is created
    /// that it should be automatically. removed.</param>
    /// <param name="unit">The unit that <paramref name="duration"/> is
    /// expressed in.</param>
    /// <returns>This cache builder instance with the expire after nanos
    /// set to the specified values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="duration"/> to a value that is less than zero.
    /// </exception>
    /// <remarks>Expired entries may be counted in <see cref="ICache.Size"/>,
    /// but should never be visible to read or write operations.
    /// </remarks>
    public CacheBuilder<T> ExpireAfterWrite(long duration, TimeUnit unit) {
      if (duration < 0) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.duration);
      }
      expiry_after_write_nanos_ = TimeUnitHelper.ToNanos(duration, unit);
      return this;
    }

    /// <summary>
    /// Specifies that active entries are eligible for automatic refresh once
    /// a fixed duration has elapsed after the entry's creation, or the most
    /// recent replacement of its value.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    /// <remarks>
    /// <para>
    /// The semantics of refreshes are specified in
    /// <see cref="ILoadingCache{T}.Refresh()"/>, and are performed by calling
    /// <see cref="CacheLoader{T}.Load"/>.
    /// <para>
    /// As the default implementation
    /// TODO: continue.
    /// </para>
    /// </para>
    /// </remarks>
    public CacheBuilder<T>  RefreshAfterWrite(long duration, TimeUnit unit) {
      if(duration < 0) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.duration);
      }
      refresh_nanos_ = TimeUnitHelper.ToNanos(duration, unit);
      return this;
    }

    /// <summary>
    /// Gets a 64-bit integer that indicates how long after the last access to
    /// an entry in the cache will retain that entry.
    /// </summary>
    public long ExpiryAfterWriteNanos {
      get { return expiry_after_write_nanos_; }
    }

    /// <summary>
    /// Gets a 64bit integer that indicates how long after the last write to
    /// an entry in the cache will retain that entry.
    /// </summary>
    public long ExpiryAfterAccessNanos {
      get { return expiry_after_access_nanos_; }
    }

    /// <summary>
    /// Gets a 64-bit integer that indicates how long after the last write an
    /// entry becomes a candidate for refresh.
    /// </summary>
    public long RefreshNanos {
      get { return refresh_nanos_; }
    }
  }
}