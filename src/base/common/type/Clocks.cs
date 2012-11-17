using System;

namespace Nohros.type
{
  /// <summary>
  /// A factory for <see cref="Clock"/>.
  /// </summary>
  public class Clocks
  {
    /// <summary>
    /// Creates a new instance of the <see cref="UserTimeClock"/>.
    /// </summary>
    /// <returns></returns>
    public static Clock UserTimeClock() {
      return new UserTimeClock();
    }
  }
}
