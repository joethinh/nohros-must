using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A clock implementation which returns the current time in epoch
  /// nanoseconds.
  /// </summary>
  public class UserTimeClock : Clock
  {
    /// <inheritdoc/>
    public override long Tick {
      get { return NanoTime; }
    }
  }
}
