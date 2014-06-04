using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

using NUnit.Framework;
using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class IConfiguration_
    {
        #region TestingConfiguration
        public class TestingConfiguration : AbstractConfiguration
        {
            long timeout_;
            string name_;
            bool debug_;

            #region .ctor
            public TestingConfiguration() {
                name_ = null;
                timeout_ = 0;
                debug_ = false;
            }
            #endregion

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

            public XmlNode XmlElement {
                get { return element; }
            }
        }
        #endregion

        [Test]
        public void DefaultConstructor() {
            TestingConfiguration config = new TestingConfiguration();
            Assert.AreEqual(0, config.Timeout);
            Assert.AreEqual(false, config.Debug);
            Assert.AreEqual(null, config.Name);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void LoadWithInvalidFileInfo() {
            TestingConfiguration config = new TestingConfiguration();
            config.LoadAndWatch(new FileInfo("C:\\WINDOWS\\desktop.config"), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadWithInvalidRootElement() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop.config", "testing");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadWithNullFileInfo() {
            TestingConfiguration config = new TestingConfiguration();
            config.LoadAndWatch((FileInfo)null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadWithNullFileName() {
            TestingConfiguration config = new TestingConfiguration();
            config.LoadAndWatch((string)null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadWithNullRootElement() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load((string)null);
        }

        [Test]
        public void LocationNull() {
            string base_path = AppDomain.CurrentDomain.BaseDirectory;

            TestingConfiguration config = new TestingConfiguration();
            Assert.AreEqual(base_path, config.Location);
        }

        [Test]
        public void AutoLoadProperties() {
            TestingConfiguration config = new TestingConfiguration();
            Assert.AreEqual(false, config.Debug);
            Assert.AreEqual(0, config.Timeout);

            config.Load("desktop.config", "desktop");
            Assert.AreEqual(true, config.Debug);
            Assert.AreEqual(3000, config.Timeout);
        }

        [Test]
        public void Version() {
            TestingConfiguration config = new TestingConfiguration();
            Assert.LessOrEqual(DateTime.Now, config.Version);
        }

        [Test]
        public void SelectNode() {
            TestingConfiguration config = new TestingConfiguration();
            config.Load("desktop.config", "desktop");
            Assert.IsNotNull(config.XmlElement);

            XmlNode node = AbstractConfiguration.SelectNode(config.XmlElement, "/nohros/common/providers");
            Assert.IsNull(node);

            node = AbstractConfiguration.SelectNode(config.XmlElement, "nohros/common/providers");
            Assert.IsNotNull(node);
        }
    }
}
