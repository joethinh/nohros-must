using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A set of factory methods for creating centrally registered metric
  /// instances.
  /// </summary>
  public class Metrics
  {
    static readonly MetricsRegistry registry_;

    #region .ctor
    static Metrics() {
      registry_ = new MetricsRegistry();
    }
    #endregion

    /// <summary>
    /// Gets the counter that is associated with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <returns>
    /// A <see cref="Counter"/> that could be identified by the specified
    /// <paramref name="name"/>.
    /// </returns>
    public Counter GetCounter(MetricName name) {
      return registry_.GetCounter(name);
    }

    /// <summary>
    /// Gets the counter that is associated with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="biased">
    /// Whether or not the histogram should be biased.
    /// </param>
    /// <returns>
    /// A <see cref="AbstractHistogram"/> that could be identified by the specified
    /// <see cref="MetricName"/>.
    /// </returns>
    public IHistogram GetHistogram(MetricName name, bool biased) {
      return registry_.GetHistogram(name, biased);
    }
  }
}
