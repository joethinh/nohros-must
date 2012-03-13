using System;
using System.Text;
using NUnit.Framework;

namespace Nohros.Toolkit.Metrics
{
  [TestFixture]
  public class ClockTest
  {
    [Test]
    public void UserTimeClock() {
      UserTimeClock clock = new UserTimeClock();
      Assert.LessOrEqual(Math.Abs(Math.Abs(clock.Time) - Math.Abs(Clock.CurrentTimeMilis)), 100);

      Assert.LessOrEqual(Math.Abs(Math.Abs(clock.Tick) - Math.Abs(Clock.NanoTime)), 100);
    }
  }
}
