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

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      callback(Report(), context);
    }

    public MetricValue[] Report() {
      string rate_unit = UnitHelper.FromRate(EventType, RateUnit);
      return new[] {
        new MetricValue("Count", Count, EventType),
        new MetricValue("MeanRate", MeanRate, rate_unit),
        new MetricValue("OneMinuteRate", OneMinuteRate, rate_unit),
        new MetricValue("FiveMinuteRate", FiveMinuteRate, rate_unit),
        new MetricValue("FifteenMinuteRate", FifteenMinuteRate, rate_unit)
      };
    }
  }
}
