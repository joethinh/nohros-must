using System;
using System.Threading;
using Nohros.Resources;

namespace Nohros.Concurrent
{
  /// <summary>
  /// An abstract implementation of <see cref="IFuture{T}"/> interface.
  /// </summary>
  /// <remarks>
  /// This class implements all methods in <see cref="IFuture{T}"/>. Subclasses
  /// should provide a way to set the result of the computation through the
  /// protected methods <see cref="Set"/> and
  /// <see cref="SetException(Exception)"/>. Subclasses may also override
  /// <see cref="InterruptTask"/>, which will be invoked automatically if a
  /// call to <see cref="Cancel(bool)"/> succeeds in canceling the future.
  /// <para>
  /// The state changing methods all returns a boolean indicating success or
  /// failure in changing the future's state. Valid states are running,
  /// completed, failed or cancelled.
  /// </para>
  /// <para>
  /// This class uses an <see cref="ExecutionList"/> to guarantee that all
  /// registered listeners will be executed, either when the future finishes
  /// or, for listeners are added after the future completes., immediatelly.
  /// <see cref="RunnableDelegate"/> - <see cref="IExecutor"/> pairs are stored
  /// in the execution list but are not necessarily executed in the order in
  /// which they are added (If a listener is added after the
  /// <see cref="IFuture{T}"/> is complete, it will be executed immediately,
  /// even if earlier listeners have not been executed. Additionally, executors
  /// need not guarantee FIFO execution, or different listeners may run in
  /// different executors).
  /// </para>
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  public abstract class AbstractFuture<T> : IFuture<T>
  {
    /// <summary>
    /// The state of the future computation.
    /// </summary>
    protected enum FutureState
    {
      /// <summary>
      /// The future computation is running.
      /// </summary>
      Running = 0,

      /// <summary>
      /// The future computation is completing.
      /// </summary>
      Completing = 1,

      /// <summary>
      /// The future computation is completed.
      /// </summary>
      Completed = 2,

      /// <summary>
      /// The future computation is cancelled.
      /// </summary>
      Cancelled = 4
    }

    readonly object async_state_;

    readonly ExecutionList execution_list_;
    readonly ManualResetEvent sync_;

    Exception exception_;

    // |state_| was declared as an int to allow atomic operation.
    volatile int state_;
    T value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractFuture{T}"/>
    /// class.
    /// </summary>
    protected AbstractFuture() {
      execution_list_ = new ExecutionList();
      exception_ = null;
      value_ = default(T);
      sync_ = new ManualResetEvent(false);
      async_state_ = null;
      state_ = (int) FutureState.Running;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractFuture{T}"/>
    /// class using the specified asynchrnous state.
    /// </summary>
    /// <param name="async_state">
    /// An object representing data to be used bt the future.
    /// </param>
    protected AbstractFuture(object async_state) : this() {
      async_state_ = async_state;
    }
    #endregion

    WaitHandle IAsyncResult.AsyncWaitHandle {
      get { return sync_; }
    }

    public virtual object AsyncState {
      get { return async_state_; }
    }

    bool IAsyncResult.CompletedSynchronously {
      get { return false; }
    }

    /// <inheritdoc/>
    public bool Cancel(bool may_interrupt_if_running) {
      if (!Cancel()) {
        return false;
      }
      execution_list_.Execute();
      if (may_interrupt_if_running) {
        InterruptTask();
      }
      return true;
    }

    /// <inheritdoc/>
    public bool IsCancelled {
      get { return state_ == (int) FutureState.Cancelled; }
    }

    /// <inheritdoc/>
    public bool IsCompleted {
      get {
        return (state_ &
          ((int) FutureState.Completed | (int) FutureState.Cancelled)) != 0;
      }
    }

    /// <inheritdoc/>
    public T Get() {
      T result;
      if (!TryGet(Timeout.Infinite, out result)) {
        throw new NotReachedException();
      }
      return result;
    }

    /// <inheritdoc/>
    public T Get(long timeout, TimeUnit unit) {
      T result;
      if (!TryGet(timeout, unit, out result)) {
        throw new TimeoutException("Timeout waiting for task");
      }
      return result;
    }

    /// <inheritdoc/>
    public bool TryGet(long timeout, TimeUnit unit, out T result) {
      if (timeout < -1) {
        throw new ArgumentOutOfRangeException("timeout",
          StringResources.ArgumentOutOfRange_NeedNonNegNum);
      }
      return TryGet((int) TimeUnitHelper.ToMillis(timeout, unit), out result);
    }

    public void AddListener(RunnableDelegate listener, IExecutor executor) {
      execution_list_.Add(listener, executor);
    }

    bool TryGet(int timeout_ms, out T result) {
      if (!IsCompleted) {
        if (sync_.WaitOne(timeout_ms)) {
          result = GetValue();
          return true;
        }
        result = default(T);
        return false;
      }
      result = GetValue();
      return true;
    }

    /// <summary>
    /// Subclasses should invoke this method to set the result of the
    /// computation to <see cref="value"/>. This will set the state of the
    /// future to COMPLETED and invoke the listeners if the state was
    /// successfully changed.
    /// </summary>
    /// <param name="value">
    /// The value that was the result of the task.
    /// </param>
    /// <returns>
    /// <c>true</c> if the state was succesfully changed.
    /// </returns>
    protected virtual bool Set(T value) {
      bool completed = Complete(value, null, FutureState.Completed);
      if (completed) {
        execution_list_.Execute();
      }
      return completed;
    }

    /// <summary>
    /// Subclasses should invoke this method to set the result of the
    /// computation to an error. This will set the state of the future to
    /// COMPLETED and invoke the listeners if the state was successfully
    /// changed.
    /// </summary>
    /// <param name="exception">
    /// The exception that the task failed with.
    /// </param>
    /// <returns>
    /// <c>true</c> if the state was successfully changed.
    /// </returns>
    protected virtual bool SetException(Exception exception) {
      bool completed = Complete(default(T), exception, FutureState.Completed);
      if (completed) {
        execution_list_.Execute();
      }
      return completed;
    }

    /// <summary>
    /// Transition to the CANCELED state.
    /// </summary>
    bool Cancel() {
      return Complete(default(T), null, FutureState.Completed);
    }

    /// <summary>
    /// Implemetaion of the actual value retrieval.
    /// </summary>
    /// <returns>
    /// The computed value.
    /// </returns>
    /// <exception cref="ExecutionException"> if the task completed with an
    /// error.</exception>
    /// <exception cref="InvalidOperationException">You try to get the value
    /// while it is computing (IsCompleted = false).</exception>
    T GetValue() {
      FutureState state = (FutureState) state_;
      switch (state) {
        case FutureState.Completed:
          if (exception_ != null) {
            throw new ExecutionException(exception_);
          }
          return value_;

        case FutureState.Cancelled:
          throw new OperationCanceledException("Task was cancelled");

        default:
          throw new InvalidOperationException(
            "Future is in a invalid state: " + state);
      }
    }

    /// <summary>
    /// Subclasses can override this method to implement interruption of
    /// the future's computation.
    /// </summary>
    /// <remarks>
    /// The method is invoked automatically by a successfull call to
    /// <see cref="Cancel(bool)"/>.
    /// <para>
    /// The default implementation does nothing.
    /// </para>
    /// </remarks>
    protected virtual void InterruptTask() {
    }

    /// <summary>
    /// Implementation of completing a task.
    /// </summary>
    /// <param name="value">
    /// The value to set as the result of the computation.
    /// </param>
    /// <param name="exception">
    /// The exception to set as the result of the computation.
    /// </param>
    /// <param name="final_state">
    /// The state to transiton to.
    /// </param>
    /// <remarks>
    /// Either <see cref="value"/> or <see cref="exception"/> will be set but
    /// not both. The <see cref="final_state"/> is the state to change to from
    /// RUNNING. If the state is not in the RUNNING state we return
    /// <c>false</c> after waiting for the state to be set to a valid final
    /// state(COMPLETED or CANCELLED).
    /// </remarks>
    /// <returns>
    /// <c>false</c> if the state is not in the RUNNING state; otherwise,
    /// <c>true</c>.
    /// </returns>
    bool Complete(T value, Exception exception, FutureState final_state) {
      int completion = Interlocked.CompareExchange(ref state_,
        (int) FutureState.Completing, (int) FutureState.Running);

      if (completion == (int) FutureState.Running) {
        // If this thread succesfully transitioned to Completing, set the
        // value and exception and then release to the final state.
        value_ = value;
        exception_ = exception;
        state_ = (int) final_state;
        sync_.Set();
      } else if (state_ == (int) FutureState.Completing) {
        // If some thread is currently completing the future, block until
        // they are done so we can guarantee completion.
        sync_.WaitOne();
      }
      return completion == (int) FutureState.Running;
    }

    protected FutureState State {
      get { return (FutureState) state_; }
    }
  }
}
