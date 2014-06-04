using System;
using System.Diagnostics;

namespace Nohros.Metrics
{
  /// <summary>
  /// A <see cref="Clock"/> that uses a <see cref="Stopwatch"/> to mark the
  /// passage of time.
  /// </summary>
  public class UserTimeClock : Clock
  {
    static readonly long kNanoSecondDelta = 1000000000L / Stopwatch.Frequency;
    readonly Stopwatch stop_watch_;

    #region .ctor
    public UserTimeClock() {
      stop_watch_ = Stopwatch.StartNew();
    }
    #endregion

    public override long Tick {
      get { return stop_watch_.ElapsedTicks*kNanoSecondDelta; }
    }

    public override long Time {
      get { return stop_watch_.ElapsedMilliseconds; }
    }
  }
}
