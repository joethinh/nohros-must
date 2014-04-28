using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// Represents the method that defines a set of criteria and determines
  /// whether a metric shoudl be included when filtering metrics.
  /// </summary>
  /// <param name="name">
  /// The name of the metric.
  /// </param>
  /// <param name="set">
  /// The metric itself.
  /// </param>
  /// <returns>
  /// <c>true</c> if <paramref name="name"/> and <paramref name="set"/>
  /// meets the criteria defined whithin the method represented by this
  /// delegate.
  /// </returns>
  /// <remarks>
  /// This delegate is used by implementations of the IMetricsReporter class
  /// to filter metrics while reporting.
  /// </remarks>
  public delegate bool MetricPredicate(MetricName name, MetricValue @value);
}
