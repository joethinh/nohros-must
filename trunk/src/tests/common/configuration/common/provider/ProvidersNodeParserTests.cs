using System;
using Nohros.IO;
using System.Xml;
using NUnit.Framework;

namespace Nohros.Configuration
{
  public class ProvidersNodeParserTests
  {
    [Test]
    public void ShouldResolveOptionsReferences() {
      const string xml_config_file = @"<providers>
    <options name='sql-connection-provider'>
      <option name='connection-string' value='SQL-CONNECTION-STRING'/>
      <option name='schema' value='dbo'/>
    </options>
    <options name=""my-options"">
      <option ref='sql-connection-provider'/>
    </options>

    <provider name=""my-provider"" type="""">
      <options>
        <option name ='my-provider-option'/>
        <option ref='sql-connection-provider'/>
      </options>
    </provider>

    <provider name=""my-provider-two"" type="""">
      <options>
        <option ref='my-options'/>
      </options>
    </provider>
  </providers>";

      var document = new XmlDocument();
      document.LoadXml(xml_config_file);
      var xml_element = (XmlElement) document.FirstChild;
      IProvidersNode providers_node = ProvidersNode.Parse(xml_element,
        Path.AbsoluteForApplication(string.Empty));

      Assert.AreEqual(1, providers_node.Count);
      IProviderNode provider = providers_node[string.Empty]["my-provider"];
      Assert.AreEqual(3, provider.Options.Count);
      Assert.AreEqual(true, provider.Options.ContainsKeys("connection-string"));
      Assert.AreEqual(true, provider.Options.ContainsKeys("schema"));
      Assert.AreEqual(true, provider.Options.ContainsKeys("my-provider-option"));

      Assert.AreEqual(1, providers_node.Count);
      provider = providers_node[string.Empty]["my-provider-two"];
      Assert.AreEqual(2, provider.Options.Count);
      Assert.AreEqual(true, provider.Options.ContainsKeys("connection-string"));
      Assert.AreEqual(true, provider.Options.ContainsKeys("schema"));
    }

    [Test]
    public void ShouldThrowExceptionWhenOptionReferenceDoesNotExist() {
      const string xml_config_file = @"<providers>
    <options name=""sql-connection-provider"">
      <connection-string>SQL-CONNECTION-STRING</connection-string>
      <schema>dbo</schema>
    </options>
    <options name=""my-options"">
      <option ref=""sql-connection""/>
    </options>
  </providers>";

      var document = new XmlDocument();
      document.LoadXml(xml_config_file);
      var xml_element = (XmlElement)document.FirstChild;

      Assert.Throws<ConfigurationException> (
        () => ProvidersNode.Parse(xml_element,
          Path.AbsoluteForApplication(string.Empty)));
    }
  }
}
