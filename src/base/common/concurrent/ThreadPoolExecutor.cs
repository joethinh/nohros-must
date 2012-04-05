using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// An <see cref="IExecutor"/> that executes each submitted task using
  /// a thread from <see cref="ThreadPool"/>.
  /// </summary>
  internal class ThreadPoolExecutor : IExecutor
  {
    #region .ctor
    /// <summary>
    /// initializes a new instance of the <see cref="ThreadPoolExecutor{T}"/>
    /// class.
    /// </summary>
    public ThreadPoolExecutor() { }
    #endregion

    #region IExecutor<T> Members
    /// <inheritdoc/>
    public void Execute(RunnableDelegate runnable) {
      ThreadPool.QueueUserWorkItem(new WaitCallback(
        delegate(object obj)
        {
          (obj as RunnableDelegate)();
        }), runnable);
    }
    #endregion
  }
}