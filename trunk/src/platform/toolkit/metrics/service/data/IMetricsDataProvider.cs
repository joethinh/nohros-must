using System;

namespace Nohros.Metrics
{
  public interface IMetricsDataProvider
  {
    /// <summary>
    /// Store a metric in the databse.
    /// </summary>
    /// <param name="name">
    /// The name of the metric to store.
    /// </param>
    /// <param name="value">
    /// The value of the metric to store.
    /// </param>
    /// <param name="timestamp">
    /// The timestamp identifying when the metric event occured.
    /// </param>
    void Store(string name, double value, long timestamp);
  }
}
