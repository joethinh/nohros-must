using System;

using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Nohros.Concurrent
{
  [TestFixture]
  public class MailboxTests
  {
    [Test]
    public void SholudReceiveWhatWasSent() {
      ManualResetEvent sync = new ManualResetEvent(false);
      Mailbox<long> mailbox = new Mailbox<long>(delegate(long number) {
        Assert.AreEqual(50, number);
        sync.Set();
      });

      mailbox.Send(50);
      bool timed_out = sync.WaitOne(3000);
      Assert.AreEqual(true, timed_out);
    }
  }
}
