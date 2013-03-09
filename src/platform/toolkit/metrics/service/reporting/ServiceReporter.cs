using System;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// A simple reporter which sends out application metrics to a metrics
  /// service periodically.
  /// </summary>
  public class ServiceReporter : AbstractPollingReporter
  {
    #region .ctor
    public ServiceReporter(IMetricsRegistry registry) : base(registry) {
    }
    #endregion

    public override void Run() {
      throw new NotImplementedException();
    }
  }
}
