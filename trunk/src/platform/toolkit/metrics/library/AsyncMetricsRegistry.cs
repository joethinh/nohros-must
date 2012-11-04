using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  public class AsyncMetricsRegistry : AbstractMetricsRegistry,
                                      IAsyncMetricsRegistry
  {
    readonly IExecutor executor_;

    #region .ctor
    public AsyncMetricsRegistry(IExecutor executor) {
      executor_ = executor;
    }
    #endregion

    public IAsyncHistogram GetHistogram(MetricName name, bool biased) {
      IAsyncHistogram histogram;
      if (!TryGetMetric(name, out histogram)) {
        histogram = (biased)
          ? (IAsyncHistogram) Histograms.Biased(executor_)
          : (IAsyncHistogram) Histograms.Uniform(executor_);
        Add(name, histogram);
      }
      return histogram;
    }

    public IAsyncMeter GetMeter(MetricName name, string event_type,
      TimeUnit rate_unit) {
      IAsyncMeter meter;
      if (!TryGetMetric(name, out meter)) {
        meter = new AsyncMeter(event_type, rate_unit, executor_);
        Add(name, meter);
      }
      return meter;
    }

    public IAsyncTimed GetTimer(MetricName name, TimeUnit duration_unit) {
      AsyncTimer timer;
      if (!TryGetMetric(name, out timer)) {
        timer = new AsyncTimer(duration_unit,
          new AsyncMeter("calls", TimeUnit.Seconds, executor_),
          Histograms.Biased(), executor_);
        Add(name, timer);
      }
      return timer;
    }

    public bool TryGetHistogram(MetricName name, out IAsyncHistogram histogram) {
      return TryGetMetric(name, out histogram);
    }

    public bool TryGetTimer(MetricName name, out IAsyncTimed timer) {
      return TryGetMetric(name, out timer);
    }

    public bool TryGetMeter(MetricName name, out IAsyncMeter meter) {
      return TryGetMetric(name, out meter);
    }
  }
}
