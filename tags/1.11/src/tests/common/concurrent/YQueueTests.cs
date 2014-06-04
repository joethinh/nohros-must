using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Nohros.Concurrent
{
  [TestFixture]
  public class YQueueTests
  {
    [Test]
    public void ShouldConcurrentlyEnqueueAndDequeueInOrder() {
      YQueue<long> queue = new YQueue<long>(1);
      queue.Enqueue(10);
      Assert.AreEqual(10, queue.Dequeue());

      BackgroundThreadFactory factory = new BackgroundThreadFactory();
      Thread enqueue_thread = factory
        .CreateThread(delegate() {
          for (int i = 0; i < 10000; i++) {
            queue.Enqueue(i);
          }
        });

      Thread dequeue_thread = factory
        .CreateThread(delegate() {
          for (int i = 0; i < 10000; i++) {
            long k;
            while (!queue.Dequeue(out k)) { }
            Assert.That(k, Is.EqualTo(i));
          }
        });

      dequeue_thread.Start();
      enqueue_thread.Start();

      enqueue_thread.Join();
      dequeue_thread.Join();
    }

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

      long l;
      queue.Dequeue(out l);
      queue.Dequeue(out l);
      queue.Dequeue(out l);

      queue.Enqueue(89);
      Assert.That(queue.Dequeue(), Is.EqualTo(89));
    }

    [Test]
    public void ShouldReportIfQueueIsEmpty() {
      YQueue<long> queue = new YQueue<long>(1);
      Assert.That(queue.IsEmpty,Is.EqualTo(true));

      queue.Enqueue(10);
      Assert.That(queue.IsEmpty, Is.EqualTo(false));
      Assert.AreEqual(10, queue.Dequeue());
      Assert.That(queue.IsEmpty, Is.EqualTo(true));

      queue.Enqueue(1);
      queue.Enqueue(2);
      queue.Enqueue(3);
      Assert.That(queue.IsEmpty, Is.EqualTo(false));
      queue.Dequeue();
      Assert.That(queue.IsEmpty, Is.EqualTo(false));
      queue.Dequeue();
      Assert.That(queue.IsEmpty, Is.EqualTo(false));
      queue.Dequeue();
      Assert.That(queue.IsEmpty, Is.EqualTo(true));
    }
  }
}
