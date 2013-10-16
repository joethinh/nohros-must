using System;
using System.Threading;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// A <see cref="IPollingMetricsReporter"/> that forwards all its methods to
  /// another <see cref="IPollingMetricsReporter"/> object.
  /// </summary>
  public class ForwarderReporter : IPollingMetricsReporter
  {
    #region .ctor
    public ForwarderReporter(IPollingMetricsReporter reporter) {
      Reporter = reporter;
    }
    #endregion

    /// <inheritdoc/>
    public void Shutdown() {
      Reporter.Shutdown();
    }

    /// <inheritdoc/>
    public void Shutdown(WaitHandle waiter) {
      Reporter.Shutdown(waiter);
    }

    /// <inheritdoc/>
    public void Start(long period, TimeUnit unit) {
      Reporter.Start(period, unit);
    }

    /// <inheritdoc/>
    public void Start(long period, TimeUnit unit,
      MetricPredicate predicate) {
      Reporter.Start(period, unit, predicate);
    }

    public IPollingMetricsReporter Reporter { get; set; }
  }
}
