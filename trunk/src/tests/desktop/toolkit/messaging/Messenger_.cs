using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Configuration;
using Nohros.Toolkit.Messaging;

namespace Nohros.Test.Toolkit.Messaging
{
    public class Messenger_
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstanceNullConfigNode() {
            Messenger.CreateInstance(null);
        }

        [Test]
        public void CreateInstanceNullOptions() {
            MessengerProviderNode node = new MessengerProviderNode("SimpleMessenger", "Nohros.Test.Toolkit.Messaging.SimpleMessenger, nohros.test.desktop");
            node.Options = null;
            SimpleMessenger messenger = Messenger.CreateInstance(node) as SimpleMessenger;
            Assert.IsNotNull(messenger);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRequiredOptionNullOption() {
            MessengerProviderNode node = new MessengerProviderNode("SimpleMessenger", "Nohros.Test.Toolkit.Messaging.SimpleMessenger, nohros.test.desktop");
            SimpleMessenger messenger = Messenger.CreateInstance(node) as SimpleMessenger;
            Assert.IsNotNull(messenger);
            messenger.GetRequiredOptionNull();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRequiredOptionMissingOption() {
            MessengerProviderNode node = new MessengerProviderNode("SimpleMessenger", "Nohros.Test.Toolkit.Messaging.SimpleMessenger, nohros.test.desktop");
            node.Options = new Dictionary<string, string>();
            node.Options["required-option-name"] = "required-option-value";
            SimpleMessenger messenger = Messenger.CreateInstance(node) as SimpleMessenger;
            Assert.IsNotNull(messenger);
            messenger.GetRequiredOption("missing-option-name");
        }

        [Test]
        public void GetRequiredOption() {
            MessengerProviderNode node = new MessengerProviderNode("SimpleMessenger", "Nohros.Test.Toolkit.Messaging.SimpleMessenger, nohros.test.desktop");
            node.Options = new Dictionary<string, string>();
            node.Options["required-option-name"] = "required-option-value";
            SimpleMessenger messenger = Messenger.CreateInstance(node) as SimpleMessenger;
            Assert.IsNotNull(messenger);
            Assert.AreEqual("required-option-value", messenger.GetRequiredOption("required-option-name"));
        }
    }
}
