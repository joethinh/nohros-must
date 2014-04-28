using System;

namespace Nohros.Metrics.Reporting
{
  public class MetricReported
  {
    public MetricReported(string metric_name, MetricValueSet metrics) {
      MetricName = metric_name;
      MetricValueSet = metrics;
    }

    public string MetricName { get; private set; }
    public MetricValueSet MetricValueSet { get; private set; }
  }
}
