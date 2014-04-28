using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  /// <summary>
  /// A set of <see cref="MetricValue"/>.
  /// </summary>
  public struct MetricValueSet : IEnumerable<MetricValue>
  {
    readonly IMetric metric_;
    readonly MetricValue[] values_;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValueSet"/> class
    /// by using the given metric and a instantaneous values.
    /// </summary>
    /// <param name="metric">
    /// The metric that produces the <paramref name="value"/>.
    /// </param>
    /// <param name="value">
    /// A instantaneous value of the given <paramref name="metric"/>.
    /// </param>
    public MetricValueSet(IMetric metric, MetricValue value)
      : this(metric, new[] {value}) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValueSet"/> class
    /// by using the given <see cref="metric"/> and its associated 
    /// instantaneous values.
    /// </summary>
    /// <param name="metric">
    /// The metric that produces the <paramref name="values"/>.
    /// </param>
    /// <param name="values">
    /// A collection of <see cref="MetricValue"/> containing instantaneous
    /// values of the given <paramref name="metric"/>.
    /// </param>
    public MetricValueSet(IMetric metric, MetricValue[] values) {
      metric_ = metric;
      values_ = values;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public IEnumerator<MetricValue> GetEnumerator() {
      foreach (var value in values_) {
        yield return value;
      }
    }

    /// <summary>
    /// Gets the associated <see cref="IMetric"/>.
    /// </summary>
    public IMetric Metric {
      get { return metric_; }
    }

    /// <summary>
    /// Gets a collection of instanteneous values.
    /// </summary>
    public MetricValue[] Values {
      get { return values_; }
    }
  }
}
