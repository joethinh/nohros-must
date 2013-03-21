using System;

namespace Nohros.Metrics.Data
{
  public interface IMetricsRepository
  {
    /// <summary>
    /// Store a metric in the repository.
    /// </summary>
    StoreMetricCommand Query(out StoreMetricCommand store);
  }
}
