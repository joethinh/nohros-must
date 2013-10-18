using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nohros.Configuration;
using Nohros.Security.Auth;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Nohros.Security
{
  public class LoginContextTests
  {
    IAuthCallbackHandler callback_;
    ILoginModule module_;
    IDictionary<string, string> options_;
    IDictionary<string, string> shared_;
    ISubject subject_;

    [SetUp]
    public void SetUp() {
      callback_ = new NopAuthCallbackHandler();
      shared_ = new Dictionary<string, string>();
      options_ = new Dictionary<string, string>();

      module_ = Mock.Create<ILoginModule>();
      Mock
        .Arrange(() => module_.Login(Arg.IsAny<IAuthCallbackHandler>()))
        .Returns(AuthenticationInfos.Sucessful());
      Mock
        .Arrange(() => module_.Commit(Arg.IsAny<IAuthenticationInfo>()))
        .Returns(true);
      Mock
        .Arrange(() => module_.ControlFlag)
        .Returns(LoginModuleControlFlag.Required);

      subject_ = Mock.Create<ISubject>();
      Mock
        .Arrange(() => subject_.Permissions)
        .Returns(new PermissionSet());
      Mock
        .Arrange(() => subject_.Principals)
        .Returns(new PrincipalSet());
    }

    [Test]
    public void should_authenticate_a_valid_user() {
      var callback = new NopAuthCallbackHandler();
      var context = new LoginContext(new[] {module_});

      Assert.That(context.Login(subject_, callback), Is.True);
    }

    [Test]
    public void should_abort_when_authentication_fail() {
      Mock
        .Arrange(() => module_.ControlFlag)
        .Returns(LoginModuleControlFlag.Required);
      Mock
        .Arrange(() => module_.Login(Arg.IsAny<IAuthCallbackHandler>()))
        .Returns(AuthenticationInfos.Failed());
      Mock
        .Arrange(() => module_.Commit(Arg.IsAny<IAuthenticationInfo>()))
        .OccursNever();
      Mock
        .Arrange(() => module_.Abort(Arg.IsAny<IAuthenticationInfo>()))
        .MustBeCalled();

      var callback = new NopAuthCallbackHandler();
      var context = new LoginContext(new[] { module_ });

      Assert.That(context.Login(subject_, callback), Is.False);
      Mock.Assert(module_);
    }
  }
}
