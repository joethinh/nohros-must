using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A timing context
  /// </summary>
  /// <seealso cref="Timer.Time{T}"/>
  public class TimerContext
  {
    readonly Timer timer_;
    readonly long start_time_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TimerContext"/> with the
    /// current time as its starting value and with the given
    /// <see cref="Timer"/>.
    /// </summary>
    /// <param name="timer">The <see cref="Timer"/> to report elapsed time.
    /// </param>
    public TimerContext(Timer timer) {
      timer_ = timer;
      start_time_ = Clock.NanoTime;
    }
    #endregion

    /// <summary>
    /// Stops recording the elapsed time, updates the timer and returns the
    /// elapsed time.
    /// </summary>
    public long Stop() {
      long elapsed_nanos = Clock.NanoTime - start_time_;
      timer_.Update(elapsed_nanos, TimeUnit.Nanoseconds);
      return elapsed_nanos;
    }
  }
}
