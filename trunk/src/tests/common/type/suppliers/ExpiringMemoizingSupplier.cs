using System;
using System.Threading;

using NUnit.Framework;

namespace Nohros
{
  [TestFixture]
  public class ExpiringMemoizingSupplierTests
  {
    long duration = 5; // seconds

    [Test]
    public void ShouldReturnSameObjectWhileItIsNotExpired() {
      TSupplierMock supplier = new TSupplierMock("supplier");
      ExpiringMemoizingSupplier<TMock> expiring_supplier =
        new ExpiringMemoizingSupplier<TMock>(supplier, duration,
                                                     TimeUnit.Seconds);
      TMock instance = expiring_supplier.Supply();
      Assert.AreEqual(instance, expiring_supplier.Supply());
      Thread.Sleep(3*1000);
      Assert.AreEqual(instance, expiring_supplier.Supply());

      Thread.Sleep(3*1000);
      Assert.AreNotEqual(instance, expiring_supplier.Supply());
    }
  }
}
