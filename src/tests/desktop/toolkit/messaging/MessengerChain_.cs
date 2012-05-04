using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Configuration;
using Nohros.Toolkit.Messaging;

namespace Nohros.Test.Toolkit.Messaging
{
    [TestFixture]
    public class MessengerChain_
    {
        [Test]
        public void FromConfiguration() {
            MessengerChain messengers = MessengerChain.FromConfiguration("pseudo-chain", MustConfiguration.DefaultConfiguration);
            Assert.IsNotNull(messengers);
            Assert.AreEqual(1, messengers.Count);
        }
    }
}
