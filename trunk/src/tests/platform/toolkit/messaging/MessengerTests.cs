using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Configuration;
using Nohros.Toolkit.Messaging;

namespace Nohros.Toolkit.Tests
{
  [TestFixture]
  public class MessengerTests
  {
    [Test]
    public void ShouldCreateAnInstanceOfTheConfiguredMessenger() {
      MessengerProviderNodeMock node =
        new MessengerProviderNodeMock("MessengerMock",
          "Nohros.Toolkit.Tests.MessengerMock, nohros.toolkit.tests");

      Messenger.CreateInstance(node);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowAnArgumentExceptionWhenARequiredOptionIsMissing() {
      Dictionary<string, string> options = new Dictionary<string, string>();
      options.Add("requiredOption", "requiredOptionValue");
      Messenger.GetRequiredOption("missingOption", options);
    }

    public void ShouldReturnTheRequiredOptionWhenItExists() {
      Dictionary<string, string> options = new Dictionary<string, string>();
      options.Add("requiredOption", "requiredOptionValue");
      string required_option_value =
        Messenger.GetRequiredOption("requiredOption", options);

      Assert.AreEqual("requiredOptionValue", required_option_value);
    }
  }
}
