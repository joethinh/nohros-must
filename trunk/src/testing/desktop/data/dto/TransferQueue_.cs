using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;
using Nohros.Data.TransferObjects;

namespace Nohros.Test.Data.DataTransferObjects
{
    [TestFixture]
    public class TransferQueue_
    {
        [Test]
        public void Enqueue()
        {
            TransferQueue<string> queue = new TransferQueue<string>(3);

            queue.Enqueue("ELM0");
            Assert.AreEqual(queue.Peek(), "ELM0");

            queue.Enqueue("ELM1");
            Assert.AreEqual(queue.Peek(), "ELM0");

            queue.Enqueue("ELM2");
            Assert.Throws<InvalidOperationException>(delegate() { queue.Enqueue("ELM3"); });
            Assert.AreEqual(queue.Peek(), "ELM0");
        }

        [Test]
        public void QueueFull()
        {
            TransferQueue<string> queue = new TransferQueue<string>(3);
            queue.QueueFull +=new TransferQueue<string>.QueueFullEventHandler(queue_QueueFull);

            queue.Enqueue("ELM0");
            queue.Enqueue("ELM1");
            queue.Enqueue("ELM2");

            // the queue is empty?
            Assert.Throws<InvalidOperationException>(delegate() { queue.Peek(); });

            // the queue has been cleaned?
            queue.Enqueue("ELM3");
            Assert.AreEqual(queue.Count, 1);
        }

        [Test]
        public void Dequeue()
        {
            TransferQueue<string> queue = new TransferQueue<string>(3);
            queue.QueueFull += new TransferQueue<string>.QueueFullEventHandler(queue_QueueFull);

            queue.Enqueue("ELM0");
            queue.Enqueue("ELM1");
            queue.Enqueue("ELM2");

            Assert.Throws<InvalidOperationException>(delegate() { queue.Dequeue(); });

            queue.Enqueue("ELM3");
            queue.Enqueue("ELM4");
            Assert.AreEqual(queue.Dequeue(), "ELM3");

            Assert.AreEqual(queue.Peek(), "ELM4");

            queue.Enqueue("ELM5");
            queue.Enqueue("ELM6");

            Assert.Throws<InvalidOperationException>(delegate() { queue.Peek(); });

            queue.Enqueue("ELM7");
            queue.Enqueue("ELM8");
            queue.Dequeue();
            queue.Dequeue();

            Assert.AreEqual(queue.Count, 0);
        }

        void queue_QueueFull(string[] array)
        {
        }
    }
}