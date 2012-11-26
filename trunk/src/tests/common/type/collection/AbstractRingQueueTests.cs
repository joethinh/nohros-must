using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Nohros.Collections
{
  public class AbstractRingQueueTests
  {
    class RingQueue_ : AbstractRingQueue<int>
    {
      #region .ctor
      public RingQueue_(IEnumerable<int> elements) : base(elements) {
      }

      public RingQueue_(int size) : base(size) {
      }
      #endregion
    }

    [Test]
    public void ShouldClearTheQueue() {
      var queue = new RingQueue_(10);
      queue.Enqueue(30);
      queue.Clear();
      Assert.That(queue.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShouldEnqueueUpToSize() {
      var queue = new RingQueue_(10);
      for (int i = 0; i < 10; i++) {
        queue.Enqueue(i);
      }
      Assert.Pass();
    }

    [Test]
    public void ShouldThrowInvalidOperationExceptionWhenQueueIsEmptyForDequeue() {
      var queue = new RingQueue_(10);
      Assert.That(() => queue.Dequeue(), Throws.InvalidOperationException);
    }

    [Test]
    public void ShouldReportNumberOfItems() {
      var queue = new RingQueue_(10);
      for (int i = 0; i < 10; i++) {
        queue.Enqueue(i);
        Assert.That(queue.Count, Is.EqualTo(i + 1));
      }

      for (int i = 10; i == 0; i++) {
        queue.Dequeue();
        Assert.That(queue.Count, Is.EqualTo(i - 1));
      }
    }

    [Test]
    public void ShouldDequeueInEnqueueOrder() {
      var queue = new RingQueue_(10);
      for (int i = 0; i < 10; i++) {
        queue.Enqueue(i);
      }

      for (int i = 0; i < 10; i++) {
        Assert.That(queue.Dequeue(), Is.EqualTo(i));
      }
    }

    [Test]
    public void ShouldNotModifyQueue() {
      var queue = new RingQueue_(10);
      queue.Enqueue(10);
      queue.Enqueue(15);
      int count = queue.Count;
      queue.Peek();
      Assert.That(queue.Count, Is.EqualTo(count));
    }

    [Test]
    public void ShouldPeekTheFirstNonConsumedItem() {
      var queue = new RingQueue_(10);
      queue.Enqueue(1);
      queue.Enqueue(2);
      queue.Enqueue(3);
      Assert.That(queue.Peek(), Is.EqualTo(1));
    }
  }
}
