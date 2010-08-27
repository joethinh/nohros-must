using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class IConfiguration_ : IConfiguration
    {
        long timeout_;
        string name_;
        bool debug_;
        
        public IConfiguration_()
        {
            timeout_ = 0;
            debug_ = false;
            name_ = "web";
        }

        [Test]
        public void IConfiguration_Load() {
            Assert.AreEqual("web", Name);
            Assert.AreEqual(0, Timeout);
            Assert.AreEqual(false, Debug);

            this.Load("desktop.config", "/root/desktop");
            Assert.AreEqual("desktop", Name);
            Assert.AreEqual(3000, Timeout);
            Assert.AreEqual(true, Debug);

            Assert.GreaterOrEqual(DateTime.Now, Version);
            Assert.AreEqual(Location, AppDomain.CurrentDomain.BaseDirectory);
        }

        public long Timeout {
            get { return timeout_; }
            internal set { timeout_ = value; }
        }

        public string Name {
            get { return name_; }
            internal set { name_ = value; }
        }

        public bool Debug {
            get { return debug_; }
            internal set { debug_ = value; }
        }
    }
}
