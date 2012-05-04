using System;

using NUnit.Framework;

namespace Nohros.Concurrent
{
  [TestFixture]
  public class AtomicLongArrayTests
  {
    public long[] GetData() {
      return new long[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    [Test]
    public void ShouldAtomicallyGetTheArrayLength() {
      AtomicLongArray ata = new AtomicLongArray(10);
      Assert.AreEqual(10, ata.Length);
    }

    [Test]
    public void ShouldAtomicallyDecrementElementAndReturnDecrementedValue() {
      long[] data = GetData();

      AtomicLongArray ata = new AtomicLongArray(data);
      long old_value = data[4];
      long lg = ata.Decrement(4);
      Assert.AreEqual(old_value - 1, lg);
      Assert.AreEqual(old_value - 1, ata[4]);
    }

    [Test]
    public void ShouldAtomicallyIncrementElementAndReturnIncrementeddValue() {
      long[] data = GetData();
      
      AtomicLongArray ata = new AtomicLongArray(data);

      long old_value = data[2];
      long lg = ata.Increment(2);
      Assert.AreEqual(old_value + 1, lg);
      Assert.AreEqual(old_value + 1, ata[2]);
    }

    [Test]
    public void ShouldAtomicallyExchangeElementAndReturnOldValue() {
      long[] data = GetData();

      AtomicLongArray ata = new AtomicLongArray(data);
      long old_value = data[3];
      long lg = ata.Exchange(3, 20);
      Assert.AreEqual(old_value, lg);
      Assert.AreEqual(20, ata[3]);
    }

    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ShouldThrowArgumentOutOfRangeExceptionWhenLengthIsLessThanZero() {
      AtomicLongArray ata = new AtomicLongArray(-1);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ShouldThrowArgumentNullExceptionWhenArrayIsNull() {
      AtomicLongArray ata = new AtomicLongArray(null);
    }
  }
}