using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Nohros.Security.Auth;

namespace Assemblies
{
    public class SampleLoginModule : ILoginModule
    {
        bool _debug;

        public void Init(Subject subject, IAuthCallbackHandler callback, IDictionary<string, object> sharedState, IDictionary<string, object> options)
        {
            string debug = options["debug"] as string;
            _debug = (debug != null);
        }

        public bool Commit()
        {
            if (_debug)
                Trace.WriteLine("Commit has been called");
            return true;
        }

        public bool Abort()
        {
            if (_debug)
                Trace.WriteLine("Abort has been called");
            return true;
        }

        public bool Login()
        {
            if (_debug)
                Trace.WriteLine("Login ha been called");
            return true;
        }

        public bool Logout()
        {
            if (_debug)
                Trace.WriteLine("User is logged out");
            return true;
        }
    }
}
