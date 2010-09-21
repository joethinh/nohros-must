using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Net;
using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class NetSettings_
    {
        [Test]
        public void ForCurrentProcess() {
            NetSettings settings = NetSettings.ForCurrentProcess;
        }
    }
}
