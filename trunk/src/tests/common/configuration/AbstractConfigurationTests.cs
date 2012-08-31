using System;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace Nohros.Configuration
{
  public class ConfigurationMock : Configuration
  {
    #region .ctor
    public ConfigurationMock(Builder builder) : base(builder) {
    }
    #endregion
  }

  public class ConfigurationLoaderTests :
    ConfigurationLoader<ConfigurationMock>
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
      new ConfigurationLoaderTests().Load();
    }

    [Test]
    public void ShouldReturnBaseDirectoryWhenLocationIsNotSpecified() {
      var loader = new ConfigurationLoaderTests();
      Assert.AreEqual(loader.Location, AppDomain.CurrentDomain.BaseDirectory);
    }

    [Test]
    public void ShouldGetTheFirstNodeThatMatchesXPath() {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<root><first><second></second></first></root>");

      XmlNode node = doc.FirstChild;

      XmlNode selected = SelectNode(node, "first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");

      selected = SelectNode(node, "/first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");

      selected = SelectNode(node, "//first/second");
      Assert.IsNotNull(selected);
      Assert.AreEqual(selected.Name, "second");
    }
  }
}
