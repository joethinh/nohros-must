using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A set of factory methods for creating centrally registered metric
  /// instances.
  /// </summary>
  public class AppMetrics
  {
    static readonly MetricsRegistry registry_;

    #region .ctor
    static AppMetrics() {
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
    public static Counter GetCounter(MetricName name) {
      return registry_.GetCounter(name);
    }

    public static Counter GetCounter(Type klass, string name) {
      return registry_.GetCounter(klass, name);
    }

    public static Counter GetCounter(Type klass, string name, string scope) {
      return registry_.GetCounter(klass, name, scope);
    }

    public static Counter GetCounter(string group, string type, string name) {
      return registry_.GetCounter(group, type, name);
    }

    public static Counter GetCounter(string group, string type, string name,
      string scope) {
      return registry_.GetCounter(group, type, name, scope);
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
    public static void AddGauge<T>(MetricName name, Gauge<T> metric) {
      registry_.AddGauge(name, metric);
    }

    public static void AddGauge<T>(Type klass, string name, Gauge<T> metric) {
      registry_.AddGauge(klass, name, metric);
    }

    public static void AddGauge<T>(Type klass, string name, string scope,
      Gauge<T> metric) {
      registry_.AddGauge(klass, name, scope, metric);
    }

    public static void AddGauge<T>(string group, string type, string name,
      Gauge<T> metric) {
      registry_.AddGauge(group, type, name, metric);
    }

    public static void AddGauge<T>(string group, string type, string name,
      string scope, Gauge<T> metric) {
      registry_.AddGauge(group, type, name, scope, metric);
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
    /// A <see cref="IHistogram"/> that could be identified by the specified
    /// <see cref="MetricName"/>.
    /// </returns>
    public static IHistogram GetHistogram(MetricName name, bool biased) {
      return registry_.GetHistogram(name, biased);
    }

    public static IHistogram GetHistogram(Type klass, string name, bool biased) {
      return registry_.GetHistogram(klass, name, biased);
    }

    public static IHistogram GetHistogram(Type klass, string name, string scope,
      bool biased) {
      return registry_.GetHistogram(klass, name, scope, biased);
    }

    public static IHistogram GetHistogram(string group, string type, string name,
      bool biased) {
      return registry_.GetHistogram(group, type, name, biased);
    }

    public static IHistogram GetHistogram(string group, string type, string name,
      string scope, bool biased) {
      return registry_.GetHistogram(group, type, name, scope, biased);
    }

    /// <summary>
    /// Gets the meter that is associated with the specified
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
    /// <returns>
    /// The metered associated with the key <paramref name="name"/>.
    /// </returns>
    public static IMetered GetMeter(MetricName name, string event_type,
      TimeUnit rate_unit) {
      return registry_.GetMeter(name, event_type, rate_unit);
    }

    public static IMetered GetMeter(Type klass, string name, string event_type,
      TimeUnit rate_unit) {
      return registry_.GetMeter(klass, name, event_type, rate_unit);
    }

    public static IMetered GetMeter(Type klass, string name, string scope,
      string event_type, TimeUnit rate_unit) {
      return registry_.GetMeter(klass, name, scope, event_type, rate_unit);
    }

    public static IMetered GetMeter(string group, string type, string name,
      string event_type, TimeUnit rate_unit) {
      return registry_.GetMeter(group, type, name, event_type, rate_unit);
    }

    public static IMetered GetMeter(string group, string type, string name,
      string scope, string event_type, TimeUnit rate_unit) {
      return registry_.GetMeter(group, type, name, scope, event_type, rate_unit);
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
    public static Timer GetTimer(MetricName name, TimeUnit duration_unit) {
      return registry_.GetTimer(name, duration_unit);
    }

    public static Timer GetTimer(Type klass, string name, TimeUnit duration_unit) {
      return registry_.GetTimer(klass, name, duration_unit);
    }

    public static Timer GetTimer(Type klass, string name, string scope,
      TimeUnit duration_unit) {
      return registry_.GetTimer(klass, name, scope, duration_unit);
    }

    public static Timer GetTimer(string group, string type, string name,
      TimeUnit duration_unit) {
      return registry_.GetTimer(group, type, name, duration_unit);
    }

    public static Timer GetTimer(string group, string type, string name,
      string scope, TimeUnit duration_unit) {
      return registry_.GetTimer(group, type, name, scope, duration_unit);
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
    public static bool TryGetHistogram(MetricName name, out IHistogram histogram) {
      return registry_.TryGetHistogram(name, out histogram);
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
    public static bool TryGetGauge<T>(MetricName name, out Gauge<T> gauge) {
      return registry_.TryGetGauge(name, out gauge);
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
    public static bool TryGetCounter(MetricName name, out Counter counter) {
      return registry_.TryGetCounter(name, out counter);
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
    public static bool TryGetMeter(MetricName name, out IMeter meter) {
      return registry_.TryGetMeter(name, out meter);
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
    /// <c>true</c> if a <see cref="Timer"/> associated with the
    /// <paramref name="name"/> exists; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryGetTimer(MetricName name, out Timer timer) {
      return registry_.TryGetTimer(name, out timer);
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
    public static bool TryGetMetric<T>(MetricName name, out T metric)
      where T : class, IMetric {
      return registry_.TryGetMetric(name, out metric);
    }
  }
}
