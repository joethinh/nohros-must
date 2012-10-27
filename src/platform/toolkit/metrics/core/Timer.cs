using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class Timer : IMetered, ISampling, ISummarizable
  {
    readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    readonly Clock clock_;
    readonly TimeUnit duration_unit_;
    readonly IHistogram histogram_;
    readonly Meter meter_;
    readonly TimeUnit rate_unit_;

    #region .ctor
    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">
    /// The scale unit for this timer's duration metrics.
    /// </param>
    /// <param name="rate_unit">
    /// The scale unit for this timer's rate metrics.
    /// </param>
    /// <param name="clock">
    /// The clock used to calculate duration.
    /// </param>
    Timer(TimeUnit duration_unit, TimeUnit rate_unit, Clock clock)
      : this(duration_unit, rate_unit, clock, Executors.ThreadPoolExecutor()) {
    }

    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">
    /// The scale unit for this timer's duration metrics.
    /// </param>
    /// <param name="rate_unit">
    /// The scale unit for this timer's rate metrics.
    /// </param>
    /// <param name="clock">
    /// The clock used to calculate duration.
    /// </param>
    Timer(TimeUnit duration_unit, TimeUnit rate_unit, Clock clock,
      IExecutor executor) {
      duration_unit_ = duration_unit;
      rate_unit_ = rate_unit;
      meter_ = new Meter("calls", rate_unit, clock);
      clock_ = clock;
      histogram_ = Histograms.Biased();
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(Run, executor);
    }
    #endregion

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return rate_unit_; }
    }

    public void GetCount(LongMetricCallback callback) {
      meter_.GetCount(callback);
    }

    /// <inheritdoc/>
    public void GetFifteenMinuteRate(DoubleMetricCallback callback) {
      meter_.GetFifteenMinuteRate(callback);
    }

    /// <inheritdoc/>
    public void GetFiveMinuteRate(DoubleMetricCallback callback) {
      meter_.GetFiveMinuteRate(callback);
    }

    /// <inheritdoc/>
    public void GetMeanRate(DoubleMetricCallback callback) {
      meter_.GetMeanRate(callback);
    }

    /// <inheritdoc/>
    public void GetOneMinuteRate(DoubleMetricCallback callback) {
      meter_.GetOneMinuteRate(callback);
    }

    /// <inheritdoc/>
    public string EventType {
      get { return meter_.EventType; }
    }

    /// <inheritdoc/>
    public void GetSnapshot(SnapshotCallback callback) {
      histogram_.GetSnapshot((snapshot, now) => {
        double[] values = snapshot.Values;
        var converted = new double[values.Length];
        for (int i = 0, j = values.Length; i < j; i++) {
          converted[i] = ConvertFromNs(values[i]);
        }
        callback(new Snapshot(converted), now);
      });
    }


    /// <summary>
    /// Gets the shortest recorded duration.
    /// </summary>
    /// <value>The shortest recorded duration.</value>
    public void GetMin(DoubleMetricCallback callback) {
      histogram_
        .GetMin((min, now) => callback(ConvertFromNs(min), now));
    }

    /// <summary>
    /// Gets the longest recorded duration.
    /// </summary>
    /// <value>The longest recorded duration.</value>
    public void GetMax(DoubleMetricCallback callback) {
      histogram_
        .GetMax(
          (max, now) => callback(ConvertFromNs(max), now));
    }

    /// <summary>
    /// Gets the arithmetic mean of all recorded durations.
    /// </summary>
    /// <value>The arithmetic mean of all recorded durations.</value>
    public void GetMean(DoubleMetricCallback callback) {
      histogram_
        .GetMean((mean, now) => callback(ConvertFromNs(mean), now));
    }

    /// <summary>
    /// Gets the standard deviation of all recorded durations.
    /// </summary>
    /// <value>The standard deviation of all recorded durations.</value>
    public void GetStandardDeviation(DoubleMetricCallback callback) {
      histogram_
        .GetStandardDeviation(
          (stdev, now) => callback(ConvertFromNs(stdev), now));
    }

    void Run(RunnableDelegate runnable) {
      runnable();
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    /// <param name="unit">The scale unit of <paramref name="duration"/></param>
    public void Update(long duration, TimeUnit unit) {
      Update(TimeUnitHelper.ToNanos(duration, unit));
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    void Update(long duration) {
      if (duration >= 0) {
        histogram_.Update(duration);
        meter_.Mark();
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
    /// <param name="method">A method whose duration should be timed.</param>
    /// <returns>The value returned by <paramref name="method"/>.</returns>
    /// <exception cref="Exception">Exception if <paramref name="method"/>
    /// tjrows an <see cref="Exception"/>.</exception>
    public T Time<T>(TimedEvent<T> method) {
      long start_time = clock_.Tick;

      // The time should be mensured even if a exception is throwed.
      try {
        return method();
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

    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return rate_unit_; }
    }
  }
}
