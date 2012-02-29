using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class Timer : IMetered, IStoppable, ISampling, ISummarizable
  {
    readonly TimeUnit duration_unit_;
    readonly TimeUnit rate_unit_;
    readonly Meter meter_;
    readonly Histogram histogram_;
    readonly Clock clock_;

    #region .ctor
    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">The scale unit for this timer's duration
    /// metrics.</param>
    /// <param name="rate_unit">The scale unit for this timer's rate metrics.
    /// </param>
    /// <param name="clock">The clock used to calculate duration.</param>
    Timer(TimeUnit duration_unit, TimeUnit rate_unit, Clock clock) {
      duration_unit_ = duration_unit;
      rate_unit_ = rate_unit;
      clock_ = clock;
    }
    #endregion

    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return rate_unit_; }
    }

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return rate_unit_; }
    }

    /// <summary>
    /// Clears all the recorded values.
    /// </summary>
    public void Clear() {
      histogram_.Clear();
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
      return ns / TimeUnitHelper.ToNanos(1, duration_unit_);
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

    /// <inheritdoc/>
    public void Stop() {
      meter_.Stop();
    }

    /// <inheritdoc/>
    public long Count {
      get { return histogram_.Count; }
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

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get {
        double[] values = histogram_.Snapshot.Values;
        double[] converted = new double[values.Length];
        for (int i = 0, j = values.Length; i < j; i++) {
          converted[i] = ConvertFromNs(values[i]);
        }
        return new Snapshot(converted);
      }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return meter_.EventType; }
    }
  }
}
