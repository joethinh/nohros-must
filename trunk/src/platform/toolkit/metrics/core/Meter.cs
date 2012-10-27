using System;

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
    public double FifteenMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return ewma_15_rate_.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double FiveMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return ewma_5_rate_.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double OneMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return ewma_1_rate_.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get {
        long timestamp = clock_.Tick;
        if (count_ == 0) {
          return 0.0;
        }

        long elapsed = timestamp - start_time_;
        double rate = count_/(double) elapsed;
        return ConvertNsRate(rate);
      }
    }

    void Run(RunnableDelegate runnable) {
      runnable();
    }

    /// <summary>
    /// Updates the moving average.
    /// </summary>
    void Tick() {
      async_tasks_mailbox_.Send(() => {
        ewma_1_rate_.Tick();
        ewma_5_rate_.Tick();
        ewma_15_rate_.Tick();
      });
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
      long timestamp = clock_.Tick;
      async_tasks_mailbox_.Send(() => {
        TickIfNecessary(timestamp);
        count_ += n;
        ewma_1_rate_.Update(n);
        ewma_5_rate_.Update(n);
        ewma_15_rate_.Update(n);
      });
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

    /// <inheritdoc/>
    public long Count {
      get { return count_; }
    }
  }
}
