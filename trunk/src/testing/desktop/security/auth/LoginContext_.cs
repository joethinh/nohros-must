using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Nohros.Security;
using Nohros.Security.Auth;

namespace Nohros.Test.Security.Auth
{
    [TestFixture]
    public class LoginContext_
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSubject()
        {
            LoginContext lc = new LoginContext((Subject)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCallbackHandler() {
            LoginContext lc = new LoginContext((IAuthCallbackHandler)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoginConfiguration() {
            LoginContext lc = new LoginContext((ILoginConfiguration) null);
        }

        [Test]
        public void LoginWithNoModule() {
            LoginConfiguration config = new LoginConfiguration();
            config.Load("with-namespace");

            LoginContext context = new LoginContext(config);
            Assert.AreEqual(false, context.Login());
        }
    }
}
