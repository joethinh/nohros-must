using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// An object that executes submitted <see cref="EventHandler{TEventArgs}"/>
  /// tasks. 
  /// </summary>
  /// <remarks>
  /// This interface provides a way of decoupling task submission from
  /// the mechanics of how each task will be run, including details of thread
  /// use, scheduling, etc. An <see cref="IExecutor{T}"/> is normally used
  /// instead of explicity creating threads. For example, rather than invoking
  /// <c>new Thread(SomeDelegate).Start()</c> for each of a set of tasksm you
  /// might use:
  /// <example>
  /// <code>
  /// IExecutor executor = AnExecutor;
  /// executor.Execute(SomeDelegate);
  /// executor.Execute(SomeOtherDelegate);
  /// ...
  /// </code>
  /// </example>
  /// However, the <see cref="IExecutor{T}"/> interface does not stricly
  /// require that execution be asynchronous. In the simplest case, an executor
  /// can run the submitted task immediately in the caller's thread (default
  /// way as the delegates are executed).
  /// <example>
  /// <code>
  /// class DirectExecutor :IExecutor {
  ///   public void Execute(RunnableDelegate{T} runnable, ExecutorState{T} state) {
  ///     runnable(state);
  ///   }
  /// }
  /// </code>
  /// </example>
  /// More typically, tasks are executed in some thread other than the caller's
  /// thread. The executor below spawns a new thread:
  /// <example>
  /// <code>
  /// class ThreadPerTaskExecutor : IExecutor{T} {
  ///   public void Execute(RunnableDelegate{T} runnable, ExecutorState{T} state) {
  ///     SerialExecutorState{T} serial_state =
  ///       new SerialExecutorState(runnable, state);
  /// 
  ///     Thread thread = new Thread(
  ///       new ParameterizedThreadStart(ThreadStart);
  ///     );
  ///     thread.Start(serial_state)
  ///   }
  /// 
  ///   static void ThreadStart(object obj) {
  ///     SerialExecutorState{T} serial_state = obj as SerialExecutorState{T};
  ///     serial_state.Runnable(serial_state.State);
  ///   }
  /// }
  /// </code>
  /// </example>
  /// Many <see cref="IExecutor{T}"/> implementations impose some sort of
  /// limitation on how and when tasks are scheduled. The executor below
  /// serializes the submission of tasks to a second executor, illustrating
  /// a composite executor.
  /// <example>
  /// <code>
  ///   Queue{SerialExecutorState{T}} tasks_ = Queue{SerialExecutorState{T}}();
  ///   IExecutor executor_;
  ///   SerialExecutorState{T} active_;
  /// 
  ///   public SerialExecutor(IExecutor{T} executor) {
  ///     executor_ = executor;
  ///   }
  /// 
  ///   public void Execute(RunnableDelegate{T} runnable, ExecutorState{T} state) {
  ///     SerialExecutorState{T} serial_state =
  ///       new SerialExecutorState(runnable, state);
  ///     tasks_.Enqueue(serial_state);
  /// 
  ///     if(active == null) {
  ///       ScheduleNext();
  ///     }
  ///   }
  ///   
  ///   public ScheduleNext() {
  ///     if(tasks_.Count > 0) {
  ///       SerialExecutorState{T} serial_state = tasks_.Dequeue();
  ///       serial_state.Runnable(serial_state.State);
  ///       ScheduleNext();
  ///     }
  ///   }
  /// }
  /// </code>
  /// </example>
  /// The <see cref="IExecutor{T}"/> implementations provided in this assembly
  /// implement <see cref="IExecutorService{T}"/>, which is a more extensive
  /// interface. The <see cref="ThreadPoolExecutor"/> class uses the built-in
  /// thread pool to run tasks. The <see cref="Executors"/> class provides
  /// convenient factory methods for these executors.
  /// </remarks>
  public interface IExecutor
  {
    /// <summary>
    /// Executes a given command at some time in the future.
    /// </summary>
    /// <param name="runnable">The runnable task.</param>
    /// <exception cref="ArgumentNullException"><paramref name="runnable"/> is
    /// <c>null</c>.</exception>
    /// <remarks>The command may execute in a new thread, in a pooled thread,
    /// or in the calling thread, at the discretion of the
    /// <see cref="IExecutor"/> implementation.</remarks>
    void Execute(RunnableDelegate runnable);
  }
}