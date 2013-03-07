using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class AsyncTimer : IAsyncTimed
  {
    readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    readonly Clock clock_;
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
      BiasedHistogram histogram, IExecutor executor)
      : this(duration_unit, meter, histogram, executor, new UserTimeClock()) {
    }

    public AsyncTimer(TimeUnit duration_unit, AsyncMeter meter,
      BiasedHistogram histogram, IExecutor executor, Clock clock) {
      duration_unit_ = duration_unit;
      meter_ = meter;
      histogram_ = histogram;
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(Run, executor);
      clock_ = clock;
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

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      var now = DateTime.Now;
      long timestamp = clock_.Tick;

      meter_.Report((values, t_context) => {
        var h_values = Report();
        var final_values = new MetricValue[values.Length + h_values.Length];
        Array.Copy(h_values, final_values, h_values.Length);
        Array.Copy(values, 0, final_values, h_values.Length, values.Length);
        callback(final_values, t_context);
      }, context);
    }

    public T Time<T>(CallableDelegate<T> method) {
      long start_time = clock_.Tick;

      // The time should be mensured even if a exception is throwed.
      try {
        return method();
      } finally {
        Update(clock_.Tick - start_time);
      }
    }

    public void Time(RunnableDelegate runnable) {
      long start_time = clock_.Tick;

      // The time should be mensured even if a exception is throwed.
      try {
        runnable();
      } finally {
        Update(clock_.Tick - start_time);
      }
    }

    /// <summary>
    /// Gets a timing <see cref="TimerContext"/>, which measures an elapsed
    /// time in nanoseconds.
    /// </summary>
    /// <returns>
    /// A new <see cref="TimerContext"/>.
    /// </returns>
    public TimerContext Time() {
      return new TimerContext(this, clock_);
    }

    protected MetricValue[] Report() {
      Snapshot snapshot = histogram_.Snapshot;
      return new[] {
        new MetricValue("Min", ConvertFromNs(histogram_.Min)),
        new MetricValue("Max", ConvertFromNs(histogram_.Max)),
        new MetricValue("Mean", ConvertFromNs(histogram_.Mean)),
        new MetricValue("StandardDeviation",
          ConvertFromNs(histogram_.StandardDeviation)),
        new MetricValue("Median", ConvertFromNs(snapshot.Median)),
        new MetricValue("Percentile75", ConvertFromNs(snapshot.Percentile75)),
        new MetricValue("Percentile95", ConvertFromNs(snapshot.Percentile95)),
        new MetricValue("Percentile98", ConvertFromNs(snapshot.Percentile98)),
        new MetricValue("Percentile99", ConvertFromNs(snapshot.Percentile99)),
        new MetricValue("Percentile999", ConvertFromNs(snapshot.Percentile999))
      };
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
    public void Update(long duration) {
      if (duration >= 0) {
        long timestamp = TimeUnitHelper
          .ToSeconds(clock_.Time, TimeUnit.Miliseconds);
        async_tasks_mailbox_.Send(
          () => histogram_.Update(duration, timestamp));
        meter_.Mark();
      }
    }

    double ConvertFromNs(double ns) {
      return ns/TimeUnitHelper.ToNanos(1, duration_unit_);
    }


    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return duration_unit_; }
    }
  }
}
