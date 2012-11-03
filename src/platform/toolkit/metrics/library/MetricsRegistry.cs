using System;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  /// <summary>
  /// A central registry and factory for metric instances.
  /// </summary>
  public class MetricsRegistry
  {
    const int kExpectedMetricCount = 1024;
    readonly Dictionary<MetricName, IMetric> metrics_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsRegistry"/> class.
    /// </summary>
    public MetricsRegistry() {
      metrics_ = new Dictionary<MetricName, IMetric>();
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
      Counter counter;
      if (!TryGetMetric(name, out counter)) {
        counter = new Counter();
        Add(name, counter);
      }
      return counter;
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
    /// A <see cref="IHistogram"/> that could be identified by the
    /// specified <see cref="MetricName"/>.
    /// </returns>
    public IHistogram GetHistogram(MetricName name, bool biased) {
      IHistogram histogram;
      if (!TryGetMetric(name, out histogram)) {
        histogram = (biased)
          ? (IHistogram) Histograms.Biased()
          : (IHistogram) Histograms.Uniform();
        Add(name, histogram);
      }
      return histogram;
    }

    /// <summary>
    /// Gets the meter that is associaed with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="rate_unit">
    /// The rate unit of the new meter.
    /// </param>
    /// <param name="event_type">
    /// The plural name of the event meter is measuring
    /// <example>
    /// <code>
    /// "requests"
    /// </code>
    /// </example>
    /// </param>
    /// <returns></returns>
    public IMetered GetMeter(MetricName name, string event_type,
      TimeUnit rate_unit) {
      IMetered meter;
      if (!TryGetMetric(name, out meter)) {
        meter = new Meter(event_type, rate_unit);
        Add(name, meter);
      }
      return meter;
    }

    /// <summary>
    /// Given a new <see cref="Gauge{T}"/>, registers it under the given metric
    /// name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="metric">
    /// The gauge metric to be added.
    /// </param>
    /// <returns></returns>
    public void AddGauge<T>(MetricName name, Gauge<T> metric) {
      Gauge<T> gauge;
      if (!TryGetMetric(name, out gauge)) {
        Add(name, gauge);
      }
    }

    /// <summary>
    /// Gets the timer that is associated with the specified
    /// <see cref="MetricName"/> or create a new one if no association exists.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="duration_unit">
    /// The duration unit of the new timer.
    /// </param>
    /// <returns>
    /// The timer associated with the key <paramref name="name"/>.
    /// </returns>
    public Timer GetTimer(MetricName name, TimeUnit duration_unit) {
      Timer timer;
      if (!TryGetMetric(name, out timer)) {
        timer = new Timer(duration_unit, new Meter("calls", TimeUnit.Seconds),
          Histograms.Biased());
        Add(name, timer);
      }
      return timer;
    }

    /// <summary>
    /// Gets the value associated with the specified metric name.
    /// </summary>
    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="histogram">
    /// When this method returns, contains the value associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="IHistogram"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetHistogram(MetricName name, out IHistogram histogram) {
      return TryGetMetric(name, out histogram);
    }

    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="gauge">
    /// When this method returns, contains the gauge associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="Gauge{T}"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetGauge<T>(MetricName name, out Gauge<T> gauge) {
      return TryGetMetric(name, out gauge);
    }

    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="counter">
    /// When this method returns, contains the counter associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="Counter"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetCounter(MetricName name, out Counter counter) {
      return TryGetMetric(name, out counter);
    }

    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="timer">
    /// When this method returns, contains the timer associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="Counter"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetTimer(MetricName name, out Timer timer) {
      return TryGetMetric(name, out timer);
    }

    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="metric">
    /// When this method returns, contains the timer associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="Counter"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetMetric<T>(MetricName name, out T metric)
      where T : class, IMetric {
      IMetric i_metric;
      if (metrics_.TryGetValue(name, out i_metric)) {
        metric = i_metric as T;
        return metric != null;
      }
      metric = null;
      return false;
    }


    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="meter">
    /// When this method returns, contains the meter associated with the
    /// specified metric name, if a metric name is found; otherwise, the
    /// <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="IMetered"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetMeter(MetricName name, out IMetered meter) {
      return TryGetMetric(name, out meter);
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
      OnMetricAdded(name, metric);
    }

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    public event MetricAddedEventHandler MetricAdded;

    void OnMetricAdded(MetricName name, IMetric metric) {
      Listeners.SafeInvoke(MetricAdded,
        (MetricAddedEventHandler handler) => handler(name, metric));
    }
  }
}
