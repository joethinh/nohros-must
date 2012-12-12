using System;

namespace Nohros.Metrics
{
  public interface IAsyncCounted
  {
    /// <summary>
    /// Get the number of values recorded.
    /// </summary>
    /// <returns>The number of values recorded.</returns>
    void GetCount(LongMetricCallback callback);
  }
}
