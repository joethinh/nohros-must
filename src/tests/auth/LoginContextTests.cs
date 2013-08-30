using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nohros.Configuration;
using Nohros.Security.Auth;
using Telerik.JustMock;

namespace Nohros.Security
{
  public class LoginContextTests
  {
    [Test]
    public void should_authenticate_a_valid_user() {
      var login_module_factory = Mock.Create<ILoginModuleFactory>();
      var subject = new Subject();
      var callback = new NopAuthCallbackHandler();
      var shared_state = new Dictionary<string, string>();
      var options = new Dictionary<string, string>();

      var module = Mock.Create<ILoginModule>();
      Mock
        .Arrange(() => module.Login())
        .Returns(true);
      Mock
        .Arrange(() => module.Commit())
        .Returns(true);

      Mock
        .Arrange(() =>
          login_module_factory.CreateLoginModule(subject, callback,
            shared_state, options))
        .Returns(module);

      var pair =
        new KeyValuePair<ILoginModuleFactory, IDictionary<string, string>>(
          login_module_factory, options);
      var context = new LoginContext(new[] {pair});

      Assert.That(context.Login(subject, callback), Is.True);
    }

    [Test]
    public void should_abort_when_authentication_fail() {
      var login_module_factory = Mock.Create<ILoginModuleFactory>();
      var subject = new Subject();
      var callback = new NopAuthCallbackHandler();
      var shared_state = new Dictionary<string, string>();
      var options = new Dictionary<string, string>();

      var module = Mock.Create<ILoginModule>();
      Mock
        .Arrange(() => module.ControlFlag)
        .Returns(LoginModuleControlFlag.Required);
      Mock
        .Arrange(() => module.Login())
        .Returns(false);
      Mock
        .Arrange(() => module.Commit())
        .OccursNever();
      Mock
        .Arrange(() => module.Abort())
        .MustBeCalled();

      Mock
        .Arrange(() =>
          login_module_factory.CreateLoginModule(subject, callback,
            shared_state, options))
        .Returns(module);

      var pair =
        new KeyValuePair<ILoginModuleFactory, IDictionary<string, string>>(
          login_module_factory, options);
      var context = new LoginContext(new[] { pair });

      Assert.That(context.Login(subject, callback), Is.False);
      Mock.Assert(module);
    }
  }
}
