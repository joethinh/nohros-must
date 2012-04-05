using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
  /// call to <see cref="Cancel(bool)"/> succeeds in canceling he future.
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
  /// even if earlier listeners have noe been executed. Additionally, executors
  /// need not guarantee FIFO execution, or different listeners may run in
  /// different executors).
  /// </para>
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  public abstract class AbstractFuture<T> : IFuture<T>
  {
    #region FutureState enum
    /// <summary>
    /// The state of the future computation.
    /// </summary>
    public enum FutureState
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
    #endregion

    readonly ExecutionList execution_list_;
    readonly object mutex_;

    Exception exception_;
    volatile int state_;
    T value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractFuture{T}"/>
    /// class.
    /// </summary>
    protected AbstractFuture() {
      execution_list_ = new ExecutionList();
      mutex_ = new object();
      exception_ = null;
      value_ = default(T);
    }
    #endregion

    #region IFuture<T> Members
    WaitHandle IAsyncResult.AsyncWaitHandle {
      get { throw new NotImplementedException(); }
    }

    public object AsyncState {
      get { throw new NotImplementedException(); }
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
      return GetValue();
    }

    /// <inheritdoc/>
    public T Get(long timeout, TimeUnit unit) {
      bool acquired = Monitor.TryEnter(mutex_, TimeSpan.FromMilliseconds(
        TimeUnitHelper.ToMillis(timeout, unit)));
      if (acquired) {
        try {
          return GetValue();
        } finally {
          Monitor.Exit(mutex_);
        }
      } else {
        throw new TimeoutException("Timeout waiting for task");
      }
    }

    /// <inheritdoc/>
    public bool TryGet(long timeout, TimeUnit unit, out T result) {
      bool acquired = Monitor.TryEnter(mutex_, TimeSpan.FromMilliseconds(
        TimeUnitHelper.ToMillis(timeout, unit)));
      if (acquired) {
        try {
          T value;
          result = GetValue();
          return true;
        } finally {
          Monitor.Exit(mutex_);
        }
      } else {
        result = default(T);
        return false;
      }
    }

    public void AddListener(RunnableDelegate listener, IExecutor executor) {
      execution_list_.Add(listener, executor);
    }
    #endregion

    /// <summary>
    /// Subclasses should invoke this method to set the result of the
    /// computation to <see cref="value"/>. This will set the state of the
    /// future to COMPLETED and invoke the listeners if the state was
    /// successfully changed.
    /// </summary>
    /// <param name="value">
    /// The value taht was the result of the task.
    /// </param>
    /// <returns>
    /// <c>true</c> if the state was succesfully changed.
    /// </returns>
    protected virtual bool Set(T value) {
      bool ok = Complete(value, null, FutureState.Completed);
      if (ok) {
        execution_list_.Execute();
      }
      return ok;
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
      bool ok = Complete(default(T), null, FutureState.Completed);
      if (ok) {
        execution_list_.Execute();
      }
      return ok;
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
          } else {
            return value_;
          }

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
        // If this thread succesfully transitioned to kCompleting, set the
        // value and exception and then release to the final state.
        value_ = value;
        exception_ = exception;
        state_ = (int) final_state;
      } else if (state_ == (int) FutureState.Completing) {
        lock (mutex_) {
          // If some thread is currently completing the future, block until
          // they are done so we can guarantee completion.
        }
      }
      return completion == (int) FutureState.Running;
    }
  }
}
