using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="IFuture{T}"/> whose result may be set by a <see cref="Set"/>
  /// or <see cref="SetException"/> call. it may also be cancelled.
  /// </summary>
  public class SettableFuture<T> : AbstractFuture<T>
  {
    /// <summary>
    /// Explicit private constructor, use the <see cref="Create"/> factory
    /// method to create instances of <see cref="SettableFuture{T}"/>.
    /// </summary>
    SettableFuture() { }

    /// <summary>
    /// Creates a new <see cref="SettableFuture{T}"/> in the default state.
    /// </summary>
    /// <returns></returns>
    public static SettableFuture<T> Create() {
      return new SettableFuture<T>();
    }

    /// <summary>
    /// Sets the value of this future.
    /// </summary>
    /// <param name="value">The value the future should hold.</param>
    /// <returns><c>true</c> if the value was successfully set, or
    /// <c>false</c> if the future has already been set or cancelled.
    /// </returns>
    public new bool Set(T value) {
      return base.Set(value);
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
    public new bool SetException(Exception exception) {
      return base.SetException(exception);
    }
  }
}
