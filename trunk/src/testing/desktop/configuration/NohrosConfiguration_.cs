using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

using NUnit.Framework;

using Nohros.Security.Auth;
using Nohros.Data;
using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class NohrosConfiguration_
    {
        #region TestingConfiguration
        public class TestingConfiguration : NohrosConfiguration
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
            Assert.AreEqual(null, config.CommonNode);
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
            Assert.IsNull(config.CommonNode);
            Assert.IsNull(config.WebNode);
        }

        [Test]
        public void LoadWithoutNamespace() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("without-namespace");
            Assert.AreEqual("without-namespace", config.Name);
            Assert.IsNull(config.CommonNode);
            Assert.IsNull(config.WebNode);
        }

        [Test]
        public void MissingCommonNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("missing-common");
            Assert.AreEqual("missing-common", config.Name);
            Assert.IsNull(config.CommonNode);
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
            Assert.NotNull(config.CommonNode);
            Assert.NotNull(config.WebNode);
        }

        [Test]
        public void RepositoryNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("repository-node");

            RepositoryNode node = config.Repositories["css-path"];
            Assert.IsNotNull(node);
            Assert.AreEqual(Path.Combine(config.Location, "css"), node.Path);
        }

        [Test]
        public void ConnectionStringNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            ConnectionStringNode node = config.ConnectionStrings["nohros"];
            Assert.AreEqual("nohros", node.Name);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
        }

        [Test]
        public void ProviderNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            DataProviderNode node = config.DataProviders["NohrosDataProvider"] as DataProviderNode;
            Assert.AreEqual("NohrosDataProvider", node.Name);
            Assert.AreEqual("Nohros.Data.SqlNohrosDataProvider, nohros.data", node.Type);
            Assert.AreEqual(config.Location, node.AssemblyLocation);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual(DataSourceType.MsSql, node.DataSourceType);
        }

        [Test]
        public void DefaultConfiguration() {
            NohrosConfiguration config = NohrosConfiguration.DefaultConfiguration;
            Assert.IsNotNull(config.CommonNode);
            Assert.IsNotNull(config.WebNode);
        }

        [Test]
        public void ChainNode() {
            NohrosConfiguration config = NohrosConfiguration.DefaultConfiguration;
            ChainNode pseudo_chain = config.Chains["pseudo-chain"] as ChainNode;
            Assert.IsNotNull(pseudo_chain);
            Assert.AreEqual("SmsMessenger", pseudo_chain.Nodes[0]);
            Assert.AreEqual("EmailMessenger", pseudo_chain.Nodes[1]);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void LoginModuleWithInvalidFlag() {
            NohrosConfiguration config = NohrosConfiguration.DefaultConfiguration;
            config.Load("login-module-flag");
        }

        [Test]
        public void LoginModuleNode() {
            NohrosConfiguration config = NohrosConfiguration.DefaultConfiguration;
            config.Load("login-module-node");

            LoginModuleNode node = config.LoginModules["auth-login-module"];
            Assert.IsNotNull(node);
            Assert.AreEqual(LoginModuleControlFlag.SUFFICIENT, node.ControlFlag);
            Assert.AreEqual("auth-login-module", node.Name);
            Assert.AreEqual(node.Options.Count, 0);
            Assert.AreEqual(node.Type, typeof(Nohros.Test.Configuration.StringLoginModule));
        }
    }
}