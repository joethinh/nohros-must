using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;
using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class NohrosConfiguration_ : NohrosConfiguration
    {
        long timeout_;
        string name_;
        bool debug_;

        static NohrosConfiguration_ configuration_;
        static NohrosConfiguration_ configuration_ns_;

        static NohrosConfiguration_() {
            configuration_ = new NohrosConfiguration_();
            configuration_.Load("desktop.config", "/root/desktop");

            configuration_ns_ = new NohrosConfiguration_();
            configuration_ns_.Load("desktop.config", "/root/desktop-ns");
        }

        public NohrosConfiguration_()
        {
            timeout_ = 0;
            debug_ = false;
            name_ = "web";
        }

        [Test]
        public void NohrosConfiguration_Load()
        {
            // configuration without namespace
            Assert.AreEqual("desktop", configuration_.Name);
            Assert.AreEqual(3000, configuration_.Timeout);
            Assert.AreEqual(true, configuration_.Debug);

            Assert.GreaterOrEqual(DateTime.Now, configuration_.Version);
            Assert.AreEqual(configuration_.Location, AppDomain.CurrentDomain.BaseDirectory);

            // configuration with namespace
            Assert.AreEqual("desktop", configuration_ns_.Name);
            Assert.AreEqual(3000, configuration_ns_.Timeout);
            Assert.AreEqual(true, configuration_ns_.Debug);

            Assert.GreaterOrEqual(DateTime.Now, configuration_ns_.Version);
            Assert.AreEqual(configuration_ns_.Location, AppDomain.CurrentDomain.BaseDirectory);
        }

        [Test]
        public void NohrosConfiguration_GetConnectionString() {
            Assert.AreEqual("SQLSERVER", configuration_.GetConnectionString("nohros"));
            Assert.AreEqual("SQLSERVER", configuration_.GetConnectionString("nohros"));
        }

        [Test]
        public void NohrosConfiguration_GetDatabaseOwner() {
            Assert.AreEqual("dbo", configuration_ns_.GetDatabaseOwner("nohros"));
            Assert.AreEqual("dbo", configuration_ns_.GetDatabaseOwner("nohros"));
        }

        [Test]
        public void NohrosConfiguration_Providers() {
            Provider provider = configuration_.GetProvider("NohrosDataProvider");
            Assert.AreNotEqual(null, provider);
            Assert.AreEqual("dbo", provider.DatabaseOwner);
            Assert.AreEqual("SQLSERVER", provider.ConnectionString);
            Assert.AreEqual(DataSourceType.MsSql, provider.DataSourceType);
            Assert.AreEqual("NohrosDataProvider", provider.Name);

            provider = configuration_ns_.GetProvider("NohrosDataProvider");
            Assert.AreNotEqual(null, provider);
            Assert.AreEqual("dbo", provider.DatabaseOwner);
            Assert.AreEqual("SQLSERVER", provider.ConnectionString);
            Assert.AreEqual(DataSourceType.MsSql, provider.DataSourceType);
            Assert.AreEqual("NohrosDataProvider", provider.Name);
        }

        public void NohrosConfiguration_ContentGroup() {
            ContentGroup content_group = configuration_.GetContentGroup("common", "release", "text/css");
            Assert.AreEqual(1, content_group.Files.Count);
            Assert.AreEqual("jquery.js", content_group.Files[0]);
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