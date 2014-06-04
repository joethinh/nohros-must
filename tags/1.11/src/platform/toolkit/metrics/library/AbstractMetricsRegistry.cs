using System;
using System.Collections.Generic;
using System.Linq;
using Nohros.Metrics.Reporting;

namespace Nohros.Metrics
{
  public abstract class AbstractMetricsRegistry : IMetricsRegistry
  {
    const int kExpectedMetricCount = 1024;
    readonly Dictionary<MetricName, IMetric> metrics_;

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/> class.
    /// </summary>
    protected AbstractMetricsRegistry() {
      metrics_ = new Dictionary<MetricName, IMetric>(kExpectedMetricCount);
    }

    /// <inheritdoc/>
    public void Report<T>(MetricsReportCallback<T> callback, T context) {
      // The code above is the same as the Report method that accepts a
      // predicate, and it is duplicated to avoid overhead of calling the
      // predicate method when there is no need.
      foreach (KeyValuePair<MetricName, IMetric> metric in metrics_) {
        MetricName name = metric.Key;
        metric.Value.Report((metrics, ctx) =>
          callback(name, metrics.Values, context), context);
      }
    }

    /// <inheritdoc/>
    public void Report<T>(MetricsReportCallback<T> callback, T context,
      MetricPredicate predicate) {
      foreach (KeyValuePair<MetricName, IMetric> metric in metrics_) {
        MetricName name = metric.Key;
        metric.Value.Report((metrics, ctx) => {
          var list = metrics.Where(m => predicate(name, m));
          callback(name, list.ToArray(), context);
        }, context);
      }
    }

    /// <inheritdoc/>
    public virtual void Add(string name, IMetric metric) {
      Add(new MetricName(name), metric);
    }

    /// <inheritdoc/>
    public virtual void Add(MetricName name, IMetric metric) {
      metrics_.Add(name, metric);
      OnMetricAdded(name, metric);
    }

    /// <inheritdoc/>
    public bool HasMetric(MetricName name) {
      return metrics_.ContainsKey(name);
    }

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    public virtual event MetricAddedEventHandler MetricAdded;

    protected void OnMetricAdded(MetricName name, IMetric metric) {
      Listeners.SafeInvoke(MetricAdded,
        (MetricAddedEventHandler handler) => handler(name, metric));
    }
  }
}
