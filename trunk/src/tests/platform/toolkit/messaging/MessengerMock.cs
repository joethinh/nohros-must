using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Toolkit.Messaging;

namespace Nohros.Toolkit.Tests
{
  public class MessengerMock : IMessenger, IMessengerFactory
  {
    IMessage sent_message_;

    public string Name {
      get { return "messengermock"; }
    }

    public IMessenger CreateMessenger(string name,
      IDictionary<string, string> options) {
      MessengerMock mock = new MessengerMock();
      return mock;
    }

    public IMessage Send(IMessage message) {
      sent_message_ = message;
      return message;
    }

    public IMessage SentMessage {
      get { return sent_message_; }
    }
  }
}
