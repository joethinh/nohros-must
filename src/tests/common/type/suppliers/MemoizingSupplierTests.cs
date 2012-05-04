using System;

using NUnit.Framework;

namespace Nohros
{
  [TestFixture]
  public class MemoizingSupplierTests
  {
    [Test]
    public void ShouldReturnTheSameObjectAfterTheFirstCall() {
      TSupplierMock supplier = new TSupplierMock("mock-supplier");
      MemoizingSupplier<TMock> memoizing =
        new MemoizingSupplier<TMock>(supplier);

      Assert.AreEqual(memoizing.Supply(), memoizing.Supply());
    }
  }
}