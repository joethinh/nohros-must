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
  public class Meter : IMetered, IMetric
  {
    struct Sample
    {
      public readonly long tick;
      public readonly long value;
      public Sample(long tick, long value) {
        this.tick = tick;
        this.value = value;
      }
    }

    const long kTickInterval = 5000000000; // 5 seconds in nanoseconds
    readonly Clock clock_;
    readonly string event_type_;
    readonly ExponentialWeightedMovingAverage ewma_15_rate_;

    readonly ExponentialWeightedMovingAverage ewma_1_rate_;
    readonly ExponentialWeightedMovingAverage ewma_5_rate_;
    readonly TimeUnit rate_unit_;
    readonly long start_time_;
    long count_;
    long last_tick_;

    #region .ctor
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
    public Meter(string event_type, TimeUnit rate_unit, Clock clock) {
      count_ = 0;
      rate_unit_ = rate_unit;
      event_type_ = event_type;
      clock_ = clock;
      start_time_ = clock_.Tick;
      last_tick_ = start_time_;
      ewma_1_rate_ = ExponentialWeightedMovingAverages.OneMinute();
      ewma_5_rate_ = ExponentialWeightedMovingAverages.FiveMinute();
      ewma_15_rate_ = ExponentialWeightedMovingAverages.FifteenMinute();
    }
    #endregion

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return rate_unit_; }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return event_type_; }
    }

    /// <inheritdoc/>
    public long Count {
      get { return count_; }
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
    }

    public void AsyncMark(Sample sample) {
      TickIfNecessary(sample.tick);
      count_ += sample.value;
      ewma_1_rate_.Update(sample.value);
      ewma_5_rate_.Update(sample.value);
      ewma_15_rate_.Update(sample.value);
    }

    void TickIfNecessary(long now) {
      long age = now - last_tick_;
      last_tick_ = now;
      if (age > kTickInterval) {
        long required_ticks = age/kTickInterval;
        for (long i = 0; i < required_ticks; i++) {
          Tick();
        }
      }
    }

    double ConvertNsRate(double rate_per_ns) {
      return rate_per_ns*TimeUnitHelper.ToNanos(1, rate_unit_);
    }
  }
}
