using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  public class ExecutorState<T> {
    object runner_;
    readonly T state_;

    internal static readonly ExecutorState<T> no_state_executor_state;

    #region .ctor
    static ExecutorState() {
      no_state_executor_state = new NoStateExecutorState<T>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutorState{T}"/>
    /// class using the specified state.
    /// </summary>
    public ExecutorState(T state) {
      state_ = state;
    }
    #endregion

    /// <summary>
    /// Gets or sets the object that delegates the execution of a method, that
    /// is the method caller.
    /// </summary>
    /// <value>The object that delgates the execution of the method, taht is,
    /// the method caller.</value>
    /// <remarks> If this property is not set before the execution of the
    /// delegate, it will be set to the <see cref="IExecutor{T}"/> object that
    /// is currently running the delegate.
    /// </remarks>
    public object Runner { get; set; }

    /// <summary>
    /// Gets an user-defined object that qualifies or contains information
    /// about the method delegate that a <see cref="IExecutor{T}"/> will run.
    /// </summary>
    public T State {
      get { return state_; }
    }
  }
}