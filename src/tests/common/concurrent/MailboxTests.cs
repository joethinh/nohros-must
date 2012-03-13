using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using NUnit.Framework;

namespace Nohros.Concurrent.Tests
{
  [TestFixture]
  public class MailboxTests
  {
    [Test]
    public void SholudReadOnWriterOrder() {
      Mailbox<long> mailbox = new Mailbox<long>();

      for (int i = 0, j = 15; i < j; i++) {
        mailbox.Send(i);
      }

      for (int i = 0, j = 15; i < j; i++) {
        long lg;
        mailbox.Receive(out lg, 0);
        Assert.AreEqual(i, lg);
      }
    }

    [Test]
    public void ShouldReturnFalseOnTimeout() {
      Mailbox<long> mailbox = new Mailbox<long>();
      long lg;
      bool ok = mailbox.Receive(out lg, 300);
      Assert.AreEqual(false, ok);
    }

    [Test]
    public void ShouldReturnTrueWhenOnSuccessfullMessageRetrieval() {
      Mailbox<long> mailbox = new Mailbox<long>();
      long lg;
      mailbox.Send(1);
      bool ok = mailbox.Receive(out lg, 0);
      Assert.IsTrue(ok);
    }

    [Test]
    public void ShouldNotBlockWhenMailboxHasMessages() {
      Mailbox<long> mailbox = new Mailbox<long>();
      long lg;
      mailbox.Send(2);

      Stopwatch watch = new Stopwatch();
      watch.Start();
      mailbox.Receive(out lg, 300);
      watch.Stop();

      Assert.LessOrEqual(watch.ElapsedMilliseconds, 300);
    }
  }
}
