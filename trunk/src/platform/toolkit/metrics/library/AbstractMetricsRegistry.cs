using System;
using System.Collections.Generic;
using System.Linq;
using Nohros.Metrics.Reporting;
using Nohros.Resources;

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
      metrics_ = new Dictionary<MetricName, IMetric>(kExpectedMetricCount,
        new MetricNameKeyEqualityComparer());
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
    public T GetMetric<T>(string name) where T : IMetric {
      return GetMetric<T>(name,
        () => {
          throw new KeyNotFoundException(
            string.Format(StringResources.Arg_KeyNotFound, name));
        });
    }

    /// <inheritdoc/>
    public T GetMetric<T>(string name, CallableDelegate<T> factory)
      where T : IMetric {
      T metric;
      if (!TryGetMetric(name, out metric)) {
        metric = factory();
        this[name] = metric;
      }
      return metric;
    }

    /// <inheritdoc/>
    bool TryGetMetric<T>(MetricName name, out T metric) where T : IMetric {
      IMetric m;
      if (metrics_.TryGetValue(name, out m)) {
        if (m is T) {
          metric = (T) m;
          return true;
        }
      }
      metric = default(T);
      return false;
    }

    /// <inheritdoc/>
    public bool TryGetMetric<T>(string name, out T metric) where T : IMetric {
      return TryGetMetric(new MetricName(name), out metric);
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

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    public virtual event MetricAddedEventHandler MetricAdded;

    /// <inheritdoc/>
    public virtual IMetric this[string name] {
      get { return this[new MetricName(name)]; }
      set { this[new MetricName(name)] = value; }
    }

    /// <inheritdoc/>
    IMetric this[MetricName name] {
      get {
        IMetric metric;
        if (!TryGetMetric(name, out metric)) {
          throw new KeyNotFoundException("name");
        }
        return metric;
      }
      set { metrics_[name] = value; }
    }

    protected void OnMetricAdded(MetricName name, IMetric metric) {
      Listeners.SafeInvoke(MetricAdded,
        (MetricAddedEventHandler handler) => handler(name, metric));
    }
  }
}
