using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

using NUnit.Framework;

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
            Assert.AreEqual(config.CommonNode.Configuration, config);
        }

        [Test]
        public void RepositoryNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            Assert.AreEqual(Path.Combine(config.Location, "css"), config.CommonNode.GetRepository("css-path"));
        }

        [Test]
        public void ConnectionStringNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            ConnectionStringNode node = config.CommonNode.GetConnectionString("nohros");
            Assert.AreEqual("nohros", node.Name);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
            Assert.AreEqual(config.CommonNode, node.ParentNode);
        }

        [Test]
        public void ProviderNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop");

            ProviderNode node = config.CommonNode.GetProvider("NohrosDataProvider");
            Assert.AreEqual("NohrosDataProvider", node.Name);
            Assert.AreEqual("Nohros.Data.SqlNohrosDataProvider, nohros.data", node.Type);
            Assert.AreEqual(config.Location, node.AssemblyLocation);
            Assert.AreEqual("SQLSERVER", node.ConnectionString);
            Assert.AreEqual("dbo", node.DatabaseOwner);
            Assert.AreEqual(DataSourceType.MsSql, node.DataSourceType);
        }
    }
}