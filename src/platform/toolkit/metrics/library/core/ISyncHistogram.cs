using System;

namespace Nohros.Metrics
{
  public interface ISyncHistogram : IHistogram, ISummarizable, ISampling, ICounted
  {
  }
}
