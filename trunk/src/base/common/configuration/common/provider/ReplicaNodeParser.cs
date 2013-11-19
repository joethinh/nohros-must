using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  internal partial class ReplicaNode
  {
    public static ReplicaNode Parse(XmlElement element) {
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      IProviderOptions options = GetOptions(name, element);
      return new ReplicaNode(name, options);
    }

    static IProviderOptions GetOptions(string name, XmlElement element) {
      IList<string> options_references = new List<string>();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kOptionsNodeName)) {
          IProviderOptions options = ProviderOptionsNode
            .Parse(name, (XmlElement) node, out options_references);
          
          // replicas could nt have referential options.
          if (options_references.Count != 0) {
            throw new ConfigurationException(
              Resources.Resources.
                Configuration_providers_replicas_with_ref_options);
          }
          return options;
        }
      }
      return new ProviderOptionsNode(name);
    }
  }
}
