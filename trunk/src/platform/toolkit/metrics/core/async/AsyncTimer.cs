using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class AsyncTimer : IAsyncMetered, IAsyncSampling, IAsyncSummarizable
  {
    readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    readonly TimeUnit duration_unit_;
    readonly BiasedHistogram histogram_;
    readonly AsyncMeter meter_;

    #region .ctor
    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">
    /// The scale unit for this timer's duration metrics.
    /// </param>
    public AsyncTimer(TimeUnit duration_unit, AsyncMeter meter,
      BiasedHistogram histogram, IExecutor executor) {
      duration_unit_ = duration_unit;
      meter_ = meter;
      histogram_ = histogram;
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(Run, executor);
    }
    #endregion

    public void GetFifteenMinuteRate(DoubleMetricCallback callback) {
      meter_.GetFifteenMinuteRate(callback);
    }

    public void GetFiveMinuteRate(DoubleMetricCallback callback) {
      meter_.GetFiveMinuteRate(callback);
    }

    public void GetMeanRate(DoubleMetricCallback callback) {
      meter_.GetMeanRate(callback);
    }

    public void GetOneMinuteRate(DoubleMetricCallback callback) {
      meter_.GetOneMinuteRate(callback);
    }

    public TimeUnit RateUnit {
      get { return meter_.RateUnit; }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return meter_.EventType; }
    }

    public void GetCount(LongMetricCallback callback) {
      meter_.GetCount(callback);
    }

    /// <inheritdoc/>
    public void GetSnapshot(SnapshotCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => {
        double[] values = histogram_.Snapshot.Values;
        var converted = new double[values.Length];
        for (int i = 0, j = values.Length; i < j; i++) {
          converted[i] = ConvertFromNs(values[i]);
        }
        callback(new Snapshot(converted), now);
      });
    }

    public void GetMax(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(
        () => callback(ConvertFromNs(histogram_.Max), now));
    }

    public void GetMean(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(
        () => callback(ConvertFromNs(histogram_.Mean), now));
    }

    public void GetMin(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(
        () => callback(ConvertFromNs(histogram_.Min), now));
    }

    public void GetStandardDeviation(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(
        () => callback(ConvertFromNs(histogram_.StandardDeviation), now));
    }

    void Run(RunnableDelegate runnable) {
      runnable();
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">
    /// The length of the duration.
    /// </param>
    /// <param name="unit">
    /// The scale unit of <paramref name="duration"/>
    /// </param>
    public void Update(long duration, TimeUnit unit) {
      Update(TimeUnitHelper.ToNanos(duration, unit));
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">
    /// The length of the duration.
    /// </param>
    void Update(long duration) {
      if (duration >= 0) {
        long timestamp = TimeUnitHelper
          .ToSeconds(Clock.CurrentTimeMilis, TimeUnit.Miliseconds);
        async_tasks_mailbox_.Send(
          () => histogram_.Update(duration, timestamp));
        meter_.Mark(duration);
      }
    }

    double ConvertFromNs(double ns) {
      return ns/TimeUnitHelper.ToNanos(1, duration_unit_);
    }

    /// <summary>
    /// Times and records the duration of event.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by
    /// <paramref name="method"/></typeparam>
    /// <param name="method">
    /// A method whose duration should be timed.
    /// </param>
    /// <returns>
    /// The value returned by <paramref name="method"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// The exception throwed by <paramref name="method"/>.
    /// </exception>
    public T Time<T>(TimedEvent<T> method) {
      long start_time = Clock.NanoTime;

      // The time should be mensured even if a exception is throwed.
      try {
        return method();
      } finally {
        Update(Clock.NanoTime - start_time);
      }
    }

    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return duration_unit_; }
    }
  }
}
