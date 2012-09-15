using System;
using System.Threading;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Concurrent
{
  /// <summary>
  /// 
  /// </summary>
  public class SingleThreadExecutor : IExecutor
  {
    const string kClassName = "SingleThreadExecutor";
    readonly YQueue<RunnableDelegate> execution_queue_;

    readonly IThreadFactory factory_;
    readonly ILogger logger_;
    readonly AutoResetEvent signal_;
    readonly Thread thread_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleThreadExecutor"/>
    /// class using the specified <see cref="IThreadFactory"/> object.
    /// </summary>
    /// <param name="factory">
    /// A <see cref="IThreadFactory"/> object that is used to create a
    /// <see cref="Thread"/> whenn needed.
    /// </param>
    public SingleThreadExecutor(IThreadFactory factory) {
      thread_ = factory.CreateThread(ThreadMain);
      signal_ = new AutoResetEvent(false);
      logger_ = MustLogger.ForCurrentProcess;
      execution_queue_ = new YQueue<RunnableDelegate>(10);
      thread_.Start();
    }
    #endregion

    /// <inheritdoc/>
    public void Execute(RunnableDelegate runnable) {
      execution_queue_.Enqueue(runnable);
      signal_.Set();
    }

    void ThreadMain() {
      while (true) {
        // A try/catch block is used here to stops the loop
        // when a ThreadInterruptedException and ThreadAbortException
        // is raised and to not stops the Thread when the runnable raises
        // an exception.
        try {
          signal_.WaitOne();

          RunnableDelegate runnable;
          while (execution_queue_.Dequeue(out runnable)) {
            runnable();
          }
        } catch (ThreadInterruptedException exception) {
          return;
        } catch (ThreadAbortException abort_exception) {
          return;
        } catch (Exception exception) {
          logger_.Error(
            string.Format(StringResources.Log_MethodThrowsException, kClassName),
            exception);
        }
      }
    }
  }
}
