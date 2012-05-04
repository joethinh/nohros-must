using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

using NUnit.Framework;

using Nohros.Security.Auth;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Data.Collections;
using Nohros.Configuration;
using Nohros.Logging;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class NohrosConfiguration_
    {
        #region TestingConfiguration
        public class TestingConfiguration : MustConfiguration
        {
            string name_;

            #region .ctor
            public TestingConfiguration() {
                name_ = null;
            }
            #endregion

            public string Name {
                get { return name_; }
                internal set { name_ = value; }
            }
        }
        #endregion

        [Test]
        public void DefaultConstructor()
        {
            TestingConfiguration config = new TestingConfiguration();
            Assert.IsNotNull(config);
            Assert.AreEqual(null, config.CommonNodeParser);
            Assert.AreEqual(null, config.WebNode);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadWithInvalidRootNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load((string)"invalidrootnode");
        }

        [Test]
        public void LoadWithNamespace() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("with-namespace");
            Assert.AreEqual("with-namespace", config.Name);
            Assert.IsNull(config.CommonNodeParser);
            Assert.IsNull(config.WebNode);
        }

        [Test]
        public void LoadWithoutNamespace() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("without-namespace");
            Assert.AreEqual("without-namespace", config.Name);
            Assert.IsNull(config.CommonNodeParser);
            Assert.IsNull(config.WebNode);
        }

        [Test]
        public void MissingCommonNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("missing-common");
            Assert.AreEqual("missing-common", config.Name);
            Assert.IsNull(config.CommonNodeParser);
            Assert.IsNull(config.WebNode);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MissingRepositoryNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("missing-repository");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void RootedRepositoryNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("rooted-repository");
        }

        [Test]
        public void Load() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");
            Assert.NotNull(config.CommonNodeParser);
            Assert.NotNull(config.WebNode);
        }

        [Test]
        public void RepositoryNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("repository-node");

            RepositoryNode node = config.RepositoryNodes["css-path"];
            Assert.IsNotNull(node);
            Assert.AreEqual(Path.Combine(config.Location, "css"), node.RelativePath);
        }

        [Test]
        public void ConnectionStringNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            ConnectionStringNode node = config.ConnectionStringNodes["nohros"];
            Assert.AreEqual("nohros", node.Name);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
        }

        [Test]
        public void DataProviderNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            DataProviderNode node = config.DataProviderNodes["NohrosDataProvider"] as DataProviderNode;
            Assert.AreEqual("NohrosDataProvider", node.Name);
            Assert.AreEqual("Nohros.Data.SqlNohrosDataProvider, nohros.data", node.Type);
            Assert.AreEqual(config.Location, node.Location);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual(DataSourceType.MsSql, node.DataSourceType);
        }

        [Test]
        public void MessengerProviderNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            MessengerProviderNode node = config.MessengerProviderNodes["SmsMessenger"] as MessengerProviderNode;
            Assert.IsNotNull(node);
            Assert.AreEqual("SmsMessenger", node.Name);
            Assert.AreEqual("Nohros.Test.Toolkit.Messaging.SmsMessenger, nohros.test.desktop", node.Type);
            Assert.AreEqual(null, node.AssemblyLocation);

            node = config.MessengerProviderNodes["SimpleMessenger"] as MessengerProviderNode;
            Assert.IsNotNull(node);
            Assert.AreEqual("SimpleMessenger", node.Name);
            Assert.AreEqual("Nohros.Test.Toolkit.Messaging.SimpleMessenger, nohros.test.desktop", node.Type);
            Assert.AreEqual(null, node.AssemblyLocation);

            string host, port;
            Assert.IsTrue(node.Options.TryGetValue("smtp-host", out host));
            Assert.IsTrue(node.Options.TryGetValue("smtp-port", out port));
            Assert.AreEqual(host, "smtp.acaoassessoria.com.br");
            Assert.AreEqual(port, "587");
        }

        [Test]
        public void DefaultConfiguration() {
            MustConfiguration config = MustConfiguration.DefaultConfiguration;
            Assert.IsNotNull(config.CommonNodeParser);
            Assert.IsNotNull(config.WebNode);
        }

        [Test]
        public void ChainNode() {
            MustConfiguration config = MustConfiguration.DefaultConfiguration;
            ChainNode pseudo_chain = config.ChainNodes["pseudo-chain"] as ChainNode;
            Assert.IsNotNull(pseudo_chain);
            Assert.AreEqual("SmsMessenger", pseudo_chain.Nodes[0]);
            Assert.AreEqual("EmailMessenger", pseudo_chain.Nodes[1]);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void LoginModuleWithInvalidFlag() {
            MustConfiguration config = MustConfiguration.DefaultConfiguration;
            config.Load("login-module-flag");
        }

        [Test]
        public void LoginModuleNode() {
            MustConfiguration config = MustConfiguration.DefaultConfiguration;
            config.Load("login-module-node");

            LoginModuleNode node = config.LoginModuleNodes["auth-login-module"];
            Assert.IsNotNull(node);
            Assert.AreEqual(LoginModuleControlFlag.SUFFICIENT, node.ControlFlag);
            Assert.AreEqual("auth-login-module", node.Name);
            Assert.AreEqual(node.Options.Count, 0);
            Assert.AreEqual(node.Type, typeof(Nohros.Test.Configuration.StringLoginModule));
        }

        [Test]
        public void Threshold() {
            MustConfiguration config = MustConfiguration.DefaultConfiguration;
            Assert.AreEqual(log4net.Core.Level.Debug, FileLogger.ForCurrentProcess.Threshold);
            Assert.AreEqual(log4net.Core.Level.Debug, ConsoleLogger.ForCurrentProcess.Threshold);
        }
    }
}