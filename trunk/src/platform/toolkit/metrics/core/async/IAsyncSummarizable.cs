using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An object which can produce statistical summaries.
  /// </summary>
  public interface IAsyncSummarizable
  {
    /// <summary>
    /// Gets the largest recorded value.
    /// </summary>
    /// <returns>The largest recorded value.</returns>
    void GetMax(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the smallest recorded value.
    /// </summary>
    /// <returns>The smallest recorded value.</returns>
    void GetMin(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the arithmetic mean of all recorded values.
    /// </summary>
    /// <returns>The arithmetic mean of all recorded values.</returns>
    void GetMean(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the standard deviation of all recorded values.
    /// </summary>
    /// <returns>The standard deviation of all recorded values.</returns>
    void GetStandardDeviation(DoubleMetricCallback callback);
  }
}
