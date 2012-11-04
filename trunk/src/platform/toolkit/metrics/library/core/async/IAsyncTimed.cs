using System;

namespace Nohros.Metrics
{
  public interface IAsyncTimed : IAsyncMetered, IAsyncSampling, IAsyncSummarizable
  {
    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    /// <param name="unit">The scale unit of <paramref name="duration"/></param>
    void Update(long duration, TimeUnit unit);
  }
}
