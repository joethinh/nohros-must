using System;

using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A meter metric which measures mean throughput and one-, five-, and
  /// fifteen-minute exponetially-weighted moving average throughputs.
  /// </summary>
  /// <remarks>
  /// <para>
  ///   http://en.wikipedia.org/wiki/Moving_average#Exponential_moving_average
  /// </para>
  /// </remarks>
  public class Meter : IMetered
  {
    const long kTickInterval = 5000000000; // 5 seconds in nanoseconds

    readonly ExponentialWeightedMovingAverage ewma_1_rate_;
    readonly ExponentialWeightedMovingAverage ewma_5_rate_;
    readonly ExponentialWeightedMovingAverage ewma_15_rate_;

    AtomicLong count_;
    AtomicLong last_tick_;
    readonly long start_time_;
    readonly TimeUnit rate_unit_;
    readonly string event_type_;
    readonly Clock clock_;

    /// <summary>
    /// Initializes a new instance of the <see cref=" Meter"/> class by using
    /// the specified meter name, rate unit and clock.
    /// </summary>
    /// <param name="rate_unit">
    /// The rate unit of the new meter.
    /// </param>
    /// <param name="event_type">
    /// The plural name of the event meter is measuring
    /// <example>
    /// <code>
    /// "requests"
    /// </code>
    /// </example>
    /// </param>
    /// <param name="clock">
    /// The clock to use for the meter ticks.
    /// </param>
    internal Meter(string event_type, TimeUnit rate_unit, Clock clock) {
      count_ = new AtomicLong();
      rate_unit_ = rate_unit;
      event_type_ = event_type;
      clock_ = clock;
      start_time_ = clock_.Tick;
      last_tick_ = new AtomicLong(start_time_);
      ewma_1_rate_ = ExponentialWeightedMovingAverages.OneMinute();
      ewma_5_rate_ = ExponentialWeightedMovingAverages.FiveMinute();
      ewma_15_rate_ = ExponentialWeightedMovingAverages.FifteenMinute();
    }

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return rate_unit_; }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return event_type_; }
    }

    /// <summary>
    /// Updates the moving average.
    /// </summary>
    void Tick() {
      ewma_1_rate_.Tick();
      ewma_5_rate_.Tick();
      ewma_15_rate_.Tick();
    }

    /// <summary>
    /// Mark the occurrence of an event.
    /// </summary>
    public void Mark() {
      Mark(1);
    }

    /// <summary>
    /// Mark the occurrence of a given number of events.
    /// </summary>
    /// <param name="n">
    /// The number of events.
    /// </param>
    public void Mark(long n) {
      TickIfNecessary();
      count_.Add(n);
      ewma_1_rate_.Update(n);
      ewma_5_rate_.Update(n);
      ewma_15_rate_.Update(n);
    }

    void TickIfNecessary() {
      long old_tick = last_tick_.Value;
      long new_tick = clock_.Tick;
      long age = new_tick - old_tick;
      if (age > kTickInterval && last_tick_.CompareSet(old_tick, new_tick)) {
        long required_ticks = age/kTickInterval;
        for (long i = 0; i < required_ticks; i++) {
          Tick();
        }
      }
    }

    /// <inheritdoc/>
    public long Count {
      get { return (long) count_; }
    }

    /// <inheritdoc/>
    public double FifteenMinuteRate {
      get {
        TickIfNecessary();
        return ewma_15_rate_.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double FiveMinuteRate {
      get {
        TickIfNecessary();
        return ewma_5_rate_.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get {
        if (Count == 0) {
          return 0.0;
        }
        long elapsed = clock_.Tick - start_time_;
        return ConvertNsRate(Count/(double) elapsed);
      }
    }

    /// <inheritdoc/>
    public double OneMinuteRate {
      get {
        TickIfNecessary();
        return ewma_1_rate_.Rate(rate_unit_);
      }
    }

    double ConvertNsRate(double rate_per_ns) {
      return rate_per_ns * (double)TimeUnitHelper.ToNanos(1, rate_unit_);
    }
  }
}
