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
  }
}
