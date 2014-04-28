using System;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// Method that is called to report metric's values for a collection of
  /// metrics.
  /// </summary>
  /// <param name="context">
  /// A user-defined object that qualifies or contains information about the
  /// reporting operation.
  /// </param>
  /// <param name="metric_name">
  /// The name of the metric.
  /// </param>
  /// <param name="metric_values">
  /// The values associated with a <see cref="IMetric"/>.
  /// </param>
  public delegate void MetricsReportCallback<in T>(MetricName metric_name,
    MetricValue[] values, T context);
}
