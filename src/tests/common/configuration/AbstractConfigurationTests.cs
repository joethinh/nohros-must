using System;
using System.IO;
using System.Xml;

using NUnit.Framework;

namespace Nohros.Configuration
{
  class AbstractConfigurationMock : AbstractConfiguration {
  }

  public class AbstractConfigurationTests
  {
    string config_file_path_;

    [SetUp]
    public void Setup() {
      config_file_path_ = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        "config-tests.xml");
    }

    [Test]
    [ExpectedException(typeof(ConfigurationException))]
    public void ShouldThrowConfigurationExceptionWhenAppConfigKeyIsMissing() {
      AbstractConfigurationMock mock = new AbstractConfigurationMock();
      mock.Load();
    }

    [Test]
    public void ShouldThrowArgumentExceptionWhenArgIsNull() {
      AbstractConfigurationMock mock = new AbstractConfigurationMock();

      Assert.Throws<ArgumentNullException>(delegate()
      {
        mock.Load((XmlElement) null);
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        mock.Load((string)null, "missing-root-node");
      });

      Assert.Throws<ArgumentNullException>(delegate()
      {
        mock.LoadAndWatch((string)null, "missing-root-node");
      });

      Assert.Throws<ArgumentNullException>(delegate()
      {
        mock.LoadAndWatch((FileInfo)null, "missing-root-node");
      });
    }

    [Test]
    public void ShouldNotThrowExceptionWhenRootNodeIsMissing() {
      AbstractConfigurationMock mock = new AbstractConfigurationMock();

      Assert.DoesNotThrow(delegate()
      {
        mock.Load(config_file_path_, null);
      });

      Assert.DoesNotThrow(delegate()
      {
        FileInfo fi = new FileInfo(config_file_path_);
        mock.Load(fi, null);
      });

      Assert.DoesNotThrow(delegate()
      {
        FileInfo fi = new FileInfo(config_file_path_);
        mock.LoadAndWatch(fi, null);
      });

      Assert.DoesNotThrow(delegate()
      {
        mock.LoadAndWatch(config_file_path_, null);
      });
    }

    [Test]
    public void ShouldReturnBaseDirectoryWhenLocationIsNotSpecified() {
      AbstractConfigurationMock mock = new AbstractConfigurationMock();

      Assert.AreEqual(mock.Location, AppDomain.CurrentDomain.BaseDirectory);
    }

    [Test]
    public void ShouldGetTheFirstNodeThatMatchesXPath() {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<root><first><second></second></first></root>");

      XmlNode node = doc.FirstChild;

      XmlNode selected = AbstractConfiguration.SelectNode(node, "first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");

      selected = AbstractConfiguration.SelectNode(node, "/first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");

      selected = AbstractConfiguration.SelectNode(node, "//first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");
    }
  }
}
