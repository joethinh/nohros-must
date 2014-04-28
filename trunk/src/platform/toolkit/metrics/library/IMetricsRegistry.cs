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
    T GetMetric<T>(string name) where T : IMetric;

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
    T GetMetric<T>(string name, CallableDelegate<T> factory) where T : IMetric;

    /// <summary>
    /// Gets the <see cref="IMetric"/> that is associated with the given
    /// metric's <paramref name="name"/>
    /// </summary>
    /// <param name="name">
    /// The name of the metric to get.
    /// </param>
    /// <param name="metric">
    /// When this mthod return contains the metric that is associated with the
    /// given <paramref name="name"/> or <c>null</c> if a there is no metric
    /// associated with the given metric's name.
    /// </param>
    /// <returns>
    /// <c>true</c> if a metric associated with the given name is found;
    /// otherwise, <c>false</c>.
    /// </returns>
    bool TryGetMetric<T>(string name, out T metric) where T : IMetric;

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
    /// Raised when a new metric is added to the registry.
    /// </summary>
    event MetricAddedEventHandler MetricAdded;

    /// <summary>
    /// Gets or sets an metric with the specified name.
    /// </summary>
    /// <param name="name">
    /// The name of the metric to get or set.
    /// </param>
    /// <returns>
    /// The <see cref="IMetric"/> associated with the specified name.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <param name="name"> is <c>null</c></param>
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// <param name="name"> is not found.</param>
    /// </exception>
    IMetric this[string name] { get; set; }
  }
}
