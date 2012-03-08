using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Nohros.Tests
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