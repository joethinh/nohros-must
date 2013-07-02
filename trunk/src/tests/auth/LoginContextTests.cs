using System;
using NUnit.Framework;
using Nohros.Configuration;
using Nohros.Security.Auth;
using Telerik.JustMock;

namespace Nohros.Security
{
  public class LoginContextTests
  {
    [Test]
    public void ShouldReturnTrueWhenUserIsValid() {
      var module = Mock.Create<ILoginModule>();
      Mock
        .Arrange(() => module.Login())
        .Returns(true);

      var node = new LoginModuleNode("Tests",
        typeof (LoginContext).AssemblyQualifiedName,
        LoginModuleControlFlag.Sufficient);

      var context = new LoginContext(new[] {
        new LoginModuleNodePair(node, module),
      });

      Assert.That(context.Login(), Is.True);
    }
  }
}
