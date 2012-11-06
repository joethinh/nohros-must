using System;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="IFuture{T}"/> whose result may be set by a <see cref="Set"/>
  /// or <see cref="SetException"/> call. it may also be cancelled.
  /// </summary>
  public class SettableFuture<T> : AbstractFuture<T>
  {
    #region .ctor
    /// <summary>
    /// Creates a new <see cref="SettableFuture{T}"/> in the default state.
    /// </summary>
    /// <returns></returns>
    public SettableFuture() {
    }

    /// <summary>
    /// Creates a new <see cref="SettableFuture{T}"/> in the default state.
    /// </summary>
    /// <returns></returns>
    public SettableFuture(object state) : base(state) {
    }
    #endregion

    /// <summary>
    /// Sets the value of this future.
    /// </summary>
    /// <param name="value">The value the future should hold.</param>
    /// <returns><c>true</c> if the value was successfully set, or
    /// <c>false</c> if the future has already been set or cancelled.
    /// </returns>
    /// <remarks>
    /// A operation is considered synchronously when it runs in the same
    /// context(Thread) as its initiator.
    /// </remarks>
    public new bool Set(T value, bool synchronously) {
      return base.Set(value, synchronously);
    }

    /// <summary>
    /// Sets the future to having failed with the given exception.
    /// </summary>
    /// <param name="exception">
    /// The exception the future should hold.
    /// </param>
    /// <returns><c>true</c> if the exception was successfully set, or
    /// <c>false</c> if the future has already been set or cancelled.
    /// </returns>
    /// <remarks>
    /// A operation is considered synchronously when it runs in the same
    /// context(Thread) as its initiator.
    /// </remarks>
    public new bool SetException(Exception exception, bool synchronously) {
      return base.SetException(exception, synchronously);
    }
  }
}
