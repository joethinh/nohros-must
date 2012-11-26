using System;
using NUnit.Framework;

namespace Nohros.Collections
{
  public class RingQueueTests
  {
    [Test]
    public void ShouldThrowInvalidOperationExceptionWhenQueueIsFullForEnqueue() {
      var queue = new RingQueue<int>(10);
      for (int i = 0; i < 10; i++) {
        queue.Enqueue(i);
      }
      Assert.That(() => queue.Enqueue(10), Throws.InvalidOperationException);
    }
  }
}
