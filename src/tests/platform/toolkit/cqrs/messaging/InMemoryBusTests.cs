using System;
using NUnit.Framework;

namespace Nohros.CQRS.Messaging
{
  public class InMemoryBusTests
  {
    class MyMessage : IMessage
    {
      public string Message { get; set; }
    }

    [Test]
    public void should_publish_message_to_subscribers() {
      var bus = new InMemoryBus();
      bus.Subscribe(
        Handlers.Runnable<MyMessage>(
          msg => Assert.That(msg.Message, Is.EqualTo("MyMessage"))));
      bus.Publish(new MyMessage {Message = "MyMessage"});
    }
  }
}
