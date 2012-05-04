using System;

using NUnit.Framework;

namespace Nohros.Concurrent
{
  [TestFixture]
  public class AtomicLongTests
  {
    [Test]
    public void ShouldAtomicallyAdd() {
      AtomicLong atl = new AtomicLong(0);
      long lg = atl.Add(1);
      Assert.AreEqual(1, lg);
    }

    [Test]
    public void ShouldAtomiccallyCompareAndExchange() {
      AtomicLong atl = new AtomicLong(69);
      long lg = atl.CompareExchange(69, 70);
      Assert.AreEqual(69, lg);

      atl = new AtomicLong(80);
      lg = atl.CompareExchange(85, 90);
      Assert.AreEqual(lg, 80);

    }

    [Test]
    public void ShouldAtomicallyDecrement() {
      AtomicLong atl = new AtomicLong(15);
      long lg = atl.Decrement();
      Assert.AreEqual(14, lg);
    }

    [Test]
    public void ShouldAtomicallyExchange() {
      AtomicLong atl = new AtomicLong(10);
      long lg = atl.Exchange(21);
      Assert.AreEqual(10, lg);
    }

    [Test]
    public void ShouldAtomicallyIncrement() {
      AtomicLong atl = new AtomicLong(45);
      long lg = atl.Increment();
      Assert.AreEqual(46, lg);
    }

    [Test]
    public void ShouldGetTheValue() {
      AtomicLong atl = new AtomicLong();
      Assert.AreEqual(0, atl.Value);

      atl = new AtomicLong(15);
      Assert.AreEqual(15, atl.Value);
    }
  }
}