using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Static utility methods pertaining to the <see cref="IFuture{T}"/>
  /// interface.
  /// </summary>
  public sealed class Futures
  {
    private Futures() { }
    
    /// <summary>
    /// Creates a <see cref="SettableFuture{T}"/> which has its value set
    /// immediately upon construction.
    /// </summary>
    /// <param name="value">The value that the returned future shoud hold.</param>
    /// <returns>A <see cref="IFuture{T}"/> whose value is
    /// <paramref name="value"/> and is in the completed state.</returns>
    /// <remarks>
    /// The getters just return the value. This
    /// <see cref="IFuture{T}"/> can't be cancelled or timed out and its
    /// <see cref="IFuture{T}.IsCompleted"/> property always returns
    /// <c>true</c>.
    /// </remarks>
    public static IFuture<T> ImmediateFuture<T>(T value) {
      SettableFuture<T> future = SettableFuture<T>.Create();
      future.Set(value);
      return future;
    }

    /// <summary>
    /// Creates a <see cref="SettableFuture{T}"/> which has an exception set
    /// immediately upon construction.
    /// <c>true</c>.
    /// </summary>
    /// <param name="exception">The exception that the returned future shoud
    /// hold.</param>
    /// <returns>A <see cref="IFuture{T}"/> whose has failed with exception
    /// <paramref name="exception"/> and is in the completed state.</returns>
    /// <remarks>
    /// The returned <see cref="IFuture{T}"/> object can't be cancelled or
    /// and its <see cref="IFuture{T}.IsCompleted"/> property always returns
    /// <c>true</c>.
    /// <para>
    /// Calling "get(...)" will immediately throw the provided exception
    /// wrapped in an <see cref="ExecutionException"/>.
    /// </para>
    /// </remarks>
    public static IFuture<T> ImmediateFailedFuture<T>(Exception exception) {
      if (exception == null)
        throw new ArgumentNullException("exception");

      SettableFuture<T> future = SettableFuture<T>.Create();
      future.SetException(exception);
      return future;
    }
  }
}
