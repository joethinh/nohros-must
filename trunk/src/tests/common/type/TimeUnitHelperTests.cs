using System;
using NUnit.Framework;

namespace Nohros.Common
{
  public class TimeUnitHelperTests
  {
    [Test]
    public void ShouldConvertToUnixEpoch() {
      var date = new DateTime(1970, 1, 1);
      var epoch = TimeUnitHelper.ToUnixTime(date);
      Assert.AreEqual(0, epoch);

      date = new DateTime(1970, 1, 1, 23, 0, 0);
      epoch = TimeUnitHelper.ToUnixTime(date);
      Assert.AreEqual(23*60*60*1000, epoch);
    }

    [Test]
    public void ShouldConvertToMilliseconds() {
      var millis = TimeUnitHelper.ToMillis(10, TimeUnit.Seconds);
      Assert.That(millis, Is.EqualTo(10 * 1000));

      var minutes = TimeUnitHelper.ToMillis(10, TimeUnit.Minutes);
      Assert.That(minutes, Is.EqualTo(10 * 60 * 1000));
    }
  }
}
