using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class Timer : ITimed
  {
    readonly TimeUnit duration_unit_;
    readonly BiasedHistogram histogram_;
    readonly Meter meter_;

    #region .ctor
    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">
    /// The scale unit for this timer's duration metrics.
    /// </param>
    public Timer(TimeUnit duration_unit, Meter meter, BiasedHistogram histogram) {
      duration_unit_ = duration_unit;
      meter_ = meter;
      histogram_ = histogram;
    }
    #endregion

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return meter_.RateUnit; }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return meter_.EventType; }
    }

    public double Count {
      get { return meter_.Count; }
    }

    /// <inheritdoc/>
    public double FifteenMinuteRate {
      get { return meter_.FifteenMinuteRate; }
    }

    /// <inheritdoc/>
    public double FiveMinuteRate {
      get { return meter_.FiveMinuteRate; }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get { return meter_.MeanRate; }
    }

    /// <inheritdoc/>
    public double OneMinuteRate {
      get { return meter_.OneMinuteRate; }
    }

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get {
        double[] values = histogram_.Snapshot.Values;
        var converted = new double[values.Length];
        for (int i = 0, j = values.Length; i < j; i++) {
          converted[i] = ConvertFromNs(values[i]);
        }
        return new Snapshot(converted);
      }
    }

    /// <summary>
    /// Gets the shortest recorded duration.
    /// </summary>
    /// <value>The shortest recorded duration.</value>
    public double Min {
      get { return ConvertFromNs(histogram_.Min); }
    }

    /// <summary>
    /// Gets the longest recorded duration.
    /// </summary>
    /// <value>The longest recorded duration.</value>
    public double Max {
      get { return ConvertFromNs(histogram_.Max); }
    }

    /// <summary>
    /// Gets the arithmetic mean of all recorded durations.
    /// </summary>
    /// <value>The arithmetic mean of all recorded durations.</value>
    public double Mean {
      get { return ConvertFromNs(histogram_.Mean); }
    }

    /// <summary>
    /// Gets the standard deviation of all recorded durations.
    /// </summary>
    /// <value>The standard deviation of all recorded durations.</value>
    public double StandardDeviation {
      get { return ConvertFromNs(histogram_.StandardDeviation); }
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    /// <param name="unit">The scale unit of <paramref name="duration"/></param>
    public void Update(long duration, TimeUnit unit) {
      Update(TimeUnitHelper.ToNanos(duration, unit));
    }

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      histogram_.Report(
        (h_values, h_context) => meter_.Report(
          (m_values, m_context) => {
            var values = new MetricValue[m_values.Length + h_values.Length];
            Array.Copy(h_values, values, h_values.Length);
            Array.Copy(m_values, 0, values, h_values.Length, m_values.Length);
            callback(values, m_context);
          }, h_context), context);
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
      long start_time = Clock.NanoTime;

      // The time should be mensured even if a exception is throwed.
      try {
        return method();
      } finally {
        Update(Clock.NanoTime - start_time);
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
      return new TimerContext(this);
    }

    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return duration_unit_; }
    }
  }
}
