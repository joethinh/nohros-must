using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A central registry and factory for metric instances.
  /// </summary>
  public class MetricsRegistry
  {
    const int kExpectedMetritCount = 1024;
    readonly Dictionary<MetricName, IMetric> metrics_;
    readonly object metrics_locker_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsRegistry"/> class.
    /// </summary>
    public MetricsRegistry() {
      metrics_locker_ = new object();
    }
    #endregion

    /// <summary>
    /// Given a new <see cref="Gauge"/>, register it under the given metric
    /// name.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the metric.
    /// </typeparam>
    /// <param name="name">The name of the metric.</param>
    /// <param name="metric">The metric to be added.</param>
    /// <returns><paramref name="metric"/></returns>
    public Gauge<T> RegisterGauge<T>(MetricName name, Gauge<T> metric) {
      return GetOrAdd<Gauge<T>>(name, metric);
    }

    /// <summary>
    /// Creates a new counter and register it under the given metric name.
    /// </summary>
    /// <param name="metric_name">The name of the metric.</param>
    /// <returns>A <see cref="Counter"/> that could be identified by the
    /// specified <paramref name="metric_name"/>.</returns>
    public Counter CreateCounter(MetricName metric_name) {
      return GetOrAdd<Counter>(metric_name, new Counter());
    }

    /// <summary>
    /// Creates a new <see cref="Histogram"/> and register it under the given
    /// metric name.
    /// </summary>
    /// <param name="metric_name">The name of the metric.</param>
    /// <param name="biased">Whether or not the histogram should be biased.
    /// </param>
    /// <returns>A <see cref="Histogram"/> that could be identified by the
    /// specified <see cref="MetricName"/>.</returns>
    public Histogram CreateHistogram(MetricName metric_name, bool biased) {
      ISample sample;
      if (biased) {
        sample = new ExponentiallyDecayingSample(
          Histogram.kDefaultSampleSize, Histogram.kDefaultAlpha);
      } else {
        sample = new UniformSample(Histogram.kDefaultSampleSize);
      }
      return GetOrAdd<Histogram>(metric_name, new Histogram(sample));
    }

    /// <summary>
    /// Gets any existing metric wich the given name or, if noe exists, adds
    /// the given metric
    /// </summary>
    /// <typeparam name="T">The type of metric.</typeparam>
    /// <param name="name">The metrics name.</param>
    /// <param name="metric">The new metric.</param>
    /// <returns>Either the existing metric or <see cref="metric"/>.</returns>
    protected T GetOrAdd<T>(MetricName name, T metric) where T: IMetric {
      IMetric existing_metric;
      if (!metrics_.TryGetValue(name, out existing_metric)) {
        lock (metrics_locker_) {
          if (!metrics_.TryGetValue(name, out existing_metric)) {
            metrics_[name] = metric;

            // notify the listeneres that a new metric was added to the
            // collection.
            OnMetricAdded(metric);

            return metric;
          }

          // another thread just added the metric to the dictionary, so
          // restart it if we can.
          if (metric is IStoppable) {
            ((IStoppable)metric).Stop();
            return (T)existing_metric;
          }
        }
      }
      return (T)existing_metric;
    }

    // TODO: implement this
    void OnMetricAdded(IMetric metric) {
    }
  }
}