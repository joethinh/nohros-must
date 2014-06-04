using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Utilities for treating interruptibles operations as uninterruptible. In
  /// all cases, if a thread is interrupted during  such a call, the call
  /// continues to block until the result is available or the timeout elapses,
  /// and only then re-interrupts the thread.
  /// </summary>
  public sealed class Uninterruptibles
  {
    /// <summary>
    /// Invokes <see cref="IFuture{T}.Get()"/> uninterruptibly.
    /// </summary>
    /// <param name="future">
    /// The future whose value should be get uninterruptibly.</param>
    /// <returns>The result of the future computation.</returns>
    /// <exception cref="ExecutionException">
    /// if the computation threw an exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the current thread was interrupted while waiting.
    /// </exception>
    /// <seealso cref="IFuture{T}"/>
    /// <seealso cref="IFuture{T}.Get()"/>
    public static T GetUninterruptibly<T>(IFuture<T> future) {
      bool interrupted = false;
      try {
        while (true) {
          try {
            return future.Get();
          } catch (ThreadInterruptedException) {
            interrupted = true;
          }
        }
      } finally {
        if (interrupted) {
          Thread.CurrentThread.Interrupt();
        }
      }
    }

    /// <summary>
    /// Invokes <see cref="IFuture{T}.Get(long, TimeUnit)"/> uninterruptibly.
    /// </summary>
    /// <param name="future">
    /// The future whose value should be get uninterruptibly.
    /// </param>
    /// <param name="timeout">
    /// The maximum time to wait.
    /// </param>
    /// <param name="unit">
    /// The time unit of the timeout argument.
    /// </param>
    /// <returns>
    /// The result of the future computation.
    /// </returns>
    /// <exception cref="ExecutionException">
    /// if the computation threw an exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the current thread was interrupted while waiting.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// If the timer expires.
    /// </exception>
    /// <seealso cref="IFuture{T}"/>
    /// <seealso cref="IFuture{T}.Get(long, TimeUnit)"/>
    public static T GetUninterruptibly<T>(IFuture<T> future, long timeout,
      TimeUnit unit) {
      bool interrupted = false;
      try {
        while (true) {
          try {
            return future.Get(timeout, unit);
          } catch (ThreadInterruptedException) {
            interrupted = true;
          }
        }
      } finally {
        if (interrupted) {
          Thread.CurrentThread.Interrupt();
        }
      }
    }

    /// <summary>
    /// Invokes <see cref="IFuture{T}.TryGet"/> uninterruptibly.
    /// </summary>
    /// <param name="future">The future whose value should be get
    /// uninterruptibly.</param>
    /// <param name="timeout">
    /// The maximum time to wait.
    /// </param>
    /// <param name="unit">
    /// The time unit of the timeout argument.
    /// </param>
    /// <param name="result">
    /// The result of the computation, or the default
    /// value for <typeparamref name="T"/> if the wait timed out.
    /// </param>
    /// <returns>
    /// The result of the future computation.
    /// </returns>
    /// <exception cref="ExecutionException">
    /// if the computation threw an exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the current thread was interrupted while waiting.
    /// </exception>
    /// <seealso cref="IFuture{T}"/>
    /// <seealso cref="IFuture{T}.TryGet(long, TimeUnit, out T)"/>
    public static bool GetUninterruptibly<T>(IFuture<T> future, long timeout,
      TimeUnit unit, out T result) {
      bool interrupted = false;
      try {
        while (true) {
          try {
            return future.TryGet(timeout, unit, out result);
          } catch (ThreadInterruptedException) {
            interrupted = true;
          }
        }
      } finally {
        if (interrupted) {
          Thread.CurrentThread.Interrupt();
        }
      }
    }
  }
}
