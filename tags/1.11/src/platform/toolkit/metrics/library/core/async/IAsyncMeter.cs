using System;

namespace Nohros.Metrics
{
  public interface IAsyncMeter : IAsyncMetered, IMeter
  {
    /// <summary>
    /// Mark the occurrence of an event.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="MeteredCallback"/> method that will be executed before the
    /// mark operation completes. The callback is executed in sync with the
    /// mark operation.
    /// </param>
    void Mark(MeteredCallback callback);

    /// <summary>
    /// Mark the occurrence of a given number of events.
    /// </summary>
    /// <param name="n">
    /// The number of events.
    /// </param>
    /// <param name="callback">
    /// A <see cref="MeteredCallback"/> method that will be executed before the
    /// mark operation completes. The callback is executed in sync with the
    /// mark operation.
    /// </param>
    void Mark(long n, MeteredCallback callback);
  }
}
