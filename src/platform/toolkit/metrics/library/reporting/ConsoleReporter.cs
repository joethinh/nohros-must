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
        foreach (MetricValue metric in metrics) {
          Console.Write(timestamp.ToString("0") + "-");
          Console.WriteLine(metric.Name + ":" + metric.Value.ToString());
        }
      }, now);
    }
  }
}
