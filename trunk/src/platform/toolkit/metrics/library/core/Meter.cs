using System;

namespace Nohros.Metrics
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
  public class Meter : ManualMeter, IMetered, IMeter
  {
    readonly Clock clock_;

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
    public Meter(string event_type, TimeUnit rate_unit)
      : this(event_type, rate_unit, new UserTimeClock()) {
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="Meter"/> class by using
    /// the given clock, the work "events" as event type and
    /// <see cref="TimeUnit.Seconds"/> as event rate unit.
    /// </summary>
    /// <param name="clock">
    /// The clock to mark the passege of time.
    /// </param>
    public Meter(Clock clock) : this("events", TimeUnit.Seconds, clock) {
      clock_ = clock;
    }

    public Meter(string event_type, TimeUnit rate_unit, Clock clock)
      : base(event_type, rate_unit, clock.Tick) {
      clock_ = clock;
    }
    #endregion

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
    public virtual void Mark(long n) {
      base.Mark(n, clock_.Tick);
    }

    public override void Mark(long n, long time) {
      throw new NotSupportedException(
        "The time could not be manualy set on Meter. If you want to manually set the time of events, use the ManualMeter.");
    }

    /// <inheritdoc/>
    public override double FifteenMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return base.FifteenMinuteRate;
      }
    }

    /// <inheritdoc/>
    public override double FiveMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return base.FiveMinuteRate;
      }
    }

    /// <inheritdoc/>
    public override double OneMinuteRate {
      get {
        TickIfNecessary(clock_.Tick);
        return base.OneMinuteRate;
      }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get { return GetMeanRate(clock_.Tick); }
    }

    public override void Report<T>(MetricReportCallback<T> callback, T context) {
      callback(Report(), context);
    }

    public override MetricValue[] Report() {
      string rate_unit = UnitHelper.FromRate(EventType, RateUnit);
      return new[] {
        new MetricValue(MetricValueType.Count, Count, EventType),
        new MetricValue(MetricValueType.MeanRate, MeanRate, rate_unit),
        new MetricValue(MetricValueType.OneMinuteRate, OneMinuteRate, rate_unit)
        ,
        new MetricValue(MetricValueType.FiveMinuteRate, FiveMinuteRate,
          rate_unit),
        new MetricValue(MetricValueType.FifteenMinuteRate, FifteenMinuteRate,
          rate_unit)
      };
    }
  }
}
