using System;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  /// <summary>
  /// Method that is called to report metric's values.
  /// </summary>
  /// <param name="context">
  /// A user-defined object that qualifies or contains information about the
  /// reporting operation.
  /// </param>
  /// <param name="values">
  /// The values associated with a <see cref="IMetric"/>.
  /// </param>
  public delegate void MetricReportCallback<in T>(
    MetricValue[] values, T context);

  /// <summary>
  /// Method that is called to report metric's values for a collection of
  /// metrics.
  /// </summary>
  /// <param name="context">
  /// A user-defined object that qualifies or contains information about the
  /// reporting operation.
  /// </param>
  /// <param name="values">
  /// The values associated with a <see cref="IMetric"/>.
  /// </param>
  public delegate void MetricsReportCallback<in T>(
    KeyValuePair<string, MetricValue[]> values, T context);
}
