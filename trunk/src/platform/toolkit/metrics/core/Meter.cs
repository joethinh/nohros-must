using System;
using System.Collections.Generic;
using System.Text;

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
  public class Meter : IMetered, IStoppable
  {
    static const long kInterval = 5; // seconds.

    readonly ExponentialWeightedMovingAverage m1Rate;
    readonly ExponentialWeightedMovingAverage m5Rate;
    readonly ExponentialWeightedMovingAverage m15Rate;

    readonly AtomicLong count_ = new AtomicLong();
    readonly long start_time_;
    readonly TimeUnit rate_unit_;
    readonly string event_type_;
    readonly Clock clock_;

    /// <summary>
    /// Initializes a new instance of the <see cref=" Meter"/> class by using
    /// the specified meter name, rate unit and clock.
    /// </summary>
    /// <param name="rate_unit">The rate unit of the new meter.</param>
    /// <param name="event_type">The plural</param>
    /// <param name="clock"></param>
    Meter(TimeUnit rate_unit, string event_type, Clock clock) {
      rate_unit_ = rate_unit;
      event_type_ = event_type;
      clock_ = clock;
      start_time_ = clock_.Tick;
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
      m1Rate.Tick();
      m5Rate.Tick();
      m15Rate.Tick();
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
    /// <param name="n">The number of events.</param>
    public void Mark(long n) {
      count_.Add(n);
      m1Rate.Update(n);
      m5Rate.Update(n);
      m15Rate.Update(n);
    }

    /// <inheritdoc/>
    public long Count {
      get {
        return (long)count_;
      }
    }

    /// <inheritdoc/>
    public double FifteenMinuteRate {
      get {
        return m15Rate.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double FiveMinuteRate {
      get {
        return m5Rate.Rate(rate_unit_);
      }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get {
        if (Count == 0) {
          return 0.0;
        } else {
          long elapsed = clock_.Tick - start_time_;
          return ConvertNsRate(Count / (double)elapsed);
        }
      }
    }

    /// <inheritdoc/>
    public double OneMinuteRate {
      get {
        return m1Rate.Rate(rate_unit_);
      }
    }

    double ConvertNsRate(double rate_per_ns) {
      return rate_per_ns * (double)TimeUnitHelper.ToNanos(1, rate_unit_);
    }

    /// <inheritdoc/>
    public void Stop() {
    }
  }
}
