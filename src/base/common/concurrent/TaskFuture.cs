using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nohros.Concurrent
{
  /// <summary>
  /// An implementation of the <see cref="IFuture{T}"/> class that forwards its
  /// method calls to a <see cref="Task{T}"/>
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class TaskFuture<T> : AbstractFuture<T>, IRunnableFuture<T>,
                               IAsyncResult
  {
    readonly Task<T> task_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskFuture{T}"/> class
    /// using the specified <paramref name="task"/> as method forwarder.
    /// </summary>
    /// <param name="task">
    /// A <see cref="Task"/> representing an asynchronous operation.
    /// </param>
    public TaskFuture(Task<T> task) {
      task_ = task;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskFuture{T}"/> class
    /// using the specified <paramref name="task"/> and asynchronous state.
    /// </summary>
    /// <param name="task">
    /// A <see cref="Task"/> representing an asynchronous operation.
    /// </param>
    /// <param name="state">
    /// An object representing data to be used bt the future.
    /// </param>
    public TaskFuture(Task<T> task, object state) : base(state) {
      task_ = task;
    }
    #endregion

    bool IAsyncResult.CompletedSynchronously {
      get { return ((IAsyncResult) task_).CompletedSynchronously; }
    }

    WaitHandle IAsyncResult.AsyncWaitHandle {
      get { return ((IAsyncResult) task_).AsyncWaitHandle; }
    }

    public override object AsyncState {
      get { return task_.AsyncState; }
    }

    /// <inheritdoc/>
    public void Run() {
      task_.Start();
    }

    /// <summary>
    /// Subclasses can override this method to implement interruption of
    /// the future's computation.
    /// </summary>
    /// <remarks>
    /// The method is invoked automatically by a successfull call to
    /// <see cref="AbstractFuture{T}.Cancel"/>.
    /// <para>
    /// The default implementation does nothing.
    /// </para>
    /// </remarks>
    protected override void InterruptTask() {
    }

    /// <summary>
    /// Gets the <see cref="Task{T}"/> associated with this
    /// <see cref="TaskFuture{T}"/>.
    /// </summary>
    public Task<T> Task {
      get { return task_; }
    }
  }
}
