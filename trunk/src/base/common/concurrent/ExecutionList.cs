using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Logging;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A list of delegates that guarantess that every delegate that is added
  /// will be executed after <see cref="Execute"/> is called. Any delegate
  /// added after the call to <see cref="Execute"/> is still guarantee to
  /// execute. The delegates will be executed in the same order that they are
  /// added.
  /// </summary>
  /// <remarks>
  /// In many cases, a notification source can simply call "delegate()" to
  /// execute all the target handler methods associated with a
  /// <see lang="MulticastDelegate"/> delegate object. However, the
  /// <see lang="MulticastDelegate"/> error handling makes awereness of the
  /// sequential notification critical. If one subscriber throws an exception
  /// then later subscribers in the chain is not executed.
  /// <para>To avoid this problem, so that all subscribers execute
  /// regardless of the behavior of the earlier subscribers, you must
  /// manually enumerate through the list of subscribers and call them
  /// individually. This class may be used to avoid doing it manually.</para>
  /// <para>
  /// We just log the exceptions that are throwed by the subscribers. The
  /// <see cref="MustLogger"/> is used to log the exceptions, by default this
  /// logger logs to nothing, clients should configure the logger that they
  /// want to use.
  /// </para>
  /// </remarks>
  public class ExecutionList
  {
    #region SerialExecutorDelegatePair
    class SerialExecutorDelegatePair
    {
      readonly IExecutor executor_;
      readonly RunnableDelegate runnable_;

      #region .ctor
      public SerialExecutorDelegatePair(RunnableDelegate runnable,
        IExecutor executor) {
        runnable_ = runnable;
        executor_ = executor;
      }
      #endregion

      public void Execute() {
        try {
          executor_.Execute(runnable_);
        } catch (Exception e) {
          // Log it nad keep going. Don't punish the other delegates
          // if we're given a bad one.
          ILogger logger = MustLogger.ForCurrentProcess;
          if (logger.IsErrorEnabled) {
            logger.Error("Exception while executing delegate "
              + runnable_.ToString() + " with executor "
                + executor_.ToString(), e);
          }
        }
      }
    }
    #endregion

    readonly Queue<SerialExecutorDelegatePair> runnables_;
    bool executed_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionList{T}"/>.
    /// </summary>
    public ExecutionList() {
      runnables_ = new Queue<SerialExecutorDelegatePair>();
    }
    #endregion

    /// <summary>
    /// Adds the <see cref="EventHandler{TEventArgs}"/> to the list of
    /// listeners to execute. If execution has already begun, the listener is
    /// executed immediately.
    /// </summary>
    /// <param name="runnable"></param>
    /// <param name="executor"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="runnable"/> or <paramref name="executor"/> or are
    /// <c>null</c>.
    /// </exception>
    public void Add(RunnableDelegate runnable, IExecutor executor) {
      if (runnable == null || executor == null) {
        Thrower.ThrowArgumentNullException(
          (runnable == null)    
          ? ExceptionArgument.runnable
              : ExceptionArgument.executor);
      }

      bool execute_immediate = false;

      // Lock while we check state. We must maitain the lock while adding the
      // new pair so that another thread can't run the list out from under us.
      // We only add to the list if we have not yet started execution.
      lock (runnables_) {
        if (!executed_) {
          SerialExecutorDelegatePair pair =
            new SerialExecutorDelegatePair(runnable, executor);

          runnables_.Enqueue(pair);
        } else {
          execute_immediate = true;
        }
      }

      // Execute the runnable immediately. Because of scheduling this may end
      // up getting called before some of the previously added runnables, but
      // we're OK with that. If we want to change the contract to guarantee
      // ordering among runnables we'd have to modify the logic here to allow
      // it.
      if (execute_immediate) {
        new SerialExecutorDelegatePair(runnable, executor).Execute();
      }
    }

    /// <summary>
    /// Runs this execution list, executing all existing pairs in the order
    /// they were added.
    /// </summary>
    /// <remarks>
    /// Note that listeners added after this point may be executed before
    /// those previously added, and note that execution order of all listener
    ///  is ultimately chosen by the implementations of the supplied executors.
    /// </remarks>
    public void Execute() {
      // lock while we update our state so the add method above will finish
      // adding listeners before we start to run them.
      lock (runnables_) {
        if (executed_) {
          return;
        }
        executed_ = true;
      }

      // at this point the "runnables_" will never be modified by another
      // thread, so we are safe using it outside of the lock block.
      while (runnables_.Count > 0) {
        runnables_.Dequeue().Execute();
      }
    }
  }
}
