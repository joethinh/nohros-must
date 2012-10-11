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
    bool running_;
    Thread thread_;

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
      factory_ = factory;
      signal_ = new AutoResetEvent(false);
      logger_ = MustLogger.ForCurrentProcess;
      execution_queue_ = new YQueue<RunnableDelegate>(10);
      running_ = false;
    }
    #endregion

    /// <summary>
    /// Starts the executor thread.
    /// </summary>
    /// <remarks>
    /// If the executor is already started, this method fails silently.
    /// </remarks>
    public void Start() {
      if (running_) {
        return;
      }

      thread_ = factory_.CreateThread(ThreadMain);
      running_ = true;
      thread_.Start();
    }

    /// <summary>
    /// Stops the executor thread.
    /// </summary>
    /// <remarks>
    /// This method discards any task tha was not executed yet. If a task is
    /// currently running, this method blocks the calling thread until the
    /// task terminates.
    /// </remarks>
    public void Stop() {
      Stop(0);
    }

    /// <summary>
    /// Stops the executor thread.
    /// </summary>
    /// <param name="milliseconds_timeout">
    /// The number of milliseconds to wait for the current running task to
    /// terminate.
    /// </param>
    /// <remarks>
    /// This method discards any task tha was not executed yet. If a task is
    /// currently running, this method blocks the calling thread until the
    /// task terminates ot the specified time elapses.
    /// </remarks>
    public void Stop(int milliseconds_timeout) {
      running_ = false;
      signal_.Set();
      if (thread_ != null) {
        thread_.Join(milliseconds_timeout);
      }
      thread_ = null;
    }

    /// <inheritdoc/>
    public void Execute(RunnableDelegate runnable) {
      execution_queue_.Enqueue(runnable);
      signal_.Set();
    }

    void ThreadMain() {
      while (running_) {
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
