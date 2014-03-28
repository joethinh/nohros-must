using System;
using System.Collections.Generic;
using Nohros.Resources;

namespace Nohros.Metrics
{
  public abstract class AbstractMetricsRegistry : IMetricsRegistry
  {
    const int kExpectedMetricCount = 1024;
    readonly Dictionary<string, IMetric> metrics_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/> class.
    /// </summary>
    protected AbstractMetricsRegistry() {
      metrics_ = new Dictionary<string, IMetric>(kExpectedMetricCount);
    }
    #endregion

    /// <inheritdoc/>
    public void Report<T>(MetricsReportCallback<T> callback, T context) {
      // The code above is the same as the Report method that accepts a
      // predicate, and it is duplicated to avoid overhead of calling the
      // predicate method when there is no need.
      foreach (KeyValuePair<string, IMetric> metric in metrics_) {
        var name = metric.Key;
        metric.Value.Report((metrics, ctx) => {
          var pair = new KeyValuePair<string, MetricValueSet>(name, metrics);
          callback(pair, ctx);
        }, context);
      }
    }

    /// <inheritdoc/>
    public void Report<T>(MetricsReportCallback<T> callback, T context,
      MetricPredicate predicate) {
      foreach (KeyValuePair<string, IMetric> metric in metrics_) {
        var name = metric.Key;
        if (predicate(name, metric.Value)) {
          metric.Value.Report((metrics, ctx) => {
            var pair = new KeyValuePair<string, MetricValueSet>(name, metrics);
            callback(pair, ctx);
          }, context);
        }
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
    public bool TryGetMetric<T>(string name, out T metric) where T : IMetric {
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
    public virtual void Add(string name, IMetric metric) {
      metrics_.Add(name, metric);
      OnMetricAdded(name, metric);
    }

    /// <summary>
    /// Raised when a new metric is added to the registry.
    /// </summary>
    public virtual event MetricAddedEventHandler MetricAdded;

    /// <inheritdoc/>
    public virtual IMetric this[string name] {
      get {
        IMetric metric;
        if (!TryGetMetric(name, out metric)) {
          throw new KeyNotFoundException("name");
        }
        return metric;
      }
      set { metrics_[name] = value; }
    }

    protected void OnMetricAdded(string name, IMetric metric) {
      Listeners.SafeInvoke(MetricAdded,
        (MetricAddedEventHandler handler) => handler(name, metric));
    }
  }
}
