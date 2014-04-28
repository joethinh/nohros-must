using System;
using System.Collections.Generic;
using Nohros.Metrics.Reporting;

namespace Nohros.Metrics
{
  /// <summary>
  /// A central registry for metric instances.
  /// </summary>
  public interface IMetricsRegistry
  {
    /// <summary>
    /// Gets the the collection of values associated with all the registered
    /// metrics.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="MetricsReportCallback{T}"/> that is called to
    /// report the metric's value.
    /// </param>
    /// <param name="context">
    /// A user-defined object that qualifies or contains information about the
    /// reporting operation.
    /// </param>
    void Report<T>(MetricsReportCallback<T> callback, T context);

    /// <summary>
    /// Gets the the collection of values associated with the metrics that
    /// matches the criteria defined by the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="MetricsReportCallback{T}"/> that is called to
    /// report the metric's value.
    /// </param>
    /// <param name="context">
    /// A user-defined object that qualifies or contains information about the
    /// reporting operation.
    /// </param>
    /// <param name="predicate">
    /// A <see cref="MetricPredicate"/> delegate that defines the conditions
    /// of the metrics to report.
    /// </param>
    void Report<T>(MetricsReportCallback<T> callback, T context,
      MetricPredicate predicate);

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
    void Add(string name, IMetric metric);

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
    void Add(MetricName name, IMetric metric);

    /// <summary>
    /// TODO: Copy documentation from the NServiceBus HasCompoenent
    /// </summary>
    /// <param name="name"></param>
    bool HasMetric(MetricName name);

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    event MetricAddedEventHandler MetricAdded;
  }
}
