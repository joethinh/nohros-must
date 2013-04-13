using System;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A implementation of the <see cref="IThreadFactory"/> class that create
  /// <see cref="Thread"/>s that runs in background(
  /// <see cref="Thread.IsBackground"/> equals to <c>true</c>).
  /// </summary>
  public class BackgroundThreadFactory : IThreadFactory
  {
    /// <summary>
    /// Creates a <see cref="Thread"/> that runs in bacjgroun and do not
    /// prevent a process from terminating.
    /// </summary>
    /// <param name="runnable"></param>
    /// <returns>
    /// A <see cref="Thread"/> that runs in background.
    /// </returns>
    public Thread CreateThread(ThreadStart runnable) {
      Thread thread = new Thread(runnable);
      thread.IsBackground = true;
      return thread;
    }

    /// <summary>
    /// Creates a <see cref="Thread"/> taht tuns in background and do not
    /// prevent a process from terminating.
    /// </summary>
    /// <param name="runnable"></param>
    /// <returns>
    /// A <see cref="Thread"/> that runs in background.
    /// </returns>
    public Thread CreateThread(ParameterizedThreadStart runnable) {
      Thread thread = new Thread(runnable);
      thread.IsBackground = true;
      return thread;
    }
  }
}
