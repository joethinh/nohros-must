using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Configuration;
using Nohros.Toolkit.Messaging;

namespace Nohros.Toolkit.Tests
{
  public class MessengerProviderNodeMock : MessengerProviderNode
  {
    public MessengerProviderNodeMock(string name, string type)
      : base(name, type) {
    }
  }
}
