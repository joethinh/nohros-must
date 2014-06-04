using System;

namespace Nohros.Metrics
{
  public interface IAsyncTimed : IAsyncMetered, IAsyncSampling, IAsyncSummarizable, ITimer
  {
  }
}
