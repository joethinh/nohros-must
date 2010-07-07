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
        public void ctor()
        {
            LoginContext lc = new LoginContext();
            lc.Login();
        }
    }
}
