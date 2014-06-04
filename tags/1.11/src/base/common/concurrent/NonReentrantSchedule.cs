using System;
using System.Threading;
using Nohros.Logging;

namespace Nohros.Concurrent
{
  public class NonReentrantSchedule
  {
    readonly TimeSpan interval_;
    readonly ManualResetEvent signaler_;
    RunnableDelegate task_;
    Thread thread_;

    /// <summary>
    /// Initializes a new instance of the <see cref="NonReentrantSchedule"/>
    /// class by using the given interval.
    /// </summary>
    /// <param name="interval">
    /// The time to wait before run the associated action.
    /// </param>
    NonReentrantSchedule(TimeSpan interval) {
      interval_ = interval;
      signaler_ = new ManualResetEvent(false);
      thread_ = null;
      task_ = null;
    }

    /// <summary>
    /// Returns a <see cref="NonReentrantSchedule"/> class than run a task
    /// at every <see cref="interval"/>.
    /// </summary>
    /// <param name="interval">
    /// The interval to schedule a task.
    /// </param>
    /// <returns>
    /// A <see cref="NonReentrantSchedule"/> object that runs a task at every
    /// <see cref="interval"/>.
    /// </returns>
    /// <remarks>
    /// The <see cref="NonReentrantSchedule"/> is non-reentrant. When a task
    /// is running the <see cref="NonReentrantSchedule"/> wait it to finish
    /// before start a new one. After a task is finished the
    /// <see cref="NonReentrantSchedule"/> waits the given
    /// <paramref name="interval"/> and start the task again.
    /// <para>
    /// The tasks runs in a background thread.
    /// </para>
    /// <para>
    /// Any unhandled exception is logged through the configured
    /// <see cref="MustLogger"/> and swallowed. Exceptions raised by one task
    /// does not stops the scheduling.
    /// </para>
    /// </remarks>
    public static NonReentrantSchedule Every(TimeSpan interval) {
      return new NonReentrantSchedule(interval);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    public void Runnable(RunnableDelegate task) {
      if (thread_ != null) {
        throw new InvalidOperationException("The task is already defined.");
      }
      task_ = task;
      thread_ = new BackgroundThreadFactory().CreateThread(ThreadMain);
      thread_.Start();
    }

    void ThreadMain(object obj) {
      while (!signaler_.WaitOne(interval_)) {
        task_();
      }
    }

    /// <summary>
    /// Stops scheduling tasks and returns an <seealso cref="WaitHandle"/> that
    /// can be used to monitor when the pending task finish.
    /// </summary>
    /// <remarks>
    /// If there is no task running <seealso cref="WaitHandle.WaitOne()"/>
    /// overloads returns imediatelly.
    /// </remarks>
    public WaitHandle Stop() {
      signaler_.Set();
      return signaler_;
    }
  }
}
