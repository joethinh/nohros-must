using System;

namespace Nohros
{
  public class UserTimeClock : Clock
  {
    /// <inheritdoc/>
    public override long Tick {
      get { return NanoTime; }
    }
  }
}
