using System;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="IFuture{T}"/> that is runnable. Successful execution of the
  /// <see cref="Run"/> method causes completion of the future and allow access
  /// to its result.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IRunnableFuture<T> : IFuture<T>
  {
    /// <summary>
    /// Sets the future to the result of its computation unless it has been
    /// cancelled.
    /// </summary>
    void Run();
  }
}
