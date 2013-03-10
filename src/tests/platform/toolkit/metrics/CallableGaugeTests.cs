using System;
using NUnit.Framework;

namespace Nohros.Metrics
{
  public class CallableGaugeTests
  {
    [Test]
    public void ShouldReturnTheValueOfCallable() {
      var gauge = new CallableGauge<int>(() => 10);
      Assert.AreEqual(10, gauge.Value);
    }
  }
}
