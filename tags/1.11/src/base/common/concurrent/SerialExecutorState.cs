using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="ExecutorState{T}"/> that encapsulates a
  /// <see cref="RunnableDelegate"/> and a <see cref="ExecutorState{T}"/>.
  /// </summary>
  /// <remarks>
  /// A <see cref="SerialExecutorState{T}"/> is a class that encapsulates the
  /// executor delegate and its associated state, so it can be used by an
  /// executor to serialize the execution of an task. It can also be used like
  /// an generic parameter to delegates that requires only one parameter such
  /// as <see cref="ParameterizedThreadStart"/> delegate.
  /// </remarks>
  public class SerialExecutorState<T> : ExecutorState<ExecutorState<T>>
  {
    readonly RunnableDelegate runnable_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SerialExecutorState{T}"/>
    /// by using the specified executor delegate and its associated state.
    /// </summary>
    /// <param name="runnable"></param>
    /// <param name="state"></param>
    public SerialExecutorState(RunnableDelegate runnable,
      ExecutorState<T> state) : base(state) {
      runnable_ = runnable;
    }
    #endregion

    /// <summary>
    /// Gets an <see cref="IExecutor{T}"/> that was encapsulated by this
    /// instance.
    /// </summary>
    public RunnableDelegate Runnable {
      get { return runnable_; }
    }
  }
}