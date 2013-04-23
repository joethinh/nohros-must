using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  internal class ProviderAliasesNode
  {
    public static ICollection<string> Parse(XmlElement element) {
      List<string> aliases = new List<string>(element.ChildNodes.Count);
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kAliasNodeName)) {
          string name = AbstractConfigurationNode
            .GetAttributeValue((XmlElement) node, Strings.kNameAttribute);
          aliases.Add(name);
        }
      }
      return aliases;
    }
  }
}
