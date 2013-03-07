using System;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// A simple reporter which prints out application metrics to the
  /// <see cref="System.Console"/> periodically.
  /// </summary>
  public class ConsoleReporter : AbstractPollingReporter
  {
    #region .ctor
    public ConsoleReporter(IMetricsRegistry registry) : base(registry) {
    }
    #endregion

    public override void Run() {
      var registry = MetricsRegsitry;
      var now = DateTime.UtcNow;
      registry.Report((metrics, timestamp) => {
        MetricName name = metrics.Key;
        foreach (MetricValue metric in metrics.Value) {
          Console.Write(timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ") + ":"
            + name + ".");
          Console.WriteLine(metric.Name + "=" + metric.Value.ToString());
        }
      }, now);
    }
  }
}
