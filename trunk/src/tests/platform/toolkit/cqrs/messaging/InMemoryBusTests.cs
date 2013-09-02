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
      bool subscriber_called = false;
      bus.Subscribe(Handlers.Runnable<MyMessage>(msg => {
        Assert.That(msg.Message, Is.EqualTo("MyMessage"));
        subscriber_called = true;
      }));
      bus.Publish(new MyMessage {Message = "MyMessage"});
      Assert.That(subscriber_called, Is.EqualTo(true));
    }

    [Test]
    public void should_unsubscrible() {
      var bus = new InMemoryBus();
      bool subscriber_called = false;
      var handler =
        Handlers.Runnable<MyMessage>(msg => { subscriber_called = true; });
      bus.Subscribe(handler);
      bus.Unsubscribe(handler);
      bus.Publish(new MyMessage {Message = "MyMessage"});
      Assert.That(subscriber_called, Is.EqualTo(false));
    }
  }
}
