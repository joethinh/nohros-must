using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An exponentially-weighted moving average.
  /// </summary>
  /// <remarks>
  /// <para>
  /// UNIX Load Average: How It Works
  ///  http://www.teamquest.com/pdfs/whitepaper/ldavg1.pdf
  /// </para>
  /// <para>
  /// UNIX Load Average: Not Your Average Average
  ///  http://www.teamquest.com/pdfs/whitepaper/ldavg2.pdf
  /// </para>
  /// </remarks>
  public class ExponentialWeightedMovingAverage
  {
    const int kInterval = 5;
    const double kMinutesPerSecond = 60.0;
    const int kOneMinute = 1;
    const int kFiveMinutes = 5;
    const int kFifteenMinutes = 15;
    const double kM1Alpha = 1 - Math.Exp(-kInterval / kMinutesPerSecond / kOneMinute);
    const double kM5Alpha = 1 - Math.Exp(-kInterval / kMinutesPerSecond / kFiveMinutes);
    const double kM15Alpha = 1 - Math.Exp(-kInterval / kMinutesPerSecond / kFifteenMinutes);

    volatile bool initialized_ = false;
    volatile double rate_ = 0.0;

    AtomicLong uncounted_;
    readonly double alpha_;
    readonly double interval_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ExponentialWeightedMovingAverage"/> class by using the
    /// specified smoothing constant, expected tick interval and time unit of
    /// the tick interval.
    /// </summary>
    /// <param name="alpha">The smoothing constant.</param>
    /// <param name="interval">The expected tick interval.</param>
    /// <param name="interval_unit">The time unit of the tick interval.</param>
    public ExponentialWeightedMovingAverage(double alpha, long interval,
      TimeUnit interval_unit) {
      interval_ = interval;
      alpha_ = alpha;
      uncounted_ = new AtomicLong();
    }
    #endregion

    public static ExponentialWeightedMovingAverage OneMinuteEWMA() {
      return new ExponentialWeightedMovingAverage(
        kM1Alpha, kInterval, TimeUnit.Seconds);
    }

    public static ExponentialWeightedMovingAverage FiveMinuteEWMA() {
      return new ExponentialWeightedMovingAverage(
        kM5Alpha, kInterval, TimeUnit.Seconds);
    }

    public static ExponentialWeightedMovingAverage FifteenMinuteEWMA() {
      return new ExponentialWeightedMovingAverage(
        kM15Alpha, kInterval, TimeUnit.Seconds);
    }

    /// <summary>
    /// Update the moving average with the new value.
    /// </summary>
    /// <param name="n">The new value.</param>
    public void Update(long n) {
      uncounted_.Add(n);
    }

    /// <summary>
    /// Mark the passage of time and decay the current rate accordingly.
    /// </summary>
    public void Tick() {
      long count = uncounted_.Exchange(0);
      double instant_rate = count / interval_;
      if (initialized_) {
        rate_ += (alpha_ * (instant_rate - rate_));
      } else {
        rate_ = instant_rate;
        initialized_ = true;
      }
    }

    /// <summary>
    /// Gets the rate in the given units of time
    /// </summary>
    public double Rate(TimeUnit rate_unit) {
      return rate_ * (double)TimeUnitHelper.ToNanos(1, rate_unit);
    }
  }
}
