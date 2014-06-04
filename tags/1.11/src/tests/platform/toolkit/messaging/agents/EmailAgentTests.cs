using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Toolkit.Messaging;

namespace Nohros.Toolkit.Tests
{
  [TestFixture]
  public class EmailAgentTests
  {
    [Test]
    public void ShouldReturnEmptyStringWhenNameIsNotSpecified() {
      EmailAgent email_agent = new EmailAgent("agent@address.com.br");
      Assert.AreEqual(string.Empty, email_agent.Name);
    }

    [Test]
    public void ShouldReturnTheSpecifiedAddress() {
      string address = "agent@address.com.br";
      EmailAgent agent = new EmailAgent(address);
      Assert.AreEqual(address, agent.Address);
    }

    [Test]
    public void ShouldReturnTheSpecifiedName() {
      string name = "AgentName";
      string address = "agent@address.com.br";
      EmailAgent agent = new EmailAgent(address, name);
      Assert.AreEqual(address, agent.Address);
      Assert.AreEqual(name, agent.Name);
    }
  }
}