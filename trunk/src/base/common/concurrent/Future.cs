using System;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A asynchronous computation.
  /// </summary>
  /// <typeparam name="T">
  /// The result type returned by this <see cref="IFuture{T}.Get()"/> method.
  /// </typeparam>
  /// <remarks>
  /// This class provides a base implemetation of <see cref="IFuture{T}"/>,
  /// with methods to start and cancel a computation, query to see if the
  /// computation is complete, and retrieves the result of the computation. The
  /// result can only be retrieved when the computation has completed; the
  /// <see cref="Get"/> method will block if the computation has not yet
  /// completed. Once the computation has completed, the computation cannot
  /// be restarted or cancelled.
  /// <para>
  /// A <see cref="Future{T}"/> can be used to wrap a
  /// <see cref="CallableDelegate{T}"/> or <see cref="RunnableDelegate"/>
  /// delegates.
  /// </para>
  /// </remarks>
  public class Future<T> : AbstractFuture<T>, IRunnableFuture<T>
  {
    readonly CallableDelegate<T> callable_;
    readonly object mutex_;

    // |is_running_| is declared as an int to allow atomic operations.
    volatile int is_running_;

    #region .ctor
    Future() : this(null) {
    }

    Future(object state) : base(state) {
      is_running_ = 0;
      mutex_ = new object();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Future{T}"/> class that
    /// will, upon running, execute the given <see cref="RunnableDelegate"/>,
    /// and arrange that <see cref="AbstractFuture{T}.Get()"/> will return the
    /// given result on successful completion.
    /// </summary>
    /// <param name="runnable">
    /// The runnable task
    /// </param>
    /// <param name="result">
    /// The result to return on successful completion. If you don't need a
    /// particular result, consider using constructions of the form:
    /// <para>
    /// <code>
    /// Future{T} f = new Future(runnable, null).
    /// </code>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="runnable"/> is null.
    /// </exception>
    public Future(RunnableDelegate runnable, T result)
      : this(runnable, result, null) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Future{T}"/> class that
    /// will, upon running, execute the given <see cref="RunnableDelegate"/>,
    /// and arrange that <see cref="AbstractFuture{T}.Get()"/> will return the
    /// given result on successful completion.
    /// </summary>
    /// <param name="runnable">
    /// The runnable task
    /// </param>
    /// <param name="state">
    /// An object representing data to be used by the future.
    /// </param>
    /// <param name="result">
    /// The result to return on successful completion. If you don't need a
    /// particular result, consider using constructions of the form:
    /// <para>
    /// <code>
    /// Future{T} f = new Future(runnable, null).
    /// </code>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="runnable"/> is null.
    /// </exception>
    public Future(RunnableDelegate runnable, T result, object state) : this() {
      if (runnable == null) {
        throw new ArgumentNullException("runnable");
      }

      callable_ = () => {
        runnable();
        return result;
      };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Future{T}"/> class that
    /// will, upon running, execute the given <see cref="CallableDelegate{T}"/>.
    /// </summary>
    /// <param name="callable">
    /// The callable task.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="callable"/> is null
    /// </exception>
    public Future(CallableDelegate<T> callable) : this(callable, null) {
      if (callable == null) {
        throw new ArgumentNullException("callable");
      }
      callable_ = callable;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Future{T}"/> class that
    /// will, upon running, execute the given <see cref="CallableDelegate{T}"/>.
    /// </summary>
    /// <param name="callable">
    /// The callable task.
    /// </param>
    /// <param name="state">
    /// An object representing data to be used by the future.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="callable"/> is null
    /// </exception>
    public Future(CallableDelegate<T> callable, object state) : this(state) {
      if (callable == null) {
        throw new ArgumentNullException("callable");
      }
      callable_ = callable;
    }
    #endregion

    /// <summary>
    /// Sets this <see cref="Future{T}"/> to the result of its computation
    /// unless it has been cancelled.
    /// </summary>
    /// <remarks>
    /// A operation is considered synchronously when it runs in the same
    /// context(Thread) as its initiator.
    /// </remarks>
    public void Run() {
      Run(false);
    }

    /// <summary>
    /// Sets this <see cref="Future{T}"/> to the result of its computation
    /// unless it has been cancelled.
    /// </summary>
    /// <param name="synchronously">
    /// A value indicating if the operation is running synchronously.
    /// </param>
    /// <remarks>
    /// A operation is considered synchronously when it runs in the same
    /// context(Thread) as its initiator.
    /// </remarks>
    public void Run(bool synchronously) {
      int state = Interlocked.CompareExchange(ref is_running_, 1, 0);

      // The future is already running or completed.
      if (state != 0) {
        return;
      }

      try {
        Set(callable_(), synchronously);
      } catch (Exception exception) {
        SetException(exception, synchronously);
      }
    }
  }
}
