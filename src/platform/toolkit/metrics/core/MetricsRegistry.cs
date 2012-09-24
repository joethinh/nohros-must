using System;
using System.Collections.Generic;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A central registry and factory for metric instances.
  /// </summary>
  public class MetricsRegistry
  {
    const int kExpectedMetricCount = 1024;
    readonly Clock clock_;
    readonly Dictionary<MetricName, IMetric> metrics_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsRegistry"/> class
    /// that uses the default a <see cref="UserTimeClock"/> as the metrics
    /// clock.
    /// </summary>
    public MetricsRegistry() : this(new UserTimeClock()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsRegistry"/> class
    /// using the specified <see cref="Clock"/> object.
    /// </summary>
    /// <param name="clock"></param>
    public MetricsRegistry(Clock clock) {
      clock_ = clock;
      metrics_ = new Dictionary<MetricName, IMetric>();
    }
    #endregion

    /// <summary>
    /// Gets the counter that is associated with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <returns>A <see cref="Counter"/> that could be identified by the
    /// specified <paramref name="name"/>.</returns>
    public Counter GetCounter(MetricName name) {
      Counter counter;
      if (!TryGetMetric(name, out counter)) {
        counter = new Counter();
        Add(name, counter);
      }
      return counter;
    }

    bool TryGetMetric<T>(MetricName name, out T t) {
      IMetric metric;
      if (metrics_.TryGetValue(name, out metric)) {
        if (metric is T) {
          t = (T) metric;
          return true;
        }
      }
      t = default(T);
      return false;
    }

    /// <summary>
    /// Gets the counter that is associated with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="biased">Whether or not the histogram should be biased.
    /// </param>
    /// <returns>A <see cref="Histogram"/> that could be identified by the
    /// specified <see cref="MetricName"/>.</returns>
    public Histogram GetHistogram(MetricName name, bool biased) {
      Histogram histogram;
      if (!TryGetMetric(name, out histogram)) {
        ISample sample = (biased) ? Samples.Biased() : Samples.Uniform();
        histogram = new Histogram(sample);
        Add(name, histogram);
      }
      return histogram;
    }

    /// <summary>
    /// Adds an metric to the metrics collection using the metrics name.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="metric">
    /// The metric to be added.
    /// </param>
    protected void Add(MetricName name, IMetric metric) {
      metrics_.Add(name, metric);
      OnMetricAdded(metric);
    }

    // TODO: implement this
    void OnMetricAdded(IMetric metric) {
    }
  }
}
