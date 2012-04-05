using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Creates an executor service that runs each task in the thread that
  /// invokes <see cref="IExecutor{T}.Execute"/>.
  /// </summary>
  /// <remarks>
  /// Tasks are immediately executed in the thread that submitted the task.
  /// </remarks>
  public class SameThreadExecutor : IExecutor
  {
    /// <inheritdoc/>
    public void Execute(RunnableDelegate runnable) {
      runnable();
    }
  }
}
