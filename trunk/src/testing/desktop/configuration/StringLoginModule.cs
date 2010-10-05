using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Security.Auth;

namespace Nohros.Test.Configuration
{
    public class StringLoginModule : ILoginModule
    {
        public void Init(Subject subject, IAuthCallbackHandler callback, IDictionary<string, object> sharedState, IDictionary<string, object> options) {
        }

        public bool Abort() { return true; }

        public bool Commit() { return true; }

        public bool Login() { return true; }

        public bool Logout() { return true; }
    }
}
