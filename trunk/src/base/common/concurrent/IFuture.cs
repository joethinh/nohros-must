using System;
using System.ComponentModel;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="IFuture{T}"/> represents the result of an asynchronous
  /// computation.
  /// </summary>
  /// <remarks>
  /// <para>
  /// <para>Applications that perform many tasks simultaneously, yet remain
  /// responsive to user interaction, often require a design that uses multiple
  /// threads. The <see cref="System.Threading"/> namespace provides all the
  /// tools nescessary to create high-performance multithreaded applications,
  /// but using these tools effectivelly requires significant experience with
  /// multithreaded software engineering. For realtive simple multithreaded
  /// applications the <see cref="BackgroundWorker"/> component provides
  /// a straightforward solution. <see cref="IFuture{T}"/> behaves like the
  /// <see cref="BackgroundWorker"/>, but they allow more flexible structuring
  /// and may have quite different properties.
  /// </para>
  /// <see cref="IFuture{T}"/> is some ways resembles the creation of a new
  /// <see cref="Thread"/> or <see cref="ThreadPool"/> work item, but with a
  /// higher level of abstraction.
  /// </para>
  /// Methods are provided to check if the computation is complete,
  /// to wait for its completion, and to retrieve the result of the computation.
  /// The result can be retrieved using the methods
  /// <see cref="Get(long, TimeUnit, out T)"/> or <see cref="Get(out T)"/> when
  /// the computation has completed, blocking if necessary until is ready.
  /// Results can also be retrieved through the use of callbacks.
  /// <see cref="IFuture{T}"/> allows you to register callbacks to be
  /// executed once the computation is complete, or if the computation is
  /// already complete, immediately. Each callback has an associated executor,
  /// and it is invoked using this executor.
  /// <para>
  /// Cancellation is performed by the <see cref="Cancel"/> method. Additional
  /// methods are provided to determine if the task completed normally or was
  /// cancelled. Once a computation has completed, the computation cannot be
  /// cancelled. If you would like to use a <see cref="IFuture{T}"/> for the
  /// sake or cancellability but not provide a usable result, you can declare
  /// types for <see cref="IFuture{T}"/> and return <c>null</c> as a
  /// result of the underlying task.
  /// </para>
  /// <para>
  /// 
  /// </para>
  /// </remarks>
  /// <typeparamref name="T"/> The result type returned by this future's
  /// <see cref="Cancel"/> method.
  public interface IFuture<T> : IAsyncResult
  {
    /// <summary>
    /// Attempts to cancel execution of this task.
    /// </summary>
    /// <param name="may_interrupt_if_running"><c>true</c> if the thread
    /// executing this task should be interrupted; otherwise, in-progress
    /// tasks are allowed to complete.</param>
    /// <returns><c>false</c> if the task could not be cancelled, typically
    /// because it has already completed normally; otherwise, true.</returns>
    /// <remarks>
    /// This attempt will fail if the task has already completed, has already
    /// been cancelled, or could not be cancelled for some reason. If
    /// successfull, and this task has not started when <c>cancel</c> is
    /// called, this task should never run. If the task has already started,
    /// then the <see cref="may_interrupt_if_running"/> parameter determines
    /// whether the thread executing this task should be interrupted in an
    /// attempt to stop the task.
    /// <para>
    /// After this method returns, subsequent calls to
    /// <see cref=" IAsyncResult.IsCompleted"/> will always return <c>true</c>.
    /// Subsequent calls to <see cref="IsCancelled"/> will alwyas return
    /// <c>true</c> if this method returned <c>true</c>.
    /// </para>
    /// </remarks>
    bool Cancel(bool may_interrupt_if_running);

    /// <summary>
    /// Waits if nescessary for the computation to complete and then retrieves
    /// its result.
    /// </summary>
    /// <returns>
    /// The computed result.
    /// </returns>
    /// <exception cref="ExecutionException">
    /// If the computation threw an exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the future computation was canceled.
    /// </exception>
    T Get();

    /// <summary>
    /// Waits if necessary for at most the given time for the computation to
    /// complete, and then retrieves its result, if available.
    /// </summary>
    /// <param name="timeout">
    /// The maximum time to wait, or <see cref="Timeout.Infinite"/> to wait
    /// indefinitely.
    /// </param>
    /// <param name="unit">
    /// The time unit of the timeout argument.
    /// </param>
    /// <returns>
    /// The result of the computation.
    /// </returns>
    /// <exception cref="ExecutionException">
    /// If the computation threw an exception.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// If the timer expires.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the future computation was canceled.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeout"/> is a negative number other than -1, which
    /// represents an infinite time-out.
    /// </exception>
    /// <remarks>
    /// If <paramref name="timeout"/> is zero the method does not block. It
    /// tests the state of the future and returns imediatelly.
    /// </remarks>
    T Get(long timeout, TimeUnit unit);

    /// <summary>
    /// Waits if necessary for at most the given time for the computation to
    /// complete, and then retrieves its result, if available.
    /// </summary>
    /// <param name="timeout">The maximum time to wait</param>
    /// <param name="unit">The time unit of the timeout argument.</param>
    /// <param name="result">
    /// The result of the computation, or the default value for
    /// <typeparamref name="T"/> if the wait timed out.
    /// </param>
    /// <returns>
    /// <c>false</c> if the timer expires, <c>true</c> if the
    /// computation has completed succesfully.
    /// </returns>
    /// <exception cref="ExecutionException">
    /// If the computation threw an exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the future computation was canceled.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeout"/> is a negative number other than -1, which
    /// represents an infinite time-out.
    /// </exception>
    /// <remarks>
    /// If <paramref name="timeout"/> is zero the method does not block. It
    /// tests the state of the future and returns imediatelly.
    /// </remarks>
    bool TryGet(long timeout, TimeUnit unit, out T result);

    /// <summary>
    /// Register a listener to be executed on the given executor.
    /// </summary>
    /// <param name="listener">
    /// The listener to run when computation is complete.</param>
    /// <param name="executor">
    /// The executor to run when the listener in.
    /// </param>
    /// <exception cref="ArgumentNullException"> if the
    /// <paramref name="listener"/> or if <paramref name="executor"/>
    /// was null.</exception>
    /// <remarks>
    /// The listener will run when the <see cref="IFuture{T}"/>'s computation
    /// is complete or, if the computation is already complete, immediately.
    /// <para>
    /// There is no guaranteed ordering of execution of listeners, but any
    /// listener added through this method is guaranteed to be called once the
    /// computation is complete.
    /// </para>
    /// <para>
    /// Exceptions thrown by a listener will be propagated up to the executor.
    /// Any exception thrown during
    /// <see cref="IExecutor.Execute(RunnableDelegate)"/> will be caught
    /// and logged.
    /// </para>
    /// </remarks>
    void AddListener(RunnableDelegate listener, IExecutor executor);

    /// <summary>
    /// Gets <c>true</c> if this task was cancelled before it completed
    /// normally.
    /// </summary>
    /// <value><c>true</c> if this task was cancelled before it completed.
    /// </value>
    bool IsCancelled { get; }
  }
}
