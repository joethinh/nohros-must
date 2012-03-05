using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Nohros.Concurrent
{
  [TestFixture]
  public class YQueueTests
  {
    [Test]
    public void ShouldEnqueueAndDequeueInOrder() {
      YQueue<long> queue = new YQueue<long>(1);
      queue.Enqueue(10);
      Assert.AreEqual(10, queue.Dequeue());

      queue.Enqueue(1);
      queue.Enqueue(2);
      queue.Enqueue(3);
      Assert.AreEqual(1, queue.Dequeue());
      Assert.AreEqual(2, queue.Dequeue());
      Assert.AreEqual(3, queue.Dequeue());

      queue.Enqueue(4);
      queue.Enqueue(5);
      Assert.AreEqual(4, queue.Dequeue());

      queue.Enqueue(3);
      Assert.AreEqual(5, queue.Dequeue());
      Assert.AreEqual(3, queue.Dequeue());
    }
  }
}
