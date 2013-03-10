using System;
using NUnit.Framework;

namespace Nohros.Metrics
{
  public class MeterTests
  {
    [Test]
    public void ShouldCreateBlankMeter() {
      var meter = new Meter("thing", TimeUnit.Seconds);
      Assert.That(meter.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShouldCreateMeterWithThreeeEvents() {
      var meter = new Meter("thing", TimeUnit.Seconds);
      meter.Mark(3);
      Assert.That(meter.Count, Is.EqualTo(3));
    }
  }
}
