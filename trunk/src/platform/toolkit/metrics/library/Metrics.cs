using System;
using System.Collections.Generic;
using Nohros.Metrics.Reporting;

namespace Nohros.Metrics
{
  /// <summary>
  /// A set of factory methods for creating centrally registered metric
  /// instances.
  /// </summary>
  /// <remarks>
  /// A <see cref="SyncMetricsRegistry"/> is used as the default metrics
  /// registry. If you want to use another <see cref="IMetricsRegistry"/>
  /// implementation, set the value of the property <see cref="Registry"/>.
  /// </remarks>
  public class AppMetrics
  {
    static IMetricsRegistry registry_;
    static readonly IDictionary<string, IMetricsReporter> reporters_;

    #region .ctor
    /// <summary>
    /// Initializes the siatic members.
    /// </summary>
    static AppMetrics() {
      registry_ = new SyncMetricsRegistry();
      reporters_ = new Dictionary<string, IMetricsReporter>();
    }
    #endregion

    /// <summary>
    /// Stops all the registered reporters and deallocates any associated
    /// resources.
    /// </summary>
    public static void Shutdown() {
      foreach (IMetricsReporter reporter in reporters_.Values) {
        reporter.Shutdown();
      }
    }

    /// <summary>
    /// Register a <see cref="IMetricsReporter"/> under the given reporter
    /// name.
    /// </summary>
    /// <param name="name">
    /// The name of the reporter.
    /// </param>
    /// <param name="reporter">
    /// The reporter to be registerd.
    /// </param>
    /// <remarks>
    /// This method allows a <see cref="IMetricsReporter"/> object to have the
    /// same lifecycle of the <see cref="AppMetrics"/> class without needing
    /// to be referenced by other classes.
    /// <para>
    /// All registered <see cref="IMetricsReporter"/> are shutedown when the
    /// <see cref="Shutdown"/> method is called.
    /// </para>
    /// </remarks>
    public static void RegisterReporter(string name, IMetricsReporter reporter) {
      reporters_[name] = reporter;
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
    public static bool TryGetMetric<T>(string name, out T metric)
      where T : class, IMetric {
      return registry_.TryGetMetric(name, out metric);
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
    /// <exception cref="ArgumentException">
    /// A metric with the same name already exists in the
    /// <see cref="IMetricsRegistry"/>.
    /// </exception>
    public static void Add(string name, IMetric metric) {
      registry_.Add(name, metric);
    }

    /// <summary>
    /// Gets the <see cref="IMetric"/> that is associated with the given
    /// metric's <paramref name="name"/>
    /// </summary>
    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <returns>
    /// The metric that is associated with the given <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A metric associated with hte given key was not found.
    /// </exception>
    public static T GetMetric<T>(string name) where T : IMetric {
      return registry_.GetMetric<T>(name);
    }

    /// <summary>
    /// Gets the <see cref="IMetric"/> that is associated with the given
    /// metric's <paramref name="name"/>
    /// </summary>
    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="factory">
    /// A <see cref="CallableDelegate{T}"/> that can be used to create a
    /// metric.
    /// </param>
    /// <returns>
    /// The metric that is associated with the given <paramref name="name"/>
    /// or the value returned by the <see cref="CallableDelegate{T}"/> method
    /// if there is no metric associated with the given name.
    /// </returns>
    /// <remarks>
    /// If a metric associated with the given <paramref name="name"/> is not
    /// found, the <paramref name="factory"/> delegate will be executed and its
    /// return value will be associated with the given <paramref name="name"/>.
    /// </remarks>
    public static T GetMetric<T>(string name, CallableDelegate<T> factory)
      where T : IMetric {
      return registry_.GetMetric(name, factory);
    }

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    public static event MetricAddedEventHandler MetricAdded {
      add { registry_.MetricAdded += value; }
      remove { registry_.MetricAdded -= value; }
    }

    /// <summary>
    /// Gets or set the singleton metrics object.
    /// </summary>
    /// <remarks>
    /// The default value of <see cref="Registry"/> is
    /// <see cref="SyncMetricsRegistry"/>.
    /// </remarks>
    public static IMetricsRegistry Registry {
      get { return registry_; }
      set {
        if (registry_ == null) {
          throw new ArgumentNullException("value");
        }
        registry_ = value;
      }
    }
  }
}
