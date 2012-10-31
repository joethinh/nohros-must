using System;
using System.Text;
using NUnit.Framework;

namespace Nohros.Metrics
{
  [TestFixture]
  public class ClockTest
  {
    [Test]
    public void UserTimeClock() {
      UserTimeClock clock = new UserTimeClock();

      Assert.That(
        Math.Abs(Math.Abs(clock.Time) - Math.Abs(Clock.CurrentTimeMilis)),
        Is.LessThanOrEqualTo(100));

      Assert.That(
        Math.Abs(Math.Abs(clock.Tick) - Math.Abs(Clock.NanoTime)),
        Is.LessThanOrEqualTo(100));
    }
  }
}
