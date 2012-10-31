using System;
using NUnit.Framework;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  [TestFixture]
  public class CounterTest
  {
    [Test]
    public void ShouldStartsAtZero() {
      Counter counter = new Counter(Executors.SameThreadExecutor());
      Assert.That(counter.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShouldIncrementByOne() {
      Counter counter = new Counter(Executors.SameThreadExecutor());
      counter.Increment();
      Assert.That(counter.Count, Is.EqualTo(1));
    }

    [Test]
    public void ShouldIncrementBySpecifiedDelta() {
      Counter counter = new Counter(Executors.SameThreadExecutor());
      counter.Increment(12);
      Assert.That(counter.Count, Is.EqualTo(12));
    }

    [Test]
    public void ShouldDecrementByOne()
    {
      Counter counter = new Counter(Executors.SameThreadExecutor());
      counter.Decrement();
      Assert.That(counter.Count, Is.EqualTo(-1));
    }

    [Test]
    public void ShouldDecrementBySpecifiedDelta()
    {
      Counter counter = new Counter(Executors.SameThreadExecutor());
      counter.Decrement(12);
      Assert.That(counter.Count, Is.EqualTo(-12));
    }
  }
}
