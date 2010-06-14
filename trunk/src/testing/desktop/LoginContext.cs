using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Nohros.Security;
using Nohros.Security.Auth;

namespace desktdd
{
    [TestFixture]
    public class LoginContextTests
    {
        [Test]
        public void ctor()
        {
            LoginContext lc = new LoginContext();
            lc.Login();
        }
    }
}
